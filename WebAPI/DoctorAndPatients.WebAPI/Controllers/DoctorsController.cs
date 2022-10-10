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

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class DoctorsController : ApiController
    {
        private IDoctorService doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }
      
        // GET: api/Doctors
        [HttpGet]
        public async Task<HttpResponseMessage> GetDoctorsAsync()
        {
            List<Doctor> doctors = await doctorService.GetAllAsync();

            if(doctors.Any())
            {
                List<DoctorREST> doctorsREST = MapToREST(doctors);
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

            Doctor doc = null;
            if( (doc = await doctorService.GetByIDAsync(id)) != null)
            { 
                List<Doctor> doctors = new List<Doctor> { doc };            
                List<DoctorREST> doctorsREST = MapToREST(doctors);
                return Request.CreateResponse(HttpStatusCode.OK, doctorsREST.First());
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
            List<DoctorREST> doctorsREST = new List<DoctorREST> { doctorREST };           
            List<Doctor> doctors = MapToDomain(doctorsREST);

            if (await doctorService.UpdateAsync(id, doctors))
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
            List<DoctorREST> doctorsREST = new List<DoctorREST> { doctorREST };
            List<Doctor> doctors = MapToDomain(doctorsREST);
            
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
        
        private List<Doctor> MapToDomain(List<DoctorREST> doctorsRest)
        {
            List<Doctor> doctors = new List<Doctor>();
            if (doctors.Any())
            {
                foreach (DoctorREST docREST in doctorsRest)
                {                    
                    Doctor doctor = new Doctor(docREST.Id.Value, docREST.FirstName, docREST.LastName, "", docREST.AmbulanceAddress);
                    doctors.Add(doctor);
                }
                return doctors;
            } 
            else
            {
                return null;
            }
        }

        private List<DoctorREST> MapToREST(List<Doctor> doctors)
        {
            List<DoctorREST> doctorsREST = new List<DoctorREST>();
            if (doctors.Any())
            {
                foreach (Doctor doc in doctors)
                {              
                    DoctorREST doctor = new DoctorREST((Guid)doc.Id, doc.FirstName, doc.LastName, doc.AmbulanceAddress);
                    doctorsREST.Add(doctor);
                }
                return doctorsREST;
            }
            else
            {
                return null;
            }
        }
        
    }
}