using System.Text.Json;
using EFDataAccess;
using EFDataAccess.Model;

namespace EFCoreApi;

internal class SeedData
{
    private readonly EFCoreDemoContext _efCoreDemoContext;
    private readonly ILogger<SeedData> _logger;

    internal SeedData(EFCoreDemoContext efCoreDemoContext, ILogger<SeedData> logger)
    {
        _efCoreDemoContext = efCoreDemoContext;
        _logger = logger;
    }

    internal void LoadDataFromJsonIfNoData()
    {
        if (_efCoreDemoContext.People.Any())
        {
            _logger.LogInformation(1, "Database has data, skip seeding...");
            return;
        }

        var file = System.IO.File.ReadAllText("../EFDataAccess/generated.json");
        var people = JsonSerializer.Deserialize<List<Person>>(file);

        if (people == null)
        {
            _logger.LogError(1, "Cannot deserialize data or data is not found");
            return;
        }

        _efCoreDemoContext.AddRange(people);
        _efCoreDemoContext.SaveChanges();   
        
        _logger.LogInformation(1, "Data loaded to Database");
    }
}