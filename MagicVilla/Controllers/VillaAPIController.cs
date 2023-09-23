using System.Net;
using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaAPIController : ControllerBase
{
  private readonly APIResponse _response;
  private readonly IVillaRepository _dbVilla;
  private readonly ILogger<VillaAPIController> _logger;
  private readonly IMapper _mapper;

  public VillaAPIController(IVillaRepository dbVilla, ILogger<VillaAPIController> logger, IMapper mapper)
  {
    _response = new();
    _dbVilla = dbVilla;
    _logger = logger;
    _mapper = mapper;
  }
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult<APIResponse>> GetVillas()
  {
    _logger.LogInformation("Getting all villas");
    try
    {
      IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
      _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
      _response.StatusCode = HttpStatusCode.OK;
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string> { ex.ToString() };
    }
    return _response;
  }

  [HttpGet("{id}", Name = "GetVilla")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<APIResponse>> GetVilla(int id)
  {
    try
    {
      Villa? villa = await _dbVilla.GetAsync((villa) => villa.Id == id);
      if (villa == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound(_response);
      }
      _response.Result = _mapper.Map<VillaDTO>(villa);
      _response.StatusCode = HttpStatusCode.OK;
      
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string>() { ex.ToString() };
    }
    return _response;
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
  {
    try
    {
      if (villaCreateDTO == null)
      {
        return BadRequest(villaCreateDTO);
      }

      Villa? villaWithSameName = await _dbVilla.GetAsync(filter: (villa) => villa.Name.ToLower() == villaCreateDTO.Name.ToLower());
      if (villaWithSameName != null)
      {
        ModelState.AddModelError("AlreadyExistError", "Villa already exist");
        return BadRequest(ModelState);
      }

      Villa newVilla = _mapper.Map<Villa>(villaCreateDTO);

      await _dbVilla.CreateAsync(newVilla);
      await _dbVilla.SaveAsync();
      _response.Result = _mapper.Map<VillaDTO>(newVilla);
      _response.StatusCode = HttpStatusCode.Created;
      
      return CreatedAtRoute(nameof(GetVilla), new { id = newVilla.Id }, _response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string>() { ex.ToString() };
    }
    return _response;
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
  {
    try
    {
      if (id == 0)
      {
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        return BadRequest(_response);
      }

      Villa? villa = await _dbVilla.GetAsync(filter: (villa) => villa.Id == id);
      if (villa == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound();
      }

      await _dbVilla.RemoveAsync(villa);
      await _dbVilla.SaveAsync();
      _response.StatusCode = HttpStatusCode.NoContent;
      
      return _response;
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string>() { ex.ToString() };
    }
    return _response;
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
  {
    try
    {
      if (villaUpdateDTO == null || id != villaUpdateDTO?.Id)
      {
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        return BadRequest(_response);
      }

      Villa? villa = await _dbVilla.GetAsync(filter: (villa) => villa.Id == id, tracked: false);

      if (villa == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound(_response);
      }

      Villa updatedVilla = _mapper.Map<Villa>(villaUpdateDTO);

      await _dbVilla.UpdateAsync(updatedVilla);
      await _dbVilla.SaveAsync();
      _response.StatusCode = HttpStatusCode.NoContent;
      
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string>() { ex.ToString() };
    }
    return _response;
  }

  [HttpPatch("{id}", Name = "UpdatePartialVilla")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchVillaDTO)
  {
    try
    {
      if (patchVillaDTO == null || id == 0)
      {
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        return BadRequest(_response);
      }

      Villa? villa = await _dbVilla.GetAsync(filter: (villa) => villa.Id == id, tracked: false);
      if (villa == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound(_response);
      }

      VillaUpdateDTO updatedVillaToVillaDTO = _mapper.Map<VillaUpdateDTO>(villa);

      patchVillaDTO.ApplyTo(updatedVillaToVillaDTO, ModelState);

      Villa updatedVilla = _mapper.Map<Villa>(updatedVillaToVillaDTO);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      await _dbVilla.UpdateAsync(updatedVilla);
      await _dbVilla.SaveAsync();

      _response.StatusCode = HttpStatusCode.NoContent;
      return Ok(_response);
    }
    catch (Exception ex)
    {
      _response.IsSuccess = false;
      _response.ErrorMessages = new List<string>() { ex.ToString() };
    }
    return _response;
  }
}
