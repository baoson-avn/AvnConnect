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
    
    public partial class Permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Permission()
        {
            this.IsAdmin = false;
            this.CanAddProject = true;
            this.CanManageStaff = false;
            this.GiveFutureAccess = false;
            this.ManageDepartment = false;
        }
    
        public int Id { get; set; }
        public string Key { get; set; }
        public string StaffKey { get; set; }
        public bool IsAdmin { get; set; }
        public bool CanAddProject { get; set; }
        public bool CanManageStaff { get; set; }
        public bool GiveFutureAccess { get; set; }
        public bool ManageDepartment { get; set; }
    }
}