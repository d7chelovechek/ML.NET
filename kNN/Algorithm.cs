using kNN.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kNN
{
    internal class Algorithm
    {
        private readonly List<Iris> _traneIrises;
        private readonly List<int> _traneIds;

        private readonly List<int> _predictions;

        private readonly int _k;

        internal Algorithm(
            List<Iris> irises,
            int k)
        {
            _traneIrises = irises;
            _traneIds = irises.Select(i => i.Id).ToList();

            _predictions = new List<int>();

            _k = k;
        }

        public void Predict(List<Iris> testIrises)
        {
            for (var index = 0; index < testIrises.Count; index++)
            {
                _predictions.Add(GetTargetId(testIrises, index));
            }
        }

        private int GetTargetId(List<Iris> testIrises, int index)
        {
            var distances = new List<(int, double)>();
            var targets = new Dictionary<int, int>();

            for (var temp = 0; temp < _traneIrises.Count; temp++)
            {
                distances.Add((
                    temp,
                    GetDistance(_traneIrises[temp], testIrises[index])));
            }
            distances = distances.OrderBy(d => d.Item2).ToList();

            for (var temp = 0; temp < _k; temp++)
            {
                var distanceId = distances[temp].Item1;

                if (targets.TryGetValue(_traneIds[distanceId], out _))
                {
                    targets[_traneIds[distanceId]]++;
                }
                else
                {
                    targets.Add(_traneIds[distanceId], 1);
                }
            }

            return targets
                .OrderByDescending(x => x.Value)
                .First().Key;
        }

        public double GetScore(List<int> testIds)
        {
            var result = 0;

            for (var index = 0; index < testIds.Count; index++)
            {
                if (testIds[index].Equals(_predictions[index]))
                {
                    result++;
                }
            }

            return (double)result / testIds.Count;
        }

        private double GetDistance(Iris trane, Iris test)
        {
            return Math.Sqrt(
                Math.Pow(trane.Params[0] - test.Params[0], 2) + 
                Math.Pow(trane.Params[1] - test.Params[1], 2));
        }

        public int GetIrisId(Iris newIris)
        {
            return GetTargetId(new List<Iris>() { newIris }, 0);
        }
    }
}