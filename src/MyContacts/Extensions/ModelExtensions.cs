using System;
using System.Collections.Generic;
using System.Text;
using MyContacts.Models;

namespace MyContacts.Extensions
{
    public static class ModelExtensions
    {
        public static Contact Clone(this Contact o) => 
            new Contact
            {
                City = o.City,
                Company = o.Company,
                DataPartitionId = o.DataPartitionId,
                Email = o.Email,
                FirstName = o.FirstName,
                Id = o.Id,
                JobTitle = o.JobTitle,
                LastName = o.LastName,
                Phone = o.Phone,
                PhotoUrl = o.PhotoUrl,
                PostalCode = o.PostalCode,
                State = o.State,
                Street = o.Street
            };
    }
}
