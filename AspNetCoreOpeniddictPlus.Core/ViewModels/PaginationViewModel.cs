namespace AspNetCoreOpeniddictPlus.Core.ViewModels;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}