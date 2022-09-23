// using AutoMapper;
// using EFCoreApi.DTOs;
// using EFDataAccess.DataAccess.Accessors;
// using EFDataAccess.Model;
// using EFDataAccess.Model.Common;
// using FluentValidation;
// using Utilities;
//
// namespace EFCoreApi.Domain;
//
// public class PeopleManager : IPeopleManager
// {
//     private readonly IPersonAccessor _personAccessor;
//     private readonly IMapper _mapper;
//     private readonly IValidator<PersonDto> _personDtoValidator;
//     private readonly ILogger<PeopleManager> _logger;
//
//     public PeopleManager(
//         IPersonAccessor personAccessor,
//         IMapper mapper,
//         IValidator<PersonDto> personDtoValidator,
//         ILogger<PeopleManager> logger)
//     {
//         _personAccessor = personAccessor;
//         _mapper = mapper;
//         _personDtoValidator = personDtoValidator;
//         _logger = logger;
//     }
//
//     public async Task<PersonDto> GetPersonById(int id)
//     {
//         var includePropNames = new[] { nameof(Person.Addresses), nameof(Person.EmailAddresses) };
//         var person = await _personAccessor.GetById(id, includePropNames);
//         
//         Exception<KeyNotFoundException>.ThrowOn(() => person == null, $"Person with id:{id} not found.");
//
//         return _mapper.Map<PersonDto>(person);
//     }
//
//     public async Task<IEnumerable<PersonDto>> ListAllPeople()
//     {
//         var includePropNames = new[] { nameof(Person.Addresses), nameof(Person.EmailAddresses) };
//         var people = await _personAccessor.ListAll(includePropNames);
//
//         return _mapper.Map<IEnumerable<PersonDto>>(people);
//     }
//
//     public async Task<IEnumerable<PersonDto>> ListAllPeople(int pageNum, int pageSize)
//     {
//         Exception<ArgumentException>.ThrowOn(() => pageNum < 1, $"{nameof(pageNum)} cannot be less than 1.");
//         Exception<ArgumentException>.ThrowOn(() => pageSize < 0, $"{nameof(pageSize)} cannot be negative.");
//
//         var includePropNames = new[] { nameof(Person.Addresses), nameof(Person.EmailAddresses) };
//
//         var paginationDescriptor = new PaginationDescriptor
//         {
//             Skip = (pageNum - 1) * pageSize,
//             Take = pageSize,
//             OrderBy = "Id",
//             Ascending = false
//         };
//         
//         var people = await _personAccessor.ListAll(includePropNames, paginationDescriptor);
//
//         return _mapper.Map<IEnumerable<PersonDto>>(people);
//     }
//
//     public async Task<PersonDto> AddPerson(PersonDto personDto)
//     {
//         await _personDtoValidator.ValidateAsync(personDto);
//         
//         var currUser = ServiceRuntimeContext.CurrentUserClaims?.Identity?.Name ?? "defaultuser@admin.com";
//         
//         var person = new Person
//         {
//             FirstName = personDto.FirstName,
//             LastName = personDto.LastName,
//             Age = personDto.Age!.Value,
//             CreatedBy = currUser, 
//             UpdatedBy = currUser, 
//             CreatedAt = DateTime.UtcNow,
//             UpdatedAt = DateTime.UtcNow
//         };
//
//         foreach (var addr in personDto.Addresses)
//         {
//             var address = new Address
//             {
//                 StreetAddress = addr,
//                 City = addr,
//                 State = addr,
//                 ZipCode = "98052",
//                 CreatedBy = currUser, 
//                 UpdatedBy = currUser, 
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow
//             };
//             
//             person.Addresses.Add(address);
//         }
//
//         foreach (var emailAddress in personDto.EmailAddresses)
//         {
//             var email = new Email
//             {
//                 Address = emailAddress,
//                 CreatedBy = currUser, 
//                 UpdatedBy = currUser, 
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow
//             };
//             
//             person.EmailAddresses.Add(email);
//         }
//
//         var createdPerson = await _personAccessor.Add(person);
//
//         return _mapper.Map<PersonDto>(createdPerson);
//     }
//
//     public async Task<PersonDto> UpdatePerson(PersonDto personDto)
//     {
//         await _personDtoValidator.ValidateAsync(personDto);
//         
//         var currUser = ServiceRuntimeContext.CurrentUserClaims?.Identity?.Name ?? "defaultuser@admin.com";
//         
//         var includePropNames = new[] { nameof(Person.Addresses), nameof(Person.EmailAddresses) };
//         var existingPerson = await _personAccessor.GetById(personDto.Id, includePropNames);
//
//         Exception<InvalidOperationException>.ThrowOn(() => existingPerson == null, "Cannot update person because unable find existing person entry in database.");
//
//         existingPerson!.Id = personDto.Id;
//         existingPerson.FirstName = string.IsNullOrWhiteSpace(personDto.FirstName)
//             ? existingPerson.FirstName
//             : personDto.FirstName;
//         existingPerson.LastName =
//             string.IsNullOrWhiteSpace(personDto.LastName) ? existingPerson.LastName : personDto.LastName;
//         existingPerson.Age = personDto.Age ?? existingPerson.Age;
//         existingPerson.UpdatedAt = DateTime.UtcNow;
//         existingPerson.UpdatedBy = currUser;
//         existingPerson.Addresses = existingPerson.Addresses;
//         existingPerson.EmailAddresses = existingPerson.EmailAddresses;
//         
//         if (personDto.Addresses.Any())
//         {
//             foreach (var address in personDto.Addresses)
//             {
//                 if (!existingPerson.Addresses.Select(e => e.StreetAddress).Contains(address))
//                 {
//                     existingPerson.Addresses.Add(new Address
//                     {
//                         StreetAddress = address,
//                         City = address,
//                         State = address,
//                         ZipCode = "98052",
//                         CreatedBy = currUser, 
//                         UpdatedBy = currUser, 
//                         CreatedAt = DateTime.UtcNow,
//                         UpdatedAt = DateTime.UtcNow
//                     });
//                 }
//             }
//         }
//         
//         if (personDto.EmailAddresses.Any())
//         {
//             foreach (var email in personDto.EmailAddresses)
//             {
//                 if (!existingPerson.EmailAddresses.Select(e => e.Address).Contains(email))
//                 {
//                     existingPerson.EmailAddresses.Add(new Email
//                     {
//                         Address = email,
//                         CreatedBy = currUser, 
//                         UpdatedBy = currUser, 
//                         CreatedAt = DateTime.UtcNow,
//                         UpdatedAt = DateTime.UtcNow
//                     });
//                 }
//             }
//         }
//
//         var updatedPerson = await _personAccessor.Update(existingPerson);
//
//         return _mapper.Map<PersonDto>(updatedPerson);
//     }
//
//     public async Task DeletePerson(int id)
//     {
//         // Cascading Delete
//         var includePropNames = new[] { nameof(Person.Addresses), nameof(Person.EmailAddresses) };
//         var existing = await _personAccessor.GetById(id, includePropNames);
//         
//         Exception<InvalidOperationException>.ThrowOn(() => existing == null, $"Cannot delete Person with Id:{id} because the entity not found");
//
//         await _personAccessor.Delete(existing!);
//     }
// }