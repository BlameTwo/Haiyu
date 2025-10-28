﻿namespace Waves.Core.Models
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

        public string? PKGId { get; set;  }
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
                PKGId = "A1381"
            };

        public static GameAPIConfig GlobalConfig =>
            new GameAPIConfig()
            {
                AppId = "50004",
                GameID = "G153",
                AppKey = "obOHXFrFanqsaIEOmuKroCcbZkQRBC7c",
                GameIdentity = "Aki",
                PKGId = "A1730",
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
                PKGId = "A1421",
                GameExeName = "Wuthering Waves.exe",
                ConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/game/G152/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/index.json",
                LauncherConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/launcher/10004_j5GWFuUFlb8N31Wi2uS3ZAVHcb7ZGN7y/G152/index.json",
                Language = "zh-Hans",
            };

        #region 战双

        /// <summary>
        /// 战双官服
        /// </summary>
        public static GameAPIConfig MainBGRConfig =>
            new GameAPIConfig()
            {
                AppId = "10012",
                GameID = "G148",
                AppKey = "RnIUKs3r59Csliu3N0rl5uRWWBOFDaJL",
                GameIdentity = "Haru",
                GameExeName = "PGR.exe",
                ConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/game/G148/10012_RnIUKs3r59Csliu3N0rl5uRWWBOFDaJL/index.json",
                LauncherConfigUrl =
                    "https://prod-volcdn-gamestarter.kurogame.xyz/launcher/launcher/10012_RnIUKs3r59Csliu3N0rl5uRWWBOFDaJL/G148/index.json",
                Language = "zh-Hans",
            };

        /// <summary>
        /// 战双B服
        /// </summary>
        public static GameAPIConfig BiliBiliBGRConfig =>
            new GameAPIConfig()
            {
                AppId = "10011",
                GameID = "G148",
                AppKey = "qYQv6TyyyhCKD3ox3gssyolNPwMoCPZt",
                GameIdentity = "Haru",
                GameExeName = "PGR.exe",
                ConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/game/G148/10011_qYQv6TyyyhCKD3ox3gssyolNPwMoCPZt/index.json",
                LauncherConfigUrl =
                    "https://prod-cn-alicdn-gamestarter.kurogame.com/launcher/launcher/10011_qYQv6TyyyhCKD3ox3gssyolNPwMoCPZt/G148/index.json",
                Language = "zh-Hans",
            };

        /// <summary>
        /// 战双国际服
        /// </summary>
        public static GameAPIConfig GlobalBGRConfig =>
            new GameAPIConfig()
            {
                AppId = "50015",
                GameID = "G143",
                AppKey = "LWdk9D2Ep9mpJmqBZZkcPBU2YNraEWBQ",
                GameIdentity = "Haru",
                GameExeName = "PGR.exe",
                PKGId=null,
                ConfigUrl =
                    "https://prod-alicdn-gamestarter.kurogame.com/launcher/game/G143/50015_LWdk9D2Ep9mpJmqBZZkcPBU2YNraEWBQ/index.json",
                LauncherConfigUrl =
                    "https://prod-alicdn-gamestarter.kurogame.com/launcher/launcher/50015_LWdk9D2Ep9mpJmqBZZkcPBU2YNraEWBQ/G143/index.json",
                Language = "en",
            };

        /// <summary>
        /// 战双台服
        /// </summary>
        public static GameAPIConfig TWBGRConfig
            => new GameAPIConfig()
            {
                AppId = "50016",
                GameID = "G279",
                AppKey = "i2n5NLmdCAmOGP3J1tJOlWKNSMQuyWL7",
                GameIdentity = "Haru",
                GameExeName = "PGR.exe",
                ConfigUrl =
                    "https://prod-alicdn-gamestarter.kurogame.com/launcher/game/G279/50016_i2n5NLmdCAmOGP3J1tJOlWKNSMQuyWL7/index.json",
                LauncherConfigUrl =
                    "https://prod-alicdn-gamestarter.kurogame.com/launcher/launcher/50016_i2n5NLmdCAmOGP3J1tJOlWKNSMQuyWL7/G279/index.json",
                Language = "zh-Hant",
            };
        #endregion
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
