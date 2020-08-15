using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    [Table("Addresses")]
    public class Address
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(20), Required]
        public string City { get; set; }

        [StringLength(300), Required]
        public string OpenAddress { get; set; }

        //1 1 ilişki (1 adresin 1 kşisi olabilir) ve virtual olmak zorunda
        public virtual Person Person { get; set; }
    }
}