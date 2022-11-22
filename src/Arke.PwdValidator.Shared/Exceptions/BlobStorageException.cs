namespace Arke.PwdValidator.Shared.Exceptions;

public class BlobStorageException : ApplicationException
{

    public BlobStorageException(string message)
        : base(message)
    { }

}
