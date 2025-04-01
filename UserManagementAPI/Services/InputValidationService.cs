using System.Text.RegularExpressions;

namespace UserManagementAPI.Services
{
    public class InputValidationService
    {
        /// <summary>
        /// Validates and cleans a user's name.
        /// </summary>
        /// <param name="name">The name to validate and clean.</param>
        /// <returns>The cleaned name if valid; otherwise, null.</returns>
        public string? ValidateAndCleanName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null; // Invalid name
            }

            // Remove any leading/trailing whitespace and ensure the name is properly formatted
            name = name.Trim();

            // Check if the name contains only valid characters (letters, spaces, hyphens)
            if (!Regex.IsMatch(name, @"^[a-zA-Z\s\-]+$"))
            {
                return null; // Invalid name
            }

            return name;
        }

        /// <summary>
        /// Validates and cleans an email address.
        /// </summary>
        /// <param name="email">The email to validate and clean.</param>
        /// <returns>The cleaned email if valid; otherwise, null.</returns>
        public string? ValidateAndCleanEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null; // Invalid email
            }

            // Trim whitespace
            email = email.Trim();

            // Validate email format
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return null; // Invalid email
            }

            return email;
        }
    }
}