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
        Task<Doctor> GetByIDAsync(Guid id);
        Task<List<Doctor>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, Doctor doctor);
        Task<bool> CreateAsync(Doctor doctor);
        Task<bool> DeleteAsync(Guid id);
    }
}
