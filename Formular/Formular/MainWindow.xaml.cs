using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Formular
{
    public partial class MainWindow : Window
    {
        private bool isTurkish = true;

        private string GenerateQRCode(string content, string filePath)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        string imagePath = System.IO.Path.ChangeExtension(filePath, ".png");
                        qrCodeImage.Save(imagePath, ImageFormat.Png);
                        System.Windows.MessageBox.Show($"QR-Code gespeichert unter: {imagePath}");
                        return imagePath;
                    }
                }
            }
        }


        public User UserData { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.UserData = new User();
            this.DataContext = this.UserData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserData.Vorname))
            {
                System.Windows.MessageBox.Show(isTurkish ? "Lütfen geçerli bir ad girin." : "Bitte geben Sie einen gültigen Vornamen ein.");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserData.Name))
            {
                System.Windows.MessageBox.Show(isTurkish ? "Lütfen geçerli bir soyad girin." : "Bitte geben Sie einen gültigen Nachnamen ein.");
                return;
            }

            if (UserData.Geburtsdatum == null || UserData.Geburtsdatum == DateTime.MinValue)
            {
                System.Windows.MessageBox.Show(isTurkish ? "Lütfen geçerli bir doğum tarihi seçin." : "Bitte wählen Sie ein gültiges Geburtsdatum aus.");
                return;
            }

            if (!IsValidEmail(UserData.Email))
            {
                System.Windows.MessageBox.Show(isTurkish ? "Lütfen geçerli bir e-posta adresi girin." : "Bitte geben Sie eine gültige E-Mail-Adresse ein.");
                return;
            }

            if (!IsValidPhoneNumber(UserData.Telefonnummer))
            {
                System.Windows.MessageBox.Show(isTurkish ? "Lütfen geçerli bir telefon numarası girin." : "Bitte geben Sie eine gültige Telefonnummer ein.");
                return;
            }

            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.InitialDirectory = @"C:\Users\user01\OneDrive\Documents";
            saveDialog.Title = isTurkish ? "Dosyanızı Buraya Kaydedin" : "Save Your File Here";
            saveDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == true)
            {
                string filePath = saveDialog.FileName;
                string imagePath = null; // ImagePath initialisieren

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("Information:");

                        if (isTurkish)
                        {
                            writer.WriteLine($"Soyad: {UserData.Name}");
                            writer.WriteLine($"Ad: {UserData.Vorname}");
                            writer.WriteLine($"E-Postasi: {UserData.Email}");
                            writer.WriteLine($"Telefon numarasi: {UserData.Telefonnummer}");
                            writer.WriteLine($"Doğum tarihi: {UserData.Geburtsdatum.ToString("dd.MM.yyyy")}");
                        }
                        else
                        {
                            writer.WriteLine($"Nachname: {UserData.Name}");
                            writer.WriteLine($"Vorname: {UserData.Vorname}");
                            writer.WriteLine($"E-Mail: {UserData.Email}");
                            writer.WriteLine($"Telefonnummer: {UserData.Telefonnummer}");
                            writer.WriteLine($"Geburtsdatum: {UserData.Geburtsdatum.ToString("dd.MM.yyyy")}");
                        }

                        if (isTurkish)
                        {
                            if (UserData.IsMisafirSelected)
                                writer.WriteLine("Bölge: Misafir");
                            else if (UserData.IsKaleArkasiSelected)
                                writer.WriteLine("Bölge: Kale Arkası");
                            else if (UserData.IsUstKatSelected)
                                writer.WriteLine("Bölge: Üst Kat");
                            else if (UserData.IsAltKatSelected)
                                writer.WriteLine("Bölge: Alt Kat");
                            else if (UserData.IsVIPAndClubLevelSelected)
                                writer.WriteLine("Bölge: VIP and Club Level");
                            else
                                writer.WriteLine("Bölge: Keine Auswahl");
                        }
                        else
                        {
                            if (UserData.IsMisafirSelected)
                                writer.WriteLine("Region: Gast");
                            else if (UserData.IsKaleArkasiSelected)
                                writer.WriteLine("Region: Hinter Tor");
                            else if (UserData.IsUstKatSelected)
                                writer.WriteLine("Region: Oben");
                            else if (UserData.IsAltKatSelected)
                                writer.WriteLine("Region: Unten");
                            else if (UserData.IsVIPAndClubLevelSelected)
                                writer.WriteLine("Region: VIP und Club Level");
                            else
                                writer.WriteLine("Region: Keine Auswahl");
                        }

                        string qrCodeContent = isTurkish
                            ? $"Soyad: {UserData.Name}\nAd: {UserData.Vorname}\nE-Postasi: {UserData.Email}\nTelefon numarasi: {UserData.Telefonnummer}\nDoğum tarihi: {UserData.Geburtsdatum.ToString("dd.MM.yyyy")}"
                            : $"Nachname: {UserData.Name}\nVorname: {UserData.Vorname}\nE-Mail: {UserData.Email}\nTelefonnummer: {UserData.Telefonnummer}\nGeburtsdatum: {UserData.Geburtsdatum.ToString("dd.MM.yyyy")}";

                        imagePath = GenerateQRCode(qrCodeContent, filePath); // Bildpfad speichern
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(isTurkish ? "Dosya kaydedilirken bir hata oluştu: " : "Fehler beim Speichern der Datei: " + ex.Message);
                }

                string selectedRegion = isTurkish ? "Bölge: " : "Region: ";

                if (UserData.IsMisafirSelected)
                    selectedRegion += isTurkish ? "Misafir" : "Gast";
                else if (UserData.IsKaleArkasiSelected)
                    selectedRegion += isTurkish ? "Kale Arkası" : "Hinter Tor";
                else if (UserData.IsUstKatSelected)
                    selectedRegion += isTurkish ? "Üst Kat" : "Oben";
                else if (UserData.IsAltKatSelected)
                    selectedRegion += isTurkish ? "Alt Kat" : "Unten";
                else if (UserData.IsVIPAndClubLevelSelected)
                    selectedRegion += isTurkish ? "VIP ve Club Levssel" : "VIP und Club Level";
                else
                    selectedRegion += isTurkish ? "Keine Auswahl" : "Keine Auswahl";

                // Nach dem Speichern der Datei eine E-Mail senden
                string emailContent = isTurkish
                    ? $"Soyad: {UserData.Name}\nAd: {UserData.Vorname}\nE-Postasi: {UserData.Email}\nTelefon numarasi: {UserData.Telefonnummer}\nDoğum tarihi: {UserData.Geburtsdatum.ToString("dd.MM.yyyy")} {selectedRegion}"
                    : $"Nachname: {UserData.Name}\nVorname: {UserData.Vorname}\nE-Mail: {UserData.Email}\nTelefonnummer: {UserData.Telefonnummer}\nGeburtsdatum: {UserData.Geburtsdatum.ToString("dd.MM.yyyy")}{selectedRegion}";

                string result = SendEmailWithAttachment(UserData.Email, "Ihre Benutzerdaten", emailContent, imagePath);
                if (result == "success")
                {
                    System.Windows.MessageBox.Show(isTurkish ? "E-posta başarıyla gönderildi." : "E-Mail wurde erfolgreich gesendet.");
                }
                else
                {
                    System.Windows.MessageBox.Show(isTurkish ? "E-posta gönderilirken bir hata oluştu: " + result : "Fehler beim Versenden der E-Mail: " + result);
                }
            }
        }



        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\d+$");
        }

        private void ImageToggle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isTurkish = !isTurkish;
            foreach (var control in FindVisualChildren<System.Windows.Controls.CheckBox>(this))
            {
                if (control.Content.ToString() == "Kale Arkası")
                    control.Content = "Hinter Tor";
                else if (control.Content.ToString() == "Hinter Tor")
                    control.Content = "Kale Arkası";
                else if (control.Content.ToString() == "Misafir")
                    control.Content = "Gast";
                else if (control.Content.ToString() == "Gast")
                    control.Content = "Misafir";
                else if (control.Content.ToString() == "Üst Kat")
                    control.Content = "Oben";
                else if (control.Content.ToString() == "Oben")
                    control.Content = "Üst Kat";
                else if (control.Content.ToString() == "Alt Kat")
                    control.Content = "Unten";
                else if (control.Content.ToString() == "Unten")
                    control.Content = "Alt Kat";
                else if (control.Content.ToString() == "Kullanıcı Sözleşmesi'nı ve Gizlilik Politikası'nı okudum ve kabul ediyorum")
                    control.Content = "Ich habe die Nutzungsbedingungen und die Datenschutzrichtlinie gelesen und akzeptiere sie.";
                else if (control.Content.ToString() == "Ich habe die Nutzungsbedingungen und die Datenschutzrichtlinie gelesen und akzeptiere sie.")
                    control.Content = "Kullanıcı Sözleşmesi'nı ve Gizlilik Politikası'nı okudum ve kabul ediyorum";
            }

            foreach (var control in FindVisualChildren<System.Windows.Controls.Label>(this))
            {
                if (control.Content.ToString() == "Soyad")
                    control.Content = "Nachname";
                else if (control.Content.ToString() == "Nachname")
                    control.Content = "Soyad";
                else if (control.Content.ToString() == "Ad")
                    control.Content = "Vorname";
                else if (control.Content.ToString() == "Vorname")
                    control.Content = "Ad";
                else if (control.Content.ToString() == "Telefon numarasi")
                    control.Content = "Telefonnummer";
                else if (control.Content.ToString() == "Telefonnummer")
                    control.Content = "Telefon numarasi";
                else if (control.Content.ToString() == "E-Postasi")
                    control.Content = "E-Mail";
                else if (control.Content.ToString() == "E-Mail")
                    control.Content = "E-Postasi";
                else if (control.Content.ToString() == "Doğum tarihi")
                    control.Content = "Geburtsdatum";
                else if (control.Content.ToString() == "Geburtsdatum")
                    control.Content = "Doğum tarihi";
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void VornameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }


        private string SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("acikgozyagmur2007@gmail.com"), // Absendert
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                message.To.Add(new MailAddress(toEmail));

                // Konfiguriere den SMTP-Client
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("acikgozyagmur2007@gmail.com", "olpd omyk ufsk lubh"); // App - Passwort 
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }

                return "success";
            }
            catch (SmtpException ex)
            {
                return $"SMTP Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"General Error: {ex.Message}";
            }
        }

        private string SendEmailWithAttachment(string toEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("acikgozyagmur2007@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                // Füge die Empfängeradresse hinzu
                message.To.Add(new MailAddress(toEmail));

                // Füge den Anhang hinzu, wenn der Pfad angegeben ist
                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    // Überprüfe, ob die Datei existiert
                    if (File.Exists(attachmentPath))
                    {
                        Attachment attachment = new Attachment(attachmentPath);
                        message.Attachments.Add(attachment);
                    }
                    else
                    {
                        return "Error: The file does not exist.";
                    }
                }

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) // SMTP-Server und Port für Gmail
                {
                    smtp.EnableSsl = true; // SSL/TLS aktivieren
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("acikgozyagmur2007@gmail.com", "olpd omyk ufsk lubh"); // Dein Passwort oder App-Passwort
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }

                return "success";
            }
            catch (SmtpException ex)
            {
                return $"SMTP Error: {ex.StatusCode} - {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"General Error: {ex.Message}";
            }
        }

    }
}
