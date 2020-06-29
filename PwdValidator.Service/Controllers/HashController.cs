using Microsoft.AspNetCore.Mvc;
using PwdValidator.Service.Responses;
using PwdValidator.Service.Services;

namespace PwdValidator.Service.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HashController : ControllerBase
    {

        private HashService hashService;
        
        public HashController()
        {
            hashService = new HashService();
        }

        /// <summary>
        /// Returns whether or not the hash specified is present in the database, and if it is, how often it has been subject to a breach.
        /// </summary>
        /// <param name="id">The hash value to lookup</param>
        /// <returns>A response message</returns>
        [HttpGet("{id}")]
        public ActionResult<Hash> Get(string id)
        {
            var hashInfo = hashService.GetHashInfo(id);
            
            if (hashInfo.Value == null)
                return new NotFoundResult();
            
            return hashInfo;
        }

    }
    
}