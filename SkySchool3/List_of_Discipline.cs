//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkySchool3
{
    using System;
    using System.Collections.Generic;
    
    public partial class List_of_Discipline
    {
        public int ID_List_of_Discipline { get; set; }
        public int ID_User { get; set; }
        public int ID_Discipline { get; set; }
        public int ID_Lesson_Type { get; set; }
    
        public virtual Discipline Discipline { get; set; }
        public virtual Lesson_Type Lesson_Type { get; set; }
        public virtual User User { get; set; }
    }
}
