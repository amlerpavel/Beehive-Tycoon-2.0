using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models.Game;
using System.Diagnostics;

namespace BeehiveTycoon.Models
{
    public class Hra0
    {
        public Datum _datum { get; set; }
        public Ul0 _ul0 { get; set; }
        
        public void NovaHra()
        {
            _datum = new();
            _datum.NoveDatum(12, 0);

            _ul0 = new();
            _ul0.NovyUl(
                "netusim",
                new List<GeneraceVcel> {
                    new GeneraceVcel { _pocet = 300, _vek = 3 },
                    new GeneraceVcel { _pocet = 400, _vek = 0 }
                },
                new List<Plastev> {
                    new Plastev { _med = 1000, _vajicka = 0 }
                }
            );
        }

        public void Dalsikolo()
        {
            _datum.ZmenaData();
            _ul0.DalsiKolo();
        }
    }
}
