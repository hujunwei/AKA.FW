// using EFDataAccess.Model;
// using EFDataAccess.Model.Common;
// using Microsoft.Extensions.Logging;
//
// namespace EFDataAccess.DataAccess.Accessors;
//
// public class PersonAccessor : EntityAccessor<Person>, IPersonAccessor
// {
//     private readonly ILogger<PersonAccessor> _logger;
//
//     public PersonAccessor(EFCoreContext context, ILogger<PersonAccessor> logger) : base(context)
//     {
//         _logger = logger;
//     }
//     
//     public async Task<Person?> GetById(int id, IEnumerable<string>? includePropNames = null)
//     {
//         return await base.Get(p => p.Id == id, includePropNames);
//     }
//
//     public async Task<IEnumerable<Person>> ListAll(IEnumerable<string> includePropNames)
//     {
//         return await base.List(includePropNames);
//     }
//     
//     public async Task<IEnumerable<Person>> ListAll(IEnumerable<string> includePropNames, PaginationDescriptor descriptor)
//     {
//         return await base.List(includePropNames, null, descriptor);
//     }
//
//     public async Task<Person> Add(Person person)
//     {
//         return await base.Create(person);
//     }
//
//     public async Task<Person> Update(Person updated)
//     {
//         return await base.Update(updated, updated.Id);
//     }
//
//     public new async Task Delete(Person person)
//     {
//         await base.Delete(person);
//     }
// }