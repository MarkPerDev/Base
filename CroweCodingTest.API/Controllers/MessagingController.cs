using CroweCodingTest.Services;
using CroweCodingTest.Services.Interface;
using System.Web.Http;

namespace CroweCodingTest.API.Controllers
{
  public class MessagingController : ApiController
  {
    private const string HELLO_WORLD = @"Hello World";
		ICorpMessaging _messaging;

    // GET api/messaging
    public string Get()
    {
			_messaging = new CorpMessaging();
			return _messaging.GetMessage(HELLO_WORLD);
    }

    // GET api/messaging/5
    public string Get(int id)
    {
      return "value";
    }

    // POST api/messages
    public void Post([FromBody]string value)
    {
    }

    // PUT api/message/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/messages/5
    public void Delete(int id)
    {
    }
  }
}
