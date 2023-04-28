using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfImageViewerApp
{
    public partial class CropWindow : Window
    {
        private BitmapImage bitmap;
        private double startX, startY, endX, endY;
        private bool isDragging = false;
        private BitmapSource bitmapSource;

        public CroppedBitmap CroppedImage { get; private set; }

        public CropWindow(BitmapImage bitmap)
        {
            InitializeComponent();
            this.bitmap = bitmap;
            image.Source = bitmap;
        }

        public CropWindow(BitmapSource bitmapSource)
        {
            this.bitmapSource = bitmapSource;
        }

        public CroppedBitmap GetCroppedImage()
        {
            if (selection.Visibility == Visibility.Visible)
            {
                
                double x = Canvas.GetLeft(selection);
                double y = Canvas.GetTop(selection);
                double width = selection.ActualWidth;
                double height = selection.ActualHeight;

                // Convert the selection dimensions to pixel values
                double dpiScale = bitmap.DpiX / 96.0;
                x *= dpiScale;
                y *= dpiScale;
                width *= dpiScale;
                height *= dpiScale;

                // Create and return the cropped bitmap
                return new CroppedBitmap(bitmap, new Int32Rect((int)x, (int)y, (int)width, (int)height));
            }
            else
            {
                // Return the original bitmap
                return new CroppedBitmap(bitmap, new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                startX = e.GetPosition(canvas).X;
                startY = e.GetPosition(canvas).Y;
                endX = startX;
                endY = startY;
                Canvas.SetLeft(selection, startX);
                Canvas.SetTop(selection, startY);
                selection.Width = 0;
                selection.Height = 0;
                selection.Visibility = Visibility.Visible;
                isDragging = true;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                endX = e.GetPosition(canvas).X;
                endY = e.GetPosition(canvas).Y;
                double x = startX < endX ? startX : endX;
                double y = startY < endY ? startY : endY;
                double width = startX < endX ? endX - startX : startX - endX;
                double height = startY < endY ? endY - startY : startY - endY;
                Canvas.SetLeft(selection, x);
                Canvas.SetTop(selection, y);
                selection.Width = width;
                selection.Height = height;
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void cropButton_Click(object sender, RoutedEventArgs e)
        {
            CroppedImage = GetCroppedImage();
            DialogResult = true;
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
