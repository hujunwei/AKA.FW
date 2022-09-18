using EFCoreApi.Domain;
using EFCoreApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

// TODO: AuthN/AuthZ
namespace EFCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleManager _peopleManager;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(IPeopleManager peopleManager, ILogger<PeopleController> logger)
        {
            _peopleManager = peopleManager;
            _logger = logger;
        }

        // GET: api/People
        [HttpGet]
        public async Task<IEnumerable<PersonDto>> List([FromQuery] int? pageNum = null, [FromQuery] int? pageSize = null)
        {
            if (pageSize != null && pageNum != null)
            {
                return await _peopleManager.ListAllPeople(pageNum.Value, pageSize.Value);
            }
            
            return await _peopleManager.ListAllPeople();
        }

        // GET: api/People/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<PersonDto> Get(int id)
        {
            Exception<ArgumentException>.ThrowOn(() => id < 0, "Invalid Id.");
            return await _peopleManager.GetPersonById(id);
        }

        // POST: api/People
        [HttpPost]
        public async Task<PersonDto> Post([FromBody] PersonDto personDto)
        {
            Exception<ArgumentNullException>.ThrowOn(() => personDto == null, $"Person to be created cannot be null.");

            return await _peopleManager.AddPerson(personDto);
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }
        
        // PUT: api/People/5
        [HttpPatch("{id}")]
        public async Task<PersonDto> Update(int id, [FromBody] PersonDto personDto)
        {
            Exception<ArgumentNullException>.ThrowOn(() => personDto == null, $"Person to be created cannot be null.");

            personDto.Id = id;

            return await _peopleManager.UpdatePerson(personDto); 
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            Exception<ArgumentException>.ThrowOn(() => id < 0, "Invalid Id.");
            await _peopleManager.DeletePerson(id);
        }
    }
}
