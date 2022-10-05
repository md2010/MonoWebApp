using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAndPatients.WebAPI.Models
{
    public class Doctor
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // unique physician identification number (UPIN)
        //6 six-character alpha-numeric identifier used to identify doctors in the US
        public string UPIN { get; set; }
        public string AmbulanceAddress { get; set; }

        public Doctor(string firstName, string lastName, string upin, string address)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UPIN = upin;
            this.AmbulanceAddress = address;
        }

        public Doctor(Guid id, string firstName, string lastName, string upin, string address)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UPIN = upin;
            this.AmbulanceAddress = address;
        }
    }

        public sealed class SingletonDoctorsList
        {
            public List<Doctor> doctors;

            private static SingletonDoctorsList instance = null;
            private SingletonDoctorsList()
            {
                doctors = new List<Doctor>()
                {
                    new Doctor("Mary", "Zick", "ab67cf", "7th Avenue"),
                    new Doctor("Michel", "Jouse", "gh604g", "Road St."),
                    new Doctor("Thomas", "Lisan", "78zho9", "3rd Avenue"),
                };
            }
            public static SingletonDoctorsList Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new SingletonDoctorsList();
                    }
                    return instance;
                }
            }
        }

    }
