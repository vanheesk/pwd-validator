using MediatR;

namespace PwdValidator.API.Application.Features.Hash.Queries.GetHash;

public class GetHashQuery : IRequest<HashVm>
{

    public GetHashQuery(string hashValue)
    {
        HashValue = hashValue;
    }

    public string HashValue { get; private set; }

}
