using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Restaurant_X.Model;

namespace Restaurant_X.Model
{
    public class FoodModel
    {
        #region Properties
        public int foodId { get; set; }
        public string foodName { get; set; }
        public int foodType { get; set; }
        public string foodCategory { get; set; }
        public double foodPrice { get; set; }
        public bool foodAvailability { get; set; }
        #endregion

        #region SQL Server Connection Setting
        static string ConnectionString = File.ReadAllText("C:\\Users\\forca\\Desktop\\Training_Sessions\\Project1\\Restaurant_X\\Restaurant_X\\ConnectionString.txt");

        SqlConnection connection = new SqlConnection(ConnectionString);
        #endregion

        #region Create
        public string AddFoodToMenu(FoodModel newFood)
        {
            SqlCommand cmd_CreateFood = new SqlCommand("INSERT INTO Food " +
                                                       "VALUES(@FoodName, @FoodType, @FoodPrice, @FoodAvailability)", connection);
            cmd_CreateFood.Parameters.AddWithValue("@FoodName", newFood.foodName);
            cmd_CreateFood.Parameters.AddWithValue("@FoodType", newFood.foodType);
            cmd_CreateFood.Parameters.AddWithValue("@FoodPrice", newFood.foodPrice);
            cmd_CreateFood.Parameters.AddWithValue("@FoodAvailability", newFood.foodAvailability);
            
            FoodTypeModel temp = new FoodTypeModel();
            int foodTypeMax = temp.GetFoodTypeCount();

            if (newFood.foodName is not null)
            {
                if (newFood.foodType >= 1 && newFood.foodType <= foodTypeMax)
                {
                    if (newFood.foodPrice != 0)
                    {
                        try
                        {
                            connection.Open();
                            cmd_CreateFood.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                        return "New Food Dish Added To Menu Successfully!";

                    }
                    else
                        throw new Exception("INVALID DATA INPUT - FOOD PRICE");
                }
                else
                    throw new Exception("INVALID DATA INPUT - FOOD TYPE");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD NAME");

        }
        // IMPLEMENTED^
        public string AddFoodToMenu(string newFoodName, int? newFoodType, double? newFoodPrice, bool? newFoodAvailability)
        {
            SqlCommand cmd_CreateFood = new SqlCommand("INSERT INTO Food " +
                                                       "VALUES(@FoodName, @FoodType, @FoodPrice, @FoodAvailability)", connection);
            cmd_CreateFood.Parameters.AddWithValue("@FoodName", newFoodName);
            cmd_CreateFood.Parameters.AddWithValue("@FoodType", newFoodType);
            cmd_CreateFood.Parameters.AddWithValue("@FoodPrice", newFoodPrice);
            cmd_CreateFood.Parameters.AddWithValue("@FoodAvailability", newFoodAvailability);
            
            if (newFoodName is not null)
            {
                if (newFoodType is not null)
                {
                    if (newFoodPrice is not null)
                    {
                        if (newFoodAvailability is not null)
                        {
                            try
                            {
                                connection.Open();
                                cmd_CreateFood.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                            return "New Food Dish Added To Menu Successfully!";

                        }
                        else
                            throw new Exception("INVALID DATA INPUT - FOOD AVAILABILITY");
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - FOOD PRICE");
                }
                else
                    throw new Exception("INVALID DATA INPUT - FOOD TYPE");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD NAME");
        }
        // IMPLEMENTED ^
        #endregion

        #region Read
        public List<FoodModel> GetFoodMenuFull()
        {
            SqlCommand cmd_FoodMenu = new SqlCommand("SELECT Food.FoodID, Food.FoodName, " +
                                                            "FoodType.FoodTypeName, Food.FoodPrice " +
                                                     "FROM Food, FoodType " +
                                                     "WHERE FoodType.FoodTypeID = Food.FoodCategory " +
                                                     "AND Food.FoodAvailable = 1", connection);
            List<FoodModel> foodMenu = new List<FoodModel>();
            SqlDataReader readFoodMenu = null;
            try
            {
                connection.Open();
                readFoodMenu = cmd_FoodMenu.ExecuteReader();

                while (readFoodMenu.Read())
                {
                    foodMenu.Add(new FoodModel()
                    {
                        foodId = Convert.ToInt32(readFoodMenu[0]),
                        foodName = readFoodMenu[1].ToString(),
                        foodCategory = readFoodMenu[2].ToString(),
                        foodPrice = Convert.ToDouble(readFoodMenu[3]),
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception (ex.Message);
            }
            finally
            {
                readFoodMenu.Close();  
                connection.Close();
            }
            return foodMenu;
        }
        // IMPLEMENTED ^
        public List<FoodModel> GetFoodMenuBasedOnType(int? foodType)
        {
            SqlCommand cmd_FoodMenuTypes = new SqlCommand("SELECT Food.FoodID, Food.FoodName, " +
                                                                 "FoodType.FoodTypeName, Food.FoodPrice " +
                                                          "FROM Food, FoodType " +
                                                          "WHERE FoodType.FoodTypeID = Food.FoodCategory " +
                                                          "AND Food.FoodAvailable = 1 " +
                                                          "AND FoodType.FoodTypeID = @FoodType ", connection);
            cmd_FoodMenuTypes.Parameters.AddWithValue("@FoodType", foodType);
            List<FoodModel> foodMenuTypes = new List<FoodModel>();
            SqlDataReader readFoodMenuTypes = null;
            FoodTypeModel temp = new FoodTypeModel();
            int foodTypeMax = temp.GetFoodTypeCount();
            
            if ((foodType is not null) && foodType >= 1 && foodType <= foodTypeMax)
            {
                try
                {
                    connection.Open();
                    readFoodMenuTypes = cmd_FoodMenuTypes.ExecuteReader();

                    while (readFoodMenuTypes.Read())
                    {
                        foodMenuTypes.Add(new FoodModel()
                        {
                            foodId = Convert.ToInt32(readFoodMenuTypes[0]),
                            foodName = readFoodMenuTypes[1].ToString(),
                            foodCategory = readFoodMenuTypes[2].ToString(),
                            foodPrice = Convert.ToDouble(readFoodMenuTypes[3]),
                        });
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    readFoodMenuTypes.Close();
                    connection.Close();
                }
                if (foodMenuTypes.Count > 0)
                    return foodMenuTypes;
                else
                    throw new Exception("NO FOOD ITEM FOUND FOR FOOD CATEGORY");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD TYPE");

        }
        // IMPLEMENTED ^
        public List<FoodModel> GetFoodMenuUnavailable()
        {
            SqlCommand cmd_FoodMenu = new SqlCommand("SELECT Food.FoodID, Food.FoodName, " +
                                                           "FoodType.FoodTypeName, Food.FoodPrice " +
                                                    "FROM Food, FoodType " +
                                                    "WHERE FoodType.FoodTypeID = Food.FoodCategory " +
                                                    "AND Food.FoodAvailable = 0", connection);
            List<FoodModel> foodMenu = new List<FoodModel>();
            SqlDataReader readFoodMenu = null;
            try
            {
                connection.Open();
                readFoodMenu = cmd_FoodMenu.ExecuteReader();

                while (readFoodMenu.Read())
                {
                    foodMenu.Add(new FoodModel()
                    {
                        foodId = Convert.ToInt32(readFoodMenu[0]),
                        foodName = readFoodMenu[1].ToString(),
                        foodCategory = readFoodMenu[2].ToString(),
                        foodPrice = Convert.ToDouble(readFoodMenu[3]),
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                readFoodMenu.Close();
                connection.Close();
            }
            return foodMenu;
        }
        // IMPLEMENTED ^
        public int GetFoodCount()
        {
            int count = 0;

            SqlCommand cmd_FoodTypes = new SqlCommand("SELECT COUNT(*) FROM Food", connection);
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
        public string UpdateFood(FoodModel updateFood)
        {
            SqlCommand cmd_UpdateFood = new SqlCommand("UPDATE Food SET FoodName = @FoodName, FoodCategory = @FoodType, " + 
                                                       "FoodPrice = @FoodPrice, FoodAvailable = @FoodAvailability " +
                                                       "WHERE FoodID = @FoodID");
            cmd_UpdateFood.Parameters.AddWithValue("@FoodID", updateFood.foodId);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodName", updateFood.foodName);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodType", updateFood.foodType);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodPrice", updateFood.foodPrice);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodAvailability", updateFood.foodAvailability);

            FoodTypeModel temp = new FoodTypeModel();
            int foodTypeMax = temp.GetFoodTypeCount();

            if (updateFood.foodName is not null)
            {
                if (updateFood.foodType >= 1 && updateFood.foodType <= foodTypeMax)
                {
                    if (updateFood.foodPrice != 0)
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
                        throw new Exception("INVALID DATA INPUT - FOOD PRICE");
                }
                else
                    throw new Exception("INVALID DATA INPUT - FOOD TYPE");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD NAME");


        }
        // IMPLEMENTED ^
        public string UpdateFoodPrice(int? updateFoodID, double? newFoodPrice)
        {
            SqlCommand cmd_UpdateFood = new SqlCommand("UPDATE Food SET FoodName = FoodPrice = @FoodPrice" +
                                                       "WHERE FoodID = @FoodID");
            cmd_UpdateFood.Parameters.AddWithValue("@FoodID", updateFoodID);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodPrice", newFoodPrice);

            FoodTypeModel tempFoodType = new FoodTypeModel();
            int foodMax = tempFoodType.GetFoodTypeCount();
            if (updateFoodID >= 1 && updateFoodID <= foodMax)
            {
                if (newFoodPrice is not null)
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
                    return "Food Price Updated In Menu Successfully!";
                }
                else
                    throw new Exception("INVALID DATA INPUT - FOOD PRICE");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ID");
        }
        // IMPLEMENTED ^
        public string UpdateFoodAvailability(int? updateFoodID, bool? updateFoodAvailability)
        {
            SqlCommand cmd_UpdateFood = new SqlCommand("UPDATE Food SET FoodAvailable = @FoodAvailability " +
                                                       "WHERE FoodID = @FoodID", connection);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodID", updateFoodID);
            cmd_UpdateFood.Parameters.AddWithValue("@FoodAvailability", updateFoodAvailability);
            FoodModel temp = new FoodModel();
            int foodMax = temp.GetFoodCount();
            if ((updateFoodID is not null) && updateFoodID >= 1 && updateFoodID <= foodMax)
            {
                if (updateFoodAvailability is not null)
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

                    return "Food Availability Updated In Menu Successfully!";
                }
                else
                    throw new Exception("INVALID DATA INPUT - FOOD AVAILABILITY");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ID");

        }
        // IMPLEMENTED ^
        #endregion

        #region Delete
        public string RemoveFood(int? removeFoodID)
        {
            SqlCommand cmd_RemoveFood = new SqlCommand("DELETE FROM Food WHERE FoodID = @RemoveID", connection);
            cmd_RemoveFood.Parameters.AddWithValue("@RemoveID", removeFoodID);

            FoodModel temp = new FoodModel();
            int foodMax = temp.GetFoodCount();
            if ((removeFoodID is not null) && removeFoodID >= 1 && removeFoodID <= foodMax)
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
                return "Food Removed From Menu Successfully!";
            }            
            else
                throw new Exception("INVALID DATA INPUT - FOOD ID");

        }
        // IMPLEMENTED ^
        #endregion


    }
}
