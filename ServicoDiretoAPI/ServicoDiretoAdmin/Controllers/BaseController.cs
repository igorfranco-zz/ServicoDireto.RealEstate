using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Admin;
using System.Xml.Linq;
using System.Web.Security;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
//using Microsoft.Practices.EnterpriseLibrary.Logging;

[HandleError]
public class BaseController : Controller
{
    public T GetFromSession<T>(string key, string contextID = null)
    {
        if (!string.IsNullOrEmpty(contextID))
            key = String.Format("{0}_{1}", key, contextID);

        return (T)HttpContext.Session[key];
    }

    public void SetInSession(string key, string contextID, object value)
    {
        if (!string.IsNullOrEmpty(contextID))
            key = String.Format("{0}_{1}", key, contextID);

        HttpContext.Session[key] = value;
    }

    public BaseController()
    {
        //var cultureInfo = new System.Globalization.CultureInfo(SpongeSolutions.ServicoDireto.Services.ServiceContext.ActiveLanguage);
        //System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
    }

    //protected override void OnActionExecuted(ActionExecutedContext filterContext)
    //{
    //    string controllerName = filterContext.Controller.ToString();
    //    string actionName = filterContext.ActionDescriptor.ActionName;
    //    ////Logger.Write(String.Format("Execução Controller:{0} - Action:{1} - UserName: {2}", controllerName, actionName, (this.User != null && this.User.Identity != null) ? this.User.Identity.Name : "Não Autenticado"), "Smarttan", 10, 1, System.Diagnostics.TraceEventType.Information);
    //    bool hasPermission = true;
    //    List<Permission> permissions = null;
    //    Permission permission = null;
    //    try
    //    {
    //        permissions = SpongeSolutions.Core.Cache.CacheManager.GetInsert<List<Permission>>("Permission", () => SpongeSolutions.Core.Helpers.Serialization.SerializationHelper.DeSerialize<List<Permission>>(Server.MapPath("/SmarttanV2/permission.xml")));
    //        permission = (from rec in permissions where rec.ControllerInfo.Name.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) select rec).FirstOrDefault();
    //        if (permission != null && permission.ControllerInfo != null)
    //            hasPermission = this.VerifyPermission(permission.ControllerInfo);

    //        if (!hasPermission)
    //        {
    //            //Buscando as permissoes das actions do controller em questão
    //            if (permission != null && permission.ActionInfo != null)
    //            {
    //                var actionPermission = (from rec in permission.ActionInfo where rec.Name.Equals(actionName, StringComparison.CurrentCultureIgnoreCase) select rec).FirstOrDefault();
    //                if (actionPermission != null)
    //                    hasPermission = this.VerifyPermission(actionPermission);
    //            }
    //        }

    //        if (!hasPermission && permission != null)
    //        {
    //            ////Logger.Write(String.Format("Permissão - Controller:{0} - Action:{1} - UserName: {2} Possui Permissão: {3}", new object[] { controllerName, actionName, (this.User != null && this.User.Identity != null) ? this.User.Identity.Name : "Não Autenticado", hasPermission }), "Smarttan", 10, 1, System.Diagnostics.TraceEventType.Information);
    //            filterContext.Result = new ViewResult() { ViewName = "Unauthorized" };
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //    base.OnActionExecuted(filterContext);
    //}

    protected bool VerifyPermision(InfoType infoType)
    {
        bool hasPermission = false;
        if (infoType != null && infoType.Roles != null)
        {
            //Verificando as restricoes de acesso em cima do controller
            foreach (var role in infoType.Roles)
            {
                if (role.Name == "*")
                    return (role.PermissionType == Enums.PermissionType.Permissive);

                bool userInRole = Roles.IsUserInRole(role.Name);
                if (userInRole && role.PermissionType == Enums.PermissionType.NonPermissive)
                    return false;
                else if (userInRole)
                    return true;
            }
        }
        return hasPermission;
    }
}
