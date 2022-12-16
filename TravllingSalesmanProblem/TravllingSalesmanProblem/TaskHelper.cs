using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using Common.Models;
using Common.Modles;
using DataRetrival;
using Microsoft.Extensions.Configuration;
using SearchTechniqe.Implementations;
using SearchTechniqe.Interfaces;

namespace TravllingSalesmanProblem
{
    internal class TaskHelper
    {
        IConfiguration _configuration;
        public TaskHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public void Execute()
        {
            DateTime startTime, endTime;
            ISearch ts;
            SearchResult result;
            ICities cities;
            string header = "Search Technique : ";
            string pathString = _configuration["PathToFiles"];
            var paths = pathString.Split(',');
            Workbook workbook = new Workbook();
            Workbook workbook1 = new Workbook();
            Workbook workbook3 = new Workbook();
            Worksheet worksheet1;
            Worksheet worksheet = workbook.Worksheets[0];
            workbook1.Worksheets.Add();
            workbook1.Worksheets.Add();
            var array = new ArrayList();
            array.Add("number of cities");
            array.Add("algorithm 1 time");
            array.Add("algorithm 1 cost");
            array.Add("algorithm 2 time");
            array.Add("algorithm 2 cost");
            array.Add("algorithm 3 time");
            array.Add("algorithm 3 cost");
            worksheet = CreateExcel(worksheet,array,0);
            var count = 1;
            foreach (var path in paths) {

                array = new ArrayList();
                cities = new FromFile(path);
                CitiesModel fileData = cities.GetCitiesGraph();
                Console.WriteLine("File Path :"+path);
                Console.WriteLine("Cities # :" + fileData.Number);
                if (count != 1)
                    workbook3.Worksheets.Add(fileData.Number + "-cities");
                else
                    workbook3.Worksheets[0].Name = fileData.Number + "-cities";
                worksheet1 = workbook3.Worksheets[count - 1];
                array.Add((int)fileData.Number);
                Console.WriteLine();
                Console.WriteLine();
                if (Convert.ToBoolean(_configuration["ExhustiveEnabeled"])){
                    startTime = DateTime.Now;
                    ts = new ExhustiveSearch(fileData.Graph, fileData.Cities, fileData.Number);
                    result=ts.ShortestPath();
                    endTime = DateTime.Now;
                    PrintResult(startTime, endTime, result.Result, header + "Exhustive");
                }
                startTime = DateTime.Now;
                ts = new HillClimping(fileData.Graph, fileData.Cities, fileData.Number);
                result = ts.ShortestPath();
                endTime = DateTime.Now;
                PrintResult(startTime, endTime, result.Result, header+"Hill Climping");
                array.Add(endTime-startTime);
                array.Add(result.Cost);
                if (result.IterationCost != null)
                {
                    workbook1 = CreateExcelArray(result.IterationCost, 0, workbook1, count - 1, fileData.Number + "-city");
                    worksheet1 = CreateExcelArrayForEachCity(result.IterationCost, worksheet1,0, "Hill Climping");
                }
                
                startTime = DateTime.Now;
                ts = new SimulatedAnnealing(fileData.Graph, fileData.Cities, fileData.Number);
                result = ts.ShortestPath();
                endTime = DateTime.Now;
                PrintResult(startTime, endTime, result.Result, header + "Simulated Annealing");
                array.Add(endTime - startTime);
                array.Add(result.Cost);
                if (result.IterationCost != null)
                {
                    workbook1 = CreateExcelArray(result.IterationCost, 1, workbook1, count - 1, fileData.Number + "-city");
                    worksheet1 = CreateExcelArrayForEachCity(result.IterationCost, worksheet1, 1, "Simulated Annealing");
                }
                startTime = DateTime.Now;
                ts = new GeneticAlgorithm(fileData.Graph, fileData.Cities, fileData.Number);
                result = ts.ShortestPath();
                endTime = DateTime.Now;
                PrintResult(startTime, endTime, result.Result, header + "Genetic");
                array.Add(endTime - startTime);
                array.Add(result.Cost);
                if (result.IterationCost != null)
                {
                    workbook1 = CreateExcelArray(result.IterationCost, 2, workbook1, count - 1, fileData.Number + "-city");
                    worksheet1 = CreateExcelArrayForEachCity(result.IterationCost, worksheet1, 2, "Genetic");
                }

                worksheet = CreateExcel(worksheet, array, count);
                Console.WriteLine("=======================================================================================");
                count++;
            }
            workbook3.Worksheets.Add();
            MemoryStream ms = new MemoryStream();
            workbook.Save(ms, SaveFormat.Xlsx);
            SaveByteArrayToFileWithBinaryWriter(ms.ToArray(), "C:/TSP.xlsx");
            MemoryStream ms1 = new MemoryStream();
            workbook1.Save(ms1, SaveFormat.Xlsx);
            SaveByteArrayToFileWithBinaryWriter(ms1.ToArray(), "C:/TSPALG.xlsx");
            MemoryStream ms2 = new MemoryStream();
            workbook3.Save(ms2, SaveFormat.Xlsx);
            SaveByteArrayToFileWithBinaryWriter(ms2.ToArray(), "C:/TSPCities.xlsx");
        }
        private Workbook CreateExcelArray(List<int> data,int sheet, Workbook workbook,int column,string label)
        {
            //creating workbook and the sheet

            Worksheet worksheet = workbook.Worksheets[sheet];
            worksheet.Cells.SetColumnWidth(0, 18);
            //set first feild value

                worksheet.Cells[0, column].PutValue(label);
            
            worksheet.Cells.ImportArrayList(new ArrayList(data), 1, column, true);

            //create new memorstream to save workbook as xlsx in it
            return workbook;
        }
        private Worksheet CreateExcelArrayForEachCity(List<int> data,  Worksheet worksheet, int column, string label)
        {
            //creating workbook and the sheet

            worksheet.Cells.SetColumnWidth(0, 18);
            //set first feild value

            worksheet.Cells[0, column].PutValue(label);

            worksheet.Cells.ImportArrayList(new ArrayList(data), 1, column, true);

            //create new memorstream to save workbook as xlsx in it
            return worksheet;
        }
        private static void SaveByteArrayToFileWithBinaryWriter(byte[] data, string filePath)
        {
            using var writer = new BinaryWriter(File.OpenWrite(filePath));
            writer.Write(data);
        }
        private Worksheet CreateExcel(Worksheet worksheet,ArrayList data,int row)
        {
            //creating workbook and the sheet
 
            worksheet.Cells.SetColumnWidth(0, 18);
            //set first feild value
            var count = 0;
            foreach(var d in data)
            {
                worksheet.Cells[row, count].PutValue(d);
                worksheet.Cells[row, count].PutValue(d);
                worksheet.Cells[row, count].PutValue(d);
                worksheet.Cells[row, count].PutValue(d);
                count++;
            }


            //import array list to the sheet

            //create new memorstream to save workbook as xlsx in it
            //MemoryStream ms = new MemoryStream();
            //workbook.Save(ms, SaveFormat.Xlsx);

            //return memory stream as array of bytes
            return worksheet;
        }
        void PrintResult(DateTime startTime, DateTime endTime,string result,string header)
        {
            Console.WriteLine(header);
            Console.WriteLine(result);
            Console.WriteLine("time elapse: " + (endTime - startTime).ToString());
            Console.WriteLine("------------------------------------------");
            
        }
    }
}
