using EFCoreApi.DTOs;

namespace EFCoreApi.Domain;

public interface IPeopleManager
{
    Task<PersonDto> GetPersonById(int id);
    Task<IEnumerable<PersonDto>> ListAllPeople();
    Task<IEnumerable<PersonDto>> ListAllPeople(int pageNum, int pageSize);
    Task<PersonDto> AddPerson(PersonDto personDto);
    Task<PersonDto> UpdatePerson(PersonDto personDto);
    Task DeletePerson(int id);
}