using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCMSPasswordValidator.DTO;
using MCMSPasswordValidator.Services;
using Microsoft.AspNetCore.Mvc;

namespace MCMSPasswordValidator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<HashInfo> Get(int id)
        {
            var hashInfo = hashService.GetHashInfo(id.ToString());
            return hashInfo;
        }

//        [HttpGet("{id}")]
//        public ActionResult<string> Get(int id)
//        {
////            var hashInfo = hashService.GetHashInfo(id.ToString());
//            return "hash";
//        }

    }
    
}