using Microsoft.ML.Data;

namespace NeuralNetwork.Models
{
	public class Output
	{
		[ColumnName("PredictedLabel")]
		public float Prediction { get; set; }
		public float[] Score { get; set; }
	}
}