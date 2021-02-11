using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ReleaseSpence.Models
{
    // Puede agregar datos del perfil del usuario agregando más propiedades a la clase ApplicationUser. Para más información, visite http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser<int, ApplicationLogin, ApplicationUserRole, ApplicationClaim>
    {
        public string FullName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar reclamaciones de usuario personalizado aquí
            return userIdentity;
        }
    }

    public class ApplicationLogin : IdentityUserLogin<int> { }
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationClaim : IdentityUserClaim<int> { }
    public class RolesSistema
    {
        public const string Administrador = "Administrador";
        public const string Modificacion = "Modificacion";
        public const string Escritura = "Escritura";
        public const string Lectura = "Lectura";
        public const string Logger = "Logger";
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationLogin, ApplicationUserRole, ApplicationClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            //Esta linea evita que se ejecute migrations, de tal forma que es posible modificar manualmente 
            //la base de datos y evitar excepciones que marca se cambie el contexto de la base de datos y marque errores
            //Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Cambiando la tabla AspNetUsers a Usuarios y campos al español para concordar con el base de datos actual.
            modelBuilder.Entity<ApplicationUser>().ToTable("Identity_Users");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Id).HasColumnName("idUsuario");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.FullName).HasColumnName("fullName");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Email).HasColumnName("email");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.PasswordHash).HasColumnName("pass");
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.EmailConfirmed).HasColumnName("ConfirmacionDeCorreo");
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.PhoneNumber).HasColumnName("NumeroDeTelefono");
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.PhoneNumberConfirmed).HasColumnName("ConfirmadoNumeroDeTelefono");
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.TwoFactorEnabled).HasColumnName("TwoFactorHabilitado");
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.LockoutEnabled).HasColumnName("FechaFinalizacionDeBloqueo");
            //modelBuilder.Entity<ApplicationUser>().Property(p => p.AccessFailedCount).HasColumnName("ContadorDeAccesosFallidos");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.UserName).HasColumnName("userName");

            //Cambiando la tabla AspNetUsers a Usuarios y campos al español para concordar con el base de datos actual.
            modelBuilder.Entity<ApplicationRole>().ToTable("Identity_Roles");
            modelBuilder.Entity<ApplicationRole>().Property(p => p.Id).HasColumnName("idRol");
            modelBuilder.Entity<ApplicationRole>().Property(p => p.Name).HasColumnName("nombre");

            //Cambiando la tabla AspNetRoles a Roles y campos al español para concordar con el base de datos actual.
            modelBuilder.Entity<ApplicationClaim>().ToTable("Identity_UserClaims");
            //modelBuilder.Entity<ApplicationClaim>().Property(p => p.Id).HasColumnName("IdUsuario_Atributo");
            //modelBuilder.Entity<ApplicationClaim>().Property(p => p.UserId).HasColumnName("IdUsuario");
            //modelBuilder.Entity<ApplicationClaim>().Property(p => p.ClaimType).HasColumnName("TipoAtributo");
            //modelBuilder.Entity<ApplicationClaim>().Property(p => p.ClaimValue).HasColumnName("ValorAtributo");

            //Cambiando la tabla AspNetUserRoles a Usuarios_Roles y campos al español para concordar con el base de datos actual.
            modelBuilder.Entity<ApplicationUserRole>().ToTable("Identity_UserRoles");
            modelBuilder.Entity<ApplicationUserRole>().Property(p => p.RoleId).HasColumnName("idRol");
            modelBuilder.Entity<ApplicationUserRole>().Property(p => p.UserId).HasColumnName("idUsuario");

            //Cambiando la tabla AspNetUserLogins a Usuarios_Acceso y campos al español para concordar con el base de datos actual.
            modelBuilder.Entity<ApplicationLogin>().ToTable("Identity_UserLogins");
            //modelBuilder.Entity<ApplicationLogin>().Property(p => p.LoginProvider).HasColumnName("ProveedorDeAcceso");
            //modelBuilder.Entity<ApplicationLogin>().Property(p => p.ProviderKey).HasColumnName("LlaveDelProveedor");
            //modelBuilder.Entity<ApplicationLogin>().Property(p => p.UserId).HasColumnName("IdUsuario");

        }
    }
}