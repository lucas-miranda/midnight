namespace Midnight;

public static class Dirs {
    private static ReadOnlyPathBuf _base, _cache, _config, _data, _user;

    /// <summary>
    /// Game base directory.
    /// </summary>
    public static ReadOnlyPathBuf Base {
        get {
            if (_base != null) {
                return _base;
            }

            // it's the same code as Microsoft.Xna.Framework.SDL2_FNAPlatform.GetBaseDirectory()
            // with some minor adaptations
            string result;

            try {
                string platform = SDL2.SDL.SDL_GetPlatform();

                if (System.Environment.GetEnvironmentVariable("FNA_SDL2_FORCE_BASE_PATH") != "1") {
                    if (platform.Equals("Windows")
                      || platform.Equals("Mac OS X")
                      || platform.Equals("Linux")
                      || platform.Equals("FreeBSD")
                      || platform.Equals("OpenBSD")
                      || platform.Equals("NetBSD")
                      || platform.Equals("Unknown")
                    ) {
                        _base = System.AppDomain.CurrentDomain.BaseDirectory;
                        return _base;
                    }
                }

                result = SDL2.SDL.SDL_GetBasePath();

                if (string.IsNullOrEmpty(result)) {
                    result = System.AppDomain.CurrentDomain.BaseDirectory;
                }

                if (string.IsNullOrEmpty(result)) {
                    result = System.Environment.CurrentDirectory;
                }

                _base = result;
            } catch (System.DllNotFoundException) {
                // SDL2 isn't available yet
                result = System.AppDomain.CurrentDomain.BaseDirectory;
            }

            return result;
        }
    }

    /// <summary>
    /// Temporary files directory.
    /// </summary>
    public static ReadOnlyPathBuf Cache {
        get {
            if (_cache == null) {
                switch (SDL2.SDL.SDL_GetPlatform()) {
                    case "Linux":
                        _cache = User.WithSections(".cache");
                        break;
                    case "Mac OS X":
                        _cache = User.WithSections("Library", "Caches");
                        break;
                    default:
                        _cache = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                        break;
                }
            }

            return _cache;
        }
    }

    /// <summary>
    /// Persistent config directory.
    /// </summary>
    public static ReadOnlyPathBuf Config {
        get {
            if (_config == null) {
                _config = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            }

            return _config;
        }
    }

    public static ReadOnlyPathBuf Data {
        get {
            if (_data == null) {
                _data = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            }

            return _data;
        }
    }

    public static ReadOnlyPathBuf User {
        get {
            if (_user == null) {
                _user = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            }

            return _user;
        }
    }
}
