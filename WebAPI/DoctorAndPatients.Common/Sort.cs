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

        public Sort(string sortBy, string order)
        {
            this.SortBy = String.IsNullOrEmpty(sortBy) ? "" : sortBy;
            this.SortOrder = String.IsNullOrEmpty(order) ? "asc" : order;
        }
    }
}
