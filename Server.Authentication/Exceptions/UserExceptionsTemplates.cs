namespace Server.Authentication.Exceptions
{
    public static class UserExceptionsTemplates
    {
        public const string UserCoolDown = "User [{0}] has been cooled down till [{1}].";
        public const string UserInvalidCredentials = "Invalid Crenedtials for user [{0}].";
        public const string UserNotFound = "User with login [{0}] is not found.";
        public const string UserAlreadyExists = "User with login [{0}] already exists.";
        public const string UnknownError = "Unknown error occured. Please try again.";
    }
}