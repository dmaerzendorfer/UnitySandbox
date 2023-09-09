public abstract class GameSettings<T> where T : GameSettings<T>, new()
{
    private static T _instance = new T();

    public static T Instance
    {
        get { return _instance; }
    }

    public GameSettings()
    {
        LoadSettings();
    }

    ~GameSettings()
    {
        SaveSettings();
    }

    public abstract void SaveSettings();
    public abstract void LoadSettings();
}