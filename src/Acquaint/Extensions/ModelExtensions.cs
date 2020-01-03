using System;
using System.Collections.Generic;
using System.Text;
using Acquaint.Models;

namespace Acquaint.Extensions
{
    public static class ModelExtensions
    {
        public static Acquaintance Clone(this Acquaintance o) => 
            new Acquaintance
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
