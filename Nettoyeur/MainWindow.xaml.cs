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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace CleanMyComputer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _softwareName = "Clean My Computer";
        private string _version = "0.0.1";
        private long _overallCleanedSize;
        private long _hasBeenCleaned;
        private bool _analyseCompleted;
        public DirectoryInfo WinTemp;
        public DirectoryInfo AppTemp;
        public MainWindow()
        {
            InitializeComponent();
            SetAppName();
            Version();
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

                    if (information != string.Empty)
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

        public void Version()
        {
            softwareVersion.Content = "Version du logiciel : " + _version;
        }
        
        public void CheckVersion()
        {
            string url = "http://localhost:81/CleanMyComputer/version.txt";

            using (WebClient client = new WebClient())
            {
                try
                {
                    string v = client.DownloadString(url);
                    if (_version != v)
                    {
                        MessageBox.Show(
                            "Une nouvelle version est disponible !", 
                            "Mise à jour", MessageBoxButton.OK, 
                            MessageBoxImage.Information
                            );

                        _version = v;

                        Version();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Votre logiciel est à jour", 
                            "Mise à jour", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information
                            );
                    }
                } catch 
                {
                    MessageBox.Show("Connexion au serveur impossible.");
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

        public void SetAppName()
        {
            appName.Title = _softwareName;
        }

        private void ButtonAnalyse(object sender, RoutedEventArgs e)
        {
            btnCleanMessage.Content = "Nettoyage";

            AnalyseFolders();

            _analyseCompleted = true;
        }

        private void ButtonNettoyage(object sender, RoutedEventArgs e)
        {
            if (_analyseCompleted)
            {
                Nettoyage();

                MessageBox.Show(
                    "Vous avez effectivement nettoyé " + _hasBeenCleaned + " MB",
                    caption: _softwareName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Il faut lancer une analyse avant de pouvoir utiliser la fonction de nettoyage.",
                    caption: _softwareName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            
        }


        private void ButtonHistorique(object sender, RoutedEventArgs e)
        {
            if (File.Exists("historique.txt"))
            {
                string data = File.ReadAllText("historique.txt");

                long totalCleanedSize = Convert.ToInt64(data.Substring(data.IndexOf('/') + 1));

                MessageBox.Show(
                    "Depuis que vous utilisez ce logiciel, vous avez retiré " + totalCleanedSize + " MB de fichiers et dossiers indésirable.", 
                    caption: _softwareName, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information
                    );
            }
            else
            {
                MessageBox.Show(
                    "Aucun historique disponible. Il faut lancer une première analyse afin d'avoir un historique.",
                    caption:_softwareName, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information
                    );
            }
        }

        private void Nettoyage()
        {
            _hasBeenCleaned = 0;

            Clipboard.Clear();

            try
            {
                long beforeCleaning = DirectorySize(WinTemp) / 1000000;

                ClearTempData(WinTemp);

                long afterCleaning = DirectorySize(WinTemp) / 1000000;

                _hasBeenCleaned += beforeCleaning - afterCleaning;
            }
            catch (Exception error)
            {
                Console.WriteLine("Erreur : " + error.Message);
            }

            try
            {
                long beforeCleaning = DirectorySize(AppTemp) / 1000000;

                ClearTempData(AppTemp);

                long afterCleaning = DirectorySize(AppTemp) / 1000000;


                _hasBeenCleaned += beforeCleaning - afterCleaning;
            }
            catch (Exception error)
            {
                Console.WriteLine("Erreur : " + error.Message);
            }

            btnCleanMessage.Content = "Nettoyage terminé !";

            Titre.Content = "Le nettoyage a été effectué";

            espace.Content = "0 MB";

            UpdateHistorique();
        }

        private void ButtonMaj(object sender, RoutedEventArgs e)
        {
            CheckVersion();
        }

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

            SaveDateAndEspace(totalSize);
            
        }
       
        public void SaveDateAndEspace(long totalSize)
        {
            GetOverallCleanedSize();

            string Info = date.Content + ":" + totalSize + "/" + _overallCleanedSize;

            File.WriteAllText("historique.txt", Info);

            _hasBeenCleaned = 0;

        }

        public void GetDate()
        {
            if (File.Exists("historique.txt"))
            {
                string lastAnalyseDate = File.ReadAllText("historique.txt");
                lastAnalyseDate = lastAnalyseDate.Split(':')[0];

                if (lastAnalyseDate != String.Empty)
                {
                    date.Content = lastAnalyseDate;
                }
            }   
        }

        public void GetOverallCleanedSize()
        {
            if (File.Exists("historique.txt"))
            {
                string data = File.ReadAllText("historique.txt");

                _overallCleanedSize = Convert.ToInt64(data.Substring(data.IndexOf('/') + 1));
            }
            else
            {
                _overallCleanedSize = 0;
            }
        }
        public void UpdateHistorique()
        {
            if (File.Exists("historique.txt"))
            {
                string data = File.ReadAllText("historique.txt");

                string dateOfAnalyse = data.Split(':')[0];

                string totalSize = data.Split('/')[0];

                totalSize = totalSize.Substring(totalSize.IndexOf(':') + 1);

                long totalSizeToLong = Convert.ToInt64(totalSize); 

                GetOverallCleanedSize();

                _overallCleanedSize += _hasBeenCleaned;

                string Info = dateOfAnalyse + ":" + totalSizeToLong + "/" + _overallCleanedSize;

                File.WriteAllText("historique.txt", Info);
            }
        }
    }
}
