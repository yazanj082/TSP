using Common.Models;
using SearchTechniqe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTechniqe.Implementations
{
    public sealed class SimulatedAnnealing : ISearch
    {
        private int _citiesNumber;
        private int[,] _graph;
        private List<string> _cities;
        private List<int> costs;
        public SimulatedAnnealing(int[,] graph, List<string> cities, int citiesNumber)
        {
            this._graph = graph;
            this._citiesNumber = citiesNumber;
            this._cities = cities;
            
        }
        public SearchResult ShortestPath()
        {
            double tempreture = 100;
            List<int> vertex = new List<int>();
            List<int> best;
            List<int> globalBest = new List<int>();
            var finalResult = new SearchResult();
            for (int i=0; tempreture > 1; i++)
            {
                this.costs = new List<int>();
                best = GenerateRandomPermutation();
                for (int j=0;  j < 2* _citiesNumber; j++)
                {
                    vertex = best;
                    best = FindNextPermutation(vertex, tempreture);
                    
                }
                if (CalculatePathWeight(globalBest) > CalculatePathWeight(best) || i == 0)
                { 
                    globalBest = best;
                    finalResult.IterationCost = costs;
                }
                tempreture -=  0.1/(_citiesNumber*1.0);

            }
            string result = "";
            var min_path = CalculatePathWeight(globalBest);
            globalBest.RemoveAt(globalBest.Count - 1);
            foreach (int i in globalBest)
            {
                result += _cities[i] + "=>";
            }
            finalResult.Cost = min_path;
            finalResult.Result = result + _cities[globalBest[0]] + "  " + min_path;
            return finalResult;
        }
        List<int> FindNextPermutation(List<int> vertex,double tempreture)
        {
            int best = CalculatePathWeight(vertex);
            List<int> bestResult = new List<int>();
            foreach (int flag in vertex)
            {
                bestResult.Add(flag);
            }
            int temp;
            int z;

            for (int i = 1; i < vertex.Count - 2; i++)
            {
                for (int j = i + 1; j < vertex.Count - 1; j++)
                {
                    z = vertex[i];
                    vertex[i] = vertex[j];
                    vertex[j] = z;
                    temp = CalculatePathWeight(vertex);
                    /*if (temp < best)
                    {
                        //best = temp;
                        bestResult.Clear();
                        foreach (int flag in vertex)
                        {
                            bestResult.Add(flag);
                        }
                        costs.Add(temp);
                        return bestResult;

                    }*/
                    if(ChooseByTempreture(tempreture, best, temp))
                    {
                        bestResult.Clear();
                        foreach (int flag in vertex)
                        {
                            bestResult.Add(flag);
                        }
                        costs.Add(temp);
                        return bestResult;
                    }
                    z = vertex[i];
                    vertex[i] = vertex[j];
                    vertex[j] = z;
                }
            }
            return vertex;
        }
        bool ChooseByTempreture(double tempreture,int vc,int vn)
        {
            Random random = new Random();
            var rand = random.NextDouble();
            var e = Math.Exp(1);
            var formula = (Math.Pow(e,  -1*(vn - vc) / tempreture));

            return (rand < formula);
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
