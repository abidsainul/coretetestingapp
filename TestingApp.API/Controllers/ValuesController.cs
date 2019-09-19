using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.API.Data;

namespace TestingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataContext _context;

        private static readonly HttpClient client = new HttpClient();

        public ValuesController(DataContext context)
        {
            this._context = context;
        }

        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            //throw new Exception("Test exception");
            //return new string[] { "value1", "value3" };
            var values= await _context.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value= await _context.Values.FirstOrDefaultAsync(x=>x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            
            //json sample file
             var sampleData=System.IO.File.ReadAllText("Data/SampleData.json");
             Console.Write(sampleData);

            // client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(
            //     new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
             client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            ////var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
            var yourusername = "testeti49@snb.ifg.iata.org";
            var yourpwd = "kzL4rzWN";
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                        $"{yourusername}:{yourpwd}")));

            Console.WriteLine(Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                        $"{yourusername}:{yourpwd}")));
            
            var stringTask= client.PostAsync("https://snb.ifg.iata.org/main/v4/payment/authorize", new StringContent(sampleData, Encoding.UTF8, "application/json"));

           var msg = await stringTask;
           Console.Write(msg);

           string resultContent = msg.Content.ReadAsStringAsync().Result;
            Console.Write("#######");
               Console.Write(resultContent);


            return Ok(resultContent);
        }

        
       
    

    }
}
