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

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class DoctorsController : ApiController
    {
        private DoctorService doctorService = new DoctorService();
      
        // GET: api/Doctors
        [HttpGet]
        public HttpResponseMessage GetDoctors()
        {
            List<Doctor> doctors = doctorService.GetAll();
            List<DoctorREST> doctorsREST = MapToREST(doctors);

            if(doctors != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, doctorsREST);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No doctors in database.");
            }           
        }

        // GET: api/Doctors/5
        [HttpGet]
        public HttpResponseMessage GetDoctor(Guid id)
        {
            Doctor doctor = doctorService.GetByID(id);
            List<DoctorREST> doctorsREST = MapToREST(new List<Doctor> { doctor });

            if (doctorsREST[0] != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, doctorsREST[0]);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Doctor with that ID doesn't exist.");
            }
        }

        // PUT: api/Doctors/5
        [HttpPut]
        public HttpResponseMessage PutDoctor(Guid id, DoctorREST doctorREST)
        {
            doctorREST.Id = id;
            List<DoctorREST> doctorsREST = new List<DoctorREST> { doctorREST };           
            List<Doctor> doctors = MapToDomain(doctorsREST);

            if (doctorService.Update(id, doctors))
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
        public HttpResponseMessage PostDoctor(DoctorREST doctorREST)
        {
            List<DoctorREST> doctorsREST = new List<DoctorREST> { doctorREST };
            List<Doctor> doctors = MapToDomain(doctorsREST);
            if (doctorService.Create(doctors))
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
        public HttpResponseMessage DeleteDoctor(Guid id)
        {
            if (doctorService.Delete(id))
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
            if (doctorsRest.Count > 0)
            {
                foreach (DoctorREST docREST in doctorsRest)
                {                    
                    Doctor doc = doctorService.GetByID((Guid)docREST.Id);
                    string UPIN = doctorService.GetByID(docREST.Id.Value).UPIN;
                    Doctor doctor = new Doctor(docREST.Id.Value, docREST.FirstName, docREST.LastName, UPIN, docREST.AmbulanceAddress);
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
            if (doctors.Count > 0)
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