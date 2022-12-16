namespace Common.Modles
{
    public class CitiesModel
    {
        public CitiesModel(List<string> Cities,int Number,int[,] Graph)
        {
            this.Cities = Cities;
            this.Number = Number;
            this.Graph = Graph;
        }
        public List<string> Cities { get; }
        public int Number { get; }
        public int[,] Graph { get; }
    }
}