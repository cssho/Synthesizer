using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Synthesizer.Converter
{
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanSliderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return default(double);
            if (value is TimeSpan ts)
            {
                switch (parameter.ToString())
                {
                    case "ms":
                        return ts.TotalMilliseconds;
                    case "s":
                        return ts.TotalSeconds;
                    case "m":
                        return ts.TotalMinutes;
                    case "h":
                        return ts.TotalHours;
                    case "d":
                        return ts.TotalDays;
                }
            }
            return default(double);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return default(TimeSpan);
            if (value is double ms)
            {
                switch (parameter.ToString())
                {
                    case "ms":
                        return TimeSpan.FromMilliseconds(ms);
                    case "s":
                        return TimeSpan.FromSeconds(ms);
                    case "m":
                        return TimeSpan.FromMinutes(ms);
                    case "h":
                        return TimeSpan.FromHours(ms);
                    case "d":
                        return TimeSpan.FromDays(ms);
                }
            }
            return default(TimeSpan);
        }
    }
}
