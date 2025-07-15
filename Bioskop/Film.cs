using System;
using System.Collections.Generic;
using System.IO;

namespace Bioskop
{
    public class Film
    {
        public string Naziv { get; set; }
        public string Zanr { get; set; }
        public int Trajanje { get; set; }
        public int Godina { get; set; }

        public Film() { }

        public Film(string naziv, string zanr, int trajanje, int godina)
        {
            Naziv = naziv;
            Zanr = zanr;
            Trajanje = trajanje;
            Godina = godina;
        }

        public override string ToString()
        {
            return $"{Naziv};{Zanr};{Trajanje};{Godina}";
        }

        public static List<Film> UcitajIzFajla(string putanja)
        {
            var lista = new List<Film>();
            if (!File.Exists(putanja)) return lista;

            foreach (var linija in File.ReadAllLines(putanja))
            {
                var delovi = linija.Split(';');
                if (delovi.Length == 4)
                {
                    lista.Add(new Film(
                        delovi[0],
                        delovi[1],
                        int.Parse(delovi[2]),
                        int.Parse(delovi[3])
                    ));
                }
            }
            return lista;
        }

        public static void SacuvajUFajl(List<Film> filmovi, string putanja)
        {
            var linije = new List<string>();
            foreach (var f in filmovi)
            {
                linije.Add(f.ToString());
            }
            File.WriteAllLines(putanja, linije);
        }
    }
}
