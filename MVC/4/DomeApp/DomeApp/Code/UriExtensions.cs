using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DomeApp.Code
{
    public static class UriExtensions
    {
        public static string CleanQueryString(this string uri, UriKind uriKind = UriKind.Relative)
        {
            var u = new Uri(uri, uriKind);
            var queryString = HttpUtility.ParseQueryString(uri);

            var queryStringBuilder = new StringBuilder(u.LocalPath);
            queryStringBuilder.Append("?");
            queryStringBuilder.Append(queryString.ToString());

            return queryStringBuilder.ToString();
        }
    }
}