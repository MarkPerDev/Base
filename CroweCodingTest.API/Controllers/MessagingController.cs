using System.Web.Http;
using System.Linq;
using CroweCodingTest.Services.Interface;
using CroweCodingTest.API.Models;
using CroweCodingTest.Services;

namespace CroweCodingTest.API.Controllers
{
  public class MessagingController : ApiController
  {
    private const string HELLO_WORLD = @"Hello World";
		ICorpMessaging _messaging;

    private readonly MessageEntities context = new MessageEntities();

    // GET api/messaging
    public string Get()
    {
			_messaging = new CorpMessaging();
			return _messaging.GetMessage(HELLO_WORLD);
    }

    // GET api/messaging/1
    public string Get(int id)
    {
      var msg = context.displays.SingleOrDefault(x => x.Id == id);

      return (msg != null) ? msg.Message : "Not Found";
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
