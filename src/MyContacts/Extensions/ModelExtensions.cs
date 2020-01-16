using MyContacts.Shared.Models;

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

        public static void CopyData(this Contact o, Contact copyInto)
        {
            copyInto.City = o.City;
            copyInto.Company = o.Company;
            copyInto.DataPartitionId = o.DataPartitionId;
            copyInto.Email = o.Email;
            copyInto.FirstName = o.FirstName;
            copyInto.JobTitle = o.JobTitle;
            copyInto.LastName = o.LastName;
            copyInto.Phone = o.Phone;
            copyInto.PhotoUrl = o.PhotoUrl;
            copyInto.PostalCode = o.PostalCode;
            copyInto.State = o.State;
            copyInto.Street = o.Street;
        }
    }
}
