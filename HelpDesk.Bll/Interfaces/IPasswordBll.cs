using HelpDesk.Bll.Models;
using HelpDesk.Helper.Models;

namespace HelpDesk.Bll.Interfaces
{
    public interface IPasswordBll
    {
        /// <summary>
        /// Change password function.
        /// </summary>
        /// <param name="model">The password data.</param>
        /// <returns></returns>
        ResultViewModel ChangePassword(PasswordViewModel model);
        /// <summary>
        /// Forget password logic function.
        /// </summary>
        /// <param name="model">The forget password information.</param>
        /// <returns></returns>
        ResultViewModel ForgetPassword(ForgetPasswordViewModel model);
    }
}
