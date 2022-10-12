using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Common
{
    public class Sort
    {
        public string SortBy { get; set; }
        public string SortOrder { get; set; }

        public Sort()
        {
            this.SortBy = "createdAt";
            this.SortOrder = "asc";
        }
        public Sort(string sortBy, string order)
        {
            this.SortBy = String.IsNullOrWhiteSpace(sortBy) ? "createdAt" : sortBy;
            this.SortOrder = String.IsNullOrWhiteSpace(order) ? "asc" : order;
        }
    }
}
