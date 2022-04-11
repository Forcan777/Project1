using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant_X.Model;
using System.Collections.Generic;

namespace Restaurant_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodOrderController : ControllerBase
    {
        #region Logger Setup
        private readonly ILogger<FoodOrderController> _logger;

        public FoodOrderController(ILogger<FoodOrderController> logger)
        {
            _logger = logger;
        }
        #endregion

        // Model class variable
        FoodOrderModel model = new FoodOrderModel();
        CustomerOrderModel customer = new CustomerOrderModel();

        #region Create
        [HttpPost]
        [Route("CreateCustomerOrderFoodItems")]
        public IActionResult CreateCustomerOrderFood(int? customerID, int? newFoodToOrder, int? newFoodQty)
        {

            try
            {
                return Created("Database Table - FoodOrder", model.CreateFoodOrder(customerID, newFoodToOrder, newFoodQty));
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
        [Route("GetCustomerOrderFoodItems")]
        public IActionResult GetCustomerOrderFoodList(int? customerOrderID)
        {
            try    
            {    
                return Ok(model.GetCustomerOrderFoodList(customerOrderID));    
            }    
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED
        [HttpGet]
        [Route("GetFoodOrderItemCountbyIDs")]
        public IActionResult GetFoodOrderItemCountByIDs(int? foodOrderID, int? customerID)
        {
            try
            {
                return Ok(model.GetFoodOrderItemCountByIDs(foodOrderID, customerID));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED
        #endregion

        #region Update
        [HttpPut]
        [Route("UpdateFoodOrderItems")]
        public IActionResult UpdateFoodOrderItemByIDs(int? foodOrderID, int? customerID, int? oldFoodItem, int? newFoodItem, int? newFoodItemQty)
        {
            try
            {
                return Accepted(model.UpdateFoodOrderItemByIDs(foodOrderID, customerID, oldFoodItem, newFoodItem, newFoodItemQty));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateFoodOrderItemsQty")]
        public IActionResult UpdateCustomerOrderItemQtyByIds(int? foodOrderID, int? customerID, int? foodItem, int? foodItemQty)
        {
            try
            {
                return Accepted(model.UpdateFoodOrderItemQtyByIDs(foodOrderID, customerID, foodItem, foodItemQty));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateFoodOrderItemsCompleted")]
        public IActionResult UpdateFoodOrderItemCompleted(int? foodOrderID, int? customerID, int? foodItem, bool? itemCompleted)
        {
            try
            {
                return Accepted(model.UpdateFoodOrderItemCompleted(foodOrderID, customerID, foodItem, itemCompleted));
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
        [Route("RemoveCustomerFoodItem")]
        public IActionResult RemoveCustomerFoodItem(int? foodOrderID, int? customerID, int? foodItem)
        {
            try
            {
                return Accepted(model.RemoveCustomerFoodItem(foodOrderID, customerID, foodItem));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpDelete]
        [Route("RemoveCustomerFoodItemsALL")]
        public IActionResult RemoveCustomerFoodItemsAll(int? foodOrderID, int? customerID)
        {
            try
            {
                return Accepted(model.RemoveCustomerFoodItemsAll(foodOrderID, customerID));
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
