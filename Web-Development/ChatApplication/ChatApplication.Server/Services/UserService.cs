namespace ChatApplication.Server.Services;

public class UserService
{
    private readonly HashSet<string> _nicknames = new();

    public bool RegisterUser(string nickname)
    {
        if (_nicknames.Contains(nickname))
            return false;

        _nicknames.Add(nickname);
        return true;
    }

    public void UnregisterUser(string nickname)
    {
        _nicknames.Remove(nickname);
    }
    public bool IsNicknameTaken(string nickname)
    {
        return _nicknames.Contains(nickname);
    }
}