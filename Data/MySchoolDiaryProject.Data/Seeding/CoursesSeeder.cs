namespace MySchoolDiaryProject.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MySchoolDiaryProject.Data.Models;

    public class CoursesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Courses.Any())
            {
                return;
            }

            var courses = new List<string> { "Class 1", "Class 2", "Class 3", "Class 4", "Class 5", "Class 6", "Class 7", "Class 8", "Class 9", "Class 10", "Class 11", "Class 12" };

            foreach (var course in courses)
            {
                await dbContext.Courses.AddAsync(new Course
                {
                    Name = course,
                    Description = course,
                });
            }
        }
    }
}
