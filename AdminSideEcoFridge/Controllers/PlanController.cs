using AdminSideEcoFridge.Models;
using AdminSideEcoFridge.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSideEcoFridge.Controllers
{
    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    public class PlanController : BaseController
    {
        public IActionResult StoragePlan()
        {
            List<StoragePlan> storage = _storagePlanRepo.GetAll();
            return View(storage);
        }

        [HttpPost]
        public IActionResult AddPlan([FromBody] StoragePlan storageAdd)
        {
            if (storageAdd == null)
            {
                return NotFound();
            }

            if (storageAdd.StorageSize <= 0)
            {
                ModelState.AddModelError("StorageSize", "Please enter a valid size.");
            }

            if (storageAdd.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "Please enter a valid duration of plan.");
            }

            if (storageAdd.Price < 0)
            {
                ModelState.AddModelError("Price", "Please enter a valid price.");
            }

            storageAdd.isActive = true;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var result = _storagePlanRepo.Create(storageAdd);
            if (result == ErrorCode.Success)
            {
                return Ok(new { message = "Storage plan added successfully." });
            }

            ModelState.AddModelError("ServerError", "Failed to add the storage plan. Please try again.");
            return BadRequest(ModelState); 
        }

        [HttpPut]
        public IActionResult EditPlan([FromBody] StoragePlan storageEdit)
        {
            if (storageEdit == null)
            {
                return NotFound();
            }

            if (storageEdit.StorageSize <= 0)
            {
                ModelState.AddModelError("StorageSize", "Please enter a valid size.");
            }

            if (storageEdit.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "Please enter a valid duration of the plan.");
            }

            if (storageEdit.Price < 0)
            {
                ModelState.AddModelError("Price", "Please enter a valid price.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPlan = _storagePlanRepo.Get(storageEdit.StoragePlanId);
            if (existingPlan == null)
            {
                return NotFound();
            }

            existingPlan.StoragePlanName = storageEdit.StoragePlanName;
            existingPlan.StorageSize = storageEdit.StorageSize;
            existingPlan.Duration = storageEdit.Duration;
            existingPlan.Price = storageEdit.Price;

            if (_storagePlanRepo.Update(existingPlan.StoragePlanId, existingPlan) == ErrorCode.Success)
            {
                return Ok(new { message = "Plan updated successfully" });
            }

            return StatusCode(500, "An error occurred while updating the plan.");
        }

        [HttpPut]
        public IActionResult DisablePlan([FromBody] StoragePlan disablePlan)
        {
            if (disablePlan == null)
            {
                return BadRequest("Invalid plan data.");
            }

            var existingPlan = _storagePlanRepo.Get(disablePlan.StoragePlanId);
            if (existingPlan == null)
            {
                return NotFound("Plan not found."); 
            }

            existingPlan.isActive = false;

            if (_storagePlanRepo.Update(existingPlan.StoragePlanId, existingPlan) == ErrorCode.Success)
            {
                return Ok(new { message = "Plan updated successfully." });
            }

            return StatusCode(500, "An error occurred while updating the plan.");
        }

        [HttpPut]
        public IActionResult ActivatePlan([FromBody] StoragePlan activatePlan)
        {
            if (activatePlan == null)
            {
                return BadRequest("Invalid plan data.");
            }

            var existingPlan = _storagePlanRepo.Get(activatePlan.StoragePlanId);
            if (existingPlan == null)
            {
                return NotFound("Plan not found.");
            }

            existingPlan.isActive = true;

            if (_storagePlanRepo.Update(existingPlan.StoragePlanId, existingPlan) == ErrorCode.Success)
            {
                return Ok(new { message = "Plan updated successfully." });
            }

            return StatusCode(500, "An error occurred while updating the plan.");
        }

        [HttpDelete]
        public IActionResult DeletePlan(int id)
        {
            if (_storagePlanRepo.Delete(id) == ErrorCode.Success)
            {
                return Ok(new { message = "Plan deleted successfully." });
            }

            return NotFound("Plan not found.");
        }

    }
}
