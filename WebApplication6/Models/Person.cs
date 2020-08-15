using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    [Table("Persons")]
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //primary key, otomatik artan
        public int Id { get; set; }

        [StringLength(25),Required]
        public string FirstName { get; set; }

        [StringLength(25),Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        //1 e çok ilişki (1 kişinin 1 den çok adresi olabilir) ve virtual olmak zorunda
        public virtual List<Address> Address { get; set; }
    }
}