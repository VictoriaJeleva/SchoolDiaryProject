namespace MySchoolDiaryProject.Web.Areas.Administration.Controllers
{
    using MySchoolDiaryProject.Common;
    using MySchoolDiaryProject.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {

    }
}
