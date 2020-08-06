namespace MySchoolDiaryProject.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MySchoolDiaryProject.Data;
    using MySchoolDiaryProject.Data.Common;
    using MySchoolDiaryProject.Data.Common.Repositories;
    using MySchoolDiaryProject.Data.Models;
    using MySchoolDiaryProject.Data.Repositories;
    using MySchoolDiaryProject.Data.Seeding;
    using MySchoolDiaryProject.Services.Data;
    using MySchoolDiaryProject.Services.Mapping;
    using MySchoolDiaryProject.Services.Messaging;
    using MySchoolDiaryProject.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    });
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();


            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICoursesService, CoursesService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<ISubjectsService, SubjectsService>();
            services.AddTransient<IGradeService, GradeService>();
            services.AddTransient<ICourseSubjectsService, CourseSubjectsService>();
            services.AddTransient<IStudentSubjectsService, StudentSubjectsService>();
            services.AddTransient<IAttendancesService, AttendancesService>();
            services.AddTransient<ISubjectTeachersService, SubjectTeachersService>();
            services.AddTransient<ICourseSubjectTeacherService, CourseSubjectTeacherService>();;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            "areaRoute",
                            "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        //endpoints.MapControllerRoute(
                        //    "studentInfoWithPage",
                        //    "{area:exists}/{controller=Student}/{action=Info}/{page}");
                        endpoints.MapControllerRoute(
                            "studentInfo",
                            "{area:exists}/{controller=Student}/{action=Info}");
                        endpoints.MapControllerRoute(
                            "schoolCourse",
                            "s/{name:minlength(3)}", new { controller = "Course", action = "ByName" });
                        endpoints.MapControllerRoute(
                            "backFromAddSubject",
                            "s/{name:minlength(3)}", new { controller = "Course", action = "AddSubject" });
                        endpoints.MapControllerRoute(
                            "default",
                            "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}