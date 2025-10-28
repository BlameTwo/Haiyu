
using System;

namespace Astronomical;

public class MoonPhaseCalculator
{
    public static double CalculateMoonPhase(DateTime date)
    {
        double phase = CalculatePhase(date);
        return phase;
    }

    private static double CalculatePhase(DateTime date)
    {
        DateTime reference = new DateTime(2000, 1, 6, 17, 14, 0, DateTimeKind.Utc);

        double days = (date - reference).TotalDays;
        double synodicMonth = 29.530588853;

        double remainder = days % synodicMonth;
        if (remainder < 0)
        {
            remainder += synodicMonth;
        }

        return remainder / synodicMonth;
    }

    public static string DeterminePhaseName(double phase)
    {
        phase = phase < 0 ? phase + 1 : phase % 1;

        if (phase < 0.05 || phase >= 0.95) return "新月"; // 图标1 (0.0 附近)

        if (phase < 0.20) return "娥眉月";

        if (phase < 0.30) return "上弦月";

        if (phase < 0.45) return "盈凸月";

        if (phase < 0.50) return "接近满月";

        if (phase < 0.55) return "满月";

        if (phase < 0.70) return "亏凸月";
        if (phase < 0.80) return "下弦月";
        return "残月";
    }

    public static string DeterminePhaseIcon(double phase)
    {
        phase = phase < 0 ? phase + 1 : phase % 1;

        if (phase < 0.05 || phase >= 0.95) return "\U0001F311";

        if (phase < 0.20) return "\U0001F312";

        if (phase < 0.30) return "\U0001F312";

        if (phase < 0.45) return "\U0001F313";

        if (phase < 0.50) return "\U0001F314";

        if (phase < 0.55) return "\U0001F315";

        if (phase < 0.70) return "\U0001F316";
        if (phase < 0.80) return "\U0001F318";
        return "\U0001F319";
    }
}