using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_management_system_api.Models;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProjectController : ControllerBase
{
    AppDbContext _context;
    ILogger<ProjectController> _logger;
    public ProjectController(AppDbContext context, ILogger<ProjectController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetProjects()
    {
        return Ok();
    }
    //api/projectsGETRetrieve a list of all projects

    ///api/projectsPOSTCreate a new project
    ///

    ///api/projects/{id}GETRetrieve details of a specific project
    ////api/projects/{id}PUTUpdate project details
    ////api/projects/{id}DELETEDelete a project
    ////api/projects/{id}/assignPOSTAssign users to a project
    ////api/projects/{id}/usersGETList users assigned to a project
}