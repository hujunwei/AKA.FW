using System.Linq.Expressions;
using EFDataAccess.Model.Common;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace EFDataAccess.DataAccess;

/// <summary>
/// A infra-level accessors abstraction.
/// Accessors should extends this, or create next level abstraction by extending this.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EntityAccessor<TEntity> where TEntity : class, new()
{
    // ReSharper disable once MemberCanBePrivate.Global
    // May used by it descendants
    protected EFCoreDemoTransaction Transaction { get; private set; } = default!;

    // ReSharper disable once MemberCanBePrivate.Global
    // May used by it descendants
    internal EFCoreContext Context { get; set; }

    protected EntityAccessor(EFCoreContext context)
    {
        this.Context = context;
    }

    protected EntityAccessor(EFCoreContext context, EFCoreDemoTransaction transaction)
    {
        Context = context;
        SetTransaction(transaction);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    // May used by it descendants
    internal void SetTransaction(EFCoreDemoTransaction transaction)
    {
        Transaction = transaction;
        Context = Transaction?.Context ?? new EFCoreContext();
    }

    /// <summary>
    /// Gets an entity by its keys
    /// Note this does not include navigation properties
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    protected async Task<TEntity?> GetByKey(params object[] keys)
    {
        return await Context.Set<TEntity>().FindAsync(keys, ServiceRuntimeContext.ServerTimeoutCancellationToken);
    }

    /// <summary>
    /// Gets an entity by a predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected async Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate, IEnumerable<string>? includePropNames = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        
        // Append Navigation Property to query
        if (includePropNames != null)
        {
            AppendIncludeProperties(ref query, includePropNames);
        }
        
        return await query.SingleOrDefaultAsync(predicate, ServiceRuntimeContext.ServerTimeoutCancellationToken);
    }

    /// <summary>
    /// Lists entities by a predicate and a pagination descriptor
    /// </summary>
    /// <param name="includePropNames"></param>
    /// <param name="predicate"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    protected async Task<List<TEntity>> List(IEnumerable<string>? includePropNames = null, Expression<Func<TEntity, bool>>? predicate = null,
        PaginationDescriptor? pagination = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        
        // Append Navigation Property to query
        if (includePropNames != null)
        {
            AppendIncludeProperties(ref query, includePropNames);
        }
        
        // Append Where to query
        if (predicate == null)
        {
            predicate = _ => true;
        }
        
        query = query.Where(predicate);

        // Append order by to query
        // order by is required for paging / sorting
        if (pagination != null && !string.IsNullOrWhiteSpace(pagination.OrderBy))
        {
            // sorting
            var orderByProperty =
                typeof(TEntity).GetProperties().FirstOrDefault(p =>
                    p.Name.Equals(pagination.OrderBy, StringComparison.InvariantCultureIgnoreCase));
            Exception<NotSupportedException>.ThrowOn(() => orderByProperty == null,
                $"Sorting by field {pagination.OrderBy} is not supported.");

            var arg = Expression.Parameter(typeof(TEntity), "_");
            var property = Expression.Property(arg, orderByProperty!.Name);

            // TODO: ugly implementation...
            if (property.Type == typeof(string))
            {
                var selector = Expression.Lambda<Func<TEntity, string>>(property, new ParameterExpression[] { arg });
                query = pagination.Ascending ? query.OrderBy(selector) : query.OrderByDescending(selector);
            }
            else if (property.Type == typeof(int))
            {
                var selector = Expression.Lambda<Func<TEntity, int>>(property, new ParameterExpression[] { arg });
                query = pagination.Ascending ? query.OrderBy(selector) : query.OrderByDescending(selector);
            }
            else if (property.Type == typeof(DateTime))
            {
                var selector = Expression.Lambda<Func<TEntity, DateTime>>(property, new ParameterExpression[] { arg });
                query = pagination.Ascending ? query.OrderBy(selector) : query.OrderByDescending(selector);
            }
            else if (property.Type == typeof(Guid))
            {
                var selector = Expression.Lambda<Func<TEntity, Guid>>(property, new ParameterExpression[] { arg });
                query = pagination.Ascending ? query.OrderBy(selector) : query.OrderByDescending(selector);
            }
            else
            {
                throw new NotSupportedException(
                    $"Sorting by field {pagination.OrderBy} of type {property.Type} is not supported.");
            }

            // paging
            if (pagination.Take > 0)
            {
                query = query.Skip(Math.Max(0, pagination.Skip)).Take(pagination.Take);
            }
        }

        return await query.ToListAsync(ServiceRuntimeContext.ServerTimeoutCancellationToken);
    }

    /// <summary>
    /// Creates an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected async Task<TEntity> Create(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);

        return await DatabaseUpdateExceptionHandler(async () =>
        {
            await Context.SaveChangesAsync(ServiceRuntimeContext.ServerTimeoutCancellationToken);
            return entity;
        });
    }

    /// <summary>
    /// Updates an entity by its keys
    /// </summary>
    /// <param name="updated"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    protected async Task<TEntity> Update(TEntity updated, params object[] keys)
    {
        Exception<ArgumentNullException>.ThrowOn(() => updated == null, $"{nameof(updated)} entity cannot be null.");
        
        var existing = await Context.Set<TEntity>().FindAsync(keys);

        Exception<ArgumentNullException>.ThrowOn(() => existing == null, $"{nameof(existing)} entity cannot be null.");

        return await Update(existing!, updated);
    }

    /// <summary>
    /// Updates an entity, providing its existing version
    /// </summary>
    /// <param name="existing"></param>
    /// <param name="updated"></param>
    /// <returns></returns>
    protected async Task<TEntity> Update(TEntity existing, TEntity updated)
    {
        Exception<ArgumentNullException>.ThrowOn(() => existing == null, $"{nameof(existing)} entity cannot be null.");
        Exception<ArgumentNullException>.ThrowOn(() => updated == null, $"{nameof(updated)} entity cannot be null.");

        // find the db entry for the entity
        var dbEntry = Context.Entry(existing);

        // update its state to modified and set updated values
        dbEntry.State = EntityState.Modified;
        dbEntry.CurrentValues.SetValues(updated);

        // if row version is enforced, the row version's current value should be preserved (EF will use the value to implement optimistic concurrency)
        if (existing is IRowVersionedEntity)
        {
            const string rowVersionPropertyName = nameof(IRowVersionedEntity._RowVersion);

            dbEntry.Property(rowVersionPropertyName).OriginalValue = dbEntry.Property(rowVersionPropertyName).CurrentValue;
        }

        return await DatabaseUpdateExceptionHandler(async () =>
        {
            await Context.SaveChangesAsync(ServiceRuntimeContext.ServerTimeoutCancellationToken);

            return existing;
        });
    }

    /// <summary>
    /// (Hard) deletes an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected async Task Delete(TEntity entity)
    {
        var dbEntry = Context.Entry(entity);
        dbEntry.State = EntityState.Deleted;

        await DatabaseUpdateExceptionHandler(async () =>
            await Context.SaveChangesAsync(ServiceRuntimeContext.ServerTimeoutCancellationToken));
    }

    /// <summary>
    /// Counts the items filtered by a predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected internal async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await Context.Set<TEntity>().CountAsync(ServiceRuntimeContext.ServerTimeoutCancellationToken);
        }
        else
        {
            return await Context.Set<TEntity>()
                .CountAsync(predicate, ServiceRuntimeContext.ServerTimeoutCancellationToken);
        }
    }

    private static void AppendIncludeProperties(ref IQueryable<TEntity> query, IEnumerable<string> includePropNames)
    {
        foreach (var propName in includePropNames)
        {
            var includeProperty = typeof(TEntity).GetProperties().FirstOrDefault(p =>
                p.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase));
            Exception<ArgumentException>.ThrowOn(() => includeProperty == null,
                $"{propName} of entity {nameof(TEntity)} cannot be found");
                
            var arg = Expression.Parameter(typeof(TEntity), "_");
            var property = Expression.Property(arg, includeProperty!.Name);
            var selector = Expression.Lambda<Func<TEntity, object>>(property, new ParameterExpression[] { arg });
            
            query = query.Include(selector);
        }
    }

    private async Task<TResult> DatabaseUpdateExceptionHandler<TResult>(Func<Task<TResult>> updateDatabase)
    {
        try
        {
            return await updateDatabase();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new EntityChangedException(
                $"The entity of type <{typeof(TEntity).FullName}> has been changed by others during the update.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new EntityUpdateException($"Cannot update the entity of type <{typeof(TEntity).FullName}>.", ex);
        }
    }
}