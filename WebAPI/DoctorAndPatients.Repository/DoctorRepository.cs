using DoctorAndPatients.Model;
using DoctorAndPatients.RepositoryCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DoctorAndPatients.Common;

namespace DoctorAndPatients.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private static readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private MySqlConnection conn;        

        private void PrepareForConnection()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connectionString; 
        }
        public async Task<bool> CreateAsync(Doctor doctor)
        {
            PrepareForConnection();
            try
            {                               
                string insert = @"insert into doctor values (@id, @firstname, @lastName, @UPIN, @ambulanceAddress);";
                MySqlCommand insertCmd = new MySqlCommand(insert, conn);
                insertCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = doctor.Id;
                insertCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = doctor.FirstName;
                insertCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = doctor.LastName;
                insertCmd.Parameters.Add("@UPIN", MySqlDbType.VarChar, 6, "UPIN").Value = doctor.UPIN;
                insertCmd.Parameters.Add("@ambulanceAddress", MySqlDbType.VarChar, 20, "ambulanceAddress").Value = doctor.AmbulanceAddress;

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
                string delete = @"delete from doctor where id = @id;";
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

        public async Task<List<Doctor>> FindAsync(Paging paging, List<Sort> sorts,
            AmbulanceAddressFilter filter)
        {
            PrepareForConnection();
            try
            {
                //paging, sorting, filtering
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM doctor ");
                if(filter != null)
                {
                    if(filter.AmbulanceAddress != "")
                        sb.Append("WHERE ambulanceAddress = @address ");
                }

                sb.Append("ORDER BY ");
                for (int i = 0; i < sorts.Count; i++)
                {
                    if (sorts[i].SortOrder == "asc")
                        sb.Append($"{sorts[i].SortBy} ASC ");
                    else
                        sb.Append($"{sorts[i].SortBy} DESC ");
                    if (i < (sorts.Count - 1))
                        sb.Append(", ");
                }

                int offset = (paging.PageNumber - 1) * paging.Rpp;
                sb.Append("LIMIT @rpp OFFSET @offset");
                
                //DB communication
                MySqlCommand command = new MySqlCommand(sb.ToString(), conn);
                command.Parameters.Add("@rpp", MySqlDbType.Int32, 4, "rpp").Value = paging.Rpp;
                command.Parameters.Add("@offset", MySqlDbType.Int32, 4, "offset").Value = offset;
                if (String.IsNullOrWhiteSpace(filter.AmbulanceAddress))
                    command.Parameters.Add("@address", MySqlDbType.VarChar, 20, "ambulanceAddress")
                        .Value = filter.AmbulanceAddress;

                conn.Open();
                var reader = await command.ExecuteReaderAsync();

                List<Doctor> list = new List<Doctor>();
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

        public async Task<Doctor> GetByIDAsync(Guid id)
        {
            PrepareForConnection();
            try
            {                       
                MySqlCommand selectCmd = new MySqlCommand("SELECT * FROM doctor WHERE id = @id", conn);
                selectCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;
                conn.Open();

                MySqlDataReader reader = selectCmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return null;
                }             
                Doctor doctor = null;
                while (await reader.ReadAsync())
                {
                    doctor = MapToObject(reader);
                }

                conn.Close();
                return doctor;
            }

            catch (MySqlException ex)
            {
                return null;
                throw ex;
            }
        }
       
        public async Task<bool> UpdateAsync(Guid id, Doctor doctor)
        {
            PrepareForConnection();
            try
            {               
                doctor = await CheckEmptyEntriesAsync(id, doctor); //TO AVOID - if user just enterd first name, other properties will become empty
                string update = @"update doctor set firstName = @firstname, lastName = @lastName, UPIN = @UPIN, 
                    ambulanceAddress = @ambulanceAddress where id = @id";
                MySqlCommand updateCmd = new MySqlCommand(update, conn);
                updateCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;
                updateCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = doctor.FirstName;
                updateCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = doctor.LastName;
                updateCmd.Parameters.Add("@UPIN", MySqlDbType.VarChar, 6, "UPIN").Value = doctor.UPIN;
                updateCmd.Parameters.Add("@ambulanceAddress", MySqlDbType.VarChar, 20, "ambulanceAddress").Value = doctor.AmbulanceAddress;

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

        private async Task<Doctor> CheckEmptyEntriesAsync(Guid id, Doctor doctor)
        {
            Doctor doctorDB = await GetByIDAsync(id);
            doctor.Id = id;
            doctor.FirstName = doctor.FirstName == null ? doctorDB.FirstName : doctor.FirstName;
            doctor.LastName = doctor.LastName == null ? doctorDB.LastName : doctor.LastName;
            doctor.AmbulanceAddress = doctor.AmbulanceAddress == null ? doctorDB.AmbulanceAddress : doctor.AmbulanceAddress;
            return doctor;
        }

        private Doctor MapToObject(IDataRecord dataRecord)
        {
            Doctor doctor = new Doctor();
            doctor.Id = (Guid)(dataRecord[0]);
            doctor.FirstName = Convert.ToString(dataRecord[1]);
            doctor.LastName = Convert.ToString(dataRecord[2]);
            doctor.UPIN = Convert.ToString(dataRecord[3]);
            doctor.AmbulanceAddress = Convert.ToString(dataRecord[4]);
            doctor.CreatedAt = Convert.ToDateTime(dataRecord[5]);           
            return doctor;
        }
    }
}
