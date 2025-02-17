namespace Midnight;

public static class ProjectDirs {
    public static string Qualifier { get; private set; }
    public static string Organization { get; private set; }
    public static string Application { get; private set; }
    public static ReadOnlyPathBuf Identifier { get; private set; }
    public static ReadOnlyPathBuf Cache { get; private set; }
    public static ReadOnlyPathBuf Config { get; private set; }
    public static ReadOnlyPathBuf Data { get; private set; }

    internal static void Initialize() {
        System.IO.Directory.CreateDirectory(Cache.ToString());
        System.IO.Directory.CreateDirectory(Config.ToString());
        System.IO.Directory.CreateDirectory(Data.ToString());
    }

    internal static void Set(string qualifier, string organization, string application) {
        if (string.IsNullOrEmpty(qualifier)) {
            throw new System.ArgumentException(nameof(qualifier));
        }

        if (string.IsNullOrEmpty(organization)) {
            throw new System.ArgumentException(nameof(organization));
        }

        if (string.IsNullOrEmpty(application)) {
            throw new System.ArgumentException(nameof(application));
        }

        Qualifier = qualifier;
        Organization = organization;
        Application = application;

        switch (SDL2.SDL.SDL_GetPlatform()) {
            case "Windows":
                Identifier = new PathBuf(Organization, Application);
                Cache = Dirs.Cache + Identifier + "cache";
                Config = Dirs.Config + Identifier + "config";
                Data = Dirs.Data + Identifier + "data";
                break;

            case "Mac OS X":
                Identifier = $"{Qualifier.Replace(' ', '-')}.{Organization.Replace(' ', '-')}.{Application.Replace(' ', '-')}";
                Cache = Dirs.Cache + Identifier;
                Config = Dirs.Config + Identifier;
                Data = Dirs.Data + Identifier;
                break;

            default:
                Identifier = Application.ToLower().Replace(" ", "");
                Cache = Dirs.Cache + Identifier;
                Config = Dirs.Config + Identifier;
                Data = Dirs.Data + Identifier;
                break;
        }
    }
}
