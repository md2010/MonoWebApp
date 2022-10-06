using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAndPatients.WebAPI.Models
{
    //without doctorID
    public class PatientREST
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //unique for every person that has health insurance
        public int HealthInsuranceID { get; set; }
        public string Diagnosis { get; set; }


        public PatientREST(Guid id, string firstName, string lastName, int hsNumber, string diagnosis)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.HealthInsuranceID = hsNumber;
            this.Diagnosis = diagnosis;
        }
    }
}