using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts;

public sealed record HttpError : Error
{
    private HttpError(DefaultErrorCode code, string message, HttpResponseMessage response) : base(code, message)
    {
        Response = response;
    }

    private HttpError(string code, string message, HttpResponseMessage response) : base(code, message)
    {
        Response = response;
    }

    public HttpResponseMessage Response { get; init; }
    public int ResponseStatusCode { get; init; }

    public static HttpError FromResponse(HttpResponseMessage response)
    {
        if ( response.IsSuccessStatusCode )
        {
            throw new InvalidOperationException("Can't create an Error from successful request");
        }

        return new HttpError(
            $"{response.StatusCode}",
            $"The error occurred while processing an HTTP request: {response.ReasonPhrase}.",
            response)
        { ResponseStatusCode = (int)response.StatusCode };
    }
}
