//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AvnConnect.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PracticingLicense
    {
        public int Id { get; set; }
        public string StaffKey { get; set; }
        public string LicenseNumber { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public string Status { get; set; }
        public string PlaceOfIssue { get; set; }
        public string ProfessionalArea { get; set; }
    }
}