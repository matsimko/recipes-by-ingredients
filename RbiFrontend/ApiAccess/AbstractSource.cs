using System.Net;
using System.Net.Http.Json;

namespace RbiFrontend.ApiAccess;

public abstract class AbstractSource<T>
{
    protected readonly HttpClient _http;
    protected abstract string ResourceName { get; }

    public AbstractSource(HttpClient http)
    {
        _http = http;
    }

    public async Task<T> Get(long id)
    {
        var response = await _http.GetAsync($"{ResourceName}/{id}");
        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden)
        {
            var errorMessage = await response.Content.ReadFromJsonAsync<ErrorMessage>();
            throw new ClientErrorException(errorMessage?.Error);
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public Task<IEnumerable<T>> GetAll()
    {
        return _http.GetFromJsonAsync<IEnumerable<T>>($"{ResourceName}");
    }

    public Task<IEnumerable<T>> Search(object queryParams)
    {
        var queryString = UrlUtils.ObjectToQueryString(queryParams);
        return _http.GetFromJsonAsync<IEnumerable<T>>($"{ResourceName}?{queryString}");
    }

    public async Task<T> Insert(T dto)
    {
        var response = await _http.PostAsJsonAsync($"{ResourceName}", dto);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public Task Update(T dto)
    {
        return _http.PutAsJsonAsync($"{ResourceName}", dto);
    }

    public Task Delete(long id)
    {
        return _http.DeleteAsync($"{ResourceName}/{id}");
    }
}
