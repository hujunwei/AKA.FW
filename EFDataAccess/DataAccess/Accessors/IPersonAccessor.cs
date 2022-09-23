// using EFDataAccess.Model;
// using EFDataAccess.Model.Common;
//
// namespace EFDataAccess.DataAccess.Accessors;
//
// public interface IPersonAccessor
// {
//     Task<Person?> GetById(int id, IEnumerable<string>? includePropNames = null);
//     Task<IEnumerable<Person>> ListAll(IEnumerable<string> includePropNames);
//     Task<IEnumerable<Person>> ListAll(IEnumerable<string> includePropNames, PaginationDescriptor descriptor);
//     Task<Person> Add(Person person);
//     Task<Person> Update(Person updated);
//     Task Delete(Person person);
// }