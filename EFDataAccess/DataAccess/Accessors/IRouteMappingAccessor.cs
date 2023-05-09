using System.Linq.Expressions;
using EFDataAccess.Model;
using EFDataAccess.Model.Common;

namespace EFDataAccess.DataAccess.Accessors;

public interface IRouteMappingAccessor
{
    Task<RouteMapping?> GetById(Guid id);
    Task<IEnumerable<RouteMapping>> List(Expression<Func<RouteMapping, bool>>? predicate = null, PaginationDescriptor? descriptor = null);
    Task<RouteMapping> Add(RouteMapping RouteMapping);
    Task<RouteMapping> Update(RouteMapping updated);
    Task Delete(RouteMapping RouteMapping);
}