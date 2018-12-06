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
    
    public partial class ResearchProjectAccession
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ResearchProjectAccession()
        {
            this.ResearchProjectAccessionTags = new HashSet<ResearchProjectAccessionTag>();
        }
    
        public int ProjectID { get; set; }
        public int Accession { get; set; }
        public long AlternateID { get; set; }
        public System.Guid AccessionGUID { get; set; }
        public string MRN { get; set; }
        public System.Guid MRNGUID { get; set; }
    
        public virtual ResearchProject ResearchProject { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResearchProjectAccessionTag> ResearchProjectAccessionTags { get; set; }
    }
}
