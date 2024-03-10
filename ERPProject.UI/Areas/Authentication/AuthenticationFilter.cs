using ERPProject.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ERPProject.UI.Areas.Authentication
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public string Role;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!string.IsNullOrEmpty(Role))
            {
                var Roles = SessionRole.RoleName;
                if (SessionRole.RoleName != null)
                {
                    bool isAuthorized = Roles.Any(role => Role.Contains(role));

                    if (!isAuthorized)
                    {
                        context.Result = new RedirectToActionResult("Forbidden", "Home", null);
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
        //Action çalışmadan önce/
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        //Action çalıştıktan sonra/
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        //Tam response'u kullanıcıya göndermeden önce çalıştırılır/
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        //Sonucu gönderdik ve bu metod çalışıyor*/
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}

