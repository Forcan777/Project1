using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant_X.Model;

namespace Restaurant_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : ControllerBase
    {
        #region Logger Setup
        private readonly ILogger<FoodTypeController> _logger;

        public FoodTypeController(ILogger<FoodTypeController> logger)
        {
            _logger = logger;
        }
        #endregion

        FoodTypeModel model = new FoodTypeModel();

        #region Create
        [HttpPost]
        [Route("AddNewFoodType_ModelObjectInput")]
        public IActionResult AddFoodType(FoodTypeModel newFoodType)
        {
            try
            {
                return Created("Database Table - Food", model.AddFoodType(newFoodType));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED 
        [HttpPost]
        [Route("AddNewFoodType_IndividualInput")]
        public IActionResult AddFoodType(string newFoodType)
        {
            try
            {
                return Created("Database Table - Food", model.AddFoodType(newFoodType));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED
        #endregion

        #region Read
        [HttpGet]
        [Route("GetFoodTypeFull")]
        public IActionResult GetFoodTypeFull()
        {
            try
            {
                return Ok(model.GetFoodTypeFull());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        [HttpGet]
        [Route("GetFoodTypeCount")]
        public IActionResult GetFoodTypeCount()
        {
            try
            {
                return Ok(model.GetFoodTypeCount());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // IMPLEMENTED ^
        #endregion

        #region Update
        [HttpPut]
        [Route("UpdateFoodType")]
        public IActionResult UpdateFoodType(FoodTypeModel updateFoodType)
        {
            try
            {
                return Accepted(model.UpdateFoodType(updateFoodType));
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
        [Route("DeleteFoodType")]
        public IActionResult DeleteFoodType(int? foodTypeID)
        {
            try
            {
                return Accepted(model.RemoveFoodType(foodTypeID));
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
