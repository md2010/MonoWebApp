using DoctorAndPatients.Model;
using DoctorAndPatients.RepositoryCommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAndPatients.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private MySqlConnection conn;

        private void PrepareForConnection()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connectionString;
        }
        public bool Create(Patient patient)
        {
            PrepareForConnection();
            try
            {
                string insert = @"insert into patient values (@id, @firstname, @lastName, @healthInsuranceID, @diagnosis, @doctorId);";
                MySqlCommand insertCmd = new MySqlCommand(insert, conn);
                insertCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = patient.Id;
                insertCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = patient.FirstName;
                insertCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = patient.LastName;
                insertCmd.Parameters.Add("@healthInsuranceID", MySqlDbType.Int32, 6, "healthInsuranceID").Value = patient.HealthInsuranceID;
                insertCmd.Parameters.Add("@diagnosis", MySqlDbType.VarChar, 100, "diagnosis").Value = patient.Diagnosis;
                insertCmd.Parameters.Add("@doctorId", MySqlDbType.VarChar, 36, "doctorId").Value = patient.DoctorId;

                conn.Open();
                insertCmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                conn.Close();
                return false;
                throw ex;
            }
        }

        public bool Delete(Guid id)
        {
            PrepareForConnection();
            try
            {
                if (GetByID(id) != null)
                {
                    string delete = @"delete from patient where id = @id;";
                    MySqlCommand deleteCmd = new MySqlCommand(delete, conn);
                    deleteCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;

                    conn.Open();
                    deleteCmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                else //doesn't exist in DB
                {
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                conn.Close();
                return false;
                throw ex;
            }
        }

        public List<Patient> GetAll()
        {
            PrepareForConnection();
            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM patient", conn);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                List<Patient> list = new List<Patient>();
                while (reader.Read())
                {
                    list.Add(ReadSingleRow(reader));
                }
                conn.Close();
                return list;
            }

            catch (MySqlException ex)
            {
                conn.Close();
                return null;
                throw ex;
            }
        }

        public Patient GetByID(Guid id)
        {
            PrepareForConnection();
            try
            {
                MySqlCommand selectCmd = new MySqlCommand("SELECT * FROM patient WHERE id = @id", conn);
                selectCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;
                conn.Open();

                MySqlDataReader reader = selectCmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return null;
                }
                Patient patient = null;
                while (reader.Read())
                {
                    patient = ReadSingleRow(reader);
                }

                conn.Close();
                return patient;
            }

            catch (MySqlException ex)
            {
                return null;
                throw ex;
            }
        }

        public bool Update(Guid id, Patient patient)
        {
            PrepareForConnection();
            try
            {
                string update = @"update patient set id = @id, firstName = @firstname, lastName = @lastName, healthInsuranceID = @healthInsuranceID, 
                        diagnosis = @diagnosis, doctorId = @doctorId where id = @id);";
                MySqlCommand updateCmd = new MySqlCommand(update, conn);
                updateCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = patient.Id;
                updateCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = patient.FirstName;
                updateCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = patient.LastName;
                updateCmd.Parameters.Add("@healthInsuranceID", MySqlDbType.Int32, 6, "healthInsuranceID").Value = patient.HealthInsuranceID;
                updateCmd.Parameters.Add("@diagnosis", MySqlDbType.VarChar, 100, "diagnosis").Value = patient.Diagnosis;
                updateCmd.Parameters.Add("@doctorId", MySqlDbType.VarChar, 36, "doctorId").Value = patient.DoctorId;

                conn.Open();
                updateCmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                conn.Close();
                return false;
                throw ex;
            }
        }

        private Patient ReadSingleRow(IDataRecord dataRecord)
        {
            Patient patient = new Patient(
                (Guid)(dataRecord[0]),
                Convert.ToString(dataRecord[1]),
                Convert.ToString(dataRecord[2]),
                Convert.ToInt32(dataRecord[3]),
                Convert.ToString(dataRecord[4]),
                (Guid)(dataRecord[5])
            );
            return patient;
        }
    }
}
