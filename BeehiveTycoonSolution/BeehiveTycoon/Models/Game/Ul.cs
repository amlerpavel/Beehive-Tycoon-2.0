using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeehiveTycoon.Models.Game
{
    public class Ul0
    {
        public int _vcelstvo { get; set; }
        public int _med { get; set; }
        public string _lokace { get; set; }
        public List<GeneraceVcel> _generaceVcelstva { get; set; }
        public List<Plastev> _plastve { get; set; }

        /*
        public Nepritel Nepritel { get; set; }
        public List<Ukol> SeznamUkolu { get; set; }
        */
        public void NovyUl(string lokace, List<GeneraceVcel> generaceVcelstva, List<Plastev> plastve)
        {
            _lokace = lokace;
            _generaceVcelstva = generaceVcelstva;
            _plastve = plastve;

            SecistMed();
            SecistVcely();
        }

        public void DalsiKolo()
        {
            ZestarnutiVcelstva();
            UlozitMedNaPlastve(_med);
            SecistVcely();
        }

        private void SecistVcely()
        {
            _vcelstvo = 0;

            foreach (GeneraceVcel generaceVcel in _generaceVcelstva)
            {
                _vcelstvo += generaceVcel._pocet;
            }

        }
        private void SecistMed()
        {
            _med = 0;

            foreach (Plastev plastev in _plastve)
            {
                _med += plastev._med;
            }

        }
        private void ZestarnutiVcelstva()
        {
            _generaceVcelstva.Sort((x, y) => x._vek.CompareTo(y._vek));
            List<int> inexy = new();

            foreach (GeneraceVcel generaceVcel in _generaceVcelstva)
            {
                _med -= generaceVcel._pocet;

                if(_med >= 0)
                    generaceVcel.Zestarnout(1);
                else
                {
                    double vek = (_med * (-3) + generaceVcel._pocet + _med) / Convert.ToDouble(generaceVcel._pocet);
                    generaceVcel.Zestarnout(Math.Round(vek, 0, MidpointRounding.AwayFromZero));
                    _med = 0;
                }

                if (generaceVcel._vek >= 7)
                    inexy.Add(_generaceVcelstva.IndexOf(generaceVcel));
            }

            foreach(int i in inexy)
            {
                _generaceVcelstva.RemoveAt(i);
            }

            _generaceVcelstva.Sort((x, y) => y._vek.CompareTo(x._vek));
        }
        private void UlozitMedNaPlastve(int med)
        {
            foreach (Plastev plastev in _plastve)
            {
                if (med >= 1000)
                {
                    plastev._med = 1000;
                    med -= 1000;
                }
                else
                {
                    plastev._med = med;
                    med = 0;
                }
            }
        }
    }
}
