using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = (ClaimsIdentity)User.Identity;
            ViewBag.Name = user.Name;
            ViewBag.CanEdit = user.FindFirst("CanEdit") != null ? "true" : "false";
            return View();
        }
    }
}
