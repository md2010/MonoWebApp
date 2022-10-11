using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Common
{
    public class DiagnosisFilter
    {
        public string Diagnosis { get; set; }

        public DiagnosisFilter(string diagnosis)
        {
            this.Diagnosis = diagnosis;
        }
    }
}
