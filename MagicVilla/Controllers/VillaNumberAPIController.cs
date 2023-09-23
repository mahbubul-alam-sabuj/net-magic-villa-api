using System.Net;
using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaNumberAPIController : ControllerBase
{
  private readonly APIResponse _response;
  private readonly IVillaNumberRepository _dbVillaNumber;
  private readonly ILogger<VillaNumberAPIController> _logger;
  private readonly IMapper _mapper;

  public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, ILogger<VillaNumberAPIController> logger, IMapper mapper)
  {
    _dbVillaNumber = dbVillaNumber;
    _logger = logger;
    _mapper = mapper;
    _response = new();
  }

  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult<APIResponse>> GetVillaNumbers()
  {
    _logger.LogInformation("Getting all villa numbers");
    try
    {
      IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync();
      _response.Result = villaNumberList;
      _response.StatusCode = HttpStatusCode.OK;
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _logger.LogError("Error occured while getting all villa numbers");
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string> { ex.ToString() };
    }
    return _response;
  }

  [HttpGet("{id}", Name = "GetVillaNumbers")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<APIResponse>> GetVillaNumbers(int id)
  {
    _logger.LogInformation($"Getting villa number for id = {id}");
    try
    {
      VillaNumber? villaNumber = await _dbVillaNumber.GetAsync(filter: (villaNumber) => villaNumber.VillaNo == id);
      if (villaNumber == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound(_response);
      }
      _response.StatusCode = HttpStatusCode.OK;
      _response.Result = villaNumber;
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string> { ex.ToString() };
    }
    return _response;
  }

  [HttpPost()]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
  {
    _logger.LogInformation("Creating villa number");
    try
    {
      if (villaNumberCreateDTO == null)
      {
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        return BadRequest(_response);
      }
      VillaNumber newVillaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);
      _response.StatusCode = HttpStatusCode.Created;
      _response.Result = _mapper.Map<VillaNumberDTO>(newVillaNumber); 
      await _dbVillaNumber.CreateAsync(newVillaNumber);
      await _dbVillaNumber.SaveAsync();
      return CreatedAtRoute(nameof(GetVillaNumbers), new { VillaNumber = newVillaNumber.VillaNo }, _response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.StatusCode = HttpStatusCode.InternalServerError;
      _response.ErrorMessages = new List<string> { ex.ToString() };
    }
    return _response;
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
  {
    _logger.LogInformation($"Deleting villa number with id = {id}");
    try
    {
      VillaNumber? villaNumber = await _dbVillaNumber.GetAsync((villaNumber) => villaNumber.VillaNo == id);
      if (villaNumber == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound(_response);
      }
      _response.StatusCode = HttpStatusCode.NoContent;
      _response.IsSuccess = true;
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _response.StatusCode = HttpStatusCode.InternalServerError;
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string> { ex.ToString() };
    }
    return _response;
  }
}
