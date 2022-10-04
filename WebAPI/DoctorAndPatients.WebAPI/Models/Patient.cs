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
        public int HealthSecurityNumber { get; set; }
        public string Diagnosis { get; set; }

        // foreign key - relation one doctor has many patients, patient has one doctor
        public int DoctorId { get; set; }

        public Patient(string firstName, string lastName, int hsNumber, string diagnosis, int docId)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.HealthSecurityNumber = hsNumber;
            this.Diagnosis = diagnosis;
            this.DoctorId = docId;
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
                new Patient("John", "Sima", 7478458, "Contact allergy", 3),
                new Patient("Mark", "Merz", 4545455, "Diabetes", 1),
                new Patient("Jess", "Rodhos", 8859403, "Nut allergy", 1),
                new Patient("Ann", "Marthos", 9663839, "High blood pressure", 2)
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