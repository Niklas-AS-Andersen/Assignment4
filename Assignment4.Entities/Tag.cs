using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Assignment4.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
