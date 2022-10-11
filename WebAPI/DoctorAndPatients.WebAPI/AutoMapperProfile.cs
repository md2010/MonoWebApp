using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DoctorAndPatients.Model;
using DoctorAndPatients.WebAPI.Models;

namespace DoctorAndPatients.WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Patient, PatientREST>().ReverseMap();

            CreateMap<Doctor, DoctorREST>().ReverseMap();
        }

    }

}