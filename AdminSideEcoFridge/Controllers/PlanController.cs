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
            ViewBag.StoragePlan = Utils.Utils.SelectPlanType();
            return View(storage);
        }

        [HttpPost]
        public IActionResult AddPlan([FromBody] StoragePlan storageAdd)
        {
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

            if (ModelState.IsValid)
            {
                if (_storagePlanRepo.Create(storageAdd) == ErrorCode.Success)
                {
                    return RedirectToAction("StoragePlan", "Plan");
                }

                return BadRequest();
            }

            return Ok();
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


    }
}
