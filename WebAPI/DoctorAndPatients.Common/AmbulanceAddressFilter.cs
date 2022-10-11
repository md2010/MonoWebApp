using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Common
{
    public class AmbulanceAddressFilter
    {
        public string AmbulanceAddress { get; set; }

        public AmbulanceAddressFilter(string ambulanceAddress)
        {
            this.AmbulanceAddress = ambulanceAddress;
        }
    }
}
