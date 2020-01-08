using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyContacts.Shared.Models;
using MyContacts.Shared.Utils;

namespace MyContacts.API.Models
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options)
            : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }

        public static void Initialize(ContactContext context)
        {
            context.Database.EnsureCreated();

            if (context.Contacts.Any())
                return;

            var contacts = ContactsGenerator.GenerateContacts();

            context.Contacts.AddRange(contacts);

            context.SaveChanges();
        }
    }
}
