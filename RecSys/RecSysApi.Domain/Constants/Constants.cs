using System;

namespace RecSysApi.Domain.Constants;

public static class Constants
{
    public static string SearchForAQueryDomain =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "localhost:5001"
            : "searchforquery-api:5001";

    public static string TagsAppenderDomain =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "localhost:5004"
            : "tagsappender-api:5004";
}