using DoctorAndPatients.Model;
using DoctorAndPatients.Repository;
using DoctorAndPatients.RepositoryCommon;
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
        private IPatientRepository patientRepository;

        public PatientService(IPatientRepository patientRepo)
        {
            this.patientRepository = patientRepo;
        }
        public async Task<bool> CreateAsync(List<Patient> patients)
        {
            foreach (Patient patient in patients)
            {
                patient.Id = Guid.NewGuid();
            }
            return await patientRepository.CreateAsync(patients);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (await GetByIDAsync(id) != null)
            {
                return await patientRepository.DeleteAsync(id);
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            List<Patient> patients = new List<Patient>();
            patients = await patientRepository.GetAllAsync();
            return patients ?? null;
        }

        public async Task<Patient> GetByIDAsync(Guid id)
        {
            Patient patient = await patientRepository.GetByIDAsync(id);
            return patient ?? null;
        }

        public async Task<bool> UpdateAsync(Guid id, List<Patient> patients)
        {
            foreach (Patient patient in patients)
            {
                if ((await GetByIDAsync(id)) == null)
                {
                    return false;
                }
            }           
             return await patientRepository.UpdateAsync(id, patients);           
        }

    }
}
