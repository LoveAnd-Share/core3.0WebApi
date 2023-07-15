using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    
    public class TestController : BaseController
    {
        [HttpGet]
        public string Ret()
        {
            return "get";
        }
    }
}
