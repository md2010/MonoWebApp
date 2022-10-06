using DoctorAndPatients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Service.Common
{
    public interface IDoctorService
    {
        Doctor GetByID(Guid id);
        List<Doctor> GetAll();
        bool Update(Guid id, List<Doctor> doctors);
        bool Create(List<Doctor> doctor);
        bool Delete(Guid id);

    }
}
