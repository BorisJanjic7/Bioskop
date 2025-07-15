using System;
using System.Collections.Generic;
using System.IO;

namespace Bioskop
{
    public class Karta
    {
        public string NazivFilma { get; set; }
        public DateTime DatumVreme { get; set; }
        public int BrojSedista { get; set; }
        public bool Rezervisana { get; set; }
        public bool Kupljena { get; set; }

        public Karta() { }

        public Karta(string nazivFilma, DateTime datumVreme, int brojSedista, bool rezervisana, bool kupljena)
        {
            NazivFilma = nazivFilma;
            DatumVreme = datumVreme;
            BrojSedista = brojSedista;
            Rezervisana = rezervisana;
            Kupljena = kupljena;
        }

        public override string ToString()
        {
            return $"{NazivFilma};{DatumVreme};{BrojSedista};{Rezervisana};{Kupljena}";
        }

        public static List<Karta> UcitajIzFajla(string putanja)
        {
            var lista = new List<Karta>();
            if (!File.Exists(putanja)) return lista;

            foreach (var linija in File.ReadAllLines(putanja))
            {
                var delovi = linija.Split(';');
                if (delovi.Length == 5)
                {
                    lista.Add(new Karta(
                        delovi[0],
                        DateTime.Parse(delovi[1]),
                        int.Parse(delovi[2]),
                        bool.Parse(delovi[3]),
                        bool.Parse(delovi[4])
                    ));
                }
            }
            return lista;
        }

        public static void SacuvajUFajl(List<Karta> karte, string putanja)
        {
            var linije = new List<string>();
            foreach (var k in karte)
            {
                linije.Add(k.ToString());
            }
            File.WriteAllLines(putanja, linije);
        }
    }
}
