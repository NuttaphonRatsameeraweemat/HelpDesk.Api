using HelpDesk.Bll.Components;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll.Models;
using HelpDesk.Data.Pocos;
using HelpDesk.Data.Repository.Interfaces;
using HelpDesk.Helper;
using HelpDesk.Helper.Interfaces;
using HelpDesk.Helper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HelpDesk.Bll
{
    public class LoginBll : ILoginBll
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The ClaimsIdentity.
        /// </summary>
        private ClaimsIdentity _identity;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginBll" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public LoginBll(IConfigSetting config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Validate username and password is valid.
        /// </summary>
        /// <param name="login">The login value.</param>
        /// <returns></returns>
        public ResultViewModel Authenticate(LoginViewModel login)
        {
            var result = new ResultViewModel();
            var data = _unitOfWork.GetRepository<Customer>().GetCache(x => x.Email == login.Username).FirstOrDefault();
            if (!(data != null && this.ValidatePassword(login)))
            {
                result = UtilityService.InitialResultError(MessageValue.LoginFailed, 401);
            }
            return result;
        }

        /// <summary>
        /// The Verify Password.
        /// </summary>
        /// <param name="login">The login value.</param>
        /// <returns></returns>
        private bool ValidatePassword(LoginViewModel login)
        {
            var password = _unitOfWork.GetRepository<Password>().Get(x => x.Email == login.Username).FirstOrDefault();
            var verifyPassword = new PasswordGenerator(password != null ? password.Password1 : new byte[64]);
            return verifyPassword.Verify(login.Password);
        }

        /// <summary>
        /// Create and setting payload on token.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal.</param>
        /// <returns></returns>
        public string BuildToken(ClaimsPrincipal principal = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config.JwtIssuer,
              _config.JwtIssuer,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds,
              claims: this.GetClaimsPrincipal(principal));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// The Method Add ClaimsIdentity Properties.
        /// </summary>
        /// <param name="username">The identity user.</param>
        public EmployeeViewModel ManageClaimsIdentity(LoginViewModel login)
        {
            var data = _unitOfWork.GetRepository<Customer>().Get(x => x.Email == login.Username).FirstOrDefault();
            if (data == null)
            {
                throw new ArgumentNullException(ConstantValue.HrEmployeeArgumentNullExceptionMessage);
            }
            var result = new EmployeeViewModel
            {
                FirstNameTh = data.FirstNameTh,
                LastNameTh = data.LastNameTh,
                FirstNameEn = data.FirstNameEn,
                LastNameEn = data.LastNameEn,
                UserType = data.UserType
            };

            _identity = new ClaimsIdentity();
            _identity.AddClaim(new Claim(ClaimTypes.Name, data.Email));
            return result;
        }

        /// <summary>
        /// Get Claims Principal.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal.</param>
        /// <returns></returns>
        private List<Claim> GetClaimsPrincipal(ClaimsPrincipal principal)
        {
            var claims = new List<Claim>();
            if (principal != null)
            {
                claims = principal.Claims.ToList();
            }
            else claims = _identity.Claims.ToList();
            return claims;
        }

        /// <summary>
        /// Setup response cookie and cookie options token.
        /// </summary>
        /// <param name="httpContext">The HttpContext.</param>
        /// <param name="token">The token value.</param>
        public void SetupCookie(HttpContext httpContext, string token)
        {
            httpContext.Response.Cookies.Append("access_token", token, new CookieOptions()
            {
                Path = "/",
                HttpOnly = false, // to prevent XSS
                Secure = false, // set to true in production
                Expires = System.DateTime.UtcNow.AddMinutes(600) // token life time
            });
        }

        #endregion

    }
}
