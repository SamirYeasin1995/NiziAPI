using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AppNiZiAPI.Infrastructure;
using AppNiZiAPI.Models.AccountModels;
using AppNiZiAPI.Models;
using AppNiZiAPI.Models.AuthModels;

namespace AppNiZiAPI.Security
{
    class Authorization : IAuthorization
    {
        public async Task<AuthResultModel> CheckAuthorization(HttpRequest req, int userId = 0, bool isDoctor = false)
        {
            AuthenticationHeaderValue.TryParse(req.Headers[HeaderNames.Authorization], out var authHeader);

            if (authHeader == null)
                return new AuthResultModel(false, AuthStatusCode.Unauthorized);

            ClaimsPrincipal claims = await Auth0.ValidateTokenAsync(authHeader);

            if (claims == null)
                return new AuthResultModel(false, AuthStatusCode.Unauthorized);

            string tokenGuid = claims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            IAuthorizationRepository authRepository = DIContainer.Instance.GetService<IAuthorizationRepository>();

            if (userId == 0 && authRepository.GetUserId(tokenGuid, isDoctor) == 0)
                return new AuthResultModel(false, AuthStatusCode.Forbidden);


            if (isDoctor && authRepository.CheckDoctorAcces(userId, tokenGuid))
                return new AuthResultModel(true, AuthStatusCode.Ok);

            if (userId != 0 && !authRepository.UserAuth(userId, tokenGuid, isDoctor))
                return new AuthResultModel(false, AuthStatusCode.Forbidden);

            return new AuthResultModel(true, AuthStatusCode.Ok);
        }

        public async Task<AuthResultModel> AuthForDoctorOrPatient(HttpRequest req, int userId)
        {
            AuthGUID authGUID = await GetGUIDAsync(req);
            if (!authGUID.Acces || authGUID.GUID == "")
                return new AuthResultModel(false, AuthStatusCode.Unauthorized);

            IAuthorizationRepository authRepository = DIContainer.Instance.GetService<IAuthorizationRepository>();

            return authRepository.HasAcces(userId, authGUID.GUID)
                ? new AuthResultModel(true, AuthStatusCode.Ok)
                : new AuthResultModel(false, AuthStatusCode.Forbidden);
        }

        public async Task<int> GetUserId(HttpRequest req)
        {
            AuthGUID authGUID = await GetGUIDAsync(req);
            if (!authGUID.Acces || authGUID.GUID == "")
                return 0;

            IAuthorizationRepository authRepository = DIContainer.Instance.GetService<IAuthorizationRepository>();
            return authRepository.GetUserId(authGUID.GUID, false);

        }

        public async Task<AuthResultModel> AuthForDoctor(HttpRequest req, int doctorId)
        {
            AuthGUID authGUID = await GetGUIDAsync(req);
            if (!authGUID.AuthResult.Result)
                return authGUID.AuthResult;

            IAuthorizationRepository authRepository = DIContainer.Instance.GetService<IAuthorizationRepository>();
            if(authRepository.UserAuth(doctorId, authGUID.GUID, true))
                return new AuthResultModel(true, AuthStatusCode.Ok);
            return new AuthResultModel(false, AuthStatusCode.Forbidden);
        }

        public async Task<AuthLogin> LoginAuthAsync(HttpRequest req)
        {
            AuthenticationHeaderValue.TryParse(req.Headers[HeaderNames.Authorization], out var authHeader);

            if (authHeader == null)
                return null;

            ClaimsPrincipal claims = await Auth0.ValidateTokenAsync(authHeader);

            Token token = new Token
            {
                Scheme = authHeader.Scheme,
                AccesCode = authHeader.Parameter
            };

            AuthLogin authLogin = new AuthLogin
            {   Token = token,
                Guid = claims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value
            };

            return authLogin;
        }

        // Helpers

        private async Task<AuthGUID> GetGUIDAsync(HttpRequest req)
        {
            AuthGUID authGUID = new AuthGUID();
            AuthResultModel authResult = new AuthResultModel(false, AuthStatusCode.Unauthorized);
            AuthenticationHeaderValue.TryParse(req.Headers[HeaderNames.Authorization], out var authHeader);

            if (authHeader == null)
                return new AuthGUID { AuthResult = authResult };

            ClaimsPrincipal claims = await Auth0.ValidateTokenAsync(authHeader);

            if (claims == null)
                return new AuthGUID { AuthResult = authResult };

            string tokenGuid = claims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            authGUID.Acces = true;
            authGUID.GUID = tokenGuid;
            authGUID.AuthResult = new AuthResultModel(true, AuthStatusCode.Ok);
            return authGUID;
        }
    }

    // Interface
    public interface IAuthorization
    {
        Task<AuthLogin> LoginAuthAsync(HttpRequest req);
        Task<AuthResultModel> CheckAuthorization(HttpRequest req, int userId = 0, bool isDoctor = false);
        Task<AuthResultModel> AuthForDoctorOrPatient(HttpRequest req, int userId);
        Task<int> GetUserId(HttpRequest req);
        Task<AuthResultModel> AuthForDoctor(HttpRequest req, int doctorId);
    }
}

