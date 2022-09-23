using System.Linq.Expressions;
using EFDataAccess.Model;
using EFDataAccess.Model.Common;

namespace EFDataAccess.DataAccess.Accessors;

public class RouteMappingAccessor : EntityAccessor<RouteMapping>, IRouteMappingAccessor
{
    protected RouteMappingAccessor(EFCoreContext context) : base(context)
    {
    }

    public async Task<RouteMapping?> GetById(Guid id)
     {
         return await base.Get(p => p.Id == id);
     }

     public async Task<IEnumerable<RouteMapping>> List(Expression<Func<RouteMapping, bool>>? predicate = null, PaginationDescriptor? descriptor = null)
     {
         return await base.List(predicate: predicate, pagination: descriptor);
     }

     public async Task<RouteMapping> Add(RouteMapping RouteMapping)
     {
         return await base.Create(RouteMapping);
     }

     public async Task<RouteMapping> Update(RouteMapping updated)
     {
         return await base.Update(updated, updated.Id);
     }

     public new async Task Delete(RouteMapping RouteMapping)
     {
         await base.Delete(RouteMapping);
     }
}