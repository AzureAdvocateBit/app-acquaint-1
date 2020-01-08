using System.Text.Json.Serialization;
using MyContacts.Shared.Interfaces;
using MvvmHelpers;

namespace MyContacts.Shared.Models
{
    public class Contact : ObservableObject, IContact
    {  
		public string DataPartitionId { get; set; }
        string id;
        [JsonPropertyName("id")]
        public string Id 
        { 
            get => id;
            set => SetProperty(ref id, value); 
        }

        string firstName;
        [JsonPropertyName("firstName")]
        public string FirstName
        {
            get => firstName;
            set
            {
                SetProperty(ref firstName, value);
                // DisplayName is dependent on FirstName
                OnPropertyChanged(nameof(DisplayName));
                // DisplayLastNameFirst is dependent on FirstName
                OnPropertyChanged(nameof(DisplayLastNameFirst));
            }
        }

        string lastName;
        [JsonPropertyName("lastName")]
        public string LastName
        {
            get => lastName;
            set
            {
                SetProperty(ref lastName, value);
                // DisplayName is dependent on LastName
                OnPropertyChanged(nameof(DisplayName));
                // DisplayLastNameFirst is dependent on LastName
                OnPropertyChanged(nameof(DisplayLastNameFirst));
            }
        }

        string company;
        [JsonPropertyName("company")]
        public string Company
        {
            get => company;
            set => SetProperty(ref company, value);
        }

        string jobTitle;
        [JsonPropertyName("jobTitle")]
        public string JobTitle
        {
            get => jobTitle;
            set => SetProperty(ref jobTitle, value);
        }

        string email;
        [JsonPropertyName("email")]
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        string phone;
        [JsonPropertyName("phone")]
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        string street;
        [JsonPropertyName("street")]
        public string Street
        {
            get => street;
            set
            {
                SetProperty(ref street, value);
                // AddressString is dependent on Street
                OnPropertyChanged(nameof(AddressString));
            }
        }

        string city;
        [JsonPropertyName("city")]
        public string City
        {
            get => city;
            set
            {
                SetProperty(ref city, value);
                // AddressString is dependent on City
                OnPropertyChanged(nameof(AddressString));
            }
        }

        string postalCode;
        [JsonPropertyName("postalCode")]
        public string PostalCode
        {
            get => postalCode;
            set
            {
                SetProperty(ref postalCode, value);
                // AddressString is dependent on PostalCode
                OnPropertyChanged(nameof(AddressString));
                // StatePostal is dependent on PostalCode
                OnPropertyChanged(nameof(StatePostal));
            }
        }


        string state;
        [JsonPropertyName("state")]
        public string State
        {
            get => state;
            set
            {
                SetProperty(ref state, value);
                // AddressString is dependent on State
                OnPropertyChanged(nameof(AddressString));
                // StatePostal is dependent on State
                OnPropertyChanged(nameof(StatePostal));
            }
        }

        string photoUrl;
        [JsonPropertyName("photoUrl")]
        public string PhotoUrl
        {
            get => photoUrl;
            set
            {
                SetProperty(ref photoUrl, value);
                // SmallPhotoUrl is dependent on PhotoUrl
                OnPropertyChanged(nameof(SmallPhotoUrl));
            }
        }

        public string SmallPhotoUrl => PhotoUrl;

        [JsonIgnore]
        public string AddressString => string.Format(
            "{0} {1} {2} {3}",
            Street,
            !string.IsNullOrWhiteSpace(City) ? City + "," : "",
            State,
            PostalCode);

        [JsonIgnore]
        public string DisplayName => ToString();

        [JsonIgnore]
        public string DisplayLastNameFirst => $"{LastName}, {FirstName}";

        [JsonIgnore]
        public string StatePostal => State + " " + PostalCode;
      
        public override string ToString() => $"{FirstName} {LastName}";
    }
}

