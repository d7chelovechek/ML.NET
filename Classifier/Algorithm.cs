namespace Classifier
{
    internal class Algorithm
    {
        private readonly Dictionary<int, double> _probabilities;
        private readonly Dictionary<int, Dictionary<int, double>> _conditionalProbabilities;

        internal Algorithm()
        {
            _probabilities = new Dictionary<int, double>();
            _conditionalProbabilities = new Dictionary<int, Dictionary<int, double>>();
        }

        public void Start(
            List<List<double>> symptoms,
            List<int> diseases,
            int patientsCount)
        {
            for (var index = 0; index < diseases.Count; index++)
            {
                _probabilities.Add(index, (double)diseases[index] / patientsCount);
            }

            for (var index = 0; index < symptoms.Count; index++)
            {
                _conditionalProbabilities.Add(index, new Dictionary<int, double>());

                for (var temp = 0; temp < symptoms[index].Count; temp++)
                {
                    _conditionalProbabilities[index].Add(temp, symptoms[index][temp]);
                }
            }
        }

        public string GetDisease(List<int> probabilities, List<string> diseasesNames)
        {
            var index = -1;
            var maxProb = double.MinValue;

            foreach (int key in _probabilities.Keys)
            {
                var prob = GetDiseaseResult(probabilities, key);

                if (prob >= maxProb)
                {
                    index = key;
                    maxProb = prob;
                }
            }

            return diseasesNames[index];
        }

        private double GetDiseaseResult(
            List<int> probabilities, 
            int index)
        {
            var result = _probabilities[index];

            for (var temp = 0; temp < probabilities.Count; temp++)
            {
                if (probabilities[temp] is 1)
                {
                    result *= _conditionalProbabilities[temp][index];
                }
            }

            return result;
        }
    }
}