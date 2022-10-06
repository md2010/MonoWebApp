using DoctorAndPatients.Model;
using DoctorAndPatients.Repository;
using DoctorAndPatients.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Service
{
    public class DoctorService : IDoctorService
    {
        private DoctorRepository doctorRepository = new DoctorRepository();
        public bool Create(List<Doctor> doctors)
        {
            doctors[0].Id = Guid.NewGuid();
            return doctorRepository.Create(doctors[0]);
        }

        public bool Delete(Guid id)
        {
            return doctorRepository.Delete(id);
        }

        public List<Doctor> GetAll()
        {
            List<Doctor> doctors = new List<Doctor>();
            doctors = doctorRepository.GetAll();
             return doctors ?? null;
        }

        public Doctor GetByID(Guid id)
        {
            Doctor doctor = doctorRepository.GetByID(id);          
            return doctor ?? null;                        
        }

        public bool Update(Guid id, List<Doctor> doctors)
        {
            return doctorRepository.Update(id, doctors[0]) ? true : false;
        }
    }
}
