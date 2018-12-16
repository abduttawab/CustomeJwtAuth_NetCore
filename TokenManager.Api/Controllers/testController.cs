using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TokenManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/test")]
    public class testController : Controller
    {
    }
}