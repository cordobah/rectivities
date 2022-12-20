using Application.Activities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Activity>>> Get()
    {
        return await Mediator.Send(new List.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> Get(Guid id)
    {
        return await Mediator.Send(new Details.Query{ Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> Put(Activity activity)
    {
        return Ok(await Mediator.Send(new Create.Command { Activity = activity }));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Activity activity)
    {
        activity.Id = id;
        return Ok(await Mediator.Send(new Edit.Command { Activity = activity }));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new Delete.Command { Id = id }));
    }

}