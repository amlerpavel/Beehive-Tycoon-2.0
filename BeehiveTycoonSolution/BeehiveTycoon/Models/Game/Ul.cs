using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeehiveTycoon.Models.Game
{
    public class Ul0
    {
        public int Vcelstvo { get; private set; }
        public int Med { get; private set; }
        public string Lokace { get; private set; }
        public List<GeneraceVcel> GeneraceVcelstva { get; private set; }
        public List<Plastev> Plastve { get; private set; }
        public List<Ukol> SeznamUkolu { get; private set; }
        public Nepritel Nepritel { get; private set; }
        public int KlidPoBitve { get; private set; }

        private struct DostupnyPocet
        {
            public int vcel;
            public int medu;
        }

        private readonly Random _nahodneCislo = new();
        private readonly int _zivotStrazce = 100;
        private int _strazci;

        // cisla mesicu, ve kterych je ...
        private readonly int[] _zima = { 1, 12 };
        private readonly int[] _neniPyl = { 11 };
        private readonly int[] _rojVcelstva = { 4, 5, 6 };
        private readonly int[] _zazimovaniUlu = { 11 };

        private readonly int[] _nejvicePylu = { 5 };
        private readonly int[] _sezonaPylu = { 4, 6, 7, 8 };
        private readonly int[] _menePylu = { 3, 9 };
        private readonly int[] _maloPylu = { 2, 10 };

        private readonly int[] _hlodavci = { 1, 11, 12 };
        private readonly int[] _vosy = { 6, 7, 8 };
        private readonly int[] _mravenci = { 4, 5, 6, 7, 8, 9 };
        private readonly int[] _zavijec = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        /*
        public Nepritel Nepritel { get; set; }
        public List<Ukol> SeznamUkolu { get; set; }
        */
        public Ul0(string lokace, List<GeneraceVcel> generaceVcelstva, List<Plastev> plastve, List<Ukol> seznamUkolu, Nepritel nepritel, int klidPoBitve)
        {
            Lokace = lokace;
            GeneraceVcelstva = generaceVcelstva;
            Plastve = plastve;
            SeznamUkolu = seznamUkolu;
            Nepritel = nepritel;
            KlidPoBitve = klidPoBitve;

            SecistMed();
            SecistVcely();
        }

        public void DalsiKolo(int cisloMesice)
        {
            SplnitUkoly(cisloMesice);
            ZestarnutiVcelstva();
            UlozitMedNaPlastve();
            SecistVcely();
            Debug.WriteLine("Před vytvořenim: " + Nepritel.Jmeno + " " + Nepritel.Pocet + " porazen: " + Nepritel.Porazen);
            VytvoritNepritele(VylosovatNepritele(cisloMesice));
            Debug.WriteLine("Vytvořen: " + Nepritel.Jmeno + " " + Nepritel.Pocet + " porazen: " + Nepritel.Porazen);
            BojSNepritelem();
            Debug.WriteLine("Po boji: " + Nepritel.Jmeno + " " + Nepritel.Pocet + " porazen: " + Nepritel.Porazen);
        }

        private void SecistVcely()
        {
            Vcelstvo = 0;

            foreach (GeneraceVcel generaceVcel in GeneraceVcelstva)
            {
                Vcelstvo += generaceVcel.Pocet;
            }

        }
        private void SecistMed()
        {
            Med = 0;

            foreach (Plastev plastev in Plastve)
            {
                Med += plastev.Med;
            }

        }
        private void ZestarnutiVcelstva()
        {
            GeneraceVcelstva.Sort((x, y) => x.Vek.CompareTo(y.Vek));
            
            foreach (GeneraceVcel generaceVcel in GeneraceVcelstva)
            {
                Med -= generaceVcel.Pocet;

                if(Med >= 0)
                    generaceVcel.Zestarnout(1);
                else
                {
                    double vek = (Med * (-3) + generaceVcel.Pocet + Med) / Convert.ToDouble(generaceVcel.Pocet);
                    generaceVcel.Zestarnout(Math.Round(vek, 0, MidpointRounding.AwayFromZero));
                    Med = 0;
                }
            }

            SmazatGeneraci(GeneraceVcelstva.Where(u => u.Vek >= 7).ToArray());

            GeneraceVcelstva.Sort((x, y) => y.Vek.CompareTo(x.Vek));
        }
        private void SmazatGeneraci(GeneraceVcel[] generaceVcelstva)
        {
            foreach (GeneraceVcel generaceVcel in generaceVcelstva.ToList())
                GeneraceVcelstva.Remove(generaceVcel);
        }
        private void UlozitMedNaPlastve()
        {
            foreach (Plastev plastev in Plastve)
            {
                Med = plastev.PridatMed(Med);
            }

            SecistMed();
        }

        public string PridatUkol(DataUkolu dataUkolu, int cisloMesice)
        {
            if (_zima.Contains(cisloMesice))
                return "Úkol nelze přidat, protože je zimní období";
            if (dataUkolu.Id == 1 && _neniPyl.Contains(cisloMesice))
                return "Úkol nelze přidat, protože nejsou kvetoucí rostliny";
            if (dataUkolu.Id == 5 && !_zazimovaniUlu.Contains(cisloMesice))
                return "Úl lze zazimovat pouze v listopadu.";
            if (dataUkolu.Id == 6 && !_rojVcelstva.Contains(cisloMesice))
                return "Úl lze vyrojit pouze v dubnu, květnu a červnu";

            Ukol ukol = VytvoritUkol(dataUkolu);
            DostupnyPocet dostupnyPocet = ZjistiDostupnyPocet(dataUkolu.Id);

            foreach (Podrobnost podrobnost in ukol.Podrobnosti)
            {
                if (podrobnost.Jmeno == "vcely" && podrobnost.Hodnota > dostupnyPocet.vcel)
                    return "Úkol nelze přidat, protože máte nedostatek včel." + dostupnyPocet.vcel;
                else if (podrobnost.Jmeno == "med" && podrobnost.Hodnota > dostupnyPocet.medu)
                    return "Úkol nelze přidat, protože máte nedostatek medu.";
            }

            SmazatUkol(dataUkolu.Id);
            SeznamUkolu.Add(ukol);

            return "přisně tajný string";
        }
        private static Ukol VytvoritUkol(DataUkolu dataUkolu)
        {
            Podrobnost[] podrobnosti = Array.Empty<Podrobnost>();
            string nazev = "";

            if (dataUkolu.Id == 1 || dataUkolu.Id == 4) // Sbírání pylu a Obrana úlu
            {
                if (dataUkolu.Id == 1)
                    nazev = "Sbírání pylu";
                else
                    nazev = "Obrana úlu";

                podrobnosti = new Podrobnost[]
                {
                    new Podrobnost("vcely", dataUkolu.Hodnota)
                };
            }
            else if (dataUkolu.Id == 2) // Nakladení vajíček
            {
                nazev = "Nakladení vajíček";
                podrobnosti = new Podrobnost[]
                {
                    new Podrobnost("kusy", dataUkolu.Hodnota),
                    new Podrobnost("vcely", Convert.ToInt32(Math.Round(dataUkolu.Hodnota / 2.0, MidpointRounding.AwayFromZero))),
                    new Podrobnost("med", dataUkolu.Hodnota * 2)
                };
            }
            else if (dataUkolu.Id == 3) // Vytvoření plástve
            {
                nazev = "Vytvoření plástve";
                podrobnosti = new Podrobnost[]
                {
                    new Podrobnost("kusy", dataUkolu.Hodnota),
                    new Podrobnost("vcely", dataUkolu.Hodnota * 100),
                    new Podrobnost("med", dataUkolu.Hodnota * 200)
                };
            }
            else if (dataUkolu.Id == 5) // Zazimování úlu
            {
                nazev = "Zazimování úlu";
                podrobnosti = new Podrobnost[]
                {
                    new Podrobnost("platnost", 3)
                };
            }
            else if (dataUkolu.Id == 6) // Vyrojení včelstva
            {
                nazev = "Vyrojení včelstva";
                podrobnosti = new Podrobnost[]
                {
                    new Podrobnost("netusim", 0)
                };
            }

            return new Ukol(dataUkolu.Id, nazev, podrobnosti);
        }
        private DostupnyPocet ZjistiDostupnyPocet(int id)
        {
            DostupnyPocet dostupnyPocet = new()
            {
                vcel = Vcelstvo,
                medu = Med
            };

            if (SeznamUkolu.Any())
            {
                foreach (Ukol ukol in SeznamUkolu)
                {
                    if (ukol.Id != id)
                    {
                        foreach (Podrobnost podrobnost in ukol.Podrobnosti)
                        {
                            if (podrobnost.Jmeno == "vcely")
                                dostupnyPocet.vcel -= podrobnost.Hodnota;
                            else if (podrobnost.Jmeno == "med")
                                dostupnyPocet.medu -= podrobnost.Hodnota;
                        }
                    }
                }
            }

            return dostupnyPocet;
        }
        public void SmazatUkol(int id)
        {
            if (SeznamUkolu.Any())
            {
                Ukol nactenyUkol = SeznamUkolu.Where(u => u.Id == id).FirstOrDefault();
                SeznamUkolu.Remove(nactenyUkol);
            }
        }
        private void SplnitUkoly(int cisloMesice)
        {
            List<Ukol> splneneUkoly = new();
            _strazci = 0;

            foreach(Ukol ukol in SeznamUkolu)
            {
                if (ukol.Id == 1)
                {
                    int vcely = ukol.Podrobnosti[0].Hodnota;

                    if (_nejvicePylu.Contains(cisloMesice))
                        Med += vcely * 8;
                    else if (_sezonaPylu.Contains(cisloMesice))
                        Med += vcely * 6;
                    else if (_menePylu.Contains(cisloMesice))
                        Med += vcely * 3;
                    else if (_maloPylu.Contains(cisloMesice))
                        Med += Convert.ToInt32(vcely * 0.5);

                    splneneUkoly.Add(ukol);
                }
                else if (ukol.Id == 2)
                {
                    int vajicka = ukol.Podrobnosti[0].Hodnota;
                    int med = ukol.Podrobnosti[2].Hodnota;

                    GeneraceVcelstva.Add(new GeneraceVcel(vajicka, -1));
                    Med -= med;
                    splneneUkoly.Add(ukol);
                }
                else if(ukol.Id == 3)
                {
                    int plastve = ukol.Podrobnosti[0].Hodnota;
                    int med = ukol.Podrobnosti[2].Hodnota;

                    for (int i = 0; i < plastve; i++)
                    {
                        Plastve.Add(new Plastev(0));
                    }

                    Med -= med;
                    splneneUkoly.Add(ukol);
                }
                else if (ukol.Id == 4)
                {
                    _strazci = ukol.Podrobnosti[0].Hodnota;
                    splneneUkoly.Add(ukol);
                }
                else if (ukol.Id == 5)
                {
                    KlidPoBitve = ukol.Podrobnosti[0].Hodnota;
                    splneneUkoly.Add(ukol);
                }
                else if (ukol.Id == 6)
                {
                    // zatim v budoucnu
                    // neco se zapise
                    splneneUkoly.Add(ukol);
                }
            }

            foreach (Ukol ukol in splneneUkoly)
                SeznamUkolu.Remove(ukol);
        }

        private int VylosovatNepritele(int cisloMesice)
        {
            int vyherce = 0;

            if (Nepritel.Porazen == true)
            {
                int sance;

                if (_strazci == 0)
                    sance = 100;
                else
                    sance = 101 - (_strazci / Plastve.Count) / 20 * 100;

                if (_nahodneCislo.Next(0, 101) <= sance)
                {
                    List<int> IdNepratel = new();

                    if (_hlodavci.Contains(cisloMesice))
                    {
                        IdNepratel.Add(1);
                        IdNepratel.Add(2);
                    }
                    else if (_vosy.Contains(cisloMesice))
                        IdNepratel.Add(3);
                    else if (_mravenci.Contains(cisloMesice))
                        IdNepratel.Add(4);
                    else if (_zavijec.Contains(cisloMesice))
                        IdNepratel.Add(5);
                    
                    if(IdNepratel.Any())
                        vyherce = IdNepratel[_nahodneCislo.Next(IdNepratel.Count)];
                }
            }

            return vyherce;
        }
        private void VytvoritNepritele(int id)
        {
            if (KlidPoBitve == 0)
            {
                if (id != 0 && Nepritel.Porazen == true)
                {
                    string jmeno = "";
                    int pocet = 0;
                    int zivotJedince = 0;
                    bool vrazednyUder = false;

                    if (id == 1 || id == 2)
                    {
                        if (id == 1)
                            jmeno = "Rejsek";
                        else
                            jmeno = "Myš";

                        pocet = 1;
                        zivotJedince = 1000;
                    }
                    else if (id == 3)
                    {
                        jmeno = "Vosy";
                        pocet = _nahodneCislo.Next(6, (_strazci / 100) * 5 + _strazci + 8);
                        zivotJedince = 200;
                        vrazednyUder = true;
                    }
                    else if (id == 4)
                    {
                        jmeno = "Mravenci";
                        pocet = _nahodneCislo.Next(10, (_strazci / 100) * 20 + _strazci + 20);
                        zivotJedince = 60;
                        vrazednyUder = true;
                    }
                    else if (id == 5)
                    {
                        jmeno = "Zavíječ";
                        pocet = _nahodneCislo.Next(2, Plastve.Count / 3 + 3);
                        zivotJedince = 1;
                    }

                    Nepritel = new Nepritel(id, jmeno, pocet, pocet * zivotJedince, zivotJedince, 0, vrazednyUder, false);
                }
                else if (Nepritel.Porazen == false)
                    Nepritel.Invaze(Vcelstvo);
            }
            else
                KlidPoBitve -= 1;

        }
        private void BojSNepritelem()
        {
            if (_strazci > 0)
            {
                OdecistVcely(Nepritel.Boj(_strazci, _strazci * _zivotStrazce, _zivotStrazce), false);
                SecistVcely();

                if (Nepritel.Porazen == true)
                {
                    if (KlidPoBitve == 0 && Nepritel.Vek > 0)
                        KlidPoBitve = 1;

                    Nepritel = new Nepritel(0, "", 0, 0, 0, 0, false, true); //nechat kvuli javascriptu???
                }
            }

            if (Nepritel.Porazen == false)
            {
                if (Nepritel.Id == 1)
                {
                    OdecistVcely(_nahodneCislo.Next(30, 101), false);
                    SecistVcely();
                }
                else if (Nepritel.Id == 2)
                {
                    ZnicitPlastve(Nepritel.Pocet, true);
                }
                else if (Nepritel.Id == 3)
                {
                    OdecistVcely(Nepritel.Pocet, true);
                    Med -= Nepritel.Pocet / 2;
                    UlozitMedNaPlastve();
                    SecistVcely();
                }
                else if (Nepritel.Id == 4)
                {
                    Med -= Nepritel.Pocet * 2;
                    UlozitMedNaPlastve();
                }
                else if (Nepritel.Id == 5)
                {
                    ZnicitPlastve(Nepritel.Pocet / 2, false);
                }
            }
        }
        private void OdecistVcely(int mrtvoly, bool mladeVcely)
        {
            if (mladeVcely == true)
                GeneraceVcelstva.Sort((x, y) => x.Vek.CompareTo(y.Vek));//0 4

            foreach (GeneraceVcel generaceVcel in GeneraceVcelstva)
                mrtvoly = generaceVcel.OdstranitVcely(mrtvoly);

            SmazatGeneraci(GeneraceVcelstva.Where(u => u.Pocet <= 0).ToArray());

            if (mladeVcely == true)
                GeneraceVcelstva.Sort((x, y) => y.Vek.CompareTo(x.Vek));//4 0
        }
        private void ZnicitPlastve(int pocet, bool pouzeSMedem)
        {
            int cisloPlastve;
            for (int i = 0; i < pocet && Plastve.Count > 0; i++)
            {
                if (pouzeSMedem == true)
                    cisloPlastve = _nahodneCislo.Next(0, Plastve.Count);
                else
                    cisloPlastve = 0;

                Med -= Plastve[cisloPlastve].Med;
                Plastve.RemoveAt(cisloPlastve);
            }
        }
    }
}
