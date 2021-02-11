using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ReleaseSpence.Models;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace ReleaseSpence.Controllers
{
    [Authorize]
	public class AccountController : ControladorBase
	{
		private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager)
		{
			UserManager = userManager;
        }

        [Authorize(Roles = RolesSistema.Administrador)]
        public ActionResult Index()
        {
			ViewBag.CurrentUserID = int.Parse(User.Identity.GetUserId());
			return View(db.Identity_Users.ToList());
        }

        [AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
        
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindAsync(model.userName, model.Password);
				if (user != null )
				{
						await SignInAsync(user, model.RememberMe);
						return RedirectToLocal(returnUrl);
				}
				else
				{
					ModelState.AddModelError("", "Nombre de usuario o contraseña no válidos.");
				}
			}
			return View(model);
		}

        [Authorize(Roles = RolesSistema.Administrador)]
        public ActionResult Register()
		{
			ViewBag.roles = new MultiSelectList(db.Identity_Roles, "idRol", "nombre");
			return View();
		}
        
		[HttpPost]
        [Authorize(Roles = RolesSistema.Administrador)]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(UserRegister model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser()
				{
					UserName = model.userName,
					Email = model.email,
					FullName = model.fullName
				};
				IdentityResult result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					//result = await UserManager.AddToRoleAsync(user.Id, RolesDelSistema.Administrador);
					//await SignInAsync(user, isPersistent: false);//esto hace que inicie sesion
					foreach (int idRol in model.roles) Identity_UsersRep.CreateRol(idRol, user.Id);
					return RedirectToAction("Index", "Account");
				}
				else
				{
					AddErrors(result);
				}
			}
            ViewBag.roles = new MultiSelectList(db.Identity_Roles, "idRol", "nombre");
            return View(model);
		}

        [Authorize(Roles = RolesSistema.Administrador)]
        public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Identity_Users usuario = db.Identity_Users.Find(id);
			if (usuario == null)
			{
				return HttpNotFound();
			}
			List<int> existentes = usuario.Identity_Roles.Select(i => i.idRol).ToList();
			ViewBag.roles = new MultiSelectList(db.Identity_Roles, "idRol", "nombre", existentes);
			return View(usuario);
		}

		[HttpPost]
        [Authorize(Roles = RolesSistema.Administrador)]
        [ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "idUsuario,email,userName,fullName,roles")] Identity_Users usuario)
		{
			if (ModelState.IsValid)
			{
				Identity_UsersRep.Update(usuario);
				usuario.roles = usuario.roles ?? new int[0];
				List<int> existentes = db.Identity_Users.Find(usuario.idUsuario).Identity_Roles.Select(i => i.idRol).ToList() ?? new List<int>();
				List<int> eliminar = existentes.Except(usuario.roles).ToList() ?? new List<int>();
				List<int> agregar = usuario.roles.Except(existentes).ToList() ?? new List<int>();
				foreach (int idRol in eliminar) Identity_UsersRep.DeleteRol(idRol, usuario.idUsuario);
				foreach (int idRol in agregar) Identity_UsersRep.CreateRol(idRol, usuario.idUsuario);
				return RedirectToAction("Index");
			}
			else
			{
				List<int> existentes = db.Identity_Users.Find(usuario.idUsuario).Identity_Roles.Select(i => i.idRol).ToList() ?? new List<int>();
				ViewBag.roles = new MultiSelectList(db.Identity_Roles, "idRol", "nombre", existentes);
				return View(usuario);
			}
		}

        [Authorize(Roles = RolesSistema.Administrador)]
        public ActionResult ResetPass(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Identity_Users usuario = db.Identity_Users.Find(id);
			if (usuario == null)
			{
				return HttpNotFound();
			}
			ResetPassVM uservm = new ResetPassVM();
			uservm.idUsuario = usuario.idUsuario;
			uservm.userName = usuario.userName;
			return View(uservm);
		}

		[HttpPost]
        [Authorize(Roles = RolesSistema.Administrador)]
        [ValidateAntiForgeryToken]
		public ActionResult ResetPass([Bind(Include = "idUsuario,NewPassword")] ResetPassVM usuario)
		{
			UserManager.RemovePassword(usuario.idUsuario);
			UserManager.AddPassword(usuario.idUsuario, usuario.NewPassword);
            return RedirectToAction("Index");
        }

        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePass(ChangePassVM model)
        {
            IdentityResult result = await UserManager.ChangePasswordAsync(int.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(int.Parse(User.Identity.GetUserId()));
                await SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                AddErrors(result);
            }
            return View(model);
        }

		[Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
		public PartialViewResult Delete(int? id)
		{
			if (id == null)
			{
				return PartialView(HttpStatusCode.BadRequest);
			}
			Identity_Users usuario = db.Identity_Users.Find(id);
			return PartialView(usuario);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			UserManager.Delete(UserManager.FindById(id));
			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Index", "Home");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && UserManager != null)
			{
				UserManager.Dispose();
				UserManager = null;
			}
			base.Dispose(disposing);
		}

		#region Aplicaciones auxiliares
		// Se usa para la protección XSRF al agregar inicios de sesión externos
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId<int>());
			if (user != null)
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		private void SendEmail(string email, string callbackUrl, string subject, string message)
		{
			// Para obtener información para enviar correo, visite http://go.microsoft.com/fwlink/?LinkID=320771
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}