using DoctorAndPatients.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Model
{
    public class Patient : IPatient
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //unique for every person that has health insurance
        public int HealthInsuranceID { get; set; }
        public string Diagnosis { get; set; }

        // foreign key - relation one doctor has many patients, patient has one doctor
        public Guid DoctorId { get; set; }

        public Patient(Guid id, string firstName, string lastName, int hsNumber, string diagnosis, Guid doctorId)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.HealthInsuranceID = hsNumber;
            this.Diagnosis = diagnosis;
            this.DoctorId = doctorId;
        }
    }
}
