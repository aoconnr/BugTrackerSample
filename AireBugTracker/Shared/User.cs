using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AireBugTracker.Shared
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Name is Required")]
        public string? Name { get; set; }

        public ICollection<Bug> AssignedBugs { get; set; }
    }
}
