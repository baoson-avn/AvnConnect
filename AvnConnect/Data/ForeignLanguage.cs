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
    
    public partial class ForeignLanguage
    {
        public int Id { get; set; }
        public string StaffKey { get; set; }
        public string Language { get; set; }
        public byte SpeakingLevel { get; set; }
        public byte ListeningLevel { get; set; }
        public byte ReadingLevel { get; set; }
        public byte WritingLevel { get; set; }
    }
}