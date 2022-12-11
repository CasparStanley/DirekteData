using DirekteDataREST.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace DirekteDataREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirekteDataController : Controller
    {
        private readonly IManageDirekteData mgr;

        // This would be where you would load the database context in for the mgr instead of creating a new one
        public DirekteDataController()
        {
            mgr = new ManageDirekteData();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: DirekteDataController
        public ActionResult GetAll()
        {
            try
            {
                return Ok(mgr.GetAll());
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(knfe);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: DirekteDataController/Details/5
        public ActionResult GetData(int id)
        {
            var data = mgr.GetById(id);
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        // GET: DirekteDataController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DirekteDataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(GetAll));
            }
            catch
            {
                return View();
            }
        }

        // GET: DirekteDataController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DirekteDataController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(GetAll));
            }
            catch
            {
                return View();
            }
        }

        // GET: DirekteDataController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DirekteDataController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(GetAll));
            }
            catch
            {
                return View();
            }
        }
    }
}
