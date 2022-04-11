using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant_X.Model;

namespace Restaurant_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        #region Logger Setup
        private readonly ILogger<FoodController> _logger;
        public FoodController(ILogger<FoodController> logger)
        {
            _logger = logger;
        }
        #endregion

        FoodModel model = new FoodModel();

        #region Create
        [HttpPost]
        [Route("AddNewFood_ModelObjectInput")]
        public IActionResult AddNewFood (FoodModel newFood)
        {
            try
            {
                return Created("Database Table - Food", model.AddFoodToMenu(newFood));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^    
        [HttpPost]
        [Route("AddNewFood_IndividualInput")]
        public IActionResult AddNewFood (string newFoodName, int? newFoodType, double? newFoodPrice, bool? newFoodAvailability)
        {
            try
            {
                return Created("Database Table - Food", model.AddFoodToMenu(newFoodName, newFoodType, newFoodPrice, newFoodAvailability));
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
        [Route("FoodMenuFull")]
        public IActionResult GetFoodMenuFull()
        {
            try
            {
                return Ok(model.GetFoodMenuFull());
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("FoodMenuBasedOnType")]
        public IActionResult GetFoodMenuBasedOnType (int? foodType)
        {
            try
            {
                return Ok(model.GetFoodMenuBasedOnType(foodType));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("FoodMenuUnavailable")]
        public IActionResult GetFoodMenuUnavailable()
        {
            try
            {
                return Ok(model.GetFoodMenuUnavailable());
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
        [Route("UpdateFood_ModelObjectInput")]
        public IActionResult UpdateFood (FoodModel updateFood)
        {
            try
            {
                return Accepted(model.UpdateFood(updateFood));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateFood_FoodPriceInput")]
        public IActionResult UpdateFoodPrice (int updateFoodID, double? newFoodPrice)
        {
            try
            {
                return Accepted(model.UpdateFoodPrice(updateFoodID, newFoodPrice));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpPut]
        [Route("UpdateFoodAvailability_OnFoodID")]
        public IActionResult UpdateFoodAvailability (int? updateFoodID, bool? updateFoodAvailability)
        {
            try
            {
                return Accepted(model.UpdateFoodAvailability(updateFoodID, updateFoodAvailability));
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
        [Route("DeleteFood")]
        public IActionResult DeleteFood (int deleteFoodID)
        {
            try
            {
                return Accepted(model.RemoveFood(deleteFoodID));
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
