namespace RbiFrontend;

public static class UtilExtensions
{
	public static string ToDateString(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.ToString("dd/MM/yyyy");
	}
}
