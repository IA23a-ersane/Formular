﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Formular
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {

        Mitarbeiter newmitarbeiter = new Mitarbeiter();

        public Mitarbeiter MitarbeiterData {  get; set; }


        public MainWindow()
        {
            InitializeComponent();
            this.MitarbeiterData = new Mitarbeiter();
            this.DataContext = this.MitarbeiterData;
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = @"C:\Users\user01\OneDrive\Documents";
            saveDialog.Title = "Save Your File Here";
            saveDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == true)
            {
                string filePath = saveDialog.FileName;

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Schreibe die Daten der TextBoxen und Labels in die Datei
                        writer.WriteLine("Form Data:");
                        writer.WriteLine($"Name: {MitarbeiterData.Name}");
                        writer.WriteLine($"Vorname: {MitarbeiterData.Vorname}");
                        writer.WriteLine($"Email: {MitarbeiterData.Email}");
                        writer.WriteLine($"Abteilung: {MitarbeiterData.Abteilung}");
                        writer.WriteLine($"Telefonnummer: {MitarbeiterData.Telefonnummer}");
                        writer.WriteLine($"Geburtsdatum: {MitarbeiterData.Geburtsdatum.ToString("dd.MM.yyyy")}"); // Das Datum wird im Format "dd.MM.yyyy" formatiert, um sicherzustellen, 
                        writer.WriteLine($"Eintrittsdatum: {MitarbeiterData.Eintrittsdatum.ToString("dd.MM.yyyy")}"); // dass nur das Datum ohne die Uhrzeit gespeichert wird.

                    }
                }
                catch (Exception ex)
                {
                    // Zeige eine Fehlermeldung an, falls beim Schreiben der Datei ein Fehler auftritt
                    MessageBox.Show("Fehler beim Speichern der Datei: " + ex.Message);
                }
            }

            string message = MitarbeiterData.Name;
            string m = MitarbeiterData.Email;
            MessageBox.Show($"The name is: {message}: The email is {m}"); // Nachricht anzeigen
        }

        private void VornameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}