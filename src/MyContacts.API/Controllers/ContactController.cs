using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyContacts.Shared;
using MyContacts.Shared.Models;
using MyContacts.Shared.Utils;

namespace MyContacts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        List<Contact> Contacts { get; set; }
        readonly ILogger<ContactController> logger;

        public ContactController(ILogger<ContactController> logger)
        {
            this.logger = logger;
            Contacts = ContactsGenerator.GenerateContacts();
        }

        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return Contacts;
        }
    }
}
