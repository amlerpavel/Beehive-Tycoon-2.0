using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Db;

namespace BeehiveTycoon.Controllers
{
    public class ZebricekController : Controller
    {
        private readonly DbDohraneHry _dbDohraneHry;

        public ZebricekController(Data.BeehiveTycoonContex contex)
        {
            _dbDohraneHry = new(contex);
        }

        [HttpGet]
        public IActionResult Zobrazit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Obtiznost([FromBody] int idObtiznosti)
        {
            if (idObtiznosti <= 0 || idObtiznosti > 3)
                return Json("Něco  se pokazilo. :(");

            List<MDokoncenaHra> mDokonceneHry = _dbDohraneHry.ZiskatDohraneHry(idObtiznosti);

            return Json(mDokonceneHry);
        }
    }
}
