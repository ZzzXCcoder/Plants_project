using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Plants.Interfaces;

namespace Plants.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IUserPlantRepository _userPlantRepository;
        public JobController(IUserPlantRepository userPlantRepository)
        {
            _userPlantRepository = userPlantRepository;
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => _userPlantRepository.SendMail(),"*/5 * * * *");
            return Ok();
        }
    }
}
