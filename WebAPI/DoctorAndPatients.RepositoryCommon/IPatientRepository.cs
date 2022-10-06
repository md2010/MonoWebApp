using DoctorAndPatients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.RepositoryCommon
{
    public interface IPatientRepository
    {
        Patient GetByID(Guid id);
        List<Patient> GetAll();
        bool Update(Guid id, Patient patient);
        bool Create(Patient patient);
        bool Delete(Guid id);
    }
}
