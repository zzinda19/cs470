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
    
    public partial class DIM_Patient
    {
        public int P_pat_itn { get; set; }
        public string P_pt_med_rec_no { get; set; }
        public string P_mrn_assign_auth_cd { get; set; }
        public string P_hosp { get; set; }
        public string PN_pt_last { get; set; }
        public string PN_pt_first { get; set; }
        public string PN_pt_middle { get; set; }
        public Nullable<System.DateTime> PN_pt_birth_dtime { get; set; }
        public string PC_corp_id { get; set; }
    }
}
