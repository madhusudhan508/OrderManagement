/*============================================================================
   Namespace        : WebAPI
   Class            : AuthorizationFilter
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : check authetication and authorization
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/
namespace WebAPI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.Results;

    public class AuthorizationFilter : AuthorizeAttribute, IAuthenticationFilter
    {
        //public bool AllowMultiple
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
                
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage requestMessage = context.Request;
            AuthenticationHeaderValue authorization = requestMessage.Headers.Authorization;
            IEnumerable<string> header;
            requestMessage.Headers.TryGetValues("Authorization", out header);
            
            if (header == null)
            {
                context.ErrorResult = new UnAuthorizeResult("Request header missing.", requestMessage);
                return;
            }
            else if (header.FirstOrDefault() == null)
            {
                context.ErrorResult = new UnAuthorizeResult("Invalid token.", requestMessage);
                return;
            }
            else
            {
                var auth = header.FirstOrDefault();
                string[] tokenParam = auth.Split(':');
                string token, tokenUserName;
                try
                {
                    token = tokenParam[0];
                    tokenUserName = tokenParam[1];
                    string validUserName = TokenGenerator.validateToken(token);
                    
                    if (tokenUserName != validUserName)
                    {
                        context.ErrorResult = new UnAuthorizeResult("Invalid user.", requestMessage);
                        return;
                    }
                }
                catch
                {
                    context.ErrorResult = new UnAuthorizeResult("Invalid user.", requestMessage);
                    return;
                }

                context.Principal = TokenGenerator.GetPrincipal(token);
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "realn:localhost"));
            }
            context.Result = new ResponseMessageResult(result);
        }

        /// <summary>
        /// Checking role 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsIdentity claimsIdentity;
            var httpContext = HttpContext.Current;
            if (!(httpContext.User.Identity is ClaimsIdentity))
            {
                return false;
            }

            claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            var Role = claimsIdentity.Claims.AsEnumerable().FirstOrDefault(e => e.Type == ClaimTypes.Role);
            if (Role == null)
            {
                // just extra defense
                return false;
            }
            var userRole = Role.Value;

            // use your desired logic on 'userRole' and `userLocId', maybe Contains if I get your example right?
            if (!base.Roles.Contains(userRole))
            {
                return false;
            }

            //Continue with the regular Authorize check
            return base.IsAuthorized(actionContext);
        }
    }

    public class UnAuthorizeResult : IHttpActionResult
    {
        public string _httpReasonPhrase { get; set; }
        public HttpRequestMessage _httpRequestMessage { get; set; }

        public UnAuthorizeResult(string httpReasonPhrase, HttpRequestMessage httpRequestMessage)
        {
            this._httpReasonPhrase = httpReasonPhrase;
            this._httpRequestMessage = httpRequestMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            responseMessage.RequestMessage = this._httpRequestMessage;
            responseMessage.ReasonPhrase = this._httpReasonPhrase;
            return responseMessage;
        }
    }
}