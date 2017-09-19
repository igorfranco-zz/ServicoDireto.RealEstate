using System.Web;
using System.Web.Profile;
namespace SpongeSolutions.ServicoDireto.Model.AccountSystem
{
    public class CustomProfile : ProfileBase
    {
        //public Personal Personal
        //{
        //    get { return (Personal)base["Personal"];}
        //}

        public Preferences Preferences
        {
            get { return (Preferences)base["Preferences"]; }
        }

        /// <summary>
        /// Get the profile of the currently logged-on user.
        /// </summary>      
        public static CustomProfile GetProfile()
        {
            return (CustomProfile)HttpContext.Current.Profile;
        }

        /// <summary>
        /// Gets the profile of a specific user.
        /// </summary>
        /// <param name="userName">The user name of the user whose profile you want to retrieve.</param>
        public static CustomProfile GetProfile(string userName)
        {
            return (CustomProfile)Create(userName);
        }
    }
}