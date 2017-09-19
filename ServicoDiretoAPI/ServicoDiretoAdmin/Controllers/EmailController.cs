using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.Core.RestFul;
using SpongeSolutions.ServicoDireto.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Controllers
{
    public class EmailController : BaseController
    {
        public ActionResult Manager(bool windowMode = false)
        {
            this.ViewBag.WindowMode = windowMode;
            return View();
        }

        public ActionResult Inbox()
        {
            return View();
        }

        public ActionResult Sent()
        {
            return View();
        }

        public ActionResult Index(int page = 1, bool windowMode = false, short status = (short)SpongeSolutions.ServicoDireto.Model.InfraStructure.Enums.EmailType.Sent, int idCustomerTo = -1, int idCustomerFrom = -1)
        {
            page--;
            int recordCount = 0;
            this.ViewBag.WindowMode = windowMode;
            IList<Model.EmailExtended> viewData = ServiceContext.EmailService.GetAll(out recordCount, -1, idCustomerTo, idCustomerFrom, status, null, SiteContext.MaximumRows * page, SiteContext.MaximumRows).ToList();
            this.ViewBag.RowCount = recordCount;
            if ((idCustomerTo != -1 && SiteContext.GetActiveProfile.Preferences.IDCustomer != idCustomerTo) ||
                (idCustomerFrom != -1 && SiteContext.GetActiveProfile.Preferences.IDCustomer != idCustomerFrom))
                return View("Unauthorized");

            this.ViewBag.IDCustomerTo = idCustomerTo;
            this.ViewBag.IDCustomerFrom = idCustomerFrom;
            this.ViewBag.Status = status;

            return View(viewData);
        }

        public ActionResult Create(long? idEmail)
        {
            EmailExtended email = new EmailExtended();
            this.ViewBag.DisableFrom = false;
            if (idEmail.HasValue)
            {
                email = ServiceContext.EmailService.GetByIdExtended(idEmail.Value);
                //--Indicando o horário que leu, que seja o receptor que tenha lido
                if (email.IDCustomerTo == SiteContext.GetActiveProfile.Preferences.IDCustomer)
                {
                    var upEmail = ServiceContext.EmailService.GetById(idEmail.Value);
                    upEmail.Read = true;
                    upEmail.ModifyDate = DateTime.Now;
                    ServiceContext.EmailService.Update(upEmail);
                }

                email.IDEmailParent = idEmail.Value;
                email.IDCustomerTo = email.IDCustomerFrom;
                email.IDCustomerToName = email.IDCustomerFromName;
                email.Subject = String.Format("{0}: [{1}] ", SpongeSolutions.ServicoDireto.Internationalization.Label.Answer, email.Subject);
                email.Content = "\n\n" + email.CreateDate.ToString() + "\n" + email.Content;

                this.ViewBag.DisableFrom = true;
            }
            email.IDCustomerFrom = SiteContext.GetActiveProfile.Preferences.IDCustomer;
            email.IDCustomerFromName = ServiceContext.CustomerService.GetById(email.IDCustomerFrom.Value).Name;
            email.Status = (short)SpongeSolutions.ServicoDireto.Model.InfraStructure.Enums.EmailType.Sent;
            return View(email);
        }

        [HttpPost]
        public ActionResult Create(Email email)
        {
            email.IDEmail = null;
            email.CreateDate = DateTime.Now;
            email.CreatedBy = ServiceContext.ActiveUserName;
            email.Read = false;
            ServiceContext.EmailService.Insert(email);
            return View(ServiceContext.EmailService.GetByIdExtended(email.IDEmail.Value));
            //return RedirectToAction("create", new { idEmail = email.IDEmail });
        }

        [HttpPost]
        public JsonResult Delete(long[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.EmailService.Deactivate(ids);
                    return Json(Response<dynamic>.WrapResponse(new { Deleted = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
                }
                else
                    return Json(Response<dynamic>.WrapResponse(new { Message = SpongeSolutions.ServicoDireto.Internationalization.Message.Select_Items_To_Be_Deleted }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

        [HttpPost]
        public JsonResult MarkAsUnread(long[] ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    ServiceContext.EmailService.MarkAsUnread(ids);
                    return Json(Response<dynamic>.WrapResponse(new { MarkedAsUnread = true }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
                }
                else
                    return Json(Response<dynamic>.WrapResponse(new { Message = SpongeSolutions.ServicoDireto.Internationalization.Message.Select_Items_To_Be_Deleted }, SpongeSolutions.Core.RestFul.Enums.ResponseStatus.OK));
            }
            catch (Exception ex)
            {
                return Json(Response<string>.WrapResponse(null, ex.ToString(), SpongeSolutions.Core.RestFul.Enums.ResponseStatus.Error));
            }
        }

    }
}
