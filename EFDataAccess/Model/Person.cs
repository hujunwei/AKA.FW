using System.ComponentModel.DataAnnotations;
using EFDataAccess.Model.Common;

namespace EFDataAccess.Model
{
    public class Person : ChangeableEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = default!;

        [Required] 
        public int Age { get; set; } = default!;
        
        public List<Address> Addresses { get; set; } = new();
        public List<Email> EmailAddresses { get; set; } = new();
    }
}

