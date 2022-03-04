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
    public class UkolyController : UlController
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
            
            Hra0 hra = NacistHru0();
            string blaboly = hra.Ul0.PridatUkol(dataUkolu, hra.Datum.CisloMesice);

            if (blaboly != "přisně tajný string")
                return Json(blaboly);

            Debug.WriteLine(JsonSerializer.Serialize(hra.Ul0.SeznamUkolu));
            UlozitHru0(hra);
            
            return Json(hra.Ul0.SeznamUkolu);
        }

        [HttpPost]
        public IActionResult Zrusit([FromBody] int idUkolu)
        {
            Hra0 hra = NacistHru0();
            hra.Ul0.SmazatUkol(idUkolu);
            UlozitHru0(hra);

            return Json(hra.Ul0.SeznamUkolu);
        }
    }
}
