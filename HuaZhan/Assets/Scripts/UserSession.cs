public static class UserSession
{
    public static string CurrentUser { get; private set; } = "Guest";

    public static void SetCurrentUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            CurrentUser = "Guest";
            return;
        }

        CurrentUser = username.Trim();
    }
}
