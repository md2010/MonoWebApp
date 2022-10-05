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
using MySql.Data.MySqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace DoctorAndPatients.WebAPI.Controllers
{
    public class DoctorsController : ApiController
    {
        //private DoctorAndPatientsWebAPIContext db = new DoctorAndPatientsWebAPIContext();       

        public List<Doctor> doctors = SingletonDoctorsList.Instance.doctors;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        MySqlConnection conn;

        // GET: api/Doctors
        [HttpGet]
        public HttpResponseMessage GetDoctors()
        {         
            try
            {              
                conn = new MySqlConnection();
                conn.ConnectionString = connectionString;               
                MySqlCommand command = new MySqlCommand("SELECT * FROM doctor", conn);
                conn.Open();

                MySqlDataReader reader = command.ExecuteReader();
                List<Doctor> list = new List<Doctor>();
                while (reader.Read())
                {
                     list.Add(ReadSingleRow(reader));
                }
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, list);                                                                      
            }

            catch (MySqlException ex)
            {
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
                throw ex;
            }
        }

        // GET: api/Doctors/5
        [HttpGet]
        public HttpResponseMessage GetDoctor(Guid id)
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connectionString;
                MySqlCommand command = new MySqlCommand("SELECT * FROM doctor WHERE id = \"" + id + "\"", conn);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if(!reader.HasRows)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No doctors in DB!");
                }

                Doctor doc = null;
                while (reader.Read())
                {
                    doc = ReadSingleRow(reader);
                }
                conn.Close();
                return Request.CreateResponse(HttpStatusCode.OK, doc);
            }

            catch (MySqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
                throw ex;
            }                     
        }

        // PUT: api/Doctors/5
        [HttpPut]
        public HttpResponseMessage PutDoctor(Guid id, Doctor doctor)
        {
            //get data
            conn = new MySqlConnection();
            conn.ConnectionString = connectionString;
            MySqlDataAdapter ad = new MySqlDataAdapter("SELECT * FROM doctor", conn);
            DataSet doc = new DataSet();
            ad.Fill(doc, "Doctors");
            DataTable docTable = doc.Tables["Doctors"];           
            DataRow row = docTable.Rows.Find(id); //get index of row for update
            int index = docTable.Rows.IndexOf(row);

            //updating....
            docTable.Rows[index]["firstName"] = doctor.FirstName;
            docTable.Rows[index]["lastName"] = doctor.LastName;
            docTable.Rows[index]["UPIN"] = doctor.UPIN;
            docTable.Rows[index]["ambulanceAddress"] = doctor.AmbulanceAddress;

            string update = @"update doctor set firstName = @firstname, lastName = @lastName, UPIN = @UPIN, ambulanceAddress = @ambulanceAddress where id = @id"; ;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(update, conn);
                cmd.Parameters.Add("@firstName", MySqlDbType.VarChar,15,"firstname");
                cmd.Parameters.Add("@id", MySqlDbType.VarChar, 15, "id");
                cmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 15, "lastName");
                cmd.Parameters.Add("@UPIN", MySqlDbType.VarChar, 15, "UPIN");
                cmd.Parameters.Add("@ambulanceAddress", MySqlDbType.VarChar, 15, "ambulanceAddress");
                //select the update command
                adapter.UpdateCommand = cmd;
                //update the data source
                adapter.Update(doc, "Doctors");
                return Request.CreateResponse(HttpStatusCode.OK, "Updated succesfully!");
            }

            catch (MySqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                throw ex;
            }

        }         

        // POST: api/Doctors
        [HttpPost]
        public HttpResponseMessage PostDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            doctors.Add(doctor);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/Doctors/5
        [HttpDelete]
        public HttpResponseMessage DeleteDoctor(Guid id)
        {
            if (!DoctorExists(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                doctors.Remove(doctors.First(i => i.Id == id));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
       
        private bool DoctorExists(Guid id)
        {
            try
            {
                if (doctors.First(i => i.Id == id) != null)
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

        private Doctor ReadSingleRow(IDataRecord dataRecord)
        { 
            Doctor doctor = new Doctor(
                (Guid)(dataRecord[0]), 
                Convert.ToString(dataRecord[1]), 
                Convert.ToString(dataRecord[2]), 
                Convert.ToString(dataRecord[3]), 
                Convert.ToString(dataRecord[4])
            );
            return doctor;
        }        
        
    }
}