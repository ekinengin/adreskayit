using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.ViewModels
{
    public class HomeViewModel
    {
        public List<Person> personModel { get; set; }
        public List<Address> addressModel { get; set; }
    }
}