using AutoMapper;
using HealthCheck.DataService.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.WebApi.Controllers.v1.Base;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {

        protected readonly IUnitOfWork _unit;
        protected readonly IMapper _mapper;

        public BaseController(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
    }
