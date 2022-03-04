using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;
using BeehiveTycoon.Models.Game;
using System.Diagnostics;
using System.Text.Json;

namespace BeehiveTycoon.Controllers
{
    public class UkolyController : HraController
    {
        [HttpPost]
        public IActionResult Pridat([FromBody] DataUkolu dataUkolu)
        {
            if (dataUkolu == null)
                return Json("Zadejte smysluplné hodnoty");

            if(dataUkolu.Id <= 0 || dataUkolu.Id > 6)
                return Json("Něco se pokazilo... :(");

            if (dataUkolu.Hodnota <= 0 && (dataUkolu.Id == 1 || dataUkolu.Id == 2 || dataUkolu.Id == 3 || dataUkolu.Id == 4))
                return Json("Prosím zadejde kladné číslo");
            
            Hra hra = NacistHru();
            string blaboly = hra.Ul.PridatUkol(dataUkolu, hra.Datum.CisloMesice);

            if (blaboly != "přisně tajný string")
                return Json(blaboly);

            Debug.WriteLine(JsonSerializer.Serialize(hra.Ul.SeznamUkolu));
            UlozitHru(hra);
            
            return Json(hra.Ul.SeznamUkolu);
        }

        [HttpPost]
        public IActionResult Zrusit([FromBody] int idUkolu)
        {
            Hra hra = NacistHru();
            hra.Ul.SmazatUkol(idUkolu);
            UlozitHru(hra);

            return Json(hra.Ul.SeznamUkolu);
        }
    }
}
