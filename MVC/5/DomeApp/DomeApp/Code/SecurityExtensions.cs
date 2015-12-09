using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DomeApp.Code
{
    public static class SecurityExtensions
    {
        public static bool HasActionPermission(this IPrincipal user, RequestContext context, string actionName)
        {
            var controllerName = context.RouteData.Values["controller"] as string;

            return HasActionPermission(user, context, actionName, controllerName);
        }

        public static bool HasActionPermission(this IPrincipal user, RequestContext context, string actionName, string controllerName)
        {
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            var controller = controllerFactory.CreateController(context, controllerName);
            var controllerType = controller.GetType();
            var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);

            var canonicalActions = controllerDescriptor.GetCanonicalActions().Where(a => a.ActionName == actionName);

            var roles = canonicalActions.SelectMany(a => a.GetAuthorizedRoles());

            //todo check for no roles
            return roles.Any(role => user.IsInRole(role));
        }

        public static IEnumerable<string> GetAuthorizedRoles(this ActionDescriptor source)
        {
            var allCustomAttributes = source.GetCustomAttributes(true);
            var authorizeAttributes = allCustomAttributes.OfType<AuthorizeAttribute>();
            var rolesLists = authorizeAttributes.Select(a => a.Roles);
            var definedRolesLists = rolesLists.Where(r => !string.IsNullOrEmpty(r));
            var definedRoles = definedRolesLists.SelectMany(r => r.Split(','));

            return definedRoles;
        }
    }
}