using Waves.Api.Models.Launcher;

namespace Waves.Api.Models.Messanger;

public record GameLauncheCacheMessager(KRSDKLauncherCache cache,bool isVerify = false);
