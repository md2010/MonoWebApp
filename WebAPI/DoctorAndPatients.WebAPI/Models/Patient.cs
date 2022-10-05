using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAndPatients.WebAPI.Models
{
    //[Serializable()] for Chrome
    public class Patient
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //unique for every person that has health insurance
        public int HealthInsuranceID { get; set; }
        public string Diagnosis { get; set; }

        // foreign key - relation one doctor has many patients, patient has one doctor
        public Guid DoctorId { get; set; }

        public Patient(string firstName, string lastName, int hsNumber, string diagnosis)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.HealthInsuranceID = hsNumber;
            this.Diagnosis = diagnosis;
        }
    }

    public sealed class SingletonPatientsList
    {
        public List<Patient> patients;

        private static SingletonPatientsList instance = null;
        private SingletonPatientsList()
        {
            patients = new List<Patient>()
            {
                new Patient("John", "Sima", 7478458, "Contact allergy"),
                new Patient("Mark", "Merz", 4545455, "Diabetes"),
                new Patient("Jess", "Rodhos", 8859403, "Nut allergy"),
                new Patient("Ann", "Marthos", 9663839, "High blood pressure")
            };
        }
        public static SingletonPatientsList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SingletonPatientsList();
                }
                return instance;
            }
        }
    }
}