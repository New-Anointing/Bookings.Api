using Bookings.Common.Domain;

namespace Bookings.Common.Application.Exceptions;

public sealed class BookingsException : Exception
{
    public BookingsException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }
    public Error? Error { get; }
}
