using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Common
{
    public class Paging
    {
        public int Rpp { get; set; }
        public int PageNumber { get; set; } 
        
        public Paging (int rpp, int pageNumber)
        {
            this.Rpp = rpp == 0 ? 2 : rpp;
            this.PageNumber = pageNumber == 0 ? 1 : pageNumber;
        }
    }
}
