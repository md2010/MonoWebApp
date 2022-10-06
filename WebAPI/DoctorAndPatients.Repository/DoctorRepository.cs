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
        public bool Create(Doctor doctor)
        {
            PrepareForConnection();
            try
            {
                string insert = @"insert into doctor values (@id, @firstname, @lastName, @UPIN, @ambulanceAddress);";
                //MySqlDataAdapter adapter = new MySqlDataAdapter(); NOT WORKING
                MySqlCommand insertCmd = new MySqlCommand(insert, conn);
                insertCmd.Parameters.Add("@id", MySqlDbType.VarChar, 36, "id").Value = doctor.Id;
                insertCmd.Parameters.Add("@firstName", MySqlDbType.VarChar, 30, "firstName").Value = doctor.FirstName;
                insertCmd.Parameters.Add("@lastName", MySqlDbType.VarChar, 50, "lastName").Value = doctor.LastName;
                insertCmd.Parameters.Add("@UPIN", MySqlDbType.VarChar, 6, "UPIN").Value = doctor.UPIN;
                insertCmd.Parameters.Add("@ambulanceAddress", MySqlDbType.VarChar, 20, "ambulanceAddress").Value = doctor.AmbulanceAddress;

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

        public List<Doctor> GetAll()
        {
            PrepareForConnection();
            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM doctor", conn);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                List<Doctor> list = new List<Doctor>();
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

        public Doctor GetByID(Guid id)
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
                while (reader.Read())
                {
                    doctor = ReadSingleRow(reader);
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

        public bool Update(Guid id, Doctor doctor)
        {
            PrepareForConnection();
            try
            {
                if (GetByID(id) != null)
                {
                    string update = @"update doctor set firstName = @firstname, lastName = @lastName, UPIN = @UPIN, 
                            ambulanceAddress = @ambulanceAddress where id = @id";
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(); NOT WORKING
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
