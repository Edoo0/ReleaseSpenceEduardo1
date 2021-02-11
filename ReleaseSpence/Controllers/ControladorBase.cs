using System.Web.Mvc;
using ReleaseSpence.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ReleaseSpence.Controllers
{
	public class ControladorBase : Controller
	{
		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			set
			{
				_userManager = value;
			}
		}
        
		public ApplicationUser CurrentUser
		{
			get
			{
				ApplicationUser _user = null;
				if (CurrentUserId != -1)
				{
					_user = UserManager.FindById(CurrentUserId);
				}
				return _user;
			}
		}

		public int CurrentUserId
		{
			get
			{
				int userId = -1;
				if (User != null &&
					User.Identity != null &&
					User.Identity.IsAuthenticated &&
					int.TryParse(User.Identity.GetUserId(), out userId))
				{
					return userId;
				}
				return -1;
			}
		}
		
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			ViewBag.CurrentUser = CurrentUser;
			base.OnActionExecuting(filterContext);
		}

        protected override void OnException(ExceptionContext filterContext)
        {
            ViewResult view = new ViewResult();
            view.ViewName = "Error";
            filterContext.Result = view;
            filterContext.ExceptionHandled = true;
        }
    }
}