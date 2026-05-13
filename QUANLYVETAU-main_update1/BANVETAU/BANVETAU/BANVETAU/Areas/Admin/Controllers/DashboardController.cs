using BANVETAU.Common;
using BANVETAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace BANVETAU.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private QUANLYBANVETAU29Entities db = new QUANLYBANVETAU29Entities();
        public DashboardController()
        {
            
        }
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var revenue = db.ordersdetails
                .Include(x => x.ticket)
                .Include(x => x.order)
                .Sum(x => x.quantity * x.ticket.price);
            ViewBag.Revenue = string.Format("{0:0,0}", revenue ?? 0);
            return View();
        }
        public ActionResult usersession()
        {
            var session = (Userlogin)Session[Common.CommonConstants.USER_SESSION];
            return View("_adminSession", session);
        }
        
    }
}