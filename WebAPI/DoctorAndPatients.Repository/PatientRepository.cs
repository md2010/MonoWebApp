using DoctorAndPatients.Common;
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
        public async Task<bool> CreateAsync(Patient patient)
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
                await insertCmd.ExecuteNonQueryAsync();               
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            PrepareForConnection();
            try
            {                
                string delete = @"delete from patient where id = @id;";
                MySqlCommand deleteCmd = new MySqlCommand(delete, conn);
                deleteCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;

                conn.Open();
                await deleteCmd.ExecuteNonQueryAsync();
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

        public async Task<List<Patient>> FindAsync(Paging paging, List<Sort> sorts, 
            DiagnosisFilter diagnosisFilter, DateOfBirthFilter dateFilter)
        {
            PrepareForConnection();
            try
            {
                //paging, sorting, filtering
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM patient WHERE 1=1 ");

                if(diagnosisFilter != null)
                    if(String.IsNullOrWhiteSpace(diagnosisFilter.Diagnosis))
                {
                    sb.Append("AND diagnosis = @diagnosis ");
                }
                if(dateFilter != null)
                {
                    if (dateFilter.DateOfBirth != default)
                    {
                        sb.Append("AND dateOfBirth >= @date ");
                    }                    
                }

                sb.Append("ORDER BY ");
                for(int i = 0; i < sorts.Count; i++)
                {
                    if (sorts[i].SortOrder == "asc")
                        sb.Append($"{sorts[i].SortBy} ASC ");
                    else
                        sb.Append($"{sorts[i].SortBy} DESC ");
                    if(i < (sorts.Count - 1))
                        sb.Append(", ");
                }              
                          
                int offset = (paging.PageNumber - 1) * paging.Rpp;
                sb.Append("LIMIT @rpp OFFSET @offset");

                //DB communication
                MySqlCommand command = new MySqlCommand(sb.ToString(), conn);
                command.Parameters.Add("@rpp", MySqlDbType.Int32, 4, "rpp").Value = paging.Rpp;
                command.Parameters.Add("@offset", MySqlDbType.Int32, 4, "offset").Value = offset;
                if(diagnosisFilter != null && String.IsNullOrWhiteSpace(diagnosisFilter.Diagnosis)) 
                    command.Parameters.Add("@diagnosis", MySqlDbType.VarChar, 100, "diagnosis").Value = diagnosisFilter.Diagnosis;
                if(dateFilter != null && dateFilter.DateOfBirth != default)
                    command.Parameters.Add("@date", MySqlDbType.DateTime).Value = dateFilter.DateOfBirth;

                conn.Open();
                var reader = await command.ExecuteReaderAsync();

                List<Patient> list = new List<Patient>();               
                while (await reader.ReadAsync())
                {                   
                    list.Add(MapToObject(reader));
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

        public async Task<Patient> GetByIDAsync(Guid id)
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
                
                Patient patient = new Patient();
                while (await reader.ReadAsync())
                {
                    patient = MapToObject(reader);                  
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

        public async Task<bool> UpdateAsync(Guid id, Patient patient)
        {
            PrepareForConnection();
            try
            {
                patient = await CheckEmptyEntriesAsync(id, patient);
                string update = @"update patient set id = @id, firstName = @firstname, lastName = @lastName, healthInsuranceID = @healthInsuranceID, 
                        diagnosis = @diagnosis, doctorId = @doctorId where id = @id;";
                MySqlCommand updateCmd = new MySqlCommand(update, conn);
                updateCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;
                updateCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = patient.FirstName;
                updateCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = patient.LastName;
                updateCmd.Parameters.Add("@healthInsuranceID", MySqlDbType.Int32, 6, "healthInsuranceID").Value = patient.HealthInsuranceID;
                updateCmd.Parameters.Add("@diagnosis", MySqlDbType.VarChar, 100, "diagnosis").Value = patient.Diagnosis;
                updateCmd.Parameters.Add("@doctorId", MySqlDbType.VarChar, 36, "doctorId").Value = patient.DoctorId;

                conn.Open();
                await updateCmd.ExecuteNonQueryAsync();
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

        private async Task<Patient> CheckEmptyEntriesAsync(Guid id, Patient patient)
        {
            Patient patientDB = await GetByIDAsync(id);
            patient.Id = id;
            patient.FirstName = patient.FirstName == null ? patientDB.FirstName : patient.FirstName;
            patient.LastName = patient.LastName == null ? patientDB.LastName : patient.LastName;
            patient.HealthInsuranceID = patient.HealthInsuranceID == 0 ? patientDB.HealthInsuranceID : patient.HealthInsuranceID;
            patient.Diagnosis = patient.Diagnosis == null ? patientDB.Diagnosis : patient.Diagnosis;
            patient.DoctorId = patient.DoctorId == default(Guid) ? patientDB.DoctorId : patient.DoctorId;
            patient.DateOfBirth = patientDB.DateOfBirth;
            patient.CreatedAt = patientDB.CreatedAt;
            return patient;
        }

        private Patient MapToObject(IDataRecord reader)
        {
            Patient patient = new Patient();
            patient.Id = (Guid)(reader[0]);
            patient.FirstName = Convert.ToString(reader[1]);
            patient.LastName = Convert.ToString(reader[2]);
            patient.HealthInsuranceID = Convert.ToInt32(reader[3]);
            patient.Diagnosis = Convert.ToString(reader[4]);
            patient.DoctorId = (Guid)(reader[5]);
            patient.DateOfBirth = Convert.ToDateTime(reader[6]);
            patient.CreatedAt = Convert.ToDateTime(reader[7]);
            return patient;
        }
        
    }
}
