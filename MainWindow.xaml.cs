using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfImageViewerApp
{
    public partial class MainWindow : Window
    {
        private List<string> imagePaths = new List<string>();
        private int currentImageIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new System.Uri(path);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            image.Source = bitmap;
        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|All files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filePath = dlg.FileName;
                imagePaths.Add(filePath);
                currentImageIndex = imagePaths.Count - 1;
                LoadImage(filePath);
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (imagePaths.Count == 0) return;
            currentImageIndex--;
            if (currentImageIndex < 0) currentImageIndex = imagePaths.Count - 1;
            LoadImage(imagePaths[currentImageIndex]);
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (imagePaths.Count == 0) return;
            currentImageIndex++;
            if (currentImageIndex >= imagePaths.Count) currentImageIndex = 0;
            LoadImage(imagePaths[currentImageIndex]);
        }

        private void cropButton_Click(object sender, RoutedEventArgs e)
        {
            if (imagePaths.Count != 0)
            {
                CropWindow cropWindow = new CropWindow(image.Source as BitmapSource);
                bool? result = cropWindow.ShowDialog();
                if (result == true)
                {
                    image.Source = cropWindow.CroppedImage;
                }
            }
        }
    }
}
