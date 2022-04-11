using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant_X.Model;

namespace Restaurant_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region Logger Setup
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        #endregion

        CustomerModel model = new CustomerModel();

        #region Create
        [HttpPost]
        [Route("AddNewCustomer_ModelObjectInput")]
        public IActionResult AddNewCustomer(CustomerModel newCustomer)
        {
            try
            {
                return Created("Database Table - Food", model.CreateCustomer(newCustomer));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^    
        [HttpPost]
        [Route("AddNewCustomer_IndividualInput")]
        public IActionResult AddNewCustomer(string newCustomerFName, string newCustomerLName, string newCustomerAddress)
        {
            try
            {
                return Created("Database Table - CustomerList", model.CreateCustomer(newCustomerFName, newCustomerLName, newCustomerAddress));        
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        #endregion

        #region Read
        [HttpGet]
        [Route("GetCustomersFullList")]
        public IActionResult GetCustomersFull()
        {
            try
            {
                return Ok(model.GetCustomersFull());
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("GetCustomerByID")]
        public IActionResult GetFoodMenuBasedOnType(int? customerID)
        {
            try
            {    
                return Ok(model.GetCustomerByID(customerID));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("GetLatestCustomerID")]
        public IActionResult GetCustomerCount()
        {
            try
            {
                return Ok(model.GetLatestCustomerID());
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^ 
        #endregion

        #region Update
        [HttpPut]
        [Route("UpdateCustomerNameByID")]
        public IActionResult UpdateCustomerNameByID(int? customerID, string fName, string lName)
        {
            try
            {            
                return Created("Database Table - CustomerList", model.UpdateCustomerNameByID(customerID, fName, lName));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateCustomerAddressByID")]
        public IActionResult UpdateCustomerAddressByID(int? customerID, string address)
        {
            try
            {
                return Created("Database Table - CustomerList", model.UpdateCustomerAddressByID(customerID, address));   
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateCustomer_IndividualInput")]
        public IActionResult UpdateCustomer(int? customerID, string fName, string lName, string address)
        {
            try
            {              
                return Accepted(model.UpdateCustomer(customerID, fName, lName, address));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateCustomer_ModelObjectInput")]
        public IActionResult UpdateCustomer(CustomerModel updateCustomer)
        {
            try
            {
                return Accepted(model.UpdateCustomer(updateCustomer));    
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        #endregion

        #region Delete
        [HttpDelete]
        [Route("DeleteCustomer")]
        public IActionResult RemoveCustomer(int? customerID)
        {
            try
            {
                return Accepted(model.RemoveCustomer(customerID));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }   
        }
        // IMPLEMENTED ^
        #endregion


    }
}
