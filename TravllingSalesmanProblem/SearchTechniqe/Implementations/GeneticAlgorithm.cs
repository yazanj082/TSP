using Common.Models;
using SearchTechniqe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTechniqe.Implementations
{
    public class GeneticAlgorithm : ISearch
    {
        private int _citiesNumber;
        private int[,] _graph;
        private List<string> _cities;
        int POPULATIONSIZE;
        public GeneticAlgorithm(int[,] graph, List<string> cities, int citiesNumber)
        {
            this._graph = graph;
            this._citiesNumber = citiesNumber;
            this._cities = cities;
        }
        class SolutionModel
        {
            public int Cost { get; set; }
            public List<int> Cities { get; set; }
        }
        public SearchResult ShortestPath()
        {
            var finalResult = new SearchResult();
            finalResult.IterationCost = new List<int>();
            List<SolutionModel> population = new List<SolutionModel>();
            POPULATIONSIZE = _citiesNumber * 100;
            var rnd = new Random();
            SolutionModel prev = null,temp;
            for (int i = 0; i < POPULATIONSIZE; i++)
            {
                population.Add(new SolutionModel() { Cities = GenerateRandomPermutation() });
            }
            for (int i = 0;  i< _citiesNumber*10;)
            {
                var offspring = CrossOver(population);
                Mutation(offspring);
                population.AddRange(offspring);
                population = Selection(population);
                temp = population[0];
                finalResult.IterationCost.Add(temp.Cost);
                if (prev != null && temp.Cost == prev.Cost)
                    i++;
                else
                    i = 0;
                prev = temp;
                population = population.OrderBy(item => rnd.Next()).ToList();
            }
            var best = Selection(population)[0];
            string result = "";
            best.Cities.RemoveAt(best.Cities.Count - 1);
            foreach (int i in best.Cities)
            {
                result += _cities[i] + "=>";
            }
            //return result + _cities[best[0]] + "  " + min_path;
            finalResult.Result = result + _cities[best.Cities[0]] + "  " + best.Cost;
            finalResult.Cost = best.Cost;
            return finalResult;
        }

        private List<SolutionModel> Selection(List<SolutionModel> data)
        {
            var result = new List<SolutionModel>();
            GiveCost(data);
            return data.OrderBy(item => item.Cost).Take(POPULATIONSIZE).ToList();
        }
        private void GiveCost(List<SolutionModel> data)
        {
            foreach (var solution in data)
            {
                if(solution.Cost == 0)
                {
                    solution.Cost = CalculatePathWeight(solution.Cities);
                }
            }
        }
        private void Mutation(List<SolutionModel> data)
        {
            Random random = new Random();
            foreach (SolutionModel solution in data)
            {
                var chromosome = solution.Cities;
                var point1 = random.Next(1,chromosome.Count-1);
                var point2 = random.Next(1, chromosome.Count-1);
                int temp;
                temp = chromosome[point1];
                chromosome[point1] = chromosome[point2];
                chromosome[point2] = temp;
            }
        }
        private List<SolutionModel> CrossOver(List<SolutionModel> data)
        {
            List<SolutionModel> results = new List<SolutionModel>();
            SolutionModel child = new SolutionModel();
            var parents = data.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / (data.Count/2))
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
            Random rand = new Random();
            for (int i = 0; i < data.Count/2; i++)
            {
                child = new SolutionModel();
                child.Cities = new List<int>();
                var splitPoint1 = rand.Next(1,_citiesNumber-1);
                //var splitPoint2 = rand.Next(splitPoint1, _citiesNumber - 1);
                for (int j = 0; j < splitPoint1; j++)
                {
                    child.Cities.Add(parents[0][i].Cities[j]);
                }
                foreach(var item in parents[1][i].Cities)
                {
                    if(!child.Cities.Contains(item))
                        child.Cities.Add(item);
                }
                child.Cities.Add(parents[0][i].Cities[0]);
                results.Add(child);
            }
            return results;
        }
        private int CalculatePathWeight(List<int> vertex)
        {
            int result = 0, k = 0, s = 0;
            for (int i = 0; i < vertex.Count; i++)
            {
                result += _graph[k, vertex[i]];
                k = vertex[i];
            }
            result += _graph[k, s];
            return result;
        }
        private List<int> GenerateRandomPermutation()
        {

            List<int> result = new List<int>();
            List<int> avilable = new List<int>();
            for (int i = 1; i < _citiesNumber; i++)
                avilable.Add(i);
            result.Add(0);
            Random rnd = new Random();
            int temp;

            while (avilable.Count > 0)
            {
                temp = rnd.Next(0, avilable.Count);
                result.Add(avilable.ElementAt(temp));
                avilable.Remove(avilable.ElementAt(temp));
            }

            result.Add(0);
            return result;
        }
    }

}
