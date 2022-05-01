using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;
using Microsoft.AspNetCore.Http;

namespace BeehiveTycoon.Controllers
{
    public class UvodController: Controller
    {
        [HttpGet]
        public IActionResult Hrat()
        {
            ViewData["uzivatel"] = HttpContext.Session.GetString("JmenoUzivatele");

            return View();
        }

        [HttpGet]
        public IActionResult Chyba()
        {
            return View();
        }
    }
}
