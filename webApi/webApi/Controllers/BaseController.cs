using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
