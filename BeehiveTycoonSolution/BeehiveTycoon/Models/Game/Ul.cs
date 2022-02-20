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

        /*
        public Nepritel Nepritel { get; set; }
        public List<Ukol> SeznamUkolu { get; set; }
        */
        public Ul0(string lokace, List<GeneraceVcel> generaceVcelstva, List<Plastev> plastve)
        {
            Lokace = lokace;
            GeneraceVcelstva = generaceVcelstva;
            Plastve = plastve;

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
        private void SplnitUkoly()
        {

        }
    }
}
