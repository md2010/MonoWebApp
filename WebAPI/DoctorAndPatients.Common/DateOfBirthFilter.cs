using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Common
{
    public class DateOfBirthFilter
    {
        public DateTime DateOfBirth { get; set; }
        private string pattern = "yyyy-MM-dd";

        public DateOfBirthFilter(string dateOfBirth)
        {
            this.DateOfBirth = dateOfBirth == "" ? default : DateTime.ParseExact(dateOfBirth, pattern,null);
        }
    }
}
