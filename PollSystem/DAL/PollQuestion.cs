//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PollQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PollQuestion()
        {
            this.PollQuesAnswers = new HashSet<PollQuesAnswer>();
        }
    
        public short Id { get; set; }
        public short PollId { get; set; }
        public string Question { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Poll Poll { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PollQuesAnswer> PollQuesAnswers { get; set; }
    }
}
