using System;
using System.Collections.Generic;
using System.IO;

namespace Bioskop
{
    public class Termin
    {
        public string NazivFilma { get; set; }
        public DateTime DatumVreme { get; set; }
        public int Sala { get; set; }
        public int SlobodnaMesta { get; set; }

        public Termin() { }

        public Termin(string nazivFilma, DateTime datumVreme, int sala, int slobodnaMesta)
        {
            NazivFilma = nazivFilma;
            DatumVreme = datumVreme;
            Sala = sala;
            SlobodnaMesta = slobodnaMesta;
        }

        public override string ToString()
        {
            return $"{NazivFilma};{DatumVreme};{Sala};{SlobodnaMesta}";
        }

        public static List<Termin> UcitajIzFajla(string putanja)
        {
            var lista = new List<Termin>();
            if (!File.Exists(putanja)) return lista;

            foreach (var linija in File.ReadAllLines(putanja))
            {
                var delovi = linija.Split(';');
                if (delovi.Length == 4)
                {
                    lista.Add(new Termin(
                        delovi[0],
                        DateTime.Parse(delovi[1]),
                        int.Parse(delovi[2]),
                        int.Parse(delovi[3])
                    ));
                }
            }
            return lista;
        }

        public static void SacuvajUFajl(List<Termin> termini, string putanja)
        {
            var linije = new List<string>();
            foreach (var t in termini)
            {
                linije.Add(t.ToString());
            }
            File.WriteAllLines(putanja, linije);
        }
    }
}
