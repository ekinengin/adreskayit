using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.BusinessManager
{
    public class DatabaseContext:DbContext
    {
        public DbSet<Person> person { get; set; }
        public DbSet<Address> address { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new CreateDb());    //en son bu alttaki db oluşturucunun çalışması için ctora eklenmesi lazım bunun.
        }
    }

    //Şimdi normalde ef ile çalışırken bu zamana kadar bir db oluşturulup, sonra bağlantı veya ef ayarları yaplırdı.. Burada ise bu yukarıdaki ilişkilendirmenin altına yazdığımız kodda, daha önceden bir db oluşturmadığımızı, bu db yi propgram çakışırken oluşturacağını söyledik

    public class CreateDb : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            Person persons = new Person();
            persons.FirstName = "Ekin";
            persons.LastName = "Candaş";
            persons.Age = 27;

            context.person.Add(persons);
            context.SaveChanges();

            List<Person> personList = context.person.ToList();    //liste haline getir

            foreach (Person person in personList)
            {
                Address addresses = new Address();
                addresses.City = "İstanbul";
                addresses.OpenAddress = "Kadikoy";
                addresses.Person = person;  //Burası Çok Çok önemli!!!!
                
                context.address.Add(addresses);
            }
                        
            context.SaveChanges();
        }
    }
}