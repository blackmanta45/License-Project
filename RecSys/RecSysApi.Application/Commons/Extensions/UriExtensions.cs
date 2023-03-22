using System;
using System.Collections.Generic;
using System.Web;

namespace RecSysApi.Application.Commons.Extensions;

public static class UriExtensions
{
    public static Uri AddParameters(this string baseUrl, IDictionary<string, string> values)
    {
        //todo make this prettier
        var queryCollection = HttpUtility.ParseQueryString(string.Empty);

        foreach (var (key, value) in values ?? new Dictionary<string, string>()) queryCollection[key] = value;
        return queryCollection.Count == 0
            ? new Uri(baseUrl, UriKind.Absolute)
            : new Uri($"{baseUrl}?{queryCollection}", UriKind.Absolute);
    }
}