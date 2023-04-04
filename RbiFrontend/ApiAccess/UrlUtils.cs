using System.Collections;
using System.Web;

namespace RbiFrontend.ApiAccess;

public static class UrlUtils
{
    public static string ObjectToQueryString(object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj) != null
                         select p.GetValue(obj) is not IEnumerable ?
                            $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(obj).ToString())}" :
                            EnumerableToQueryString(p.Name, (IEnumerable<object>) p.GetValue(obj));

        return string.Join("&", properties);
    }

    private static string EnumerableToQueryString(string name, IEnumerable<object> enumerable)
    {
        var items = from item in enumerable
                    select $"{name}={HttpUtility.UrlEncode(item.ToString())}";
        return string.Join("&", items);
    }
}
