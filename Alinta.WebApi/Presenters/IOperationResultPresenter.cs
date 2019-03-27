using Alinta.Core;
using Microsoft.AspNetCore.Mvc;

namespace Alinta.WebApi.Presenters
{
    public interface IOperationResultPresenter<T> where T:class
    {
        IActionResult Handle(OperationResult<T> result);
    }
}