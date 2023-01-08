using DirekteDataREST.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLib;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace DirekteDataREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirekteDataController : Controller
    {
        private const string RESTURL = "https://direktedatarest2022.azurewebsites.net/api/DirekteData";
        private readonly IManageDirekteData mgr;

        private bool useDB = true;

        // This would be where you would load the database context in for the mgr instead of creating a new one
        public DirekteDataController(DirekteDataContext context)
        {
            // If no context, use the "mock"/offline manager
            if (useDB && context != null)
            {
                mgr = new ManageDirekteDataDB(context);
            }
            else
            {
                mgr = ManageDirekteData.Instance;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: DirekteData
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

        [HttpGet("{dataSetId}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: DirekteData/0
        public ActionResult GetRecording(int dataSetId, int id)
        {
            var data = mgr.GetRecordingById(dataSetId, id);
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: DirekteData/0
        public ActionResult GetDataSet(int id)
        {
            var data = mgr.GetDataSetById(id);
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        // POST api/DirekteData/AddRecording
        [HttpPost("AddRecording")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DataStructure> PostRecording(DataStructure newSensorRecording)
        {
            try
            {
                DataStructure recording = mgr.AddRecording(newSensorRecording);
                return Created(RESTURL, recording);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/DirekteData/AddDataSet
        [HttpPost("AddDataSet")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DataSet> PostDataSet(DataSet newDataSet)
        {
            try
            {
                DataSet dataSet = mgr.AddDataSet(newDataSet);
                return Created(RESTURL, dataSet);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: DirekteData/Edit/5
        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public ActionResult Edit(int id)
        //{
        //    try
        //    {
        //        DataStructure recording = mgr.GetById(id);
        //        return Ok(recording);
        //    }
        //    catch (KeyNotFoundException knfe)
        //    {
        //        return NotFound(knfe);
        //    }
        //}
    }
}
