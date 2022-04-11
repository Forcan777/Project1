using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Restaurant_X.Model;

namespace Restaurant_X.Model
{
    public class CustomerOrderModel
    {
        #region Properties
        public int customerOrderId { get; set; }
        public DateTime customerOrderDate { get; set; }
        public int customerId { get; set; }
        public double customerOrderCost { get; set; }
        public bool customerOrderPaid { get; set; }
        #endregion

        #region SQL Sever Connection Setting
        static string ConnectionString = File.ReadAllText("C:\\Users\\forca\\Desktop\\Training_Sessions\\Project1\\Restaurant_X\\Restaurant_X\\ConnectionString.txt");

        SqlConnection connection = new SqlConnection(ConnectionString);
        #endregion

        #region Create
        
        public string CreateCustomerOrder(int? customerID)
        {
            DateTime newOrderTime = DateTime.Now;

            SqlCommand cmd_CreateCustomerOrder = new SqlCommand("INSERT INTO CustomerOrder VALUES(@OrderTime, " +
                                                                "@CustomerID, @OrderCost, @OrderPaid)", connection);
            cmd_CreateCustomerOrder.Parameters.AddWithValue("@OrderTime", newOrderTime);
            cmd_CreateCustomerOrder.Parameters.AddWithValue("@CustomerID", customerID);
            cmd_CreateCustomerOrder.Parameters.AddWithValue("@OrderCost", 0);
            cmd_CreateCustomerOrder.Parameters.AddWithValue("@OrderPaid", false);

            CustomerModel customer = new CustomerModel();
            int customerMax = customer.GetLatestCustomerID();
            if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
            {
                try
                {
                    connection.Open();
                    cmd_CreateCustomerOrder.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return "CREATE: Customer Order Record Created";
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^ 
        #endregion

        #region Read
        public int GetLatestCustomerOrderID()
        {
            int customerOrderID = 0;
            SqlCommand cmd_CustomerOrderID = new SqlCommand("SELECT CustomerOrderID FROM CustomerOrder " +
                                                            "WHERE CustomerOrderID = ( " + 
                                                            "SELECT IDENT_CURRENT('CustomerOrder'))", connection);

            try
            {
                connection.Open();
                customerOrderID = Convert.ToInt32(cmd_CustomerOrderID.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return customerOrderID;
        }
        // IMPLEMENTED ^ 
        public int GetLatestCustomerID()
        {
            int customerID = 0;
            SqlCommand cmd_CustomerID = new SqlCommand("SELECT CustomerID FROM CustomerOrder " +
                                                       "WHERE CustomerOrderID = ( " +
                                                       "SELECT IDENT_CURRENT('CustomerOrder'))", connection);

            try
            {
                connection.Open();
                customerID = Convert.ToInt32(cmd_CustomerID.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return customerID;
        }
        // IMPLEMENTED ^ 
        public int GetLatestOrderIDByCustomerID(int? customerID)
        {
            SqlCommand cmd_CustomerOrderID = new SqlCommand("SELECT TOP 1 CustomerOrderID FROM CustomerOrder " +
                                                "WHERE CustomerID = @CustomerID " +
                                                "ORDER BY CustomerOrderID DESC", connection);
            cmd_CustomerOrderID.Parameters.AddWithValue("@CustomerID", customerID);
            CustomerModel tempCustomerModel = new CustomerModel();
            int customerIDMax = tempCustomerModel.GetLatestCustomerID();
            int customerOrderID = 0;
            
            if ((customerID is not null) && customerID >= 1 && customerID <= customerIDMax)
            {

                try
                {
                    connection.Open();
                    customerOrderID = Convert.ToInt32(cmd_CustomerOrderID.ExecuteScalar());
                    if (customerOrderID == 0)
                    {
                        throw new Exception("LATEST CUSTOMER ORDER FOR CUSTOMER #" + customerID + " NOT FOUND");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return customerOrderID;
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
            
        }
        // IMPLEMENTED ^ 
        public int GetCustomerIDByOrderID(int? orderID)
        {
            SqlCommand cmd_CustomerID = new SqlCommand("SELECT CustomerID FROM CustomerOrder " +
                                                       "WHERE CustomerOrderID = @OrderID", connection);
            cmd_CustomerID.Parameters.AddWithValue("@OrderID", orderID);
         
            CustomerOrderModel temp = new CustomerOrderModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            int customerID = 0;

            if ((orderID is not null) && orderID >= 1 && orderID <= customerOrderMax)
            {
                try
                {
                    connection.Open();
                    customerID = Convert.ToInt32(cmd_CustomerID.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return customerID;
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER ID");

        }
        // IMPLEMENTED ^ 
        public List<CustomerOrderModel> GetAllOrdersByCustomerID(int? customerID)
        {
            List<CustomerOrderModel> customerOrderList = new List<CustomerOrderModel>();
            CustomerModel tempModel = new CustomerModel();
            int customerIDMax = tempModel.GetLatestCustomerID();
            

            if ((customerID is not null) && customerID >= 1 && customerID <= customerIDMax)
            {
                SqlCommand cmd_CustomerOrderID = new SqlCommand("SELECT * FROM CustomerOrder " +
                                                                "WHERE CustomerID = @CustomerID ", connection);
                cmd_CustomerOrderID.Parameters.AddWithValue("@CustomerID", customerID);
                SqlDataReader readCustomerOrder = null;
                try
                {
                    connection.Open();
                    readCustomerOrder = cmd_CustomerOrderID.ExecuteReader();

                    while (readCustomerOrder.Read())
                    {

                        customerOrderList.Add(new CustomerOrderModel()
                        {
                            customerOrderId = Convert.ToInt32(readCustomerOrder[0]),
                            customerOrderDate = Convert.ToDateTime(readCustomerOrder[1]),
                            customerId = Convert.ToInt32(readCustomerOrder[2]),
                            customerOrderCost = Convert.ToDouble(readCustomerOrder[3]),
                            customerOrderPaid = Convert.ToBoolean(readCustomerOrder[4])
                        });
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return customerOrderList;
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");

        }
        // IMPLEMENTED ^
        public List<CustomerOrderModel> GetAllCustomerOrders()
        {
            List<CustomerOrderModel> customerOrderList = new List<CustomerOrderModel>();
            SqlCommand cmd_CustomerOrderID = new SqlCommand("SELECT * FROM CustomerOrder ", connection);    
            SqlDataReader readCustomerOrder = null;
                
            try
            {
                connection.Open();
                readCustomerOrder = cmd_CustomerOrderID.ExecuteReader();
                while (readCustomerOrder.Read())
                {
                    customerOrderList.Add(new CustomerOrderModel()
                    {
                        customerOrderId = Convert.ToInt32(readCustomerOrder[0]),
                        customerOrderDate = Convert.ToDateTime(readCustomerOrder[1]),
                        customerId = Convert.ToInt32(readCustomerOrder[2]),
                        customerOrderCost = Convert.ToDouble(readCustomerOrder[3]),
                        customerOrderPaid = Convert.ToBoolean(readCustomerOrder[4])
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            if (customerOrderList.Count > 0)
                return customerOrderList;
            else
                throw new Exception("NO CUSTOMER ORDER EXIST");

        }
        // IMPLEMENTED ^
        #endregion

        #region Update
        public string UpdateCustomerOrderPaid(int? customerOrderID, bool? updateOrderPaid)
        {
            SqlCommand cmd_UpdateCustomerOrderPaid = new SqlCommand("UPDATE CustomerOrder SET CustomerOrderPaid = @OrderPaid " +
                                                                    "WHERE CustomerOrder.CustomerOrderID = @CustomerOrderID ", connection);
            cmd_UpdateCustomerOrderPaid.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
            cmd_UpdateCustomerOrderPaid.Parameters.AddWithValue("@OrderPaid", updateOrderPaid);
            CustomerOrderModel temp = new CustomerOrderModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();
            
            if ((customerOrderID is not null) && customerOrderID >= 1 && customerOrderID <= customerOrderMax)
            {
                if (updateOrderPaid is not null)
                {
                    try
                    {
                        connection.Open();
                        cmd_UpdateCustomerOrderPaid.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return "UPDATE: Customer Order Paid Updated Successfully";
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER PAID");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER ID");
            
            
        }
        // IMPLEMENTED ^
        public string UpdateCustomerOrderCost(int? customerOrderID)
        {
            SqlCommand cmd_UpdateCustomerOrderCost = new SqlCommand("EXEC spCustomerOrderCost @OrderId", connection);
            cmd_UpdateCustomerOrderCost.Parameters.AddWithValue("@OrderId", customerOrderID);
            
            CustomerOrderModel temp = new CustomerOrderModel();
            int customerOrderMax = temp.GetLatestCustomerOrderID();

            if ((customerOrderID is not null) && customerOrderID >= 1 && customerOrderID <= customerOrderMax)
            {
                try
                {
                    connection.Open();
                    cmd_UpdateCustomerOrderCost.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return "UPDATE: Customer Order Cost Updated";
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER ID");

        }
        // IMPLETENTED ^
        #endregion

        #region Delete
        public string RemoveCustomerOrderByIDTime(int? customerID, DateTime? orderTime)
        {

            SqlCommand cmd_RemoveCustomerOrder = new SqlCommand("DELETE FROM CustomerOrder " +
                                                           "WHERE CustomerID = @CustomerID " +
                                                           "AND CustomerOrderDate = @OrderTime ", connection);
            cmd_RemoveCustomerOrder.Parameters.AddWithValue("@CustomerID", customerID);
            cmd_RemoveCustomerOrder.Parameters.AddWithValue("@OrderTime", orderTime);

            CustomerModel temp = new CustomerModel();
            int customerMax = temp.GetLatestCustomerID();

            if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
            {
                if (orderTime is not null)
                {
                    try
                    {
                        connection.Open();
                        cmd_RemoveCustomerOrder.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return "Customer Order Removed Successfully";
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ORDER TIME");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        #endregion


    }
}
