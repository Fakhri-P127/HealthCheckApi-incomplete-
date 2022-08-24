using AutoMapper;
using HealthCheck.Entities.DbSet;
using HealthCheck.Entities.DTOs.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Entities.Mapping
{
    public class GeneralMap:Profile
    {
        public GeneralMap()
        {
            CreateMap<UserDto, User>();
        }
    }
}
