using Microsoft.AspNetCore.Html;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Helpers;

public class ColumnDefinition<TItem>
{
    public string Header { get; set; }
    public Func<TItem, IHtmlContent> Template { get; set; }
}
