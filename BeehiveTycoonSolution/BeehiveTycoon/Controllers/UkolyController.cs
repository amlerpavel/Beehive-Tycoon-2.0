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

            Hra hra = NacistHru();

            if (hra.Vyhra == true || hra.Prohra == true)
                return Json("Blahopřejeme");

            Ul vybranyUl = hra.Uly.Where(u => u.Lokace.Id == dataUkolu.IdLokaceUlu).FirstOrDefault();

            if (dataUkolu.Id <= 0 || dataUkolu.Id > 6 || !hra.Uly.Contains(vybranyUl))
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

            int i = hra.Uly.IndexOf(vybranyUl);

            string blaboly = hra.Uly[i].PridatUkol(dataUkolu, hra.Datum.CisloMesice);

            if (blaboly != "přisně tajný string")
                return Json(blaboly);

            Debug.WriteLine(JsonSerializer.Serialize(hra.Uly[i].SeznamUkolu));
            UlozitHru(hra);
            
            return Json(hra.Uly[i].SeznamUkolu);
        }

        [HttpPost]
        public IActionResult Zrusit([FromBody] DataUkolu dataUkolu)
        {
            Hra hra = NacistHru();

            if (hra.Vyhra == true || hra.Prohra == true)
                return Json("Blahopřejeme");

            Ul vybranyUl = hra.Uly.Where(u => u.Lokace.Id == dataUkolu.IdLokaceUlu).FirstOrDefault();

            if (dataUkolu == null || vybranyUl == null)
                return Json("Něco se pokazilo... :(");

            int i = hra.Uly.IndexOf(vybranyUl);
            hra.Uly[i].SmazatUkol(dataUkolu.Id);
            UlozitHru(hra);

            return Json(hra.Uly[i].SeznamUkolu);
        }
    }
}
