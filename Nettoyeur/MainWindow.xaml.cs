using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Xml.Serialization;

namespace Nettoyeur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _SoftwareName = "MonLogiciel";
        string version = "0.0.0";
        public DirectoryInfo WinTemp;
        public DirectoryInfo AppTemp;
        public MainWindow()
        {
            InitializeComponent();
            SetAppName();
            WinTemp = new DirectoryInfo(@"C:\Windows\Temp");
            AppTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            CheckActu();
            GetDate();



        }

        public void CheckActu()
        {
            try
            {
                string url = "http://localhost:81/CleanMyComputer/informationPrimaire.txt";
                using (WebClient client = new WebClient())
                {
                    string information = client.DownloadString(url);
                    if (information != String.Empty)
                    {
                        actu.Content = information;
                        actu.Visibility = Visibility.Visible;
                        bandeau.Visibility = Visibility.Visible;
                    }
                };
            } catch (Exception error)
            {
                Console.WriteLine("Erreur : " + error.Message);   
            }
            
        }

        
        public void CheckVersion()
        {
            string url = "http://localhost:81/CleanMyComputer/version.txt";

            using (WebClient client = new WebClient())
            {
                try
                {
                    string v = client.DownloadString(url);
                    if (version != v)
                    {
                        MessageBox.Show("Une nouvelle version est disponible !", "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Votre logiciel est à jour", "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                } catch 
                {
                    MessageBox.Show("Connection au serveur impossible.");
                }
                
            };
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

        /// <summary>
        /// Nettoie le presse papier, le dossier WinTemp et AppTemp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNettoyage(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            
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
            MessageBox.Show("Fonction à développer", caption: "historique", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Verifie la version du logiciel depuis un fichier texte sur le serveur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMaj(object sender, RoutedEventArgs e)
        {
            CheckVersion();
        }

        /// <summary>
        /// A l'appui du bouton, redirige l'utilisateur vers une page web
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMoreInfo(object sender, RoutedEventArgs e)
        {
            try
            {
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
            SaveDate();
        }
       
        public void SaveDate()
        {
            string DateOfLastAnalyse = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy");
            File.WriteAllText("date.txt", DateOfLastAnalyse);   
        }

        public void GetDate()
        {
            if (File.Exists("date.txt"))
            {
                string LastAnalyseDate = File.ReadAllText("date.txt");
                if (LastAnalyseDate != String.Empty)
                {
                    date.Content = LastAnalyseDate;
                }
            }   
        }
    }
}
