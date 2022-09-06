using Microsoft.AspNetCore.Mvc;
using TodoApp.Domain;

namespace TodoApp.Presentation;

public static class ResultExtensions
{
    public static ActionResult Handle(this Result result, Func<ActionResult> onSuccess, Func<Error, ActionResult> onError) => result.HasError() ? onError(result) : onSuccess();

    public static ActionResult Handle<T>(this Result<T> result, Func<T, ActionResult> onSuccess, Func<Error, ActionResult> onError) => result.HasError() ? onError(result) : onSuccess(result);
}
