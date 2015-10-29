using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SharedLib.Managers;

namespace MultiTenantWebApi.Controllers
{
    public class HelloController : ApiController
    {
        private readonly IHelloManager _helloManager;

        public HelloController(IHelloManager helloManager)
        {
            _helloManager = helloManager;
        }

        public HttpResponseMessage Get()
        {
            var result = Request.CreateResponse(HttpStatusCode.OK, _helloManager.Say("Hi"));

            return result;
        }
    }
}
