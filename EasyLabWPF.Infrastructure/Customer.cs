//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyLabWPF.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    [DataContract(IsReference = true)]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Orders = new HashSet<Order>();
            this.CustomerDemographics = new HashSet<CustomerDemographic>();
        }
    
        [DataMember]
		public string CustomerID { get; set; }
        [DataMember]
		public string CompanyName { get; set; }
        [DataMember]
		public string ContactName { get; set; }
        [DataMember]
		public string ContactTitle { get; set; }
        [DataMember]
		public string Address { get; set; }
        [DataMember]
		public string City { get; set; }
        [DataMember]
		public string Region { get; set; }
        [DataMember]
		public string PostalCode { get; set; }
        [DataMember]
		public string Country { get; set; }
        [DataMember]
		public string Phone { get; set; }
        [DataMember]
		public string Fax { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember]
		public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember]
		public virtual ICollection<CustomerDemographic> CustomerDemographics { get; set; }
    }
}
