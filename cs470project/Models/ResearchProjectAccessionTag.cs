//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cs470project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ResearchProjectAccessionTag
    {
        public long TagID { get; set; }
        public int ProjectID { get; set; }
        public int Accession { get; set; }
        public System.DateTime RecordInsertDate { get; set; }
    
        public virtual ResearchProjectAccession ResearchProjectAccession { get; set; }
        public virtual ResearchProjectTag ResearchProjectTag { get; set; }
    }
}
