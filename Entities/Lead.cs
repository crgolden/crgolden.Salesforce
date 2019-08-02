namespace Clarity.Salesforce
{
    using System;

    public abstract class Lead : Record
    {
        public string MasterRecordId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Salutation { get; set; }

        public string Title { get; set; }

        public string Company { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string GeocodeAccuracy { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string PhotoUrl { get; set; }

        public string Description { get; set; }

        public string LeadSource { get; set; }

        public string Status { get; set; }

        public string Industry { get; set; }

        public string Rating { get; set; }

        public double? AnnualRevenue { get; set; }

        public int? NumberOfEmployees { get; set; }

        public bool? HasOptedOutOfEmail { get; set; }

        public bool? IsConverted { get; set; }

        public DateTime? ConvertedDate { get; set; }

        public string ConvertedAccountId { get; set; }

        public string ConvertedContactId { get; set; }

        public string ConvertedOpportunityId { get; set; }

        public bool? IsUnreadByOwner { get; set; }

        public string Jigsaw { get; set; }

        public string JigsawContactId { get; set; }

        public string EmailBouncedReason { get; set; }

        public DateTime? EmailBouncedDate { get; set; }
    }
}
