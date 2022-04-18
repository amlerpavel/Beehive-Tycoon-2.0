using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;

namespace BeehiveTycoon.Controllers
{
    public class UvodController: Controller
    {
        [HttpGet]
        public IActionResult Hrat()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Chyba()
        {
            return View();
        }
    }
}
