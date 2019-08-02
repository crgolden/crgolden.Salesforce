namespace Clarity.Salesforce
{
    using System;

    public abstract class Record
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public bool? IsDeleted { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string LastModifiedById { get; set; }

        public DateTime? SystemModstamp { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime? LastViewedDate { get; set; }

        public DateTime? LastReferencedDate { get; set; }
    }
}
