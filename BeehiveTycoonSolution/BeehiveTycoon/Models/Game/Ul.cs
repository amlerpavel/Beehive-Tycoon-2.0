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

        private struct DostupnyPocet
        {
            public int vcel;
            public int medu;
        }

        private readonly int[] _zima = { 12, 1 };
        private readonly int[] _neniPyl = { 11, 2 };
        private readonly int[] _rojVcelstva = { 4 ,5, 6 };
        private readonly int _zazimovaniUlu = 11;

        /*
        public Nepritel Nepritel { get; set; }
        public List<Ukol> SeznamUkolu { get; set; }
        */
        public Ul0(string lokace, List<GeneraceVcel> generaceVcelstva, List<Plastev> plastve, List<Ukol> seznamUkolu)
        {
            Lokace = lokace;
            GeneraceVcelstva = generaceVcelstva;
            Plastve = plastve;
            SeznamUkolu = seznamUkolu;

            SecistMed();
            SecistVcely();
        }

        public void DalsiKolo()
        {
            SplnitUkoly();
            ZestarnutiVcelstva();
            UlozitMedNaPlastve();
            SecistVcely();
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
            List<int> inexy = new();

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

                if (generaceVcel.Vek >= 7)
                    inexy.Add(GeneraceVcelstva.IndexOf(generaceVcel));
            }

            foreach(int i in inexy)
            {
                GeneraceVcelstva.RemoveAt(i);
            }

            GeneraceVcelstva.Sort((x, y) => y.Vek.CompareTo(x.Vek));
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
            if (dataUkolu.Id == 5 && !(_zazimovaniUlu == cisloMesice))
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
        private void SplnitUkoly()
        {

        }
    }
}
