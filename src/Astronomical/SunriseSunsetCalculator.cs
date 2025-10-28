using System;

namespace Astronomical;

using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class SunriseSunsetCalculator
{
    #region 公共方法
    public static double GetDayLength(DateTime date, double longitude, double latitude)
    {
        double result = DayLen(
            date.Year,
            date.Month,
            date.Day,
            longitude,
            latitude,
            -35.0 / 60.0,
            1
        );
        return result;
    }

    public static SunTimeResult GetSunTime(DateTime date, double longitude, double latitude)
    {
        double start = 0;
        double end = 0;
        SunRiset(
            date.Year,
            date.Month,
            date.Day,
            longitude,
            latitude,
            -35.0 / 60.0,
            1,
            ref start,
            ref end
        );
        DateTime sunrise = ToLocalTime(date, start);
        DateTime sunset = ToLocalTime(date, end);
        return new SunTimeResult(sunrise, sunset);
    }
    #endregion

    #region 私有方法

    #region 时间转换
    private static DateTime ToLocalTime(DateTime time, double utTime)
    {
        TimeZoneInfo localZone = TimeZoneInfo.Local;

        TimeSpan offset = localZone.GetUtcOffset(time.Date);
        double timeZoneOffsetHours = offset.TotalHours;
        int hour = Convert.ToInt32(Math.Floor(utTime));
        double temp = utTime - hour;
        hour += (int)timeZoneOffsetHours;
        temp = temp * 60;
        int minute = Convert.ToInt32(Math.Floor(temp));
        try
        {
            return new DateTime(time.Year, time.Month, time.Day, hour, minute, 0);
        }
        catch
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
        }
    }
    #endregion

    #region 与日出日落时间相关计算
    private static double DayLen(
        int year,
        int month,
        int day,
        double lon,
        double lat,
        double altit,
        int upper_limb
    )
    {
        double d, 
            obl_ecl,
            sr,
            slon,
            sin_sdecl,
            cos_sdecl,
            sradius, 
            t;

        d = Days_since_2000_Jan_0(year, month, day) + 0.5 - lon / 360.0;

        obl_ecl = 23.4393 - 3.563E-7 * d;
        slon = 0.0;
        sr = 0.0;
        Sunpos(d, ref slon, ref sr);

        sin_sdecl = Sind(obl_ecl) * Sind(slon);
        cos_sdecl = Math.Sqrt(1.0 - sin_sdecl * sin_sdecl);

        sradius = 0.2666 / sr;

        if (upper_limb != 0)
            altit -= sradius;

        double cost;
        cost = (Sind(altit) - Sind(lat) * sin_sdecl) / (Cosd(lat) * cos_sdecl);
        if (cost >= 1.0)
            t = 0.0;
        else if (cost <= -1.0)
            t = 24.0;
        else
            t = (2.0 / 15.0) * Acosd(cost); /* The diurnal arc, hours */
        return t;
    }

    private static void Sunpos(double d, ref double lon, ref double r)
    {
        double M, 
            w, 
            e, 
            E,
            x,
            y,
            v; 

        M = Revolution(356.0470 + 0.9856002585 * d);
        w = 282.9404 + 4.70935E-5 * d;

        e = 0.016709 - 1.151E-9 * d;

        E = M + e * Radge * Sind(M) * (1.0 + e * Cosd(M));
        x = Cosd(E) - e;
        y = Math.Sqrt(1.0 - e * e) * Sind(E);
        r = Math.Sqrt(x * x + y * y);
        v = Atan2d(y, x);
        lon = v + w;
        if (lon >= 360.0)
            lon -= 360.0;
    }

    private static void Sun_RA_dec(double d, ref double RA, ref double dec, ref double r)
    {
        double lon,
            obl_ecl,
            x,
            y,
            z;
        lon = 0.0;

        Sunpos(d, ref lon, ref r);

        x = r * Cosd(lon);
        y = r * Sind(lon);

        obl_ecl = 23.4393 - 3.563E-7 * d;

        z = y * Sind(obl_ecl);
        y = y * Cosd(obl_ecl);

        RA = Atan2d(y, x);
        dec = Atan2d(z, Math.Sqrt(x * x + y * y));
    }

    /// <summary>
    /// 日出没时刻计算
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="lon"></param>
    /// <param name="lat"></param>
    /// <param name="altit"></param>
    /// <param name="upper_limb"></param>
    /// <param name="trise">日出时刻</param>
    /// <param name="tset">日没时刻</param>
    /// <returns>太阳有出没现象，返回0 极昼，返回+1 极夜，返回-1</returns>
    private static int SunRiset(
        int year,
        int month,
        int day,
        double lon,
        double lat,
        double altit,
        int upper_limb,
        ref double trise,
        ref double tset
    )
    {
        double d,

            sr,

            sRA,

            sdec,

            sradius,

            t,

            tsouth,
            sidtime;

        int rc = 0;
        d = Days_since_2000_Jan_0(year, month, day) + 0.5 - lon / 360.0;
        sidtime = Revolution(GMST0(d) + 180.0 + lon);
        sRA = 0.0;
        sdec = 0.0;
        sr = 0.0;
        Sun_RA_dec(d, ref sRA, ref sdec, ref sr);
        tsouth = 12.0 - Rev180(sidtime - sRA) / 15.0;
        sradius = 0.2666 / sr;
        if (upper_limb != 0)
            altit -= sradius;
        double cost;
        cost = (Sind(altit) - Sind(lat) * Sind(sdec)) / (Cosd(lat) * Cosd(sdec));
        if (cost >= 1.0)
        {
            rc = -1;
            t = 0.0;
        }
        else
        {
            if (cost <= -1.0)
            {
                rc = +1;
                t = 12.0;
            }
            else
                t = Acosd(cost) / 15.0;
        }

        trise = tsouth - t;
        tset = tsouth + t;

        return rc;
    }
    #endregion

    #region 辅助函数
    /// <summary>
    /// 历元2000.0，即以2000年第一天开端为计日起始（天文学以第一天为0日而非1日）。
    /// 它与UT（就是世界时，格林尼治平均太阳时）1999年末重合。
    /// </summary>
    /// <param name="y"></param>
    /// <param name="m"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    private static long Days_since_2000_Jan_0(int y, int m, int d)
    {
        return (
            367L * (y) - ((7 * ((y) + (((m) + 9) / 12))) / 4) + ((275 * (m)) / 9) + (d) - 730530L
        );
    }

    private static double Revolution(double x)
    {
        return (x - 360.0 * Math.Floor(x * Inv360));
    }

    private static double Rev180(double x)
    {
        return (x - 360.0 * Math.Floor(x * Inv360 + 0.5));
    }

    private static double GMST0(double d)
    {
        double sidtim0;
        sidtim0 = Revolution((180.0 + 356.0470 + 282.9404) + (0.9856002585 + 4.70935E-5) * d);
        return sidtim0;
    }

    private static double Inv360 = 1.0 / 360.0;
    #endregion

    #region 度与弧度转换系数，为球面三角计算作准备
    private static double Sind(double x)
    {
        return Math.Sin(x * Degrad);
    }

    private static double Cosd(double x)
    {
        return Math.Cos(x * Degrad);
    }

    private static double Tand(double x)
    {
        return Math.Tan(x * Degrad);
    }

    private static double Atand(double x)
    {
        return Radge * Math.Atan(x);
    }

    private static double Asind(double x)
    {
        return Radge * Math.Asin(x);
    }

    private static double Acosd(double x)
    {
        return Radge * Math.Acos(x);
    }

    private static double Atan2d(double y, double x)
    {
        return Radge * Math.Atan2(y, x);
    }

    private static double Radge = 180.0 / Math.PI;
    private static double Degrad = Math.PI / 180.0;

    #endregion

    #endregion
}

/// <summary>
/// 日出日落时间结果
/// </summary>
public class SunTimeResult
{
    #region 构造与析构
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sunrise">日出时间</param>
    /// <param name="sunset">日落时间</param>
    public SunTimeResult(DateTime sunrise, DateTime sunset)
    {
        sunriseTime = sunrise;
        sunsetTime = sunset;
    }
    #endregion

    #region 属性定义
    /// <summary>
    /// 获取日出时间
    /// </summary>
    public DateTime SunriseTime
    {
        get { return sunriseTime; }
    }

    /// <summary>
    /// 获取日落时间
    /// </summary>
    public DateTime SunsetTime
    {
        get { return sunsetTime; }
    }
    #endregion


    #region 私成员
    private DateTime sunriseTime; //日出时间
    private DateTime sunsetTime; //日落时间
    #endregion
}
