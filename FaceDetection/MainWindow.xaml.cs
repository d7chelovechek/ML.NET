using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FaceDetection
{
	public partial class MainWindow : Window
	{
		private readonly VideoCapture _capture;
		private readonly CascadeClassifier _cascade;

		[DllImport("gdi32")]
		private static extern int DeleteObject(IntPtr o);

		public MainWindow()
		{
			InitializeComponent();

			_capture = new VideoCapture();
			_cascade = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_ = Task.Run(Tick);
		}

		private async Task Tick()
		{
			while (true)
			{
				var currentFrame = _capture.QueryFrame().ToImage<Bgr, byte>();

				if (currentFrame is not null)
				{
					Image<Gray, byte> grayFrame = currentFrame.Convert<Gray, byte>();

					var detectedFaces = _cascade.DetectMultiScale(
						grayFrame, 
						1.1, 
						1, 
						new System.Drawing.Size(100, 100), 
						new System.Drawing.Size(1920, 1080));

					foreach (Rectangle face in detectedFaces)
					{
						currentFrame.Draw(face, new Bgr(0, 0, 255), 1);
					}

					Dispatcher.Invoke(() =>
					{
						Camera.Source = ToBitmapSource(currentFrame);
					});
				}
				await Task.Delay(1);
			}
		}

		public static BitmapSource ToBitmapSource(Image<Bgr, byte> image)
		{
			using (var ms = new MemoryStream(image.ToJpegData()))
			{
				var bitmap = new Bitmap(ms);

				IntPtr ptr = bitmap.GetHbitmap();

				BitmapSource bs = System.Windows.Interop
				  .Imaging.CreateBitmapSourceFromHBitmap(
					ptr,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

				DeleteObject(ptr);
				bitmap.Dispose();
				return bs;
			}
		}
	}
}