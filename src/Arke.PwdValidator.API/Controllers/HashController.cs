using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using PwdValidator.API.Application.Features.Hash.Queries;
using PwdValidator.API.Application.Features.Hash.Queries.GetHash;

namespace Arke.PwdValidator.API.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class HashController : ControllerBase
{

    private readonly IMediator _mediator;

    public HashController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns whether or not the hash specified is present in the database, and if it is, how often it has been subject to a breach.
    /// </summary>
    /// <param name="id">The hash value to lookup</param>
    /// <returns>A response message</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<HashVm> Get(string id)
    {
        var dto = await _mediator.Send(new GetHashQuery(id));
        return dto;
    }

}
