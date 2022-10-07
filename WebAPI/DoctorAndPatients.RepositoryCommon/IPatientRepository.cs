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
        Task<Patient> GetByIDAsync(Guid id);
        Task<List<Patient>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, Patient patient);
        Task<bool> CreateAsync(Patient patient);
        Task<bool> DeleteAsync(Guid id);
    }
}
