//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApplication3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teacher
    {
        public Teacher()
        {
            this.ModuleStudyAction = new HashSet<ModuleStudyAction>();
            this.TeacherWorkload = new HashSet<TeacherWorkload>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string TeacherType { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
    
        public virtual ICollection<ModuleStudyAction> ModuleStudyAction { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<TeacherWorkload> TeacherWorkload { get; set; }
    }
}
