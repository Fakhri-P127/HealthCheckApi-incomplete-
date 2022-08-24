using AutoMapper;
using HealthCheck.DataService.Data;
using HealthCheck.DataService.IConfiguration;
using HealthCheck.Entities.DbSet;
using HealthCheck.Entities.DTOs.Incoming;
using HealthCheck.WebApi.Controllers.v1.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.WebApi.Controllers.v1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : BaseController
{
    public UsersController(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
    {
        
    }

    #region Queries
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        
        var users = await _unit.Users.GetAll();
        return Ok(users);
    }


    [HttpGet("{id}", Name = "GetById")]
    public async Task<IActionResult> Get(Guid id)
    {
        User user = await _unit.Users.GetById(id);
        if (user == null) return BadRequest();
        return Ok(user);
    }
    #endregion

    #region Commands
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto dto)
    {
        User user = _mapper.Map<User>(dto); ;
        user.UpdatedDate = DateTime.Now;
        user.Status = 1;

        await _unit.Users.Add(user);
        await _unit.SaveChangesAsync();
        return CreatedAtRoute(routeName: "GetById", routeValues: new { id = user.Id }, dto);
    }
    #endregion
}
