using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls; // Potrebno za SelectionChangedEventArgs

namespace Bioskop
{
    public partial class MainWindow : Window
    {
        private List<Film> filmovi = new();
        private List<Termin> termini = new();
        private List<Karta> karte = new();

        private string filmFajl = "filmovi.txt";
        private string terminFajl = "termini.txt";
        private string kartaFajl = "karte.txt";

        public MainWindow()
        {
            InitializeComponent();
            UcitajFilmove(); // Ovo bi trebalo da učita i prikaže filmove
        }

        private void UcitajFilmove()
        {
            try
            {
                filmovi = Film.UcitajIzFajla(filmFajl);
                dgFilmovi.ItemsSource = null; // Prvo resetuj izvor podataka
                dgFilmovi.ItemsSource = filmovi; // Zatim postavi novi izvor
                MessageBox.Show($"Učitano {filmovi.Count} filmova.", "Info o učitavanju", MessageBoxButton.OK, MessageBoxImage.Information); // Debug poruka
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju filmova: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OcistiPoljaFilma();    // Očisti polja za unos filma
            OcistiPoljaTermina();  // Očisti polja za unos termina
        }

        private void BtnDodajFilm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) || string.IsNullOrWhiteSpace(txtZanr.Text) ||
                !int.TryParse(txtTrajanje.Text, out int trajanje) || !int.TryParse(txtGodina.Text, out int godina))
            {
                MessageBox.Show("Popunite sve podatke za film (Naziv, Žanr, Trajanje (broj), Godina (broj)) ispravno.", "Greška pri unosu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Film novi = new(txtNaziv.Text, txtZanr.Text, trajanje, godina);
            filmovi.Add(novi);
            Film.SacuvajUFajl(filmovi, filmFajl);
            UcitajFilmove(); // Ponovo učitaj i osveži DataGrid
            MessageBox.Show("Film je uspešno dodat.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDodajTermin_Click(object sender, RoutedEventArgs e)
        {
            if (dgFilmovi.SelectedItem is not Film izabraniFilm)
            {
                MessageBox.Show("Molimo izaberite film iz tabele za koji želite dodati termin.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!DateTime.TryParse(txtDatumVreme.Text, out DateTime datumVreme))
            {
                MessageBox.Show("Unesite ispravan datum i vreme (npr. YYYY-MM-DD HH:MM).", "Greška pri unosu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(txtSala.Text, out int sala))
            {
                MessageBox.Show("Unesite ispravan broj sale (celi broj).", "Greška pri unosu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int brojMesta = 50; // Možete ovo učiniti poljem za unos
            Termin noviTermin = new(izabraniFilm.Naziv, datumVreme, sala, brojMesta);

            // Učitaj postojeće termine, dodaj novi, pa sačuvaj sve
            termini = Termin.UcitajIzFajla(terminFajl);
            termini.Add(noviTermin);
            Termin.SacuvajUFajl(termini, terminFajl);

            // Generisanje karata za novi termin
            karte = Karta.UcitajIzFajla(kartaFajl); // Učitaj sve postojeće karte
            for (int i = 1; i <= brojMesta; i++)
            {
                // Proverite da li karta već postoji za taj film, datum/vreme i sedište
                if (!karte.Any(k => k.NazivFilma == izabraniFilm.Naziv &&
                                    k.DatumVreme == datumVreme &&
                                    k.BrojSedista == i))
                {
                    karte.Add(new Karta(izabraniFilm.Naziv, datumVreme, i, false, false));
                }
            }
            Karta.SacuvajUFajl(karte, kartaFajl);

            MessageBox.Show($"Termin za film '{izabraniFilm.Naziv}' u sali {sala} i {brojMesta} karata uspešno dodati.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            OcistiPoljaTermina(); // Očisti polja za termin
        }

        private void OcistiPoljaFilma()
        {
            txtNaziv.Text = "";
            txtZanr.Text = "";
            txtTrajanje.Text = "";
            txtGodina.Text = "";
        }

        private void OcistiPoljaTermina()
        {
            txtDatumVreme.Text = "";
            txtSala.Text = "";
        }

        private void DgFilmovi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kada se izabere film, možemo očistiti polja za termin
            OcistiPoljaTermina();
        }
    }
}