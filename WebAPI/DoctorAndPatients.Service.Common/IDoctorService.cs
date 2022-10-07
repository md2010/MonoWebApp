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
        Task<Doctor> GetByIDAsync(Guid id);
        Task<List<Doctor>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, List<Doctor> doctors);
        Task<bool> CreateAsync(List<Doctor> doctor);
        Task<bool> DeleteAsync(Guid id);

    }
}
