using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTechniqe.Interfaces
{
    public interface ISearch
    {
        public SearchResult ShortestPath();
    }
}
