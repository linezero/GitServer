using System;

namespace GitServer.Services
{
	public class GitException : Exception
    {
		public GitException(string message, Exception innerException)
			: base(message, innerException)
		{ }
    }
}
