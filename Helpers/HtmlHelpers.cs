
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Jiji.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent DisplayCurrency(this IHtmlHelper htmlHelper, decimal value, string cultureCode)
        {
            var cultureInfo = new CultureInfo(cultureCode);
            var formattedValue = string.Format(cultureInfo, "{0:C}", value);
            return new HtmlString(formattedValue);
        }
    }
}
