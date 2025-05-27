using BistroBoss.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class BasePageModel : PageModel
{
    protected readonly ApplicationDbContext _dbContext;

    public BasePageModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
    {
        ViewData["Kategorie"] = _dbContext.Kategorie.OrderBy(k => k.Nazwa).ToList();
        base.OnPageHandlerExecuting(context);
    }
}