using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using MvcLinkedInApplication.LinkedIn;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace MvcLinkedInApplication.Controllers
{
    [HandleError]
    public class LinkedInController : Controller
    {
        public ActionResult Index()
        {
            return AuthenticateToLinkedIn();
        }

        static string token_secret = "";
        public ActionResult AuthenticateToLinkedIn()
        {
            var credentials = new OAuthCredentials
            {
                CallbackUrl = "http://localhost:3492/LinkedIn/Callback",
                ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"],
                Verifier = "123456",
                Type = OAuthType.RequestToken
            };

            var client = new RestClient { Authority = "https://www.linkedin.com/uas/oauth", Credentials = credentials };
            var request = new RestRequest { Path = "requestToken?scope=r_basicprofile,r_emailaddress,rw_company_admin,w_share" };
            RestResponse response = client.Request(request);

            token = response.Content.Split('&')[0].Split('=')[1];
            token_secret = response.Content.Split('&')[1].Split('=')[1];
            Response.Redirect("https://www.linkedin.com/uas/oauth/authorize?oauth_token=" + token);
            return null;
        }

        string token = "";
        string verifier = "";
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Callback()
        {
            token = Request["oauth_token"];
            verifier = Request["oauth_verifier"];
            var credentials = new OAuthCredentials
            {
                ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"],
                Token = token,
                TokenSecret = token_secret,
                Verifier = verifier,
                Type = OAuthType.AccessToken,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Version = "1.0"
            };

            var client = new RestClient { Authority = "https://www.linkedin.com/uas/oauth2/accessToken", Credentials = credentials, Method = WebMethod.Post };
            var request = new RestRequest { Path = "/accessToken" };
            RestResponse response = client.Request(request);
            string content = response.Content;

            string accessToken = response.Content.Split('&')[0].Split('=')[1];
            string accessTokenSecret = response.Content.Split('&')[1].Split('=')[1];

            //Some commented call to API
            //var company = new LinkedInService(accessToken, accessTokenSecret).GetCompany(162479);
            //var company = new LinkedInService(accessToken, accessTokenSecret).GetCompanyByUniversalName("linkedin");
            //var companies = new LinkedInService(accessToken, accessTokenSecret).GetCompaniesByEmailDomain("apple.com");            
            //var companies1 = new LinkedInService(accessToken, accessTokenSecret).GetCompaniesByEmailDomain("linkedin.com");           
            //var companies2 = new LinkedInService(accessToken, accessTokenSecret).GetCompaniesByIdAnduniversalName("162479", "linkedin");
            //var people = new LinkedInService(accessToken, accessTokenSecret).GetPersonById("f7cp5sKscd");
            //var people = new LinkedInService(accessToken, accessTokenSecret).GetCurrentUser();

            //string url = Url.Encode("http://bd.linkedin.com/pub/rakibul-islam/37/522/653");
            //var people = new LinkedInService(accessToken, accessTokenSecret).GetPeoPleByPublicProfileUrl(url);
            //var peopleSearchresult = new LinkedInService(accessToken, accessTokenSecret).SearchPeopleByKeyWord("Princes");

            var peopleSearchResult = new LinkedInService(accessToken, accessTokenSecret).GetPeopleByFirstName("Dawid");
            String peopleList = peopleSearchResult.People.Persons.ToString();
            return View(Content(peopleList));
        }
    }
}