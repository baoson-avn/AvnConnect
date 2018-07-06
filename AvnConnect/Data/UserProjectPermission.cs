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
    
    public partial class UserProjectPermission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProjectPermission()
        {
            this.IsAdmin = false;
            this.CanViewUpdate = true;
            this.CanAddUpdate = false;
            this.CanViewTask = true;
            this.CanViewEstimatedTime = true;
            this.CanCreateTask = true;
            this.CanUpdateAllTask = false;
            this.CanViewMessageAndFile = true;
            this.CanUpdateMessageAndFile = true;
            this.CanViewNoteBook = true;
            this.CanUpdateNoteBook = true;
            this.CanViewLinks = true;
            this.CanAddLinks = true;
            this.CanViewRisk = false;
            this.CanUpdateRisk = false;
        }
    
        public int Id { get; set; }
        public string ProjectStaffKey { get; set; }
        public bool IsAdmin { get; set; }
        public bool CanViewUpdate { get; set; }
        public bool CanAddUpdate { get; set; }
        public bool CanViewTask { get; set; }
        public bool CanViewEstimatedTime { get; set; }
        public bool CanCreateTask { get; set; }
        public bool CanUpdateAllTask { get; set; }
        public bool CanViewMessageAndFile { get; set; }
        public bool CanUpdateMessageAndFile { get; set; }
        public bool CanViewNoteBook { get; set; }
        public bool CanUpdateNoteBook { get; set; }
        public bool CanViewLinks { get; set; }
        public bool CanAddLinks { get; set; }
        public bool CanViewRisk { get; set; }
        public bool CanUpdateRisk { get; set; }
    }
}