using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public User? AssignedTo { get; set; }
        
        public string? Description { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public State State { get; set; }

        public DateTime StateUpdated { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
