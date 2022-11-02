using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NombreController : ControllerBase
    {
        
        [HttpGet]
        public string Get()
        {
                 return "SISTEMA DE GESTION VERSION 1.0";
        }
    }
}
