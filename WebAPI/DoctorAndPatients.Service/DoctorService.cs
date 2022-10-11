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
    public class DoctorService : IDoctorService
    {
        private IDoctorRepository doctorRepository;

        public DoctorService(IDoctorRepository doctorRepo)
        {
            this.doctorRepository = doctorRepo;
        }
        public async Task<bool> CreateAsync(List<Doctor> doctors)
        {
            foreach (Doctor doctor in doctors)
            {
                doctor.Id = Guid.NewGuid();
                if(! (await doctorRepository.CreateAsync(doctor)))
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
                return await doctorRepository.DeleteAsync(id);
            }
            else
            { 
                return false;
            }
        }

        public async Task<List<Doctor>> FindAsync(Paging paging, Sort sortBy, 
            AmbulanceAddressFilter ambulanceAddressFilter)
        {
            List<Doctor> doctors = new List<Doctor>();
            doctors = await doctorRepository.FindAsync(paging, sortBy, ambulanceAddressFilter);
            return doctors ?? null;
        }

        public async Task<Doctor> GetByIDAsync(Guid id)
        {
            Doctor doctor = await doctorRepository.GetByIDAsync(id);          
            return doctor ?? null;                        
        }

        public async Task<bool> UpdateAsync(Guid id, Doctor doctor)
        {         
            if ((await GetByIDAsync(id)) != null)
            {
                return false;
            }                        
            return await doctorRepository.UpdateAsync(id, doctor);
        }
    }
}
