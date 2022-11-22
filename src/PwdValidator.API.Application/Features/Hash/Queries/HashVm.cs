namespace PwdValidator.API.Application.Features.Hash.Queries;

public record HashVm
{

    public HashVm(string? value = null, int count = -1, string? url = null)
    {
        Value = value;
        Count = count;
        Url = url;
    }

    /// <summary>
    /// The password as a SHA-1 encrypted string
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// The prevalence of the password.
    /// Phrased differently: The total number of cases in the dataset of known breaches at a specific time. 
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Get the url to the actual file
    /// </summary>
    public string? Url { get; }

}