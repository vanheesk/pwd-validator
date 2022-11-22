using Arke.PwdValidator.API.Application.Contracts.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace PwdValidator.API.Application.Features.Hash.Queries.GetHash;

public class GetHashQueryHandler : IRequestHandler<GetHashQuery, HashVm>
{

    private readonly IConfiguration _configuration;
    private readonly IBlobStorageService _azureBlobStorageService;

    public GetHashQueryHandler(IConfiguration configuration, IBlobStorageService azureBlobStorageService)
    {
        _azureBlobStorageService = azureBlobStorageService;
        _configuration = configuration;
    }

    public async Task<HashVm> Handle(GetHashQuery request, CancellationToken cancellationToken)
    {
        var containerName = string.Empty;
        var found = await _azureBlobStorageService.GetFileUri(containerName, request.HashValue);
        var hash = new HashVm(request.HashValue, -1);

        return hash;
    }
}
