using Common.Modles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRetrival
{
    public sealed class FromFile : ICities
    {
        private string _fileName;
        public FromFile(string path)
        {
            _fileName = path;
        }
        public CitiesModel GetCitiesGraph()
        {
            List<string> cities1;
            int number;
            string[] str = ReadFile();
            if(str == null || str.Length == 0)
            {
                return null;
            }
            number = str.Length;
            int[,] cities = new int[number, number];
            List<string> city = new List<string>();
            int count = 0;
            int count1;
            foreach (string s in str)
            {
                city.Add(s.Split(" ")[0]);
                var temp = (s.Split(" ")[1]).Split(",");
                count1 = 0;
                foreach (string i in temp)
                {
                    cities[count,count1]=int.Parse(i);
                    count1++;
                }
                count++;
            }
            cities1 = city;
            return new CitiesModel(cities1,number,cities);
        }
        private string[] ReadFile()
        {
            string[] lines = File.ReadAllLines(_fileName);
            return lines;
        }
    }
}
