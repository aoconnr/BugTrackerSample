using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AireBugTracker.Shared
{
    public class Bug
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MinLength(1, ErrorMessage = "Title is Required")]
        public string? Title { get; set; }
        [MinLength(1, ErrorMessage = "Description is Required")]
        public string? Description { get; set; }

        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        [ForeignKey("Users")]
        public User? AssignedUser { get; set; }
    }
}
