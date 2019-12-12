using Microsoft.AspNetCore.Http;

namespace Reviews.Utils
{
    public static class HttpContextUtils
    {
        public static bool RetrieveRouteParameter(this HttpContext httpContext, string routeParameter, out string value)
        {
            if (!httpContext.Request.RouteValues.TryGetValue(routeParameter, out var valueObject))
            {
                value = null;
                return false;
            }

            value = valueObject as string;
            return true;
        }

        public static bool RetrieveFormField(this HttpContext httpContext, string formField, out string value)
        {
            if (!httpContext.Request.Form.TryGetValue(formField, out var fieldValues))
            {
                value = null;
                return false;
            }
            if (fieldValues.Count == 0)
            {
                value = null;
                return false;
            }

            value = fieldValues[0];
            return true;
        }
    }
}