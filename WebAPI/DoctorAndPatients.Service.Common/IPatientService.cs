using DoctorAndPatients.Common;
using DoctorAndPatients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Service.Common
{
    public interface IPatientService
    {
        Task<Patient> GetByIDAsync(Guid id);
        Task<List<Patient>> GetAllAsync(Paging paging, Sort sortBy, 
            DiagnosisFilter diagnosisFilter, DateOfBirthFilter dateFilter);
        Task<bool> UpdateAsync(Guid id, Patient patients);
        Task<bool> CreateAsync(List<Patient> patients);
        Task<bool> DeleteAsync(Guid id);
    }
}
