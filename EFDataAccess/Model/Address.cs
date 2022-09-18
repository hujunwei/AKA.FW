using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFDataAccess.Model.Common;

namespace EFDataAccess.Model {
    public class Address : ChangeableEntity<int>
    {
        [Required]
        [MaxLength(200)]
        public string StreetAddress { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string State { get; set; } = default!;

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string ZipCode { get; set; } = default!;
    }
}

