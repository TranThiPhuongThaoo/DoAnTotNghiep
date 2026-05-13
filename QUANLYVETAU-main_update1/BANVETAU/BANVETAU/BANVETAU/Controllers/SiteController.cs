using BANVETAU.Common;
using BANVETAU.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BANVETAU.Controllers
{
    public class SiteController : Controller
    {
        QUANLYBANVETAU29Entities db = new QUANLYBANVETAU29Entities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]        
        public ActionResult flightSearch(FormCollection fc, int? page)
        {
            
            string typeTicket = fc["typeticket"];
            if (page == null) 
            { 
                page = 1; 
            }
            int pageSize = 4;
          
            int songuoi1 = int.Parse(fc["songuoi1"]);
            int songuoi2 = int.Parse(fc["songuoi2"]);
            int songuoi3 = int.Parse(fc["songuoi3"]);
            int tong = songuoi1 + songuoi2 + songuoi3;
            int songuoi = tong;
            ViewBag.songuoi = songuoi;
            string noidi = fc["departure_address"];
            string noiVe = fc["arrival_address"];
            string ngaydi = fc["departure_date"];
           
            ViewBag.url = "chuyen-tau";
            //convert sang mm/dd/yy cho may hieu 
            DateTime ngaydi1 = DateTime.ParseExact(ngaydi, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //sang mm/dd/yy
            string ngaydi2 = ngaydi1.ToString("MM-dd-yyyy");
            DateTime ngaydi3 = DateTime.Parse(ngaydi2);
            ViewBag.noidi = noidi;
            ViewBag.noiVe = noiVe;
            ViewBag.ngaydi = ngaydi;
           
            // neu la ve 2 chieu
            if (typeTicket.Equals("enable"))
            {
                string ngayve = fc["arrival_date"];
                DateTime ngayden1 = DateTime.ParseExact(ngayve, "d/M/yyyy", CultureInfo.InvariantCulture);
                string ngayden2 = ngayden1.ToString("MM-dd-yyyy");
                DateTime ngayden3 = DateTime.Parse(ngayden2);
                ViewBag.ngayden = ngayve;
                ViewBag.date = ngayden3;

                if (ngaydi1 > ngayden1)
                {
                    Message.set_flash("Return date must be greater than or equal to departure date!", "danger");
                    return Redirect("~/Home/Index");
                }
                var list = db.tickets.Where(m => m.city.cityName.Contains(noidi) && m.city1.cityName.Contains(noiVe)).
             Where(m => m.departure_date == ngaydi3).Where(m => m.status == 1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchReturn", list.ToPagedList(pageNumber, pageSize));
                    
                
            }
            else
            {

                //ve 1 chieu
                var list = db.tickets.Where(m => m.city.cityName.Contains(noidi) && m.city1.cityName.Contains(noiVe)).
             Where(m => m.departure_date == ngaydi3).Where(m=>m.status==1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchOnway", list.ToPagedList(pageNumber, pageSize));
            
            }

        }
        
        public ActionResult return_ticket(DateTime date,string noidi, string noiden)
        {
           
            var list = db.tickets.Where(m => m.city.cityName.Contains(noiden) && m.city1.cityName.Contains(noidi)).
               Where(m => m.departure_date == date).Where(m => m.status == 1).ToList();
            return View("_returnTicket", list);
        }
        public ActionResult AllChuyenTau(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            var singleC = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).First();
            ViewBag.url = "all-chuyen-tau";
            int pageNumber = (page ?? 1);
            //không biết sử dụng

            var list_flight = db.tickets.Where(m => m.status == 1).ToList();
            return View("allflight", list_flight.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult postOftoPic(int? page, string slug)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            var singleC = db.topics.Where(m => m.status == 1 && m.slug == slug).Where(m => m.status == 1).First();
            ViewBag.nameTopic = slug;
            ViewBag.url = "tin-tuc/" + slug + "";
            int pageNumber = (page ?? 1);
            var listPost = db.posts.Where(m => m.status == 1 && m.topid == singleC.ID).OrderByDescending(m => m.ID).ToList();
            return View("postOftoPic", listPost.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult topic()
        {

            var list = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).ToList();
            return View("_topic", list);
        }

        public ActionResult postSearch(string keyw, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.url = "tim-kiem-bai-viet?keyw=" + keyw + "";
            @ViewBag.nameTopic = "Tim kiếm từ khóa: " + keyw;
            var list = db.posts.Where(m => m.title.Contains(keyw) || m.detail.Contains(keyw)).Where(m => m.status == 1).OrderBy(m => m.ID);
            return View("postOftoPic", list.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PostDetal(string slug)
        {

            var single = db.posts.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.Bra = single.title;
            return View("PostDetal", single);
        }
        
            public ActionResult flightDetail(int id)
        {
            var single = db.tickets.Where(m => m.status == 1 && m.id == id).First();
            return View("flightDetail", single);
        }
        public ActionResult lienHe()
        {
            var single = db.posts.Where(m => m.status == 1 && m.slug.Equals("Liên Hê")).First();
            return View("PostDetal", single);
        }

    }
}