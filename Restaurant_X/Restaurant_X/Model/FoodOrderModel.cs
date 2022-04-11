using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Restaurant_X.Model;

namespace Restaurant_X.Model
{
    public class FoodOrderModel
    {
        #region Properties
        public int foodOrderId { get; set; }
        public int foodOrderCustomer { get; set; }
        public int foodOrderItems { get; set; }
        public int foodOrderItemQty { get; set; }
        public bool foodOrderCompleted { get; set; }
        #endregion

        #region SQL Service Connection Setting
        static string ConnectionString = File.ReadAllText("C:\\Users\\forca\\Desktop\\Training_Sessions\\Project1\\Restaurant_X\\Restaurant_X\\ConnectionString.txt");

        SqlConnection connection = new SqlConnection(ConnectionString);
        #endregion


        #region Create
        public string CreateFoodOrder(int? customerID, int? newFoodToOrder, int? newFoodQty)
        {
            CustomerOrderModel temp = new CustomerOrderModel();
            int orderID = temp.GetLatestCustomerOrderID();

            SqlCommand cmd_CreateCustomerFoodOrder = new SqlCommand("INSERT INTO FoodOrder VALUES(@CustomerOrderID, @CustomerID, " +
                                                                    "@FoodItem, @FoodItemQty, @OrderCompleted)", connection);
            cmd_CreateCustomerFoodOrder.Parameters.AddWithValue("@CustomerOrderID", orderID);
            cmd_CreateCustomerFoodOrder.Parameters.AddWithValue("@CustomerID", customerID);
            cmd_CreateCustomerFoodOrder.Parameters.AddWithValue("@FoodItem", newFoodToOrder);
            cmd_CreateCustomerFoodOrder.Parameters.AddWithValue("@FoodItemQty", newFoodQty);
            cmd_CreateCustomerFoodOrder.Parameters.AddWithValue("@OrderCompleted", false);

            CustomerModel customerTemp = new CustomerModel();
            int customerIDMax = customerTemp.GetLatestCustomerID();
            FoodModel foodTemp = new FoodModel();
            int foodMax = foodTemp.GetFoodCount();
            
            if((customerID is not null) && customerID >= 1 && customerID <= customerIDMax)
            {
                if ((newFoodToOrder is not null) && newFoodToOrder >= 1 && newFoodToOrder <= foodMax)
                { 
                    if (newFoodQty is not null)
                    {
                        try
                        {
                            connection.Open();
                            cmd_CreateCustomerFoodOrder.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }

                        return "CREATE: Food Added To Order";
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - FOOD QUANTITY");
                }
                else
                    throw new Exception("INVALID DATA INPUT - FOOD ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        #endregion

        #region Read
        public List<FoodOrderModel> GetCustomerOrderFoodList(int? customerOrderID)
        {

            List<FoodOrderModel> customerOrderFoodList = new List<FoodOrderModel>();
            SqlCommand cmd_GetCustomerOrderFoodList = new SqlCommand("SELECT * FROM FoodOrder " +
                                                                     "WHERE FoodOrderID = @OrderID ", connection);
            cmd_GetCustomerOrderFoodList.Parameters.AddWithValue("@OrderID", customerOrderID);
            SqlDataReader readCustomerFoodList = null;

            CustomerOrderModel temp = new CustomerOrderModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();

            if ((customerOrderID is not null) && customerOrderID >= 1 && customerOrderID <= customerOrderMax)
            {
                try
                {
                    connection.Open();
                    readCustomerFoodList = cmd_GetCustomerOrderFoodList.ExecuteReader();

                    while (readCustomerFoodList.Read())
                    {
                        customerOrderFoodList.Add(new FoodOrderModel()
                        {
                            foodOrderId = Convert.ToInt32(readCustomerFoodList[0]),
                            foodOrderCustomer = Convert.ToInt32(readCustomerFoodList[1]),
                            foodOrderItems = Convert.ToInt32(readCustomerFoodList[2]),
                            foodOrderItemQty = Convert.ToInt32(readCustomerFoodList[3]),
                            foodOrderCompleted = Convert.ToBoolean(readCustomerFoodList[4])
                        });
                    }
                }
                catch (SqlException ex)
                {

                    throw new Exception(ex.Message);
                }

                return customerOrderFoodList;
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER ID");
        }
        // IMPLEMENTED ^
        public int GetFoodOrderItemCountByIDs(int? foodOrderID, int? customerID)
        {
            int count = 0;
            SqlCommand cmd_GetFoodOrderItemCount = new SqlCommand("SELECT COUNT(*) FROM FoodOrder " +
                                                                  "WHERE FoodOrderID = @FoodOrderID AND FoodOrderCustomer = @CustomerID", connection);
            cmd_GetFoodOrderItemCount.Parameters.AddWithValue("@FoodOrderID", foodOrderID);
            cmd_GetFoodOrderItemCount.Parameters.AddWithValue("@CustomerID", customerID);
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();

            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    try
                    {
                        connection.Open();
                        count = Convert.ToInt32(cmd_GetFoodOrderItemCount.ExecuteScalar());
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    if (count > 0)
                        return count;
                    else
                        throw new Exception("NOT FOUND - NO FOOD ITEM FOUND FOR FOOD ORDER");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ORDER ID");

            
        }
        // IMPLEMENTED ^
        public int GetFoodOrderItemInOrderCountByIDs(int? foodOrderID, int? customerID, int? foodItem)
        {
            int count = 0;
            SqlCommand cmd_GetFoodOrderItemInOrderCountByIDs = new SqlCommand("SELECT COUNT(FoodOrderItems) FROM FoodOrder " +
                                                                              "WHERE FoodOrderID = @foodOrderID AND FoodOrderCustomer = @customerID " +
                                                                              "AND FoodOrderItems = @foodItem", connection);
            cmd_GetFoodOrderItemInOrderCountByIDs.Parameters.AddWithValue("@foodOrderID", foodOrderID);
            cmd_GetFoodOrderItemInOrderCountByIDs.Parameters.AddWithValue("@customerID", customerID);
            cmd_GetFoodOrderItemInOrderCountByIDs.Parameters.AddWithValue("@foodItem", foodItem);
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();

            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    try
                    {
                        connection.Open();
                        count = Convert.ToInt32(cmd_GetFoodOrderItemInOrderCountByIDs.ExecuteScalar());
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
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER ID");
         

        }
        // IMPLEMENTED ^
        #endregion

        #region Update
        public string UpdateFoodOrderItemByIDs(int? foodOrderID, int? customerID, int? oldFoodItem, int? newFoodItem, int? newFoodItemQty)
        {
            #region variable used to check if Food Order Item exist in Food Order
            FoodOrderModel temp1 = new FoodOrderModel();
            #endregion

            #region SQL Command
            SqlCommand cmd_UpdateFoodOrderItemByIDs = new SqlCommand("UPDATE FoodOrder SET FoodOrderItems = @newFoodItem, FoodOrderItemQty = @newFoodItemQty " + 
                                                                     "WHERE FoodOrderID = @foodOrderID AND FoodOrderCustomer = @customerID " +
                                                                     "AND FoodOrderItems = @oldFoodItem", connection);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodOrderID", foodOrderID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@customerID", customerID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@oldFoodItem", oldFoodItem);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@newFoodItem", newFoodItem);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@newFoodItemQty", newFoodItemQty);
            #endregion

            #region Get Upper Limit for ID validation
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();
            FoodModel food = new FoodModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();
            int foodMax = food.GetFoodCount();
            #endregion
            

            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    if ((oldFoodItem is not null) && oldFoodItem >= 1 && oldFoodItem <= foodMax)
                    {
                        if ((newFoodItem is not null) && newFoodItem >= 1 && newFoodItem <= foodMax)
                        {
                            if (newFoodItemQty is not null && newFoodItemQty < 0)
                            {
                                if (temp1.GetFoodOrderItemInOrderCountByIDs(foodOrderID, customerID, oldFoodItem) == 1)
                                {
                                    try
                                    {
                                        connection.Open();
                                        cmd_UpdateFoodOrderItemByIDs.ExecuteNonQuery();
                                    }
                                    catch (SqlException ex)
                                    {
                                        throw new Exception(ex.Message);
                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                    return "UPDATE: Food Item " + oldFoodItem +
                                           " Change To Food Item " + newFoodItem +
                                           " In Food Order " + foodOrderID + ".";
                                }
                                else
                                    throw new Exception("NOT FOUND - FOOD ITEM NOT FOUND IN FOOD ORDER");
                            }
                            else
                                throw new Exception("INVALID DATA INPUT - FOOD ITEM QUANTITY");
                        }
                        else
                            throw new Exception("INVALID DATA INPUT - FOOD ITEM");
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - FOOD ITEM");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ID");
                
        }
        // IMPLEMENTED ^
        public string UpdateFoodOrderItemQtyByIDs(int? foodOrderID, int? customerID, int? foodItem, int? foodItemQty)
        {
            #region variable used to check if Food Order Item exist in Food Order
            FoodOrderModel temp1 = new FoodOrderModel();
            #endregion

            #region SQL Command
            SqlCommand cmd_UpdateFoodOrderItemByIDs = new SqlCommand("UPDATE FoodOrder SET FoodOrderItemQty = @foodItemQty " +
                                                                     "WHERE FoodOrderID = @foodOrderID AND FoodOrderCustomer = @customerID " +
                                                                     "AND FoodOrderItems = @foodItem", connection);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodOrderID", foodOrderID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@customerID", customerID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodItem", foodItem);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodItemQty", foodItemQty);
            #endregion

            #region Get Upper Limit for ID validation
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();
            FoodModel food = new FoodModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();
            int foodMax = food.GetFoodCount();
            #endregion

            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    if ((foodItem is not null) && foodItem >= 1 && foodItem <= foodMax)
                    {
                        if (foodItemQty is not null && foodItemQty < 0)
                        {
                            if (temp1.GetFoodOrderItemInOrderCountByIDs(foodOrderID, customerID, foodItem) == 1)
                            {
                                try
                                {
                                    connection.Open();
                                    cmd_UpdateFoodOrderItemByIDs.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    throw new Exception(ex.Message);
                                }
                                finally
                                {
                                    connection.Close();
                                }
                                return "UPDATE: Food Item " + foodItem + " Quantity Updated In Food Order " + foodOrderID + ".";
                            }
                            else
                                throw new Exception("NOT FOUND - FOOD ITEM NOT FOUND IN FOOD ORDER");
                        }
                        else
                            throw new Exception("INVALID DATA INPUT - FOOD ITEM QUANTITY");
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - FOOD ITEM");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ORDER ID");

        }
        // IMPLEMENTED ^
        public string UpdateFoodOrderItemCompleted(int? foodOrderID, int? customerID, int? foodItem, bool? itemCompleted)
        {
            #region variable used to check if Food Order Item exist in Food Order
            FoodOrderModel temp1 = new FoodOrderModel();
            #endregion

            #region SQL Command
            SqlCommand cmd_UpdateFoodOrderItemByIDs = new SqlCommand("UPDATE FoodOrder SET FoodOrderCompleted = @itemCompleted " +
                                                                     "WHERE FoodOrderID = @foodOrderID AND FoodOrderCustomer = @customerID " +
                                                                     "AND FoodOrderItems = @foodItem", connection);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodOrderID", foodOrderID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@customerID", customerID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodItem", foodItem);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@itemCompleted", itemCompleted);
            #endregion

            #region Get Upper Limit for ID validation
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();
            FoodModel food = new FoodModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();
            int foodMax = food.GetFoodCount();
            #endregion


            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    if ((foodItem is not null) && foodItem >= 1 && foodItem <= foodMax)
                    {
                        if (temp1.GetFoodOrderItemInOrderCountByIDs(foodOrderID, customerID, foodItem) == 1)
                        {
                            try
                            {
                                connection.Open();
                                cmd_UpdateFoodOrderItemByIDs.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                            return "UPDATE: Food Item " + foodItem + " Completed In Food Order " + foodOrderID + ".";
                        }
                        else
                            throw new Exception("NOT FOUND - FOOD ITEM NOT FOUND IN FOOD ORDER");
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - FOOD ITEM");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ORDER ID");
          

        }
        // IMPLEMENTED

        #endregion

        #region Delete
        public string RemoveCustomerFoodItem(int? foodOrderID, int? customerID, int? foodItem)
        {
            #region variable used to check if Food Order Item exist in Food Order
            FoodOrderModel temp1 = new FoodOrderModel();
            #endregion

            #region SQL Command
            SqlCommand cmd_UpdateFoodOrderItemByIDs = new SqlCommand("DELETE FROM FoodOrder " +
                                                                     "WHERE FoodOrderID = @foodOrderID AND FoodOrderCustomer = @customerID " +
                                                                     "AND FoodOrderItems = @foodItem", connection);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodOrderID", foodOrderID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@customerID", customerID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodItem", foodItem);
            #endregion

            #region Get Upper Limit for ID validation
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();
            FoodModel food = new FoodModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();
            int foodMax = food.GetFoodCount();
            #endregion

            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    if ((foodItem is not null) && (foodItem >= 1) && foodItem <= foodMax)
                    {
                        if (temp1.GetFoodOrderItemInOrderCountByIDs(foodOrderID, customerID, foodItem) == 1)
                        {
                            try
                            {
                                connection.Open();
                                cmd_UpdateFoodOrderItemByIDs.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                            return "DELETE: Food Item " + foodItem + " Removed From Food Order " + foodOrderID + ".";
                        }
                        else
                            throw new Exception("NOT FOUND - FOOD ITEM NOT FOUND IN FOOD ORDER");
                    }
                    else 
                        throw new Exception("INVALID DATA INPUT - FOOD ITEM");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ORDER ID");
            
        }
        // IMPLEMENTED ^
        public string RemoveCustomerFoodItemsAll(int? foodOrderID, int? customerID)
        {
            #region SQL Command
            SqlCommand cmd_UpdateFoodOrderItemByIDs = new SqlCommand("DELETE FROM FoodOrder " +
                                                                     "WHERE FoodOrderID = @foodOrderID " +
                                                                     "AND FoodOrderCustomer = @customerID ", connection);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@foodOrderID", foodOrderID);
            cmd_UpdateFoodOrderItemByIDs.Parameters.AddWithValue("@customerID", customerID);
            #endregion

            #region Get Upper Limit for ID validation
            CustomerOrderModel temp = new CustomerOrderModel();
            CustomerModel customer = new CustomerModel();

            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerMax = customer.GetLatestCustomerID();
            #endregion

            if ((foodOrderID is not null) && foodOrderID >= 1 && foodOrderID <= customerOrderMax)
            {
                if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
                {
                    try
                    {
                        connection.Open();
                        cmd_UpdateFoodOrderItemByIDs.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return "DELETE: All Food Items Removed From Food Order " + foodOrderID + ".";

                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            }
            else
                throw new Exception("INVALID DATA INPUT - FOOD ORDER ID");

        }
        // IMPLEMENTED ^
        #endregion

    }
}
