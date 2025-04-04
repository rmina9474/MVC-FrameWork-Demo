using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Claims;

namespace Reina.MacCredy.Utilities
{
    [HtmlTargetElement("*", Attributes = "require-permission")]
    [HtmlTargetElement("*", Attributes = "require-role")]
    [HtmlTargetElement("*", Attributes = "require-admin")]
    [HtmlTargetElement("*", Attributes = "hide-for-admin")]
    public class PermissionTagHelper : TagHelper
    {
        private readonly ClaimsPrincipal _user;

        public PermissionTagHelper(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext?.User;
        }

        [HtmlAttributeName("require-permission")]
        public string RequirePermission { get; set; }

        [HtmlAttributeName("require-role")]
        public string RequireRole { get; set; }

        [HtmlAttributeName("require-admin")]
        public bool RequireAdmin { get; set; }

        [HtmlAttributeName("hide-for-admin")]
        public bool HideForAdmin { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            bool shouldHide = false;

            // No user is logged in
            if (_user == null || !_user.Identity.IsAuthenticated)
            {
                // If any permission is required, hide the element
                if (!string.IsNullOrEmpty(RequirePermission) || 
                    !string.IsNullOrEmpty(RequireRole) || 
                    RequireAdmin)
                {
                    shouldHide = true;
                }
            }
            else
            {
                // Check role requirement
                if (!string.IsNullOrEmpty(RequireRole) && !_user.IsInRole(RequireRole))
                {
                    shouldHide = true;
                }

                // Check admin requirement
                if (RequireAdmin && !PermissionHelper.IsAdmin(_user))
                {
                    shouldHide = true;
                }

                // Check permission requirement
                if (!string.IsNullOrEmpty(RequirePermission))
                {
                    switch (RequirePermission.ToLower())
                    {
                        case "manageproducts":
                            shouldHide = !PermissionHelper.CanManageProducts(_user);
                            break;
                        case "managecategories":
                            shouldHide = !PermissionHelper.CanManageCategories(_user);
                            break;
                        case "vieworders":
                            shouldHide = !PermissionHelper.CanViewOrders(_user);
                            break;
                        case "manageallorders":
                            shouldHide = !PermissionHelper.CanManageAllOrders(_user);
                            break;
                        default:
                            shouldHide = true;
                            break;
                    }
                }

                // Hide for admin if user is admin
                if (HideForAdmin && PermissionHelper.IsAdmin(_user))
                {
                    shouldHide = true;
                }
            }

            if (shouldHide)
            {
                output.SuppressOutput();
            }
        }
    }
} 