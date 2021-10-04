using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Assignment4.Entities
{
    public class User
    {
        public int Id { get; set; }

        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Key]
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
