using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nettoyeur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _SoftwareName = "MonLogiciel";
        public DirectoryInfo WinTemp;
        public DirectoryInfo AppTemp;
        public MainWindow()
        {
            InitializeComponent();
            SetAppName();
            WinTemp = new DirectoryInfo(@"C:\Windows\Temp");
            AppTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            

        }

        public long DirectorySize(DirectoryInfo directory)
        {
            // Calcul la taille du dossier fournit en paramètre
            // Somme de la taille de tout les fichiers du répertoire : directory.GetFiles().Sum(fichier => fichier.Length)
            // Somme de la taille de tout les dossiers du répertoire : directory.GetDirectories().Sum(dossier => DirectorySize(dossier))

            return directory.GetFiles().Sum(fichier => fichier.Length) + directory.GetDirectories().Sum(dossier => DirectorySize(dossier));
        }

        public void ClearTempData(DirectoryInfo directory)
        {
            foreach (FileInfo fichier in directory.GetFiles())
            {
                try
                {
                    fichier.Delete();
                    Console.WriteLine(fichier.FullName);
                    //totalRemovedFiles++; Si besoin statistique
                }
                catch (Exception error)
                {
                    Console.WriteLine("Erreur : " + error.Message);
                }
            }

            foreach (DirectoryInfo repertoire in directory.GetDirectories())
            {
                try
                {
                    repertoire.Delete(true); // true car on supprime aussi tout ce qui a l'intérieur d'un dossier
                    Console.WriteLine(repertoire.FullName);
                    //totalRemovedDirectories++; Si besoin statistique
                }
                catch (Exception error)
                {
                    Console.WriteLine("Erreur : " + error.Message);
                }
            }
        }

        public string SoftwareTitle
        {
            get { return _SoftwareName; }
            set { _SoftwareName = value; }

        }

        public void SetAppName()
        {
            appName.Title = SoftwareTitle;
        }

        private void ButtonAnalyse(object sender, RoutedEventArgs e)
        {
            AnalyseFolders();
        }

        private void ButtonNettoyage(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear(); // Vide le presse-papier
            
            try
            {
                ClearTempData(WinTemp);
            } 
            catch (Exception error)
            {
                Console.WriteLine("Erreur : " + error.Message);
            }

            try
            {
                ClearTempData(AppTemp);
            }
            catch (Exception error)
            {
                Console.WriteLine("Erreur : " + error.Message);
            }

            btnCleanMessage.Content = "Nettoyage terminé !";
            Titre.Content = "Le nettoyage a été effectué";
            espace.Content = "0 Mb";
        }

        private void ButtonHistorique(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonMaj(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Votre logiciel est à jour !", caption: SoftwareTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonMoreInfo(object sender, RoutedEventArgs e)
        {
            // Bonne pratique si jamais absence chez l'utilisateur de navigateur web
            try
            {
                // Pour ouvrir une page web depuis le logiciel
                Process.Start(new ProcessStartInfo("https://github.com/Rijenth")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception error)
            {
                Console.WriteLine("Erreur : " + error.Message);
            }
        }

        public void AnalyseFolders()
        {
            Console.WriteLine("Début de l'analyse...");
            long totalSize = 0;

            // Donnée sensible, bonne pratique car privilège admin
            try
            {
                // DirectorySize(WinTemp) renvoie une taille en octet, on divise afin d'avoir une valeur en Mb.
                totalSize += DirectorySize(WinTemp) / 1000000;
                totalSize += DirectorySize(AppTemp) / 1000000;
            }
            catch (Exception error)
            {
                Console.WriteLine("Impossible d'analyser les dossiers : " + error.Message);
            }
            
            // On met à jour les champs de l'application
            espace.Content = totalSize + " Mb à nettoyer";
            Titre.Content = "Analyse effectuée";
            date.Content = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy");
        }
    }
}
