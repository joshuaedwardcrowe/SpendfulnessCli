using Ynab.Clients;
using YnabProgress.ViewModels;

namespace YnabProgress.Compilers;

public class RequestLogCompiler : IViewModelCompiler<IEnumerable<ApiRequestLog>>
{
    private readonly ViewModel _requestLogViewModel = new();
    
    public ViewModel Compile(IEnumerable<ApiRequestLog> amountsPerYear)
    {
        _requestLogViewModel.Columns = ["Method", "URL", "Remaining Requests", "Request Time"];
        
        _requestLogViewModel.Rows = amountsPerYear
            .Select(data => new List<object>
            {
                data.Method,
                data.Path,
                data.RemainingRequestAllowance,
                $"{data.ResponeTime}ms"
            })
            .ToList();
        
        return _requestLogViewModel;
    }
}