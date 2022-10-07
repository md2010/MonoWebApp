using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> GetPatientsAsync()
        {
            List<Patient> patients = await patientService.GetAllAsync();            

            if (patients.Any())
            {
                List<PatientREST> patientsREST = MapToREST(patients);
                return Request.CreateResponse(HttpStatusCode.OK, patientsREST);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No patients in database.");
            }
        }

        // GET: api/Patients/5
        [HttpGet]
        public async Task<HttpResponseMessage> GetPatientAsync(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Empty ID.");
            }          

            Patient patient = null;
            if ( (patient = await patientService.GetByIDAsync(id)) != null ) 
            {
                List<Patient> patients = new List<Patient> { patient };
                List<PatientREST> patientsREST = MapToREST(patients);
                return Request.CreateResponse(HttpStatusCode.OK, patientsREST[0]);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient with that ID doesn't exist.");
            }
        }

        // PUT: api/Patients/5
        [HttpPut]
        public async Task<HttpResponseMessage> PutPatientAsync(Guid id, PatientREST patientREST)
        {
            if (patientREST == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty entry.");
            }
            List<PatientREST> patientsREST = new List<PatientREST> { patientREST };
            List<Patient> patients = MapToDomain(patientsREST);

            if (await patientService.UpdateAsync(id, patients))
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
        public async Task<HttpResponseMessage> PostPatientAsync(PatientREST patientREST)
        {
            if (patientREST == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty entry.");
            }
            List<PatientREST> patientsREST = new List<PatientREST> { patientREST };
            List<Patient> patients = MapToDomain(patientsREST);

            if (await patientService.CreateAsync(patients))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Inserted successfully!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong.");
            }
        }

        // DELETE: api/Patients/5
        public async Task<HttpResponseMessage> DeletePatientAsync(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Empty ID.");
            }
            if (await patientService.DeleteAsync(id))
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
            if (patientsRest.Any())
            {
                foreach (PatientREST pREST in patientsRest)
                {
                    Patient patient = new Patient(pREST.Id.Value, pREST.FirstName, pREST.LastName, pREST.HealthInsuranceID, pREST.Diagnosis, pREST.DoctorId);
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
            if (patients.Any())
            {
                foreach (Patient p in patients)
                {
                    PatientREST patient = new PatientREST((Guid)p.Id, p.FirstName, p.LastName, p.HealthInsuranceID, p.Diagnosis, p.DoctorId);
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