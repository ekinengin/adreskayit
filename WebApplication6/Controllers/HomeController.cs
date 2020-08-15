using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.BusinessManager;
using WebApplication6.Models;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {

        DatabaseContext db = new DatabaseContext();
        HomeViewModel model = new HomeViewModel();

        // GET: Home
        public ActionResult HomePage()
        {
            //List<Person> persons = db.person.ToList();  //Bu select ile db tetiklenecek. Artık gerek yok

            model.personModel = db.person.ToList();
            model.addressModel = db.address.ToList();
            
            return View(model);
        }

        //INSERT İŞLEMLERİ---------------------------------------------------------------------------
        //Yeni Kişi Ekleme get / post

        [HttpGet]
        public ActionResult NewPerson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPerson(Person person)
        {
            db.person.Add(person);

            int sonuc = db.SaveChanges();

            if (sonuc > 0)
            {
                ViewBag.result = "Kişi başarıyla eklendi!";
                ViewBag.status = "success";
            }
            else
            {
                ViewBag.result = "Kişi eklenemedi!";
                ViewBag.status = "error";
            }

            return View();
        }

        //Yeni Adres Ekleme get / post

        [HttpGet]
        public ActionResult NewAddress()
        {
            List<SelectListItem> List = new List<SelectListItem>();  //1) Burası sayfanın cshtml kısmındaki dropdlistin bizden isteği liste bunun içinde Person classı  tipinde bir liste oluşturacağız.

            List<Person> persons = db.person.ToList();  // 2) Person clası özelliğinde bir liste oluştur.
            foreach (Person person in persons)            //2) Person tipinde herbiri için dön
            {
                SelectListItem listItem = new SelectListItem(); //3) sli tipinde listemize
                listItem.Text = person.FirstName + " " + person.LastName;
                listItem.Value = person.Id.ToString(); 

                List.Add(listItem);
            }
            
            ViewBag.PersonList = List;  /*Tüm herşeyi viewbag e attık ddl te bunu çekeceğiz*/
            TempData["PersonList"] = ViewBag.PersonList;

            return View();
        }

        [HttpPost]
        public ActionResult NewAddress(Address address)
        {
            Person person = db.person.Where(x => x.Id == address.Person.Id).FirstOrDefault();

            if (person != null)
            {
                address.Person = person;

                db.address.Add(address);
                int sonuc = db.SaveChanges();

                if (sonuc > 0)
                {
                    ViewBag.result = "Adres başarıyla eklendi!";
                    ViewBag.status = "success";
                }
                else
                {
                    ViewBag.result = "Adres eklenemedi!";
                    ViewBag.status = "error";
                }

            }

            ViewBag.PersonList = TempData["PersonList"];

            return View();
        }

        //UPDATE İŞLEMLERİ--------------------------------------------------------------------------
        //Kişi düzenle
        [HttpGet]
        public ActionResult UpdatePerson(int? personid)
        {
            Person person = null;

            if (personid != null)
            {
                person = db.person.Where(x => x.Id == personid).FirstOrDefault();
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult UpdatePerson(Person model, int? personid) //dönecek
        {
            Person person = db.person.Where(x => x.Id == personid).FirstOrDefault();

            if (person != null)
            {
                person.FirstName = model.FirstName;
                person.LastName = model.LastName;
                person.Age = model.Age;

                int sonuc = db.SaveChanges();
                
                if (sonuc > 0)
                {
                    ViewBag.result = "Kişi başarıyla güncellendi!";
                    ViewBag.status = "success";
                }
                else
                {
                    ViewBag.result = "Kişi güncellenemedi!";
                    ViewBag.status = "error";
                }
            }

            return View();
        }

        //address düzenle

        [HttpGet]
        public ActionResult UpdateAddress(int? addressid)
        {
            Address address = null;

            if (addressid != null)
            {
                List<SelectListItem> List = new List<SelectListItem>();

                List<Person> persons = db.person.ToList();
                foreach (Person person in persons)
                {
                    SelectListItem listItem = new SelectListItem();
                    listItem.Text = person.FirstName;
                    listItem.Value = person.Id.ToString();

                    List.Add(listItem);
                }

                address = db.address.Where(x => x.Id == addressid).FirstOrDefault();

                ViewBag.PersonList = List;
                TempData["PersonList"] = ViewBag.PersonList;
            }  
            
            return View(address);
        }

        [HttpPost]
        public ActionResult UpdateAddress(Address model, int? addressid )
        {
           
            Person person = db.person.Where(x => x.Id == model.Person.Id).FirstOrDefault();
            Address address = db.address.Where(x => x.Id == addressid).FirstOrDefault();

            if (person != null)
            {
                address.Person = person;
                address.OpenAddress = model.OpenAddress;
                address.City = model.City;

                int sonuc = db.SaveChanges();

                if (sonuc > 0)
                {
                    ViewBag.result = "Adres başarıyla güncellendi!";
                    ViewBag.status = "success";
                }
                else
                {
                    ViewBag.result = "Adres güncellenemedi!";
                    ViewBag.status = "error";
                }
            }

            ViewBag.PersonList = TempData["PersonList"];

            return View();
        }

        //DELETE İŞLEMLERİ-------------------------------------------------------------------------
        //person delete
        [HttpGet]
        public ActionResult DeletePerson(int? personid)
        {
            Person person = null;

            if (personid != null)
            {
                person = db.person.Where(x => x.Id == personid).FirstOrDefault();
            }

            return View(person);
        }

        //aynı isimdeler aslında
        [HttpPost, ActionName("DeletePerson")]
        public ActionResult DeletePersonOk(int? personid)
        {

            List<Address> address = null;

            if (personid != null)
            {
                Person person = db.person.Where(x => x.Id == personid).FirstOrDefault();
                address = db.address.Where(y => y.Person.Id == personid).ToList();

                if (address != null)
                {
                    foreach (Address item in address)
                    {
                        db.address.Remove(item);
                    }
                }

                db.person.Remove(person);
                db.SaveChanges();
            }
            
            return RedirectToAction("HomePage");
        }

        //Address delete
        [HttpGet]
        public ActionResult DeleteAddress(int? addressid)
        {
            Address address = null;

            if (addressid != null)
            {
                address = db.address.Where(x => x.Id == addressid).FirstOrDefault();
            }

            return View(address);
        }

        [HttpPost, ActionName("DeleteAddress")]
        public ActionResult DeleteAddressOk(int? addressid)
        {
            if (addressid != null)
            {
                Address address = db.address.Where(x => x.Id == addressid).FirstOrDefault();

                db.address.Remove(address);
                db.SaveChanges();
            }

            return RedirectToAction("HomePage");
        }
    }
}