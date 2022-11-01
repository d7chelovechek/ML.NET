namespace Classifier
{
    internal class Program
    {
        public static void Main()
        {
            var symptoms = File.ReadAllLines("Files/symptom.csv").ToList();
            var diseases = File.ReadAllLines("Files/disease.csv").ToList();

            var symptomsNames = symptoms.Skip(1).Select(s => s.Split(';')[0]).ToList();
            var diseasesNames = symptoms[0].Split(';').Skip(1).ToList();
            var patientsCount = Convert.ToInt32(diseases[^1].Split(';')[1]);

            symptoms = symptoms.Skip(1).ToList();

            var diseasesNamesCount = diseasesNames.Count;
            var symptomsCount = symptoms.Count;

            var diseasesMatrix = 
                    diseases
                    .Skip(1)
                    .SkipLast(1)
                    .Select(d => Convert.ToInt32(d.Split(';')[1]))
                    .ToList();

            var symptomsMatrix = new List<List<double>>();
            for (var index = 0; index < symptomsCount; index++)
            {
                symptomsMatrix.Add(
                    symptoms[index]
                        .Split(';')
                        .Skip(1)
                        .Select(s => Convert.ToDouble(s.Replace('.', ',')))
                        .ToList());
            }

            var probabilities = new List<int>();
            for (var index = 0; index < symptomsCount; index++)
            {
                _ = int.TryParse(Console.ReadLine(), out int result);

                probabilities.Add(result > 1 ? 1 : result < 0 ? 0 : result);
            }

            Console.WriteLine();
            for (var index = 0; index < probabilities.Count; index++)
            {
                if (probabilities[index] > 0)
                {
                    Console.WriteLine(symptomsNames[index]);
                }
            }
            Console.WriteLine();

            var algorithm = new Algorithm();
            algorithm.Start(
                symptomsMatrix,
                diseasesMatrix, 
                patientsCount);

            Console.WriteLine(
                algorithm.GetDisease(
                    probabilities, 
                    diseasesNames));
        }
    }
}