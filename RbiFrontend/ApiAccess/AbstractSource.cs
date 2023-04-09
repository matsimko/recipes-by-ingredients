using System.Net;
using System.Net.Http.Json;

namespace RbiFrontend.ApiAccess;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Regular DTO</typeparam>
/// <typeparam name="U">Creation/update DTO</typeparam>
/// <typeparam name="V">Detail DTO</typeparam>
public abstract class AbstractSource<T, U, V>
{
    protected readonly HttpClient _http;
    protected abstract string ResourceName { get; }

    public AbstractSource(HttpClient http)
    {
        _http = http;
    }

    public async Task<V> Get(long id)
    {
        var response = await _http.GetAsync($"{ResourceName}/{id}");
        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden)
        {
            var errorMessage = await response.Content.ReadFromJsonAsync<ErrorMessage>();
            throw new ClientErrorException(errorMessage?.Error);
        }
        return await response.Content.ReadFromJsonAsync<V>();
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

    public async Task<V> Insert(U dto)
    {
        var response = await _http.PostAsJsonAsync($"{ResourceName}", dto);
        return await response.Content.ReadFromJsonAsync<V>();
    }

    public Task Update(long id, U dto)
    {
        return _http.PutAsJsonAsync($"{ResourceName}/{id}", dto);
    }

    public Task Delete(long id)
    {
        return _http.DeleteAsync($"{ResourceName}/{id}");
    }
}
