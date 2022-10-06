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
using DoctorAndPatients.Model;
using DoctorAndPatients.Service;
using DoctorAndPatients.WebAPI.Data;
using DoctorAndPatients.WebAPI.Models;

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class PatientsController : ApiController
    {
        private PatientService patientService = new PatientService();

        // GET: api/Patients
        [HttpGet]
        public HttpResponseMessage GetPatients()
        {
            List<Patient> patients = patientService.GetAll();
            List<PatientREST> patientsREST = MapToREST(patients);

            if (patientsREST != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, patientsREST);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No patients in database.");
            }
        }

        // GET: api/Patients/5
        [HttpGet]
        public HttpResponseMessage GetPatient(Guid id)
        {
            List<Patient> patients = new List<Patient> { patientService.GetByID(id) };
            List<PatientREST> patientsREST = MapToREST(patients);

            if (patientsREST[0] != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, patientsREST[0]);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Patient with that ID doesn't exist.");
            }
        }

        // PUT: api/Patients/5
        [HttpPut]
        public HttpResponseMessage PutPatient(Guid id, PatientREST patientREST)
        {
            patientREST.Id = id;
            List<PatientREST> patientsREST = new List<PatientREST> { patientREST };
            List<Patient> patients = MapToDomain(patientsREST);

            if (patientService.Update(id, patients))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Updated successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
        }         

        // POST: api/Patients
        [HttpPost]
        public HttpResponseMessage PostPatient(PatientREST patientREST)
        {
            List<PatientREST> patientsREST = new List<PatientREST> { patientREST };
            List<Patient> patients = MapToDomain(patientsREST);

            if (patientService.Create(patients))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Inserted successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
        }

        // DELETE: api/Patients/5
        public HttpResponseMessage DeletePatient(Guid id)
        {
            if (patientService.Delete(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
        }


        private List<Patient> MapToDomain(List<PatientREST> patientsRest)
        {
            List<Patient> patients = new List<Patient>();
            if (patientsRest.Count > 0)
            {
                foreach (PatientREST pREST in patientsRest)
                {
                    Guid docID = patientService.GetByID(pREST.Id.Value).DoctorId;
                    Patient patient = new Patient((Guid)pREST.Id, pREST.FirstName, pREST.LastName, pREST.HealthInsuranceID, pREST.Diagnosis, docID);
                    patients.Add(patient);
                }
                return patients;
            }
            else
            {
                return null;
            }
        }

        private List<PatientREST> MapToREST(List<Patient> patients)
        {
            List<PatientREST> patientsREST = new List<PatientREST>();
            if (patients.Count > 0)
            {
                foreach (Patient p in patients)
                {
                    PatientREST patient = new PatientREST(p.Id.Value, p.FirstName, p.LastName, p.HealthInsuranceID, p.Diagnosis);
                    patientsREST.Add(patient);
                }
                return patientsREST;
            }
            else
            {
                return null;
            }
        }
       
    }
}