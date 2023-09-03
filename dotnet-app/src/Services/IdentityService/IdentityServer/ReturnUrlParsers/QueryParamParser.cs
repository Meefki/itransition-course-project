namespace IdentityServer.ReturnUrlParsers;

public static class QueryParamParser
{
    public static T? GetParam<T>(string queryString, string paramName)
        where T : class
    {
        paramName += '=';

        int startIndex = queryString.IndexOf(paramName) == -1 ? 0 : queryString.IndexOf(paramName) + paramName.Length;
        int lenght = !queryString.Contains('&') ? queryString.Length - startIndex : queryString.IndexOf('&', startIndex) - startIndex;

        T? value = queryString.Substring(startIndex, lenght) as T;
        return value;
    }
}