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
        public async Task<bool> CreateAsync(List<Doctor> doctors)
        {
            doctors[0].Id = Guid.NewGuid();
            return await doctorRepository.CreateAsync(doctors[0]);
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

        public async Task<List<Doctor>> GetAllAsync()
        {
            List<Doctor> doctors = new List<Doctor>();
            doctors = await doctorRepository.GetAllAsync();
            return doctors ?? null;
        }

        public async Task<Doctor> GetByIDAsync(Guid id)
        {
            Doctor doctor = await doctorRepository.GetByIDAsync(id);          
            return doctor ?? null;                        
        }

        public async Task<bool> UpdateAsync(Guid id, List<Doctor> doctors)
        {
            if ( (doctors[0] = await GetByIDAsync(id)) != null) //returns Doctor with missing data, by ID          
            {
                return await doctorRepository.UpdateAsync(id, doctors[0]);
            }
            else
            {
                return false;
            }
        }
    }
}
