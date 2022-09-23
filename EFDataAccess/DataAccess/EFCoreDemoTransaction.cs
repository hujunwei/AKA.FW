using Microsoft.EntityFrameworkCore.Storage;

namespace EFDataAccess.DataAccess;

/// <summary>
/// The SQL transaction wrapper, which can be used in data accessors AND business logic managers.
///
/// If there is an existing transaction, use existing.
/// Else use new.
/// </summary>
public class EFCoreDemoTransaction : IDisposable
{
    internal EFCoreContext Context { get; }

    private readonly IDbContextTransaction _transaction = default!;

    public EFCoreDemoTransaction(EFCoreContext context, EFCoreDemoTransaction? existingTransaction = null)
    {
        if (existingTransaction == null)
        {
            // new transaction
            // TODO: ctx should use constructor with option? How to pass options?
            this.Context = context;
            this._transaction = this.Context.Database.BeginTransaction();
        }
        else
        {
            this.Context = existingTransaction.Context;
        }
    }

    public void Complete()
    {
        if (this._transaction != null)
        {
            this._transaction.Commit();
        }
    }

    public void Dispose()
    {
        if (this._transaction != null)
        {
            this._transaction.Dispose();
        }
    }
}