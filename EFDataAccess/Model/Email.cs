using System.ComponentModel.DataAnnotations;
using EFDataAccess.Model.Common;

namespace EFDataAccess.Model
{
    public class Email : ChangeableEntity<int>
    {
        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = default!;
    }
}

