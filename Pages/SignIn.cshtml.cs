using System.Security.AccessControl;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{

    [AllowAnonymous]
    public class SignInModel : PageModel
    {
        private readonly IConfiguration Configuration;
        private TelemetryClient _telemetry;

        public SignInModel(IConfiguration configuration, TelemetryClient telemetry)
        {
            Configuration = configuration;
            _telemetry = telemetry;
        }

        private IActionResult TrackAndAuth(string ID,
            string redirectUri = "/",
            bool reauth = false, string?
            extraParam = null, string?
            extraParamValue = null,
            string? ui_locales = null,
            string? login_hint = null)
        {
            _telemetry.TrackPageView($"Sign-in:{ID}");

            ChallengeResult challengeResult = new ChallengeResult(
                OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                });

            // Force re-authentication
            if (this.Request.Query["force"].Count > 0 || reauth)
            {
                challengeResult.Properties.Items.Add("force", "true");
            }

            // Step up flow
            if (ID == "StepUp" || ID == "EditProfileMfa" || ID == "MFA")
            {
                challengeResult.Properties.Items.Add("StepUp", "true");
            }


            // ui_locales
            if (ID == "PreSelectLanguage")
            {
                challengeResult.Properties.Items.Add("ui_locales", ui_locales);
            }

            // Propmt create
            if (ID == "SignUpLink")
            {
                challengeResult.Properties.Items.Add("prompt", "create");
            }

            // login_hint
            if (!string.IsNullOrEmpty(login_hint))
            {
                challengeResult.Properties.Items.Add("login_hint", login_hint);
            }

            // Extra parameter
            if (!string.IsNullOrEmpty(extraParam) && !string.IsNullOrEmpty(extraParamValue))
            {
                challengeResult.Properties.Items.Add(extraParam, extraParamValue);
            }

            return challengeResult;
        }


        public IActionResult OnGetDefault()
        {
            return this.TrackAndAuth("Default");
        }

        public IActionResult OnGetModifyAttributeValues()
        {
            return this.TrackAndAuth("ModifyAttributeValues", "/", true);
        }

        public IActionResult OnGetBlockSignUp()
        {
            return this.TrackAndAuth("BlockSignUp", "/", true);
        }
        public IActionResult OnGetCSA()
        {

            _telemetry.TrackPageView($"Sign-in:CSA");

            return Redirect(this.Configuration.GetSection("Demos:CustomSecurityAttributesURL").Value);
        }

        public IActionResult OnGetStepUp()
        {
            return this.TrackAndAuth("StepUp", "/#cmd=StepUpCompleted", false);
        }

        public IActionResult OnGetStepUpIntro()
        {
            return this.TrackAndAuth("StepUp-Start", "/#usecase=StepUp", true);
        }

        public IActionResult OnGetPolicyAgreement()
        {
            return this.TrackAndAuth("PolicyAgreement", "/", true);
        }

        public IActionResult OnGetMfa()
        {
            return this.TrackAndAuth("MFA", "/", true);
        }

        public IActionResult OnGetEditProfileMfa()
        {
            return this.TrackAndAuth("EditProfileMfa", "/profile", false);
        }

        public IActionResult OnGetUserLastActivity()
        {
            return this.TrackAndAuth("UserLastActivity", "/profile", true);
        }

        public IActionResult OnGetCa()
        {
            return this.TrackAndAuth("CA", "/", true);
        }

        public IActionResult OnGetObo()
        {
            return this.TrackAndAuth("OBO", "/token", true);
        }

        public IActionResult OnGetOnlineRetail()
        {
            return this.TrackAndAuth("OnlineRetail", "/", true);
        }

        public IActionResult OnGetCompanyBranding()
        {
            return this.TrackAndAuth("CompanyBranding", "/", true);
        }

        public IActionResult OnGetLanguage()
        {
            return this.TrackAndAuth("Language", "/", true);
        }
        public IActionResult OnGetPreSelectLanguage()
        {
            return this.TrackAndAuth("PreSelectLanguage", "/", true, null, null, this.Request.Query["ui_locales"]);
        }
        public IActionResult OnGetProfileSignin(string? id)
        {
            return this.TrackAndAuth("ProfileSignin", "/", false, null, null, null, id);
        }
        public IActionResult OnGetLoginHint(string id)
        {
            return this.TrackAndAuth("LoginHint", "/", true, null, null, null, id);
        }

        public IActionResult OnGetSignUpLink(string id)
        {
            return this.TrackAndAuth("SignUpLink", "/", false, null, null, null, id);
        }

        public IActionResult OnGetTokenSignin()
        {
            return this.TrackAndAuth("TokenSignin", "/token");
        }

        public IActionResult OnGetEmailAndPassword()
        {
            return this.TrackAndAuth("EmailAndPassword", "/", true);
        }

        public IActionResult OnGetSocial()
        {
            return this.TrackAndAuth("Social", "/", true);
        }

        public IActionResult OnGetTokenTTL()
        {
            return this.TrackAndAuth("TokenTTL", "/", true);
        }

        public IActionResult OnGetSSO1()
        {
            return this.TrackAndAuth("SSO-Start");
        }

        public IActionResult OnGetForceSignIn()
        {
            return this.TrackAndAuth("ForceSignIn", "/", true);
        }

        public IActionResult OnGetInvalidSession()
        {
            return this.TrackAndAuth("ForceSignIn", "/", true);
        }

        public IActionResult OnGetDisableAccount()
        {
            return this.TrackAndAuth("DisableAccount", "/Profile", true);
        }

        public IActionResult OnGetSSO2()
        {

            _telemetry.TrackPageView($"Sign-in:SSO-Continue");

            return Redirect(this.Configuration.GetSection("Demos:WoodgroveBankURL").Value + "/Auth/Login");
        }

        public IActionResult OnGetAssignmentRequired()
        {
            _telemetry.TrackPageView($"Sign-in:AssignmentRequired");

            return Redirect(this.Configuration.GetSection("Demos:AssignmentRequiredURL").Value);
        }

        public IActionResult OnGetTokenAugmentation()
        {
            return this.TrackAndAuth("TokenAugmentation", "/", true);
        }

        public IActionResult OnGetTokenClaims()
        {
            return this.TrackAndAuth("TokenClaims", "/", true);
        }

        public IActionResult OnGetPreAttributeCollection()
        {
            return this.TrackAndAuth("PreAttributeCollection", "/", true);
        }

        public IActionResult OnGetPostAttributeCollection()
        {
            return this.TrackAndAuth("PostAttributeCollection", "/", true);
        }

        public IActionResult OnGetRBAC()
        {
            return this.TrackAndAuth("RBAC", "/", true);
        }

        public IActionResult OnGetGBAC()
        {
            return this.TrackAndAuth("GBAC", "/", true);
        }

        public IActionResult OnGetCustomDomain()
        {
            return this.TrackAndAuth("CustomDomain", "/", true, "domain", this.Configuration.GetSection("Demos:CustomDomain").Value);
        }

        public IActionResult OnGetSSPR()
        {
            return this.TrackAndAuth("SSPR", "/", true);
        }
        public IActionResult OnGetCustomAttributes()
        {
            return this.TrackAndAuth("CustomAttributes", "/", true);
        }

        public IActionResult OnGetKiosk()
        {
            _telemetry.TrackPageView($"Sign-in:Kiosk");

            return Redirect("https://woodgroverestaurants.com");
        }

        public IActionResult OnGetFinance()
        {
            _telemetry.TrackPageView($"Sign-in:Kiosk");

            return Redirect("https://woodgrovebanking.com/");
        }
    }
}
