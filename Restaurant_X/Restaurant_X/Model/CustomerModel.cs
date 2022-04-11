using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Restaurant_X.Model;

namespace Restaurant_X.Model
{
    public class CustomerModel
    {
        #region Properties
        public int customerId { get; set; }
        public string customerFName { get; set; }
        public string customerLName { get; set; }
        public string customerAddress { get; set; }
        #endregion

        #region SQL Server Connection Setting
        static string ConnectionString = File.ReadAllText("C:\\Users\\forca\\Desktop\\Training_Sessions\\Project1\\Restaurant_X\\Restaurant_X\\ConnectionString.txt");

        SqlConnection connection = new SqlConnection(ConnectionString);
        #endregion

        #region Create
        public string CreateCustomer(CustomerModel newCustomer)
        {
            SqlCommand cmd_CreateCustomer = new SqlCommand("INSERT INTO CustomerList " + 
                                                                "VALUES(@FName, @LName, @Address) ", connection);
            cmd_CreateCustomer.Parameters.AddWithValue("@FName", newCustomer.customerFName);
            cmd_CreateCustomer.Parameters.AddWithValue("@LName", newCustomer.customerLName);
            cmd_CreateCustomer.Parameters.AddWithValue("@Address", newCustomer.customerAddress);
            if (newCustomer.customerFName is not null)
            {
                if (newCustomer.customerLName is not null)
                {
                    if (newCustomer.customerAddress is not null)
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

                        return "New Customer Record Created";
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - CUSTOMER ADDRESS");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER LAST NAME");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER FIRST NAME");   
        }
        // IMPLEMENTED ^
        public string CreateCustomer(string newCustomerFName, string newCustomerLName, string newCustomerAddress)
        {
            SqlCommand cmd_CreateCustomer = new SqlCommand("INSERT INTO CustomerList " +
                                                           "VALUES(@FName, @LName, @Address) ", connection);
            cmd_CreateCustomer.Parameters.AddWithValue("@FName", newCustomerFName);
            cmd_CreateCustomer.Parameters.AddWithValue("@LName", newCustomerLName);
            cmd_CreateCustomer.Parameters.AddWithValue("@Address", newCustomerAddress);
            
            if (newCustomerFName is not null)
            {
                if (newCustomerLName is not null)
                {
                    if (newCustomerAddress is not null)
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

                        return "New Customer Record Created";
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - CUSTOMER ADDRESS");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER LAST NAME");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER FIRST NAME");
        }
        // IMPLEMENTED ^
        #endregion

        #region Read
        public List<CustomerModel> GetCustomersFull()
        {
            SqlCommand cmd_CustomerList = new SqlCommand("SELECT * FROM CustomerList", connection);
            List<CustomerModel> customerList = new List<CustomerModel>();
            SqlDataReader readCustomerFull = null;
            try
            {
                connection.Open();
                readCustomerFull = cmd_CustomerList.ExecuteReader();

                while (readCustomerFull.Read())
                {
                    customerList.Add(new CustomerModel()
                    {
                        customerId = Convert.ToInt32(readCustomerFull[0]),
                        customerFName = readCustomerFull[1].ToString(),
                        customerLName = readCustomerFull[2].ToString(),
                        customerAddress = readCustomerFull[3].ToString()
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                readCustomerFull.Close();
                connection.Close();
            }
            return customerList;
        }
        // IMPLEMENTED ^
        public CustomerModel GetCustomerByID(int? customerID)
        {
            SqlCommand cmd_Customer = new SqlCommand("SELECT * FROM CustomerList " +
                                                     "WHERE CustomerID = @customerID", connection);
            cmd_Customer.Parameters.AddWithValue("@customerID", customerID);
            SqlDataReader readCustomer = null;
            CustomerModel newCustomer = new CustomerModel();

            CustomerModel temp = new CustomerModel();
            int customerIDMax = temp.GetLatestCustomerID();
            if ((customerID is not null) && customerID >= 1 && customerID <= customerIDMax)
            {
                try
                {
                    connection.Open();
                    readCustomer = cmd_Customer.ExecuteReader();

                    if (readCustomer.Read())
                    {
                        newCustomer.customerId = Convert.ToInt32(readCustomer[0]);
                        newCustomer.customerFName = readCustomer[1].ToString();
                        newCustomer.customerLName = readCustomer[2].ToString();
                        newCustomer.customerAddress = readCustomer[3].ToString();
                    }
                    else
                    {
                        throw new Exception("CUSTOMER NOT FOUND");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    readCustomer.Close();
                    connection.Close();
                }

                return newCustomer;
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        public int GetLatestCustomerID()
        {
            int count = 0;

            SqlCommand cmd_CustomerIDLatest = new SqlCommand("SELECT CustomerID FROM CustomerList " +
                                                       "WHERE CustomerID = ( " +
                                                       "SELECT IDENT_CURRENT('CustomerList'))", connection);
            try
            {
                connection.Open();
                count = Convert.ToInt32(cmd_CustomerIDLatest.ExecuteScalar());
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
        public string UpdateCustomerNameByID(int? customerID, string fName, string lName)
        {
            SqlCommand cmd_UpdateCustomer = new SqlCommand("UPDATE CustomerList SET CustomerFName = @fName, CustomerLName = @lName " +
                                                            "WHERE CustomerID = @customerID ", connection);
            cmd_UpdateCustomer.Parameters.AddWithValue("@fName", fName);
            cmd_UpdateCustomer.Parameters.AddWithValue("@lName", lName);
            cmd_UpdateCustomer.Parameters.AddWithValue("@customerID", customerID);
            CustomerModel customer = new CustomerModel();
            int customerMax = customer.GetLatestCustomerID();

            if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
            {
                if (fName is not null)
                {
                    if (lName is not null)
                    {

                        try
                        {
                            connection.Open();
                            cmd_UpdateCustomer.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }

                        return "UPDATE: Customer Name Updated";
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - CUSTOMER LAST NAME");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER FIRST NAME");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        public string UpdateCustomerAddressByID(int? customerID, string address)
        {
            SqlCommand cmd_UpdateCustomer = new SqlCommand("UPDATE CustomerList SET CustomerAddress = @address " +
                                                           "WHERE CustomerID = @customerID ", connection);
            cmd_UpdateCustomer.Parameters.AddWithValue("@address", address);
            cmd_UpdateCustomer.Parameters.AddWithValue("@customerID", customerID);
            CustomerModel customer = new CustomerModel();
            int customerMax = customer.GetLatestCustomerID();

            if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
            {
                if (address is not null)
                {
                    try
                    {
                        connection.Open();
                        cmd_UpdateCustomer.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return "UPDATE: Customer Address Updated";
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER ADDRESS");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        public string UpdateCustomer(int? customerID, string fName, string lName, string address)
        {
            SqlCommand cmd_UpdateCustomer = new SqlCommand("UPDATE CustomerList SET CustomerFName = @fName, CustomerLName = @lName, CustomerAddress = @address " +
                                                           "WHERE CustomerID = @customerID ", connection);
            cmd_UpdateCustomer.Parameters.AddWithValue("@fName", fName);
            cmd_UpdateCustomer.Parameters.AddWithValue("@lName", lName);
            cmd_UpdateCustomer.Parameters.AddWithValue("@address", address);
            cmd_UpdateCustomer.Parameters.AddWithValue("@customerID", customerID);
            CustomerModel customer = new CustomerModel();
            int customerMax = customer.GetLatestCustomerID();

            if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
            {
                if (fName is not null)
                {
                    if (lName is not null)
                    {
                        if (address is not null)
                        {

                            try
                            {
                                connection.Open();
                                cmd_UpdateCustomer.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }

                            return "UPDATE: Customer Record Updated";
                        }
                        else
                            throw new Exception("INVALID DATA INPUT - CUSTOMER ADDRESS");
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - CUSTOMER LAST NAME");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER FIRST NAME");
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        public string UpdateCustomer(CustomerModel updateCustomer)
        {
            SqlCommand cmd_UpdateCustomer = new SqlCommand("UPDATE CustomerList SET CustomerFName = @fName, CustomerLName = @lName, CustomerAddress = @address " +
                                                           "WHERE CustomerID = @customerID ", connection);
            cmd_UpdateCustomer.Parameters.AddWithValue("@fName", updateCustomer.customerFName);
            cmd_UpdateCustomer.Parameters.AddWithValue("@lName", updateCustomer.customerLName);
            cmd_UpdateCustomer.Parameters.AddWithValue("@address", updateCustomer.customerAddress);
            cmd_UpdateCustomer.Parameters.AddWithValue("@customerID", updateCustomer.customerId);
            int customerMax = updateCustomer.GetLatestCustomerID();

            if (updateCustomer.customerId >= 1 && updateCustomer.customerId <= customerMax)
            {
                if (updateCustomer.customerFName is not null)
                {
                    if (updateCustomer.customerLName is not null)
                    {
                        if (updateCustomer.customerAddress is not null)
                        {
                            try
                            {
                                connection.Open();
                                cmd_UpdateCustomer.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                throw new Exception(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }

                            return "UPDATE: Customer Record Updated";
                        }
                        else
                            throw new Exception("INVALID DATA INPUT - CUSTOMER ADDRESS");
                    }
                    else
                        throw new Exception("INVALID DATA INPUT - CUSTOMER LAST NAME");
                }
                else
                    throw new Exception("INVALID DATA INPUT - CUSTOMER FIRST NAME");
            }
            else
                throw new Exception("INVALID DATE INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        #endregion

        #region Delete
        public string RemoveCustomer(int? customerID)
        {
            CustomerModel model = new CustomerModel();
            int customerCount = model.GetLatestCustomerID();
            SqlCommand cmd_RemoveFood = new SqlCommand("DELETE FROM CustomerList WHERE CustomerID = @RemoveID", connection);
            cmd_RemoveFood.Parameters.AddWithValue("@RemoveID", customerID);

            CustomerModel customer = new CustomerModel();
            int customerMax = customer.GetLatestCustomerID();
            if ((customerID is not null) && customerID >= 1 && customerID <= customerMax)
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
                return "DELETE: Customer Removed Successfully!";
            }
            else
                throw new Exception("INVALID DATA INPUT - CUSTOMER ID");
        }
        // IMPLEMENTED ^
        #endregion

    }
}
