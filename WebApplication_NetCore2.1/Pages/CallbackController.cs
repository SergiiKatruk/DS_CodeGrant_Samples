using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Pages
{
    [Route("Callback/[action]")]
    public class CallbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}