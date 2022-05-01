using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeehiveTycoon.Game
{
    public class Ul
    {
        public int Vcelstvo { get; private set; }
        public int Med { get; private set; }
        public Lokace Lokace { get; private set; }
        public List<GeneraceVcel> GeneraceVcelstva { get; private set; }
        public List<Plastev> Plastve { get; private set; }
        public List<Ukol> SeznamUkolu { get; private set; }
        public Nepritel Nepritel { get; private set; }
        public int KlidPoBitve { get; private set; }
        public bool ExistujeMrtvyNepritel { get; private set; }
        public bool VyrojitUl { get; private set; }
        public bool BylVyrojenUl { get; private set; }
        public int MaxPlastvi { get; private set; }

        private struct DostupnyPocet
        {
            public int vcel;
            public int medu;
        }

        private readonly Random _nahodneCislo = new();
        private readonly int _zivotStrazce = 100;
        private int _strazci;
        private Lokace _lokace;
        private int _maxPlastvi;

        // cisla mesicu, ve kterych je ...
        private readonly int[] _zima = { 1, 12 };
        private readonly int[] _neniPyl = { 11 };
        private readonly int[] _rojVcelstva = { 4, 5, 6 };
        private readonly int[] _zazimovaniUlu = { 11 };

        private readonly int[] _hlodavci = { 1, 11, 12 };
        private readonly int[] _vosy = { 6, 7, 8 };
        private readonly int[] _mravenci = { 4, 5, 6, 7, 8, 9 };
        private readonly int[] _zavijec = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        public Ul(Lokace lokace, List<GeneraceVcel> generaceVcelstva, List<Plastev> plastve, List<Ukol> seznamUkolu, Nepritel nepritel, int klidPoBitve, bool existujeMrtvyNepritel, bool vyrojitUl, bool bylVyrojenUl, int maxPlastvi)
        {
            Lokace = lokace;
            GeneraceVcelstva = generaceVcelstva;
            Plastve = plastve;
            SeznamUkolu = seznamUkolu;
            Nepritel = nepritel;
            KlidPoBitve = klidPoBitve;
            ExistujeMrtvyNepritel = existujeMrtvyNepritel;
            VyrojitUl = vyrojitUl;
            BylVyrojenUl = bylVyrojenUl;
            MaxPlastvi = maxPlastvi;

            SecistMed();
            SecistVcely();
        }

        public void DalsiKolo(int cisloMesice, Obtiznost obtiznost)
        {
            SplnitUkoly(cisloMesice, obtiznost.PMedu);
            ZestarnutiVcelstva();
            UlozitMedNaPlastve();
            ZtraceneVcely(obtiznost.PZtraceneVcely);
            Debug.WriteLine("Před vytvořenim: " + Nepritel.Jmeno + " " + Nepritel.Pocet + " porazen: " + Nepritel.Porazen);
            VytvoritNepritele(VylosovatNepritele(cisloMesice, obtiznost.PMaxOchrana), obtiznost.PNepratel);
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
            SecistVcely();

            GeneraceVcelstva.Sort((x, y) => y.Vek.CompareTo(x.Vek));
        }
        private void SmazatGeneraci(GeneraceVcel[] generaceVcelstva)
        {
            foreach (GeneraceVcel generaceVcel in generaceVcelstva)
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
                return "Úkol nelze přidat, protože je zimní období.";
            if (dataUkolu.Id == 1 && _neniPyl.Contains(cisloMesice))
                return "Úkol nelze přidat, protože nejsou kvetoucí rostliny.";
            if (dataUkolu.Id == 5 && !_zazimovaniUlu.Contains(cisloMesice))
                return "Úl lze zazimovat pouze v listopadu.";
            if (dataUkolu.Id == 6)
            {
                if(!_rojVcelstva.Contains(cisloMesice))
                    return "Úl lze vyrojit pouze v dubnu, květnu a červnu.";
                else if(Nepritel.Porazen == false)
                    return "Úkol nelze přidat, protože je v úlu nepřítel.";
                else if(BylVyrojenUl == true)
                    return "Úkol nelze přidat, protože z toho úlu jste se již vyrojili.";
                else if (MaxPlastvi > Plastve.Count)
                    return "Úkol nelze přidat, protože v tomto úlu je ještě místo pro další plástve.";
            }
            if (dataUkolu.Id == 3)
            {
                if(MaxPlastvi <= Plastve.Count)
                    return "V tomto úlu již není místo pro další plástve.";
                else if (MaxPlastvi - Plastve.Count < dataUkolu.Hodnota)
                    return "Úkol nelze přidat, protože není místo pro tyto nové plástve.";
            }

            Ukol ukol = VytvoritUkol(dataUkolu);
            DostupnyPocet dostupnyPocet = ZjistiDostupnyPocet(dataUkolu.Id);

            foreach (Podrobnost podrobnost in ukol.Podrobnosti)
            {
                if (podrobnost.Jmeno == "vcely" && podrobnost.Hodnota > dostupnyPocet.vcel)
                    return "Úkol nelze přidat, protože máte nedostatek včel. K dispozici: " + dostupnyPocet.vcel;
                else if (podrobnost.Jmeno == "med" && podrobnost.Hodnota > dostupnyPocet.medu)
                    return "Úkol nelze přidat, protože máte nedostatek medu. K dispozici: " + dostupnyPocet.medu;
            }

            SmazatUkol(dataUkolu.Id);
            SeznamUkolu.Add(ukol);

            return "přisně tajný string";
        }
        private Ukol VytvoritUkol(DataUkolu dataUkolu)
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
                    new Podrobnost("lokace", dataUkolu.Hodnota),
                    new Podrobnost("vcely", Vcelstvo),
                    new Podrobnost("med", MaxPlastvi * Plastve[0].MaxMedu)
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
        private void SplnitUkoly(int cisloMesice, int pMedu)
        {
            List<Ukol> splneneUkoly = new();
            _strazci = 0;

            foreach(Ukol ukol in SeznamUkolu)
            {
                if (ukol.Id == 1)
                {
                    int vcely = ukol.Podrobnosti[0].Hodnota;

                    Med += Lokace.Pyl.ZiskatMed(vcely, cisloMesice, pMedu);

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
                    VyrojitUl = true;
                    KlidPoBitve = 1;

                    string nazev;
                    int id = ukol.Podrobnosti[0].Hodnota;
                    Vystkyt vystkytPylu;
                    MnostviNaVcelu mnostviNaVcelu;
                    int pNepratel;
                    int maximalniOchrana;
                    int pZtraceneVcely;

                    if (id == 1)
                    {
                        nazev = "Zahrada";
                        vystkytPylu = new(new int[] { 5 }, new int[] { 4, 6, 7, 8 }, new int[] { 3, 9 }, new int[] { 2, 10 });
                        mnostviNaVcelu = new(8, 6, 3, 0.5);
                        pNepratel = 0;
                        maximalniOchrana = 90;
                        pZtraceneVcely = 0;
                        _maxPlastvi = 25;
                    }
                    else if (id == 2)
                    {
                        nazev = "Les";
                        vystkytPylu = new(new int[] { 4, 5 }, new int[] { 3, 6 }, new int[] { 7, 8, 9 }, new int[] { 2, 10 });
                        mnostviNaVcelu = new(10, 8, 1.5, 0.25);
                        pNepratel = 40;
                        maximalniOchrana = 60;
                        pZtraceneVcely = 0;
                        _maxPlastvi = 40;
                    }
                    else if (id == 3)
                    {
                        nazev = "Louka";
                        vystkytPylu = new(new int[] { 6, 7, 8 }, new int[] { 5, 9 }, new int[] { 4, 10 }, new int[] { 2, 3 });
                        mnostviNaVcelu = new(9, 6, 2, 0.3);
                        pNepratel = 20;
                        maximalniOchrana = 80;
                        pZtraceneVcely = 0;
                        _maxPlastvi = 30;
                    }
                    else if (id == 4)
                    {
                        nazev = "Pole";
                        vystkytPylu = new(new int[] { 4,5 }, new int[] { 6, 7, 8 }, new int[] { 3, 9 }, new int[] { 2, 10 });
                        mnostviNaVcelu = new(15, 2, 1.5, 0.25);
                        pNepratel = 0;
                        maximalniOchrana = 90;
                        pZtraceneVcely = 5;
                        _maxPlastvi = 20;
                    }
                    else //if (id == 5)
                    {
                        nazev = "Město";
                        vystkytPylu = new(new int[] { 5 }, new int[] { 4, 6, 7, 8 }, new int[] { 3, 9 }, new int[] { 2, 10 });
                        mnostviNaVcelu = new(4, 3, 1.5, 0.25);
                        pNepratel = -20;
                        maximalniOchrana = 90;
                        pZtraceneVcely = 10;
                        _maxPlastvi = 10;
                    }

                    _lokace = new(nazev, id, new Pyl(vystkytPylu, mnostviNaVcelu), pNepratel, maximalniOchrana, pZtraceneVcely);

                    splneneUkoly.Add(ukol);
                }
            }

            foreach (Ukol ukol in splneneUkoly)
                SeznamUkolu.Remove(ukol);
        }

        private int VylosovatNepritele(int cisloMesice, int pMaxOchrana)
        {
            int vyherce = 0;

            if (Nepritel.Porazen == true)
            {
                int sance;
                double ochrana;

                if (_strazci == 0)
                    ochrana = (Lokace.MaximalniOchrana + Lokace.MaximalniOchrana / 100.0 * pMaxOchrana) / 3.0;
                else
                {
                    double ochranaPlastvi = _strazci / Plastve.Count / 20;

                    if (ochranaPlastvi > 1)
                        ochranaPlastvi = 1;

                    ochrana = ochranaPlastvi * (Lokace.MaximalniOchrana + Lokace.MaximalniOchrana / 100.0 * pMaxOchrana);
                }

                sance = 100 - Convert.ToInt32(ochrana);

                if (_nahodneCislo.Next(1, 101) <= sance)
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
        private void VytvoritNepritele(int id, int oPNepratel)
        {
            if (KlidPoBitve == 0)
            {
                if (id != 0 && Nepritel.Porazen == true)
                {
                    string jmeno = "";
                    int zivotJedince = 0;
                    bool vrazednyUder = false;
                    double min = 0;
                    double max = 0;

                    if (id == 1 || id == 2)
                    {
                        if (id == 1)
                            jmeno = "Rejsek";
                        else
                            jmeno = "Myš";

                        min = 1;
                        max = 2;
                        zivotJedince = 1000;
                    }
                    else if (id == 3)
                    {
                        jmeno = "Vosy";
                        min = 6 + (_strazci / 10);
                        max = 8 + (_strazci / 10) * 5;
                        zivotJedince = 200;
                        vrazednyUder = true;
                    }
                    else if (id == 4)
                    {
                        jmeno = "Mravenci";
                        min = 10 + (_strazci / 10);
                        max = 20 + (_strazci / 10) * 20;
                        zivotJedince = 60;
                        vrazednyUder = true;
                    }
                    else if (id == 5)
                    {
                        jmeno = "Zavíječ";
                        min = 3 + Plastve.Count / 6;
                        max = 5 + Plastve.Count / 3;
                        zivotJedince = 1;
                    }

                    min += min / 100 * Lokace.PNepratel;
                    max += max / 100 * Lokace.PNepratel;

                    min += min / 100 * oPNepratel;
                    max += max / 100 * oPNepratel;

                    int pocet = _nahodneCislo.Next(Convert.ToInt32(min), Convert.ToInt32(max));

                    Nepritel = new Nepritel(id, jmeno, pocet, pocet * zivotJedince, zivotJedince, 0, vrazednyUder, false);
                }
                else if (Nepritel.Porazen == false)
                    Nepritel.Invaze(Vcelstvo, Lokace.PNepratel, oPNepratel);
            }
            else
            {
                KlidPoBitve -= 1;
                if (Nepritel.Porazen == false)
                    Nepritel.Invaze(Vcelstvo, Lokace.PNepratel, oPNepratel);
                ExistujeMrtvyNepritel = false;
            }
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
                    if (Nepritel.Pocet <= 0 && Nepritel.Id != 0)
                        ExistujeMrtvyNepritel = true;

                    Nepritel = new Nepritel(0, "", 0, 0, 0, 0, false, true);
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
                    SecistVcely();

                    Med -= Nepritel.Pocet / 2;
                    UlozitMedNaPlastve();
                }
                else if (Nepritel.Id == 4)
                {
                    Med -= Nepritel.Pocet * 2;
                    UlozitMedNaPlastve();
                }
                else if (Nepritel.Id == 5)
                {
                    ZnicitPlastve(Nepritel.Pocet / 3, false);
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

        public Ul Vyrojit()
        {
            BylVyrojenUl = true;
            VyrojitUl = false;

            List<GeneraceVcel> generaceVcelstvaRoj = VyrojitGeneraceVcelstva();
            int pocetVcel = 0;

            foreach (GeneraceVcel generaceVcel in generaceVcelstvaRoj)
                pocetVcel += generaceVcel.Pocet;

            int med = Convert.ToInt32(pocetVcel * 1.5);

            Med -= med;
            UlozitMedNaPlastve();

            List<Plastev> plastve = VytvoritPlastve(med);

            Ul ul = new(
                _lokace,
                generaceVcelstvaRoj,
                plastve,
                new List<Ukol>(),
                new Nepritel(0, "", 0, 0, 0, 0, false, true),
                0,
                false,
                false,
                false,
                _maxPlastvi
            );

            return ul;
        }
        private List<GeneraceVcel> VyrojitGeneraceVcelstva()
        {
            List<GeneraceVcel> GeneraceVcelstvaRoj = new();

            foreach (GeneraceVcel generaceVcel in GeneraceVcelstva)
            {
                int pulkaVcel = generaceVcel.Pocet / 2;
                generaceVcel.OdstranitVcely(pulkaVcel);
                GeneraceVcel generaceVcelRoj = new(pulkaVcel, generaceVcel.Vek);
                GeneraceVcelstvaRoj.Add(generaceVcelRoj);
            }

            SecistVcely();

            return GeneraceVcelstvaRoj;
        }
        private static List<Plastev> VytvoritPlastve(int med)
        {
            List<Plastev> plastve = new();

            while (med > 0)
            {
                med -= 200;
                if(med >= 1000)
                {
                    plastve.Add(new Plastev(1000));
                    med -= 1000;
                }
                else
                {
                    if (med <= 0)
                        med = 0;
                    plastve.Add(new Plastev(med));
                    med = 0;
                }
            }

            return plastve;
        }
        public void PovolitZnovuRoj()
        {
            BylVyrojenUl = false;
        }

        private void ZtraceneVcely(int pZtraceneVcely)
        {
            if (Lokace.PZtraceneVcely != 0)
            {
                double ztraceneVcely = Vcelstvo / 100 * Lokace.PZtraceneVcely;

                ztraceneVcely += ztraceneVcely / 100 * pZtraceneVcely;

                double min = ztraceneVcely - ztraceneVcely / 100 * 5;
                double max = ztraceneVcely + ztraceneVcely / 100 * 10;

                OdecistVcely(_nahodneCislo.Next(Convert.ToInt32(min), Convert.ToInt32(max)), false);
                SecistVcely();
            }
        }
    }
}
