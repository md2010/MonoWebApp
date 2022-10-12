using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DoctorAndPatients.WebAPI.Data;
using DoctorAndPatients.Model;
using DoctorAndPatients.Service;
using DoctorAndPatients.WebAPI.Models;
using System.Threading.Tasks;
using DoctorAndPatients.Service.Common;
using AutoMapper;
using DoctorAndPatients.Common;

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class DoctorsController : ApiController
    {
        private IDoctorService doctorService;
        private IMapper mapper;

        public DoctorsController(IDoctorService doctorService, IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }
      
        // GET: api/Doctors
        [HttpGet]
        public async Task<HttpResponseMessage> FindAsync(int rpp = 0, int pageNumber = 0, 
            string sort = "", string ambulanceAddress="")
        {
            Paging paging = new Paging(rpp, pageNumber);
            List<Sort> sorts = ResolveSortURLParameters(sort);
            AmbulanceAddressFilter ambulanceAddressFilter = new AmbulanceAddressFilter(ambulanceAddress);

            List<Doctor> doctors = await doctorService.FindAsync(paging, sorts, ambulanceAddressFilter);

            if(doctors.Any())
            {
                List<DoctorREST> doctorsREST = new List<DoctorREST>();
                doctorsREST = mapper.Map(doctors, doctorsREST);

                return Request.CreateResponse(HttpStatusCode.OK, doctorsREST);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No doctors in database.");
            }           
        }

        // GET: api/Doctors/5
        [HttpGet]
        public async Task<HttpResponseMessage> GetDoctorAsync(Guid id)
        {
            if(id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty ID.");
            }

            Doctor doctor;
            if( (doctor = await doctorService.GetByIDAsync(id)) != null)
            {
                DoctorREST doctorREST = null;
                doctorREST = mapper.Map(doctor, doctorREST);

                return Request.CreateResponse(HttpStatusCode.OK, doctorREST);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Doctor with that ID doesn't exist.");
            }
        }

        // PUT: api/Doctors/5
        [HttpPut]
        public async Task<HttpResponseMessage> PutDoctorAsync(Guid id, DoctorREST doctorREST)
        {
            if(doctorREST == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty entry.");
            }        
            
            Doctor doctor = null;
            doctor = mapper.Map(doctorREST, doctor);

            if (await doctorService.UpdateAsync(id, doctor))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Updated successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
            
        }         

        // POST: api/Doctors
        [HttpPost]
        public async Task<HttpResponseMessage> PostDoctorAsync(DoctorREST doctorREST)
        {
            if (doctorREST == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty entry.");
            }

            List<Doctor> doctors = new List<Doctor>();
            doctors = mapper.Map(new List<DoctorREST> { doctorREST }, doctors);

            if (await doctorService.CreateAsync(doctors))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Inserted successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
        }
        
        // DELETE: api/Doctors/5
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteDoctorAsync(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Empty ID.");
            }
            if (await doctorService.DeleteAsync(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
        }

        private List<Sort> ResolveSortURLParameters(string sort)
        {
            List<Sort> sorts = new List<Sort>();
            string[] splitted = sort.Split(',');
            foreach (string s in splitted)
            {
                string[] properties = s.Split('|');
                sorts.Add(new Sort(properties[0], properties[1]));
            }
            return sorts.Distinct().ToList(); //so there's no same sorting requests --> query exception
        }

    }
}