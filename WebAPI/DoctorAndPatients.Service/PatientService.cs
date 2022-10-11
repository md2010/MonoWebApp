using DoctorAndPatients.Common;
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
                if(! await patientRepository.CreateAsync(patient))
                {
                    return false;
                }
            }
            return true;
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

        public async Task<List<Patient>> GetAllAsync(Paging paging, Sort sort, 
            DiagnosisFilter diagnosisFilter, DateOfBirthFilter dateFilter)
        {
            List<Patient> patients = new List<Patient>();
            patients = await patientRepository.GetAllAsync(paging, sort, diagnosisFilter, dateFilter);
            return patients ?? null;
        }

        public async Task<Patient> GetByIDAsync(Guid id)
        {
            Patient patient = await patientRepository.GetByIDAsync(id);
            return patient ?? null;
        }

        public async Task<bool> UpdateAsync(Guid id, Patient patient)
        {          
            if ((await GetByIDAsync(id)) != null)
            {
                return await patientRepository.UpdateAsync(id, patient);
            }                                     
            return false;           
        }

    }
}
