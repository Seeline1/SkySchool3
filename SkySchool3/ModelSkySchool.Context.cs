﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SkySchool3Entities : DbContext
    {
        public SkySchool3Entities()
            : base("name=SkySchool3Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Discipline> Discipline { get; set; }
        public virtual DbSet<Discipline_Assessment> Discipline_Assessment { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Lesson_Type> Lesson_Type { get; set; }
        public virtual DbSet<List_of_Discipline> List_of_Discipline { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
