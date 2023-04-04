namespace RbiFrontend.ApiAccess;

public class ClientErrorException : Exception
{
	public ClientErrorException(string? message) : base(message)
	{

	}
}
