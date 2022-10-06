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
    public class PatientService : IPatientService
    {
        private PatientRepository patientRepository = new PatientRepository();
        public bool Create(List<Patient> patients)
        {
            patients[0].Id = Guid.NewGuid();
            return patientRepository.Create(patients[0]);
        }

        public bool Delete(Guid id)
        {
            return patientRepository.Delete(id);
        }

        public List<Patient> GetAll()
        {
            List<Patient> patients = new List<Patient>();
            patients = patientRepository.GetAll();
            return patients ?? null;
        }

        public Patient GetByID(Guid id)
        {

            Patient patient = patientRepository.GetByID(id);
            return patient ?? null;
        }

        public bool Update(Guid id, List<Patient> patients)
        {
            return patientRepository.Update(id, patients[0]) ? true : false;
        }
    }
}
