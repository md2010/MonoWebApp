using DoctorAndPatients.Common;
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
        Task<List<Patient>> FindAsync(Paging paging, List<Sort> sorts, 
            DiagnosisFilter diagnosisFilter, DateOfBirthFilter dateFilter);
        Task<bool> UpdateAsync(Guid id, Patient patient);
        Task<bool> CreateAsync(Patient patient);
        Task<bool> DeleteAsync(Guid id);
    }
}
