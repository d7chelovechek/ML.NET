using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NeuralNetwork
{
	public partial class MainWindow : Window
	{
		private const string _tempFile = "temp.tif";
		private const int _sizeOfImage = 32;
		private const int _sizeOfArea = 4;
		private const string _modelName = "MLModel.zip";

		public MainWindow()
		{
			InitializeComponent();

			InitializeInkCanvas();
		}

		private void InitializeInkCanvas()
		{
			Paint.DefaultDrawingAttributes.Width = 15;
			Paint.DefaultDrawingAttributes.Height = 15;
			Paint.DefaultDrawingAttributes.Color = Colors.Black;
		}

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			string pressedKey;
			if (!e.Key.Equals(Key.System))
			{
				pressedKey = e.Key.ToString();
			}
			else
			{
				pressedKey = e.SystemKey.ToString();
			}

			switch (pressedKey)
			{
				case "C":
					{
						Paint.Strokes.Clear();

						break;
					}
				case "R":
					{
						if (File.Exists(_tempFile))
						{
							File.Delete(_tempFile);
						}

						using (var stream = new FileStream(_tempFile, FileMode.Create))
						{
							var bitmap = new RenderTargetBitmap(
								(int)Paint.ActualWidth,
								(int)Paint.ActualHeight,
								96,
								96,
								PixelFormats.Default);
							bitmap.Render(Paint);

							var encoder = new TiffBitmapEncoder();
							encoder.Frames.Add(BitmapFrame.Create(bitmap));

							encoder.Save(stream);
						}

						var cvImage = new Image<Gray, byte>(_tempFile).ThresholdBinary(new Gray(0), new Gray(255));
						var vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
						var hier = new Mat();

						CvInvoke.FindContours(
							cvImage, 
							vectorOfVectorOfPoint, 
							hier, 
							RetrType.Tree, 
							ChainApproxMethod.ChainApproxNone);
						var contours = vectorOfVectorOfPoint.ToArrayOfArray();

						var rects = new List<Rectangle>();
						for (var index = 1; index < contours.Length; index++)
						{
							var rect = CvInvoke.BoundingRectangle(contours[index]);
							
							if (!rects.Where(r =>
								!r.Equals(rect) &&
								r.X < rect.X &&
								r.Y < rect.Y &&
								r.Right > rect.Right &&
								r.Bottom > rect.Bottom).Any())
							{
								rects.Add(rect);
							}
						}

						var result = new StringBuilder();
						using (var image = Image.FromFile(_tempFile))
						{
							foreach (Rectangle rect in rects.OrderBy(r => r.X))
							{
								var bitmap = (image as Bitmap)
									.Clone(rect, image.PixelFormat);

								var mlContext = new MLContext();
								var mlModel = mlContext.Model.Load(_modelName, out var modelInputSchema);
								var predEngine =
									mlContext.Model.CreatePredictionEngine<Models.Input, Models.Output>(mlModel);

								var base64Image = string.Empty;
								using (var ms = new MemoryStream())
								{
									using (var tempBitmap = new Bitmap(bitmap))
									{
										tempBitmap.Save(ms, ImageFormat.Jpeg);
										base64Image = Convert.ToBase64String(ms.GetBuffer());
									}
								}

								var datasetValue = GetDatasetValuesFromImage(base64Image);

								var input = new Models.Input
								{
									PixelValues = datasetValue.ToArray()
								};

								result.Append(predEngine.Predict(input).Prediction);
							}
						}

						MessageBox.Show(result.ToString());

						break;
					}
			}
		}

		private static List<float> GetDatasetValuesFromImage(string base64Image)
		{
			var imageBytes = Convert.FromBase64String(base64Image);

			Image image;
			using (var stream = new MemoryStream(imageBytes))
			{
				image = Image.FromStream(stream);
			}

			var res = new Bitmap(_sizeOfImage, _sizeOfImage);
			using (var g = Graphics.FromImage(res))
			{
				g.Clear(System.Drawing.Color.White);
				g.DrawImage(image, 0, 0, _sizeOfImage, _sizeOfImage);
			}

			var datasetValue = new List<float>();

			for (int i = 0; i < _sizeOfImage; i += _sizeOfArea)
			{
				for (int j = 0; j < _sizeOfImage; j += _sizeOfArea)
				{
					var sum = 0;
					for (int k = i; k < i + _sizeOfArea; k++)
					{
						for (int l = j; l < j + _sizeOfArea; l++)
						{
							sum += res.GetPixel(l, k).Name is "ffffffff" ? 0 : 1;
						}
					}
					datasetValue.Add(sum);
				}
			}

			return datasetValue;
		}
	}
}