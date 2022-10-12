using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Model.Common
{
    public interface IPatient
    {
        Guid? Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }

        //unique for every person that has health insurance
        int HealthInsuranceID { get; set; }
        string Diagnosis { get; set; }

        // foreign key - relation one doctor has many patients, patient has one doctor
        Guid DoctorId { get; set; }
        DateTime DateOfBirth { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
