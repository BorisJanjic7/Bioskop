using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bioskop
{
    public partial class ViewerWindow : Window
    {
        private List<Termin> termini = new();
        private List<Karta> karte = new();
        private string terminFajl = "termini.txt";
        private string kartaFajl = "karte.txt";

        public ViewerWindow()
        {
            InitializeComponent();
            UcitajTermine();
        }

        private void UcitajTermine()
        {
            termini = Termin.UcitajIzFajla(terminFajl);
            cmbTermini.ItemsSource = termini;
            // DisplayMemberPath nije potreban jer Termin.ToString() već formatira lepo
            // cmbTermini.DisplayMemberPath = "NazivFilma"; // Ova linija može biti izbrisana ili zakomentarisana
            cmbTermini.SelectionChanged += CmbTermini_SelectionChanged;

            if (termini.Any())
            {
                cmbTermini.SelectedIndex = 0; // Automatski izaberi prvi termin
            }
        }

        private void CmbTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTermini.SelectedItem is Termin t)
            {
                karte = Karta.UcitajIzFajla(kartaFajl)
                                .Where(k => k.NazivFilma == t.NazivFilma && k.DatumVreme == t.DatumVreme)
                                .ToList();

                dgKarte.ItemsSource = null; // Prvo resetuj izvor
                dgKarte.ItemsSource = karte; // Zatim postavi novi
            }
        }

        private void BtnRezervisi_Click(object sender, RoutedEventArgs e)
        {
            if (dgKarte.SelectedItem is Karta k)
            {
                if (!k.Kupljena) // Može se rezervisati samo ako nije već kupljena
                {
                    k.Rezervisana = true;
                    // Ažuriraj listu svih karata i sačuvaj je nazad u fajl
                    Karta.SacuvajUFajl(AzurirajKarte(k), kartaFajl);
                    dgKarte.Items.Refresh(); // Osveži prikaz u DataGrid-u

                    // Ažuriraj broj slobodnih mesta za termin
                    if (cmbTermini.SelectedItem is Termin izabraniTermin)
                    {
                        izabraniTermin.SlobodnaMesta--;
                        // Pronađi i ažuriraj taj termin u glavnoj listi termina
                        var sveTermine = Termin.UcitajIzFajla(terminFajl);
                        var terminZaAzuriranje = sveTermine.FirstOrDefault(t =>
                            t.NazivFilma == izabraniTermin.NazivFilma &&
                            t.DatumVreme == izabraniTermin.DatumVreme &&
                            t.Sala == izabraniTermin.Sala);

                        if (terminZaAzuriranje != null)
                        {
                            terminZaAzuriranje.SlobodnaMesta = izabraniTermin.SlobodnaMesta;
                            Termin.SacuvajUFajl(sveTermine, terminFajl);
                            // Ponovo učitaj termine da se ComboBox osveži
                            UcitajTermine();
                        }
                    }
                    MessageBox.Show("Karta je uspešno rezervisana.", "Rezervacija", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ova karta je već kupljena i ne može se rezervisati.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Molimo izaberite kartu za rezervaciju.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnKupi_Click(object sender, RoutedEventArgs e)
        {
            if (dgKarte.SelectedItem is Karta k)
            {
                if (!k.Kupljena) // Može se kupiti samo ako nije već kupljena
                {
                    k.Kupljena = true;
                    k.Rezervisana = true; // Kupljena karta je automatski i rezervisana
                    // Ažuriraj listu svih karata i sačuvaj je nazad u fajl
                    Karta.SacuvajUFajl(AzurirajKarte(k), kartaFajl);
                    dgKarte.Items.Refresh(); // Osveži prikaz u DataGrid-u

                    // Ažuriraj broj slobodnih mesta za termin
                    if (cmbTermini.SelectedItem is Termin izabraniTermin)
                    {
                        izabraniTermin.SlobodnaMesta--;
                        // Pronađi i ažuriraj taj termin u glavnoj listi termina
                        var sveTermine = Termin.UcitajIzFajla(terminFajl);
                        var terminZaAzuriranje = sveTermine.FirstOrDefault(t =>
                            t.NazivFilma == izabraniTermin.NazivFilma &&
                            t.DatumVreme == izabraniTermin.DatumVreme &&
                            t.Sala == izabraniTermin.Sala);

                        if (terminZaAzuriranje != null)
                        {
                            terminZaAzuriranje.SlobodnaMesta = izabraniTermin.SlobodnaMesta;
                            Termin.SacuvajUFajl(sveTermine, terminFajl);
                            // Ponovo učitaj termine da se ComboBox osveži
                            UcitajTermine();
                        }
                    }
                    MessageBox.Show("Karta je uspešno kupljena.", "Kupovina", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ova karta je već kupljena.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Molimo izaberite kartu za kupovinu.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private List<Karta> AzurirajKarte(Karta kartaZaAzuriranje)
        {
            var sveKarte = Karta.UcitajIzFajla(kartaFajl);
            for (int i = 0; i < sveKarte.Count; i++)
            {
                if (sveKarte[i].NazivFilma == kartaZaAzuriranje.NazivFilma &&
                    sveKarte[i].DatumVreme == kartaZaAzuriranje.DatumVreme &&
                    sveKarte[i].BrojSedista == kartaZaAzuriranje.BrojSedista)
                {
                    sveKarte[i] = kartaZaAzuriranje;
                    break;
                }
            }
            return sveKarte;
        }
    }
}