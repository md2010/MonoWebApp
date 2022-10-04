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
using DoctorAndPatients.WebAPI.Models;

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class PatientsController : ApiController
    {
        //private DoctorAndPatientsWebAPIContext db = new DoctorAndPatientsWebAPIContext();
        public List<Patient> patients = SingletonPatientsList.Instance.patients;

        // GET: api/Patients
        [HttpGet]
        public HttpResponseMessage GetPatients()
        {
            if(patients.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, patients);
            }          
        }

        // GET: api/Patients/5
        [HttpGet]
        public HttpResponseMessage GetPatient(Guid id)
        {
            if(!PatientExists(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, patients.First(i => i.Id == id));
            } 
        }

        // PUT: api/Patients/5
        [HttpPut]
        public HttpResponseMessage PutPatient(Guid id, Patient patient)
        {
            if (!PatientExists(id) || patient == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                Patient existingPatient = patients.First(i => i.Id == id);
                existingPatient.FirstName = patient.FirstName;
                existingPatient.LastName = patient.LastName;
                existingPatient.HealthSecurityNumber = patient.HealthSecurityNumber;
                existingPatient.Diagnosis = patient.Diagnosis;
                existingPatient.DoctorId = patient.DoctorId;

                return Request.CreateResponse(HttpStatusCode.OK);
            }

        }         

        // POST: api/Patients
        [HttpPost]
        public HttpResponseMessage PostPatient(Patient patient)
        {
            if(patient == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            patients.Add(patient);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/Patients/5
        public HttpResponseMessage DeletePatient(Guid id)
        {
            if (!PatientExists(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                patients.Remove(patients.First(i => i.Id == id));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        private bool PatientExists(Guid id)
        {
            try
            {
                if(patients.First(i => i.Id == id) != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                throw (e);
            }
            return false;
        }
    }
}