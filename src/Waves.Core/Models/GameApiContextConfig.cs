namespace Waves.Core.Models
{
    public class GameAPIConfig
    {
        public static string[] BaseAddress =
        [
            "https://prod-cn-alicdn-gamestarter.kurogame.com",
            "https://prod-alicdn-gamestarter.kurogame.com",
            "https://prod-volcdn-gamestarter.kurogame.com",
            "https://prod-tencentcdn-gamestarter.kurogame.com",
        ];

        #region BaseData

        public string GameID { get; set; }

        public string AppId { get; set; }

        public string AppKey { get; set; }

        public string GameIdentity { get; set; }

        public string GameExeName { get; set; }

        public string ConfigUrl { get; set; }

        public string LauncherConfigUrl { get; set; }

        public string Language { get; set; }
        #endregion

        public static GameAPIConfig MainAPiConfig =>
            new GameAPIConfig()
            {
                AppId = "10003",
                GameID = "G152",
                AppKey = "Y8xXrXk65DqFHEDgApn3cpK5lfczpFx5",
                GameIdentity = "Aki",
                GameExeName = "Wuthering Waves.exe",
                ConfigUrl =
                    "https://prod-volcdn-gamestarter.kurogame.xyz/launcher/game/G152/10003_Y8xXrXk65DqFHEDgApn3cpK5lfczpFx5/index.json",
                LauncherConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/launcher/10003_Y8xXrXk65DqFHEDgApn3cpK5lfczpFx5/G152/index.json",
                Language = "zh-Hans",
            };

        public static GameAPIConfig GlobalConfig =>
            new GameAPIConfig()
            {
                AppId = "50004",
                GameID = "G153",
                AppKey = "obOHXFrFanqsaIEOmuKroCcbZkQRBC7c",
                GameIdentity = "Aki",
                GameExeName = "Wuthering Waves.exe",
                ConfigUrl =
                    "https://prod-alicdn-gamestarter.kurogame.com/launcher/game/G153/50004_obOHXFrFanqsaIEOmuKroCcbZkQRBC7c/index.json",
                LauncherConfigUrl =
                    "https://prod-alicdn-gamestarter.kurogame.com/launcher/launcher/50004_obOHXFrFanqsaIEOmuKroCcbZkQRBC7c/G153/index.json",
                Language = "zh-Hant",
            };

        public static GameAPIConfig BilibiliConfig =>
            new GameAPIConfig()
            {
                AppId = "10004",
                GameID = "G152",
                AppKey = "j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y",
                GameIdentity = "Aki",
                GameExeName = "Wuthering Waves.exe",
                ConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/game/G152/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/index.json",
                LauncherConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/launcher/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/G152/index.json",
                Language = "zh-Hans",
            };
    }

    #region 旧API

    //public class GameApiContextConfig
    //{
    //    public string Starter_Source { get; set; }

    //    public string Launcher_Source { get; set; }

    //    public string LauncherHeader_Source { get; set; }

    //    public static GameApiContextConfig Main =>
    //        new()
    //        {
    //            Starter_Source =
    //                "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/10003_Y8xXrXk65DqFHEDgApn3cpK5lfczpFx5/G152/information/zh-Hans.json",
    //            Launcher_Source =
    //                "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/game/G152/10003_Y8xXrXk65DqFHEDgApn3cpK5lfczpFx5/index.json",
    //            LauncherHeader_Source =
    //                "https://prod-cn-alicdn-gamestarter.kurogame.com/pcstarter/prod/game/G152/10003_Y8xXrXk65DqFHEDgApn3cpK5lfczpFx5/social/zh-Hans.json",
    //        };

    //    public static GameApiContextConfig BiliBili =>
    //        new()
    //        {
    //            Starter_Source =
    //                "https://prod-cn-alicdn-gamestarter.kurogame.com/pcstarter/prod/game/G152/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/index.json",
    //            Launcher_Source =
    //                "https://prod-cn-alicdn-gamestarter.kurogame.com/pcstarter/prod/starter/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/G152/index.json",
    //            LauncherHeader_Source =
    //                "https://prod-cn-alicdn-gamestarter.kurogame.com/pcstarter/prod/game/G152/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/social/zh-Hans.json",
    //        };

    //    public static GameApiContextConfig Global =>
    //        new()
    //        {
    //            Starter_Source =
    //                "https://prod-alicdn-gamestarter.kurogame.com/pcstarter/prod/game/G153/50004_obOHXFrFanqsaIEOmuKroCcbZkQRBC7c/index.json",
    //            Launcher_Source =
    //                "https://prod-alicdn-gamestarter.kurogame.com/pcstarter/prod/starter/50004_obOHXFrFanqsaIEOmuKroCcbZkQRBC7c/G153/index.json",
    //            LauncherHeader_Source =
    //                "https://prod-alicdn-gamestarter.kurogame.com/pcstarter/prod/game/G153/50004_obOHXFrFanqsaIEOmuKroCcbZkQRBC7c/social/zh-Hant.json",
    //        };
    //}
    #endregion
}
