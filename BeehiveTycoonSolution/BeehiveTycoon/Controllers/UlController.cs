using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using BeehiveTycoon.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BeehiveTycoon.Controllers
{
    public class UlController : Controller
    {
        /*
        public IActionResult Plastev()
        {
            Hra hra = NacistHru();

            if (hra == null)
            {
                hra = AktualizovatHru(hra);
                UlozitHru(hra);
            }
            else if (hra.Ul.Vcelstvo <= 0)
                return Redirect(Url.Action("Prohra", "Uvod"));
            else if (hra.Vyhra == true)
                return Redirect(Url.Action("Vyhra", "Uvod"));

            return View(hra);
        }

        public IActionResult DalsiKolo()
        {
            Hra hra = NacistHru();
            
            if (hra.Ul.Vcelstvo <= 0)
                return Redirect(Url.Action("Prohra", "Uvod"));
            else if(hra.Vyhra == true)
                return Redirect(Url.Action("Vyhra", "Uvod"));

            Hra novaHra = AktualizovatHru(hra);
            UlozitHru(novaHra);

            return RedirectToAction("Plastev");
        }

        private static Hra AktualizovatHru(Hra hra)
        {
            Hra novaHra;

            if (hra == null)
            {
                List<GeneraceVcelstva> generaceVcelstev = new();
                List<Plastev> plastve = new();
                generaceVcelstev.Add(VytvoritNovouGeneraci(400, 0));
                generaceVcelstev.Add(VytvoritNovouGeneraci(300, 3));
                plastve.Add(VytvoritPlastev(1000, 0));

                novaHra = new()
                {
                    GeneraceVcelstev = generaceVcelstev,
                    Ul = VytvoritUl(SecistVcely(generaceVcelstev), SecistMed(plastve), "Březen"),
                    Nepritel = new(),
                    Plastve = plastve,
                    SeznamUkolu = new()
                };
            }
            else
            {
                List<GeneraceVcelstva> generaceVcelstev = ZestarnutiVcelstva(hra.GeneraceVcelstev);
                List<Plastev> plastve = hra.Plastve;
                List<Ukol> ukoly = new();
                int med = hra.Ul.Med;
                Nepritel nepritel = hra.Nepritel;
                string novyMesic = ZmenaData(hra.Ul.Mesic);
                bool nepritelVytvoren = false;
                bool vyhra = false;

                // ukoly
                hra.SeznamUkolu.Sort((x, y) => x.IdUkolu.CompareTo(y.IdUkolu));
                foreach (Ukol ukol in hra.SeznamUkolu)
                {
                    if (ukol.IdUkolu == 0)
                    {
                        string[] mesice0 = { "Leden", "Únor", "Listopad", "Prosinec" };
                        string[] mesice1 = { "Květen", "Červen", "Červenec" };
                        string[] mesice2 = { "Březen", "Duben", "Srpen", "Září" };

                        if(mesice0.Contains(hra.Ul.Mesic))
                        {
                            med += ukol.PocetVcel * 0;
                        }
                        else if (mesice1.Contains(hra.Ul.Mesic))
                        {
                            med += ukol.PocetVcel * 8;
                        }
                        else if (mesice2.Contains(hra.Ul.Mesic))
                        {
                            med += ukol.PocetVcel * 3;
                        }
                        else if (hra.Ul.Mesic == "Říjen")
                        {
                            med += ukol.PocetVcel * 2;
                        }
                    }
                    else if (ukol.IdUkolu == 1)
                    {
                        generaceVcelstev.Add(VytvoritNovouGeneraci(ukol.PocetVajicek, 0));
                        med -= ukol.PocetMedu;
                    }
                    else if(ukol.IdUkolu == 2)
                    {
                        for (int i = 0; i < ukol.PocetPlastvi; i++)
                        {
                            plastve.Add(VytvoritPlastev(0, 0));
                        }
                        med -= ukol.PocetMedu;
                    }
                    else if(ukol.IdUkolu == 3)
                    {
                        nepritel = VytvoritNepritele(ukol.PocetVcel, novyMesic, hra.Plastve.Count, hra.Nepritel, hra.Ul.Vcelstvo);
                        nepritelVytvoren = true;
                        Random nahodneCislo = new();

                        if (nepritel.IdNepritele == 0)
                        {
                            //larvy
                            if(nepritel.DalsiUtok == -1)
                            {
                                nepritel.PocetNepratel = 0;
                            }
                        }
                        else if (nepritel.IdNepritele == 1 || nepritel.IdNepritele == 2)
                        {
                            //hlodavci
                            int vcely = ukol.PocetVcel;
                            
                            while (nepritel.HPNepritele > 0 && vcely > 0)
                            {
                                nepritel.HPNepritele -= 5; //damage 2
                                vcely -= 1;
                            }

                            generaceVcelstev = OdstranenitVcel(generaceVcelstev, ukol.PocetVcel - vcely, false);
                            if (nepritel.HPNepritele <= 0)
                                nepritel.PocetNepratel = 0;
                        }
                        else if (nepritel.IdNepritele == 3)
                        {
                            //vosy
                            int vcely = ukol.PocetVcel;
                            int vosy = nepritel.PocetNepratel;
                            int hpVos = vosy * 200;
                            int hpVcel = vcely * 100;

                            while (vosy > 0 && vcely > 0)
                            {
                                hpVos -= vcely * 2; //damage 2
                                hpVcel -= vosy * 50; //damage 50
                                vosy = hpVos / 200;
                                vcely = hpVcel / 100;
                            }

                            generaceVcelstev = OdstranenitVcel(generaceVcelstev, ukol.PocetVcel - vcely, false);
                            if (vosy <= 0)
                                nepritel.PocetNepratel = 0;
                            else
                            {
                                nepritel.PocetNepratel = vosy;
                                nepritel.DalsiUtok -= 1;
                            }
                        }
                        else
                        {
                            //mravenci
                            int vcely = ukol.PocetVcel;
                            int hpVcel = vcely * 100;
                            int mravenci = nepritel.PocetNepratel;
                            int hpMravenci = mravenci * 60;

                            while (mravenci > 0 && vcely > 0)
                            {
                                hpMravenci -= vcely * 30;
                                hpVcel -= mravenci * 20;
                                mravenci = hpMravenci / 60;
                                vcely = hpVcel / 100;
                            }

                            generaceVcelstev = OdstranenitVcel(generaceVcelstev, ukol.PocetVcel - vcely, false);
                            if (mravenci <= 0)
                                nepritel.PocetNepratel = 0;
                            else
                            {
                                nepritel.PocetNepratel = mravenci;
                                nepritel.DalsiUtok -= 1;
                            }
                        }
                    }
                    else if (ukol.IdUkolu == 4)
                    {
                        if (hra.Nepritel.JmenoNepritele != nepritel.JmenoNepritele || nepritel.JmenoNepritele == null)
                        {
                            nepritel = new();
                            nepritelVytvoren = true;
                        }

                        if (ukol.Platnost >= -2)
                        {
                            Ukol ukolN = ukol;
                            ukolN.Platnost -= 1;
                            ukoly.Add(ukolN);
                        }
                    }
                    else if (ukol.IdUkolu == 5)
                    {
                        vyhra = true;
                    }
                }

                if(nepritelVytvoren == false)
                {
                    nepritel = VytvoritNepritele(0, novyMesic, hra.Plastve.Count, hra.Nepritel, hra.Ul.Vcelstvo);
                    if (nepritel.IdNepritele == 3 || nepritel.IdNepritele == 4)
                        nepritel.DalsiUtok -= 1;
                }

                
                Debug.WriteLine(nepritel.JmenoNepritele);
                Debug.WriteLine(nepritel.PocetNepratel);
                //
                if (nepritel.IdNepritele == 4)
                {
                    
                }
                else
                {
                    Debug.WriteLine("smazano");
                    nepritel = new();
                }
                //
                // odebirani vcel, medu, plastvi po vniknuti nepratel do ulu
                if (nepritel.IdNepritele == 0)
                {
                    for(int i = 0; i < hra.Nepritel.PocetNepratel / 2 && plastve.Count > 0 && nepritel.DalsiUtok == -1; i++)
                    {
                        plastve.RemoveAt(0);
                    }
                    if (plastve.Count == 0)
                        nepritel = new();
                }
                else if (nepritel.PocetNepratel != 0)
                {
                    if (nepritel.IdNepritele == 1)
                    {
                        generaceVcelstev = OdstranenitVcel(generaceVcelstev, 100, false);
                    }
                    else if (nepritel.IdNepritele == 2)
                    {
                        if (plastve.Count != 0 && med >= 1000 && nepritel.DalsiUtok >= 2)
                        {
                            Debug.WriteLine("ddd");
                            plastve.RemoveAt(0);
                            med -= 1000;
                            nepritel.DalsiUtok = 1;
                        }
                        else
                            nepritel.DalsiUtok += 1;

                    }
                    else if (nepritel.IdNepritele == 3)
                    {
                        generaceVcelstev = OdstranenitVcel(generaceVcelstev, nepritel.PocetNepratel, true);
                        med -= nepritel.PocetNepratel / 2;
                    }
                    else
                    {
                        med -= nepritel.PocetNepratel * 2;
                    }
                }

                //vcelar
                //starnuti + konzmace med
                int pocetVcel = SecistVcely(generaceVcelstev);
                med = ZmenaPoctuMedu(pocetVcel, med);

                if (med < 0)
                {
                    generaceVcelstev = ZestarnutiVcelstva(generaceVcelstev, hra.Ul.Med);
                    pocetVcel = SecistVcely(generaceVcelstev);
                    med = 0;
                }
                
                plastve = UlozeniMeduNaPlastve(plastve.Count, med);

                novaHra = new()
                {
                    GeneraceVcelstev = generaceVcelstev,
                    Ul = VytvoritUl(pocetVcel, SecistMed(plastve), novyMesic),
                    Nepritel = nepritel,
                    Plastve = plastve,
                    SeznamUkolu = ukoly,
                    Vyhra = vyhra
                };
            }

            return novaHra;
        }

        private static List<GeneraceVcelstva> ZestarnutiVcelstva(List<GeneraceVcelstva> generaceVcelstev)
        {
            List<GeneraceVcelstva> aktualizaceGeneraceVcelstev = new();

            foreach (GeneraceVcelstva generace in generaceVcelstev)
            {
                GeneraceVcelstva Aktualizacegenerace = generace;
                Aktualizacegenerace.Vek += 1;

                if (Aktualizacegenerace.Vek <= 6)
                    aktualizaceGeneraceVcelstev.Add(Aktualizacegenerace);
            }

            return aktualizaceGeneraceVcelstev;
        }
        private static List<GeneraceVcelstva> ZestarnutiVcelstva(List<GeneraceVcelstva> generaceVcelstev, int med)
        {
            List<GeneraceVcelstva> aktualizaceGeneraceVcelstev = new();
            generaceVcelstev.Sort((x, y) => x.Vek.CompareTo(y.Vek));
            int novyMed;

            foreach (GeneraceVcelstva generace in generaceVcelstev)
            {
                GeneraceVcelstva Aktualizacegenerace = generace;
                novyMed = ZmenaPoctuMedu(generace.Pocet, med);
                med = novyMed;

                if (novyMed < 0)
                    Aktualizacegenerace.Vek += 3;

                if (Aktualizacegenerace.Vek <= 6)
                    aktualizaceGeneraceVcelstev.Add(Aktualizacegenerace);
            }

            return aktualizaceGeneraceVcelstev;
        }
        private static List<GeneraceVcelstva> OdstranenitVcel(List<GeneraceVcelstva> generaceVcelstev, int pocetVcel, bool mladouXstarou)
        {
            List<GeneraceVcelstva> aktualizaceGeneraceVcelstev = new();
            if(mladouXstarou == false)
                generaceVcelstev.Sort((x, y) => y.Vek.CompareTo(x.Vek));
            else
                generaceVcelstev.Sort((x, y) => x.Vek.CompareTo(y.Vek));

            foreach (GeneraceVcelstva generace in generaceVcelstev)
            {
                GeneraceVcelstva Aktualizacegenerace = generace;
                Aktualizacegenerace.Pocet -= pocetVcel;

                if (Aktualizacegenerace.Pocet > 0)
                {
                    aktualizaceGeneraceVcelstev.Add(Aktualizacegenerace);
                    pocetVcel = 0;
                }
                else
                    pocetVcel = Aktualizacegenerace.Pocet * (-1);
            }

            return aktualizaceGeneraceVcelstev;
        }
        private static List<Plastev> UlozeniMeduNaPlastve(int pocetPlastvi, int med)
        {
            List<Plastev> plastve = new();

            for (int i = 0; i < pocetPlastvi; i++)
            {
                if (med >= 1000)
                {
                    plastve.Add(VytvoritPlastev(1000, 0));
                    med -= 1000;
                }
                else
                {
                    plastve.Add(VytvoritPlastev(med, 0));
                    med = 0;
                }
            }

            return plastve;
        }

        private static int SecistVcely(List<GeneraceVcelstva> GeneraceVcelstev)
        {
            int PocetVcel = 0;

            foreach (GeneraceVcelstva generaceVcelstva in GeneraceVcelstev)
            {
                PocetVcel += generaceVcelstva.Pocet;
            }

            return PocetVcel;
        }
        private static int SecistMed(List<Plastev> plastve)
        {
            int pocetMedu = 0;

            foreach (Plastev plastev in plastve)
            {
                pocetMedu += plastev.Med;
            }

            return pocetMedu;
        }
        private static int ZmenaPoctuMedu(int pocetVcel, int staryPocetMedu)
        {
            int med = staryPocetMedu;

            med -= pocetVcel;

            return med;
        }

        private static string ZmenaData(string aktualniMesic)
        {
            string[] mesice = { "Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen", "Listopad", "Prosinec" };
            int cisloMesice = Array.IndexOf(mesice, aktualniMesic);

            if (cisloMesice == 11)
                cisloMesice = 0;
            else
                cisloMesice += 1;

            string novyMesic = mesice[cisloMesice];

            return novyMesic;
        }

        private static GeneraceVcelstva VytvoritNovouGeneraci(int pocet, int vek)
        {
            GeneraceVcelstva novaGenerace = new()
            {
                Pocet = pocet,
                Vek = vek
            };

            return novaGenerace;
        }
        private static Ul VytvoritUl(int vcelstvo, int med, string mesic)
        {
            Ul ul = new()
            {
                Vcelstvo = vcelstvo,
                Med = med,
                Mesic = mesic
            };

            return ul;
        }
        private static Plastev VytvoritPlastev(int med, int vajicka)
        {
            Plastev plastev = new()
            {
                Med = med,
                Vajicka = vajicka
            };

            return plastev;
        }
        private static Nepritel VytvoritNepritele(int pocetVcel, string mesic, int pocetPlastvi, Nepritel staryNepritel, int celkemVcel)
        {
            Random nahodneCislo = new();
            Nepritel nepritel = new();

            if (staryNepritel.JmenoNepritele == null)
            {
                int cislo = nahodneCislo.Next(0, 100);
                int sance;

                if (pocetVcel == 0)
                    sance = 100;
                else
                    sance = 101 - (pocetVcel / pocetPlastvi) / 20 * 100;

                if (cislo <= sance)
                {
                    List<string> jmenaNepratel = new();
                    List<int> idNepratel = new();

                    string[] mesiceHlodavc = { "Leden", "Únor", "Prosinec" };
                    string[] mesiceVosy = { "Srpen", "Září", "Říjen" };
                    string[] mesiceMravene = { "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen" };

                    jmenaNepratel.Add("Zavíječ");
                    idNepratel.Add(0);
                    if (mesiceHlodavc.Contains(mesic))
                    {
                        jmenaNepratel.Add("Rejsek");
                        idNepratel.Add(1);
                        jmenaNepratel.Add("Myš");
                        idNepratel.Add(2);
                    }
                    if (mesiceVosy.Contains(mesic))
                    {
                        jmenaNepratel.Add("Vosy");
                        idNepratel.Add(3);
                    }
                    if (mesiceMravene.Contains(mesic))
                    {
                        jmenaNepratel.Add("Mravenci");
                        idNepratel.Add(4);
                    }
                    int i = nahodneCislo.Next(0, jmenaNepratel.Count);
                    int pocetNepratel;
                    int utok;
                    int hp = 0;

                    if (idNepratel[i] == 0)
                    {
                        pocetNepratel = nahodneCislo.Next(2, pocetPlastvi / 10 + 2);
                        utok = 0; //predstavit jako vek
                    }
                    else if (idNepratel[i] == 1 || idNepratel[i] == 2)
                    {
                        pocetNepratel = 1;
                        hp = nahodneCislo.Next(500, 1000);
                        utok = 2;
                    }
                    else if (idNepratel[i] == 3)
                    {
                        pocetNepratel = nahodneCislo.Next(6, (pocetVcel / 100) * 5 + pocetVcel + 8);
                        utok = 1;
                    }
                    else
                    {
                        pocetNepratel = nahodneCislo.Next(10, (pocetVcel / 100) * 20 + pocetVcel + 20);
                        utok = 1;
                    }

                    nepritel = new()
                    {
                        IdNepritele = idNepratel[i],
                        JmenoNepritele = jmenaNepratel[i],
                        PocetNepratel = pocetNepratel,
                        HPNepritele = hp,
                        DalsiUtok = utok
                    };
                }
            }
            else if (staryNepritel.PocetNepratel == 0)
            {
                nepritel = new();
            }
            else if(staryNepritel.DalsiUtok <= 0)
            {
                nepritel = staryNepritel;
                if (staryNepritel.DalsiUtok == 0)
                {
                    if (staryNepritel.IdNepritele == 3)
                        nepritel.PocetNepratel = nahodneCislo.Next(celkemVcel / 8, celkemVcel / 4);
                    else if (staryNepritel.IdNepritele == 4)
                        nepritel.PocetNepratel = nahodneCislo.Next(celkemVcel / 2, (celkemVcel / 100) * 20 + celkemVcel);
                    else if (staryNepritel.IdNepritele == 0)
                        nepritel.DalsiUtok = -1;
                }
                else
                {
                    if (nepritel.IdNepritele == 0)
                    {
                        nepritel.PocetNepratel += 2;
                        nepritel.DalsiUtok = 0;
                    }
                    else
                        nepritel.PocetNepratel += nahodneCislo.Next(4, 28);
                }
            }
            else
            {
                nepritel = staryNepritel;
            }
            
            return nepritel;
        }
        */
        public void UlozitHru(Hra hra)
        {
            HttpContext.Session.SetString("Hra", JsonSerializer.Serialize(hra));

            HttpContext.Response.Cookies.Append("Hra", JsonSerializer.Serialize(hra), new CookieOptions() { SameSite = SameSiteMode.Lax, HttpOnly = true });
        }
        public Hra NacistHru()
        {
            Hra hra;

            if (HttpContext.Session.GetString("Hra") == null)
            {
                if(HttpContext.Request.Cookies["Hra"] == null)
                    hra = null;
                else
                    hra = JsonSerializer.Deserialize<Hra>(HttpContext.Request.Cookies["Hra"]);
            }
            else
                hra = JsonSerializer.Deserialize<Hra>(HttpContext.Session.GetString("Hra"));

            return hra;
        }
        //
        public IActionResult Plastev0()
        {
            Hra0 hra = NacistHru0();

            if (hra == null)
            {
                hra = new();
                hra.NovaHra();
                UlozitHru0(hra);
            }

            return View(hra);
        }

        public IActionResult DalsiKolo()
        {
            Hra0 hra0 = NacistHru0();
            hra0.Dalsikolo();
            //Debug.WriteLine(JsonSerializer.Serialize(hra0));
            UlozitHru0(hra0);

            return RedirectToAction("Plastev0");
        }

        public void UlozitHru0(Hra0 hra)
        {
            HttpContext.Session.SetString("Hra", JsonSerializer.Serialize(hra));

            //HttpContext.Response.Cookies.Append("Hra", JsonSerializer.Serialize(hra), new CookieOptions() { SameSite = SameSiteMode.Lax, HttpOnly = true });
        }
        public Hra0 NacistHru0()
        {
            Hra0 hra;

            if (HttpContext.Session.GetString("Hra") == null)
            {
                if (HttpContext.Request.Cookies["Hra"] == null)
                    hra = null;
                else
                    hra = JsonSerializer.Deserialize<Hra0>(HttpContext.Request.Cookies["Hra"]);
            }
            else
                hra = JsonSerializer.Deserialize<Hra0>(HttpContext.Session.GetString("Hra"));

            return hra;
        }
    }
}
