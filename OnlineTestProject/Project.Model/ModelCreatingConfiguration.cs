using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.DataModels
{
    public static class ModelBuilderExtention
    {
        public static ModelBuilder ModelCreatingConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => p.IsDeleted == false);
            modelBuilder.Entity<Exam>().HasQueryFilter(p => p.IsDeleted == false);
            modelBuilder.Entity<Question>().HasQueryFilter(p => p.IsDeleted == false);
            modelBuilder.Entity<QuestionOption>().HasQueryFilter(p => p.IsDeleted == false);

            return modelBuilder;
        }
    }
}
