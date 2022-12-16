using Common.Models;
using SearchTechniqe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTechniqe.Implementations
{
    public class HillClimping : ISearch
    {
        private int _citiesNumber;
        private int[,] _graph;
        private List<string> _cities;
        private List<int> costs;
        public HillClimping(int[,] graph, List<string> cities, int citiesNumber)
        {
            this._graph = graph;
            this._citiesNumber = citiesNumber;
            this._cities = cities;
        }
        public SearchResult ShortestPath()
        {
            List<int> vertex;
            List<int> best = new List<int>();
            List<int> innerBest;
            int min_path = Int32.MaxValue;
            int innerMin_path;
            int current_pathweight;
            var finalResult = new SearchResult();
            for (int i=0; i< _citiesNumber*2; i++)
            {
                this.costs = new List<int>();
                vertex = GenerateRandomPermutation();
                current_pathweight = CalculatePathWeight(vertex);
                if (i==0)
                {
                    min_path = current_pathweight;
                    best = vertex;
                }
                innerBest = vertex;
                innerMin_path = CalculatePathWeight(innerBest);
                FindNextPermutation(ref innerBest, ref innerMin_path);
                if (innerMin_path < min_path)
                {
                    min_path = innerMin_path;
                    best = innerBest;
                    finalResult.IterationCost = costs;
                }
            }
            string result = "";
            best.RemoveAt(best.Count - 1);
            foreach (int i in best)
            {
                result += _cities[i] + "=>";
            }
            //return result + _cities[best[0]] + "  " + min_path;
            finalResult.Result = result + _cities[best[0]] + "  " + min_path;
            finalResult.Cost = min_path;
            
            return finalResult;  
        }
        void FindNextPermutation(ref List<int> vertex,ref int test)
        {
            int best = test;
            List<int> bestResult = new List<int>();
            foreach (int flag in vertex)
            {
                bestResult.Add(flag);
            }
            int temp;
            int z;
            while (true)
            {
                for (int i =1; i < vertex.Count-2; i++)
                {
                    for(int j=i+1; j < vertex.Count-1; j++)
                    {
                        //result.Clear();
                        //for(int k=0;k<vertex.Count; k++)
                        //{
                        //    if(k == i)
                        //        result.Add(vertex[j]);
                        //    else if(k==j)
                        //        result.Add(vertex[i]);
                        //    else
                        //        result.Add(vertex[k]);
                        //}
                        //temp = CalculatePathWeight(result);
                        z= vertex[i];
                        vertex[i]= vertex[j];
                        vertex[j]=z;
                        temp = CalculatePathWeight(vertex);
                        if ( temp< best)
                        {
                            best = temp;
                            bestResult.Clear();
                            foreach (int flag in vertex)
                            {
                                bestResult.Add(flag);
                            }

                        }
                        z = vertex[i];
                        vertex[i] = vertex[j];
                        vertex[j] = z;
                    }
                }
                if(best<test){
                        test = best;
                    vertex.Clear();
                    foreach (int flag in bestResult)
                    {
                        vertex.Add(flag);
                    }
                    costs.Add(best);
                }
                else if(best == test){
                    return;
                }
                
            }
        }
            private int CalculatePathWeight(List<int> vertex)
        {
            int result=0,k=0, s = 0;
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

            List< int> result = new List<int>();
            List<int> avilable = new List<int>();
            for(int i=1;i<_citiesNumber;i++)
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
