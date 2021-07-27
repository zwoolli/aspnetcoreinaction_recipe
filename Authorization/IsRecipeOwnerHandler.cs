using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using RecipeApp.Data;

namespace RecipeApp.Authorization
{
    public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, Recipe>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IsRecipeOwnerHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRecipeOwnerRequirement requirement, Recipe resource)
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(context.User);

            if (appUser == null)
            {
                return;
            }
            if (resource.User_Id == appUser.User_Id)
            {
                context.Succeed(requirement);
            }

        }
    }
}