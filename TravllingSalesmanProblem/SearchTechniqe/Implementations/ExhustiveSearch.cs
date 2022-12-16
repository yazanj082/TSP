using Common.Models;
using SearchTechniqe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTechniqe.Implementations
{
    public sealed class ExhustiveSearch : ISearch
    {
		private int _citiesNumber;
		private int[,] _graph;
		private List<string> _cities;
		// C# program to implement
		// traveling salesman problem
		// using naive approach.
		public ExhustiveSearch(int[,] graph,List<string>cities, int citiesNumber)
        {
			this._graph = graph;
			this._citiesNumber = citiesNumber;
			this._cities = cities;
        }

		// implementation of traveling Salesman Problem
		public SearchResult ShortestPath()
		{
			int s = 0;
			List<int> vertex = new List<int>();
			
			for (int i = 0; i < _citiesNumber; i++)
					vertex.Add(i);
			vertex.Add(vertex[0]);
			// store minimum weight
			// Hamiltonian Cycle.
			int min_path = Int32.MaxValue;
			List<int> best = new List<int>();
			do
			{
				
				// store current Path weight(cost)
				int current_pathweight = 0;
				int k = s;

				// compute current path weight
				for (int i = 0; i < vertex.Count; i++)
				{
					current_pathweight += _graph[k, vertex[i]];
					k = vertex[i];
				}

				current_pathweight += _graph[k, s];

				if(current_pathweight < min_path)
                {
					best.Clear();
					foreach(int i in vertex)
						best.Add(i);

				}
				// update minimum
				min_path
					= Math.Min(min_path, current_pathweight);

			} while (FindNextPermutation(vertex));
			string result="";
			best.RemoveAt(best.Count-1);
			foreach(int i in best)
            {
				result += _cities[i]+"=>";
            }
			
			return new SearchResult() { Result =result + _cities[best[0]] + "  " + min_path ,Cost = min_path };
		}

		// Function to swap the data resent in the left and
		// right indices
		List<int> Swap(List<int> data, int left,int right)
		{
			// Swap the data
			int temp = data[left];
			data[left] = data[right];
			data[right] = temp;

			// Return the updated array
			return data;
		}

		// Function to reverse the sub-array starting from left
		// to the right both inclusive
		List<int> Reverse(List<int> data,int left, int right)
		{
			// Reverse the sub-array
			while (left < right)
			{
				int temp = data[left];
				data[left++] = data[right];
				data[right--] = temp;
			}

			// Return the updated array
			return data;
		}

		// Function to find the next permutation of the given
		// integer array
		bool FindNextPermutation(List<int> data)
		{
			// If the given dataset is empty
			// or contains only one element
			// next_permutation is not possible
			if (data.Count <= 1)
				return false;
			int last = data.Count - 2;

			// find the longest non-increasing
			// suffix and find the pivot
			while (last >= 1)
			{
				if (data[last] < data[last + 1])
					break;
				last--;
			}

			// If there is no increasing pair
			// there is no higher order permutation
			if (last < 1)
				return false;
			int nextGreater = data.Count - 2;

			// Find the rightmost successor
			// to the pivot
			for (int i = data.Count - 2; i > last; i--)
			{
				if (data[i] > data[last])
				{
					nextGreater = i;
					break;
				}
			}

			// Swap the successor and
			// the pivot
			data = Swap(data, nextGreater, last);

			// Reverse the suffix
			data = Reverse(data, last + 1, data.Count - 2);

			// Return true as the
			// next_permutation is done
			return true;
		}

       
    }

// This code is contributed by Tapesh(tapeshdua420)


}
