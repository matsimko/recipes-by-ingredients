using System.Net;
using System.Net.Http.Json;

namespace RbiFrontend.ApiAccess;

public abstract class AbstractSource<TDto, TCreationDto, TDetailDto>
{
    protected readonly HttpClient _http;
    protected abstract string ResourceName { get; }

    public AbstractSource(HttpClient http)
    {
        _http = http;
    }

    public async Task<TDetailDto> Get(long id)
    {
        var response = await _http.GetAsync($"{ResourceName}/{id}");
        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden)
        {
            var errorMessage = await response.Content.ReadFromJsonAsync<ErrorMessage>();
            throw new ClientErrorException(errorMessage?.Error);
        }
        return await response.Content.ReadFromJsonAsync<TDetailDto>();
    }

    public Task<IEnumerable<TDto>> GetAll()
    {
        return _http.GetFromJsonAsync<IEnumerable<TDto>>($"{ResourceName}");
    }

    public Task<IEnumerable<TDto>> Search(object queryParams)
    {
        var queryString = UrlUtils.ObjectToQueryString(queryParams);
        return _http.GetFromJsonAsync<IEnumerable<TDto>>($"{ResourceName}?{queryString}");
    }

    public async Task<TDetailDto> Insert(TCreationDto dto)
    {
        var response = await _http.PostAsJsonAsync($"{ResourceName}", dto);
        return await response.Content.ReadFromJsonAsync<TDetailDto>();
    }

    public Task Update(long id, TCreationDto dto)
    {
        return _http.PutAsJsonAsync($"{ResourceName}/{id}", dto);
    }

    public Task Delete(long id)
    {
        return _http.DeleteAsync($"{ResourceName}/{id}");
    }
}
