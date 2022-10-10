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
        Task<bool> UpdateAsync(Guid id, List<Patient> patients);
        Task<bool> CreateAsync(List<Patient> patients);
        Task<bool> DeleteAsync(Guid id);
    }
}
