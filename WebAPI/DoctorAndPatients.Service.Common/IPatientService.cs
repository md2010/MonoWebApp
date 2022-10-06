using DoctorAndPatients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Service.Common
{
    public interface IPatientService
    {
        Patient GetByID(Guid id);
        List<Patient> GetAll();
        bool Update(Guid id, List<Patient> patients);
        bool Create(List<Patient> patients);
        bool Delete(Guid id);
    }
}
