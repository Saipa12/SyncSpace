using System.Runtime.Serialization;

namespace SyncSpace.Core.Exceptions;

[Serializable]
public class InternetConnectionException : Exception
{
	public InternetConnectionException()
	{
	}

	public InternetConnectionException(string message) : base(message)
	{
	}

	public InternetConnectionException(string message, Exception innerException) : base(message, innerException)
	{
	}

	protected InternetConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}