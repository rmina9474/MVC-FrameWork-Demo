using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Utilities
{
    public static class PermissionHelper
    {
        /// <summary>
        /// Check if user has a specific role
        /// </summary>
        public static bool HasRole(ClaimsPrincipal user, string role)
        {
            return user.IsInRole(role);
        }

        /// <summary>
        /// Check if user has admin role
        /// </summary>
        public static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }

        /// <summary>
        /// Check if user has permission to view products management
        /// </summary>
        public static bool CanManageProducts(ClaimsPrincipal user)
        {
            return IsAdmin(user);
        }

        /// <summary>
        /// Check if user has permission to view categories management
        /// </summary>
        public static bool CanManageCategories(ClaimsPrincipal user)
        {
            return IsAdmin(user);
        }

        /// <summary>
        /// Check if user has permission to view orders
        /// </summary>
        public static bool CanViewOrders(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Check if user has permission to manage all orders
        /// </summary>
        public static bool CanManageAllOrders(ClaimsPrincipal user)
        {
            return IsAdmin(user);
        }

        /// <summary>
        /// Check if user has permission to edit a specific resource
        /// </summary>
        public static bool CanEditResource<T>(ClaimsPrincipal user, T resource, string userId = null) where T : class
        {
            // Admin can edit everything
            if (IsAdmin(user))
                return true;
            
            // If there's a userId parameter and it matches the current user
            if (!string.IsNullOrEmpty(userId) && user.FindFirstValue(ClaimTypes.NameIdentifier) == userId)
                return true;
                
            return false;
        }
    }
} 