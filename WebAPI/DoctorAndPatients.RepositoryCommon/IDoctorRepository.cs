using DoctorAndPatients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.RepositoryCommon
{
    public interface IDoctorRepository
    {
        Doctor GetByID(Guid id);
        List<Doctor> GetAll();
        bool Update(Guid id, Doctor doctor);
        bool Create(Doctor doctor);
        bool Delete(Guid id);
    }
}
