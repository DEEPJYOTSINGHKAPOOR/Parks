using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


// we can create different dto's for update, create , delete as per need.

namespace ParkyAPI.Models
{
    public class NationalParkDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        public string State { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Created { get; set; }

        public DateTime Established { get; set; }


    }
}
