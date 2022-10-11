﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAndPatients.WebAPI.Models
{
    //without UPIN
    public class DoctorREST
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public string AmbulanceAddress { get; set; }

    }
}
