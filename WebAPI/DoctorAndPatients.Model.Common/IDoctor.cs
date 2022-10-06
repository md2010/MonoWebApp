using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Model.Common
{
    public interface IDoctor    
    {
        Guid? Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }

        // unique physician identification number (UPIN)
        //6 six-character alpha-numeric identifier used to identify doctors in the US
        string UPIN { get; set; }
        string AmbulanceAddress { get; set; }
    }
}
