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
    
    public partial class Education
    {
        public int Id { get; set; }
        public string StaffKey { get; set; }
        public string EducationDegree { get; set; }
        public string NameOfSchool { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Speciality { get; set; }
        public System.DateTime FromYear { get; set; }
        public Nullable<System.DateTime> ToYear { get; set; }
        public Nullable<bool> IsLearning { get; set; }
    }
}
