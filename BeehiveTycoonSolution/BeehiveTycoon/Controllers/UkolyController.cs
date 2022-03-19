﻿using Microsoft.AspNetCore.Mvc;
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

            Hra hra = NacistHru();

            if (dataUkolu.Id <= 0 || dataUkolu.Id > 6 || dataUkolu.CisloUlu < 0 || dataUkolu.CisloUlu >= hra.Uly.Count)
                return Json("Něco se pokazilo... :(");

            if (dataUkolu.Id == 6)
            {
                if (dataUkolu.Hodnota <= 0 || dataUkolu.Hodnota > 5)
                    return Json("Rozbili jste to.");

                foreach(Ul ul in hra.Uly)
                    if(ul.Lokace.Id == dataUkolu.Hodnota)
                        return Json("Do této lokace se znovu vyrojit nemůžete");
            }

            if (dataUkolu.Hodnota <= 0 && (dataUkolu.Id == 1 || dataUkolu.Id == 2 || dataUkolu.Id == 3 || dataUkolu.Id == 4))
                return Json("Prosím zadejde kladné číslo");
            
            string blaboly = hra.Uly[dataUkolu.CisloUlu].PridatUkol(dataUkolu, hra.Datum.CisloMesice);

            if (blaboly != "přisně tajný string")
                return Json(blaboly);

            Debug.WriteLine(JsonSerializer.Serialize(hra.Uly[dataUkolu.CisloUlu].SeznamUkolu));
            UlozitHru(hra);
            
            return Json(hra.Uly[dataUkolu.CisloUlu].SeznamUkolu);
        }

        [HttpPost]
        public IActionResult Zrusit([FromBody] DataUkolu dataUkolu)
        {
            if (dataUkolu == null)
                return Json("Něco se pokazilo... :(");

            Hra hra = NacistHru();
            hra.Uly[dataUkolu.CisloUlu].SmazatUkol(dataUkolu.Id);
            UlozitHru(hra);

            return Json(hra.Uly[dataUkolu.CisloUlu].SeznamUkolu);
        }
    }
}
