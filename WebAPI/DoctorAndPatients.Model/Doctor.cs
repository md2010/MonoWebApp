﻿using DoctorAndPatients.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Model
{
    public class Doctor : IDoctor
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // unique physician identification number (UPIN)
        //6 six-character alpha-numeric identifier used to identify doctors in the US
        public string UPIN { get; set; }
        public string AmbulanceAddress { get; set; }
        
        public Doctor(Guid id, string firstName, string lastName, string upin, string address)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UPIN = upin;
            this.AmbulanceAddress = address;
        }
    }
}
