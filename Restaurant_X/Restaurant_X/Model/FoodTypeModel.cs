using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Restaurant_X.Model;

namespace Restaurant_X.Model
{
    public class FoodTypeModel
    {
        public int foodTypeId { get; set; }
        public string foodTypeName { get; set; }

        #region SQL Server Connection Setting
        static string ConnectionString = File.ReadAllText("C:\\Users\\forca\\Desktop\\Training_Sessions\\Project1\\Restaurant_X\\Restaurant_X\\ConnectionString.txt");

        SqlConnection connection = new SqlConnection(ConnectionString);
        #endregion

        #region Create
        public string AddFoodType(FoodTypeModel newFoodType)
        {
            SqlCommand cmd_CreateCustomer = new SqlCommand("INSERT INTO FoodType " +
                                                           "VALUES(@FoodTypeName)", connection);
            cmd_CreateCustomer.Parameters.AddWithValue("@FoodTypeName", newFoodType.foodTypeName);
    
            try
            {
                connection.Open();
                cmd_CreateCustomer.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return "New Food Type Added Successfully!";
        }
        // IMPLEMENTED ^
        public string AddFoodType(string newFoodType)
        {
            SqlCommand cmd_CreateCustomer = new SqlCommand("INSERT INTO FoodType " +
                                                           "VALUES(@FoodTypeName)", connection);
            cmd_CreateCustomer.Parameters.AddWithValue("@FoodTypeName", newFoodType);
            if (newFoodType is not null)
            {
                try
                {
                    connection.Open();
                    cmd_CreateCustomer.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return "New Food Type Added Successfully!";
            }
            else
                throw new Exception("Invalid Data Input - Food Type");
        }
        // IMPLEMENTED ^
        #endregion

        #region Read
        public List<FoodTypeModel> GetFoodTypeFull()
        {
            SqlCommand cmd_FoodTypes = new SqlCommand("SELECT * FROM FoodType", connection);
            List<FoodTypeModel> foodTypeList = new List<FoodTypeModel>();
            SqlDataReader readFoodTypes = null;
            try
            {
                connection.Open();
                readFoodTypes = cmd_FoodTypes.ExecuteReader();

                while (readFoodTypes.Read())
                {
                    foodTypeList.Add(new FoodTypeModel()
                    {
                        foodTypeId = Convert.ToInt32(readFoodTypes[0]),
                        foodTypeName = readFoodTypes[1].ToString(),
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                readFoodTypes.Close();
                connection.Close();
            }
            return foodTypeList;
        }
        // IMPLEMENTED ^
        public int GetFoodTypeCount()
        {
            int count = 0;

            SqlCommand cmd_FoodTypes = new SqlCommand("SELECT COUNT(*) FROM FoodType", connection);
            try
            {
                connection.Open();
                count = Convert.ToInt32(cmd_FoodTypes.ExecuteScalar());
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return count;
        }
        // IMPLEMENTED ^
        #endregion

        #region Update
        public string UpdateFoodType(FoodTypeModel updateFoodType)
        {
            SqlCommand cmd_UpdateFood = new SqlCommand("UPDATE Food SET FoodTypeName = @FoodTypeName " +
                                                       "WHERE FoodTypeID = @FoodID");
            cmd_UpdateFood.Parameters.AddWithValue("@FoodTypeID", updateFoodType.foodTypeId);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodTypeName", updateFoodType.foodTypeName);

            FoodTypeModel temp = new FoodTypeModel();
            int foodTypeMax = temp.GetFoodTypeCount();

            if (updateFoodType.foodTypeId >= 1 && updateFoodType.foodTypeId <= foodTypeMax)
            {
                if (updateFoodType.foodTypeName is not null)
                {
                    try
                    {
                        connection.Open();
                        cmd_UpdateFood.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return "Food Updated In Menu Successfully!";
                }
                else
                    throw new Exception("Invalid Data Input - Food Type Name");
            }
            else
                throw new Exception("Invalid Data Input - Food Type ID");
        }
        // IMPLEMENTED ^
        #endregion

        #region Delete
        public string RemoveFoodType(int? foodTypeID)
        {
            SqlCommand cmd_RemoveFood = new SqlCommand("DELETE FROM FoodType WHERE FoodTypeID = @RemoveID", connection);
            cmd_RemoveFood.Parameters.AddWithValue("@RemoveID", foodTypeID);

            FoodTypeModel temp = new FoodTypeModel();
            int foodTypeMax = temp.GetFoodTypeCount();

            if ((foodTypeID is not null) && foodTypeID >= 1 && foodTypeID <= foodTypeMax)
            {
                try
                {
                    connection.Open();
                    cmd_RemoveFood.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return "Food Type Removed Successfully!";
            }
            else
                throw new Exception("Invalid Data Input - Food Type");
        }
        // IMPLEMENTED ^
        #endregion


    }
}
