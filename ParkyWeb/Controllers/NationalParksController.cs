using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _repository;

        public NationalParksController(INationalParkRepository nationalParkRepository)
        {
            _repository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }


        //used for update and create
        //for create id will be nul and not null for updte
        public async Task<IActionResult> Upsert(int ?id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null)
            {
                //this will be true for insert/create
                return View(nationalPark);
            }
            else 
            {
                //for update
                nationalPark = await _repository.GetAsync(SD.NationalParkApiPath, id.GetValueOrDefault());
                if (nationalPark == null)
                {
                    return NotFound();
                }

                return View(nationalPark);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]/// need to know about this
        public async Task<IActionResult> Upsert(NationalPark obj) {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using(var fs1 = files[0].OpenReadStream())
                    {
                        using var ms1 = new MemoryStream();
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    obj.Picture = p1;
                }
                else
                {
                    var objFromDb = await _repository.GetAsync(SD.NationalParkApiPath + obj.Id,obj.Id);
                    if(objFromDb!=null)
                        obj.Picture = objFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    await _repository.CreateAsync(SD.NationalParkApiPath, obj);

                }
                else
                {
                    await _repository.UpdateAsync(SD.NationalParkApiPath+obj.Id,obj);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }


        public async Task<IActionResult> GetAllNationalPark()
        {
            var d = await _repository.GetAllAsync(SD.NationalParkApiPath);


            ///will return the jsonString.
            return Json(new { data = d});
        }



        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repository.DeleteAsync(SD.NationalParkApiPath, id);
            if (status) {
                return Json(new { success = true, message = "Delete Successful" });
            }
            else
            {
                return Json(new { success = false, message = "Delete Not Successful" });
            }

        }


    }
}
