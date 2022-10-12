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
using AutoMapper;
using DoctorAndPatients.Common;
using DoctorAndPatients.Model;
using DoctorAndPatients.Service;
using DoctorAndPatients.Service.Common;
using DoctorAndPatients.WebAPI.Data;
using DoctorAndPatients.WebAPI.Models;

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class PatientsController : ApiController
    {
        private IPatientService patientService;
        private IMapper mapper;

        public PatientsController(IPatientService patientService, IMapper mapper)
        {
            this.patientService = patientService;
            this.mapper = mapper;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<HttpResponseMessage> FindAsync(int rpp=0, int pageNumber=0, 
            string sort = "", string diagnosis="", string dateOfBirth="")
        {
            DiagnosisFilter diagnosisFilter = null;
            diagnosisFilter =  String.IsNullOrWhiteSpace(diagnosis) ? null : new DiagnosisFilter(diagnosis);
            DateOfBirthFilter dateFilter = null;
            dateFilter = String.IsNullOrWhiteSpace(dateOfBirth) ? null : new DateOfBirthFilter(dateOfBirth);
            Paging paging = new Paging(rpp, pageNumber);
            //Sort sort = new Sort(sortBy, order);
            List<Sort> sorts = ResolveSortURLParameters(sort);

            List<Patient> patients = await patientService.FindAsync(paging, sorts, diagnosisFilter, dateFilter);            

            if (patients.Any())
            {
                List<PatientREST> patientsREST = new List<PatientREST>();
                patientsREST = mapper.Map(patients, patientsREST);
            
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty ID.");
            }          

            Patient patient = null;
            if ( (patient = await patientService.GetByIDAsync(id)) != null ) 
            {
                PatientREST patientREST = null;
                patientREST = mapper.Map(patient, patientREST);

                return Request.CreateResponse(HttpStatusCode.OK, patientREST);
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

            Patient patient = null;
            patient = mapper.Map(patientREST, patient);

            if (await patientService.UpdateAsync(id, patient))
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
            
            List<Patient> patients = new List<Patient>();
            patients = mapper.Map(new List<PatientREST> { patientREST }, patients);

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
        [HttpDelete]
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

        private List<Sort> ResolveSortURLParameters(string sort)
        {
            List<Sort> sorts = new List<Sort>();
            if (String.IsNullOrWhiteSpace(sort))
            {
                sorts.Add(new Sort());
                return sorts;
            }
            string[] splitted = sort.Split(',');
            foreach(string s in splitted)
            {
                string[] properties = s.Split('|');
                sorts.Add(new Sort(properties[0], properties[1]));
            }
            return sorts.Distinct().ToList(); //so there's no same sorting requests --> query exception
        }
       
    }
}