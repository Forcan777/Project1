using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant_X.Model;
using System;

namespace Restaurant_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        #region Logger Setup
        private readonly ILogger<CustomerOrderController> _logger;

        public CustomerOrderController(ILogger<CustomerOrderController> logger)
        {
            _logger = logger;
        }
        #endregion

        CustomerOrderModel model = new CustomerOrderModel();


        #region Create
        [HttpPost]
        [Route("CustomerOrderCreation")]
        public IActionResult CreateCustomerOrder(int? customerID)
        {

            try
            {
                return Created("Database Table - CustomerOrder", model.CreateCustomerOrder(customerID));
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
        [Route("GetLatestCustomerOrderID")]
        public IActionResult GetLatestCustomerOrderID()
        {
            try
            {
                return Ok(model.GetLatestCustomerOrderID());
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
        public IActionResult GetLatestCustomerID()
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
        [HttpGet]
        [Route("GetLatestOrderIDByCustomerID")]
        public IActionResult GetLatestOrderIDByCustomerID(int? customerID)
        {
            try
            {
                return Ok(model.GetLatestOrderIDByCustomerID(customerID));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("GetCustomerIDbyOrderID")]
        public IActionResult GetCustomerIDByOrderID(int? orderID)
        {
            try
            {
                return Ok(model.GetCustomerIDByOrderID(orderID));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("GetAllOrdersByCustomerID")]
        public IActionResult GetAllOrdersByCustomerID(int? customerID)
        {
            try
            {
                return Ok(model.GetAllOrdersByCustomerID(customerID));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("GetAllCustomerOrders")]
        public IActionResult GetAllCustomerOrders()
        {
            try
            {
                return Ok(model.GetAllCustomerOrders());
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
        [Route("UpdateCustomerOrderPaid")]
        public IActionResult UpdateCustomerOrderPaid(int? customerOrderID, bool? updateOrderPaid)
        {
            try
            {
                return Accepted(model.UpdateCustomerOrderPaid(customerOrderID, updateOrderPaid));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateCustomerOrderCost")]
        public IActionResult UpdateCustomerOrderCost(int? customerOrderID)
        {
            try
            {
                return Accepted(model.UpdateCustomerOrderCost(customerOrderID));
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
        [Route("RemoveCustomerOrderByID")]
        public IActionResult RemoveCustomerOrderByIDTime(int? customerID, DateTime? orderTime)
        {
            try
            {
                return Accepted(model.RemoveCustomerOrderByIDTime(customerID, orderTime));
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
