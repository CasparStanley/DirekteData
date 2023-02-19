using DirekteDataREST.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

//[ApiController]
//[Route("api/[controller]")]
public class DirekteDataController : MonoBehaviour
{
    // https://direktedatarest2022.azurewebsites.net/api/DirekteData
    // http://localhost:5290/api/DirekteData

    private const string RESTURL = "https://direktedatarest2022.azurewebsites.net/api/DirekteData";
    private readonly IManageDirekteData mgr;

    // This would be where you would load the database context in for the mgr instead of creating a new one
    public DirekteDataController()
    {
        mgr = ManageDirekteData.Instance;
    }

    //[HttpGet("live")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    // GET: DirekteData/live
    public DataStructure GetLive()
    {
        try
        {
            return mgr.GetLive();
        }
        catch
        {
            Debug.LogError("Live data not found");
            return null;
        }
    }

    //[HttpGet]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    // GET: DirekteData
    public IEnumerable<DataStructure> GetAll()
    {
        try
        {
            return mgr.GetAll();
        }
        catch (KeyNotFoundException knfe)
        {
            Debug.LogError(knfe.Message);
            return null;
        }
    }

    //[HttpGet("{dataSetId}/{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    // GET: DirekteData/0
    public DataStructure GetRecording(int dataSetId, int id)
    {
        var data = mgr.GetRecordingById(dataSetId, id);
        if (data is null)
        {
            Debug.LogError($"Recording with id {id} in DataSet {dataSetId} not found");
            return null;
        }
        return data;
    }

    //[HttpGet("{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    // GET: DirekteData/0
    public DataSet GetDataSet(int id)
    {
        var data = mgr.GetDataSetById(id);
        if (data is null)
        {
            Debug.LogError($"DataSet with id {id} not found");
            return null;
        }
        return data;
    }

    //[HttpGet]
    //[Route("search")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    // GET: DirekteData/dataset1
    public DataSet GetDataSetByName(FilterDataSets filter)
    {
        var data = mgr.GetDataSetByName(filter);
        if (data is null)
        {
            Debug.LogError($"DataSet with name {filter.Name} not found");
            return null;
        }
        return data;
    }

    // POST api/DirekteData/AddRecording
    //[HttpPost("AddRecording")]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    public DataStructure PostRecording(DataStructure newSensorRecording)
    {
        try
        {
            DataStructure recording = mgr.AddRecording(newSensorRecording);
            return recording;
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    // POST api/DirekteData/AddDataSet
    //[HttpPost("AddDataSet")]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    public DataSet PostDataSet(DataSet newDataSet)
    {
        try
        {
            DataSet dataSet = mgr.AddDataSet(newDataSet);
            return dataSet;
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }
}