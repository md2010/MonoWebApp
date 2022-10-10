﻿using DoctorAndPatients.Model;
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
        public async Task<bool> CreateAsync(List<Doctor> doctors)
        {
            PrepareForConnection();
            try
            {
                conn.Open();
                foreach (Doctor doctor in doctors)
                {
                    string insert = @"insert into doctor values (@id, @firstname, @lastName, @UPIN, @ambulanceAddress);";
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(); NOT WORKING
                    MySqlCommand insertCmd = new MySqlCommand(insert, conn);
                    insertCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = doctor.Id;
                    insertCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = doctor.FirstName;
                    insertCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = doctor.LastName;
                    insertCmd.Parameters.Add("@UPIN", MySqlDbType.VarChar, 6, "UPIN").Value = doctor.UPIN;
                    insertCmd.Parameters.Add("@ambulanceAddress", MySqlDbType.VarChar, 20, "ambulanceAddress").Value = doctor.AmbulanceAddress;
                  
                    await insertCmd.ExecuteNonQueryAsync();
                }
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
                if (await GetByIDAsync(id) != null)
                {
                    string delete = @"delete from doctor where id = @id;";
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

        public async Task<List<Doctor>> GetAllAsync()
        {
            PrepareForConnection();
            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM doctor", conn);
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

        /* IF ADAPTER WORKS 
         * private DataSet GetAllWithAdapter()
        {
            PrepareForConnection();
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM doctor", conn);
                DataSet dsDoctor = new DataSet();
                adapter.Fill(dsDoctor, "doctor");
                return dsDoctor;
            }
            catch (MySqlException ex)
            {
                return null;
                throw ex;
            }
        }*/

        public async Task<bool> UpdateAsync(Guid id, List<Doctor> doctors)
        {
            PrepareForConnection();
            try
            {
            conn.Open();
            foreach(Doctor d in doctors)
            {                   
                Doctor doctor = await CheckEmptyEntriesAsync(id, d); //TO AVOID - if user just enterd first name, other properties will become empty
                string update = @"update doctor set firstName = @firstname, lastName = @lastName, UPIN = @UPIN, 
                    ambulanceAddress = @ambulanceAddress where id = @id";
                MySqlCommand updateCmd = new MySqlCommand(update, conn);
                updateCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = id;
                updateCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = doctor.FirstName;
                updateCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = doctor.LastName;
                updateCmd.Parameters.Add("@UPIN", MySqlDbType.VarChar, 6, "UPIN").Value = doctor.UPIN;
                updateCmd.Parameters.Add("@ambulanceAddress", MySqlDbType.VarChar, 20, "ambulanceAddress").Value = doctor.AmbulanceAddress;

                updateCmd.ExecuteNonQuery();
                }
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
            doctor.FirstName = doctor.FirstName == null ? doctorDB.FirstName : doctor.FirstName;
            doctor.LastName = doctor.LastName == null ? doctorDB.LastName : doctor.LastName;
            doctor.AmbulanceAddress = doctor.AmbulanceAddress == null ? doctorDB.AmbulanceAddress : doctor.AmbulanceAddress;
            return doctor;
        }

        private Doctor MapToObject(IDataRecord dataRecord)
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
