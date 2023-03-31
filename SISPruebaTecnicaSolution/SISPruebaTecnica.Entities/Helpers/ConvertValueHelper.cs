using Newtonsoft.Json.Linq;
using System;

namespace SISPruebaTecnica.Entities.Helpers
{
    public class ConvertValueHelper
    {
        public static TimeSpan ConvertHourValue(object obj)
        {
            try
            {
                if (obj == null)
                    return DateTime.Now.TimeOfDay;

                string value = Convert.ToString(obj);

                if (TimeSpan.TryParse(value, out TimeSpan hour))
                    return DateTime.Now.TimeOfDay;

                return hour;
            }
            catch (Exception)
            {
                return DateTime.Now.TimeOfDay;
            }
        }
        public static DateTime ConvertDateValue(object obj)
        {
            try
            {
                if (obj == null)
                    return DateTime.Now;

                string value = Convert.ToString(obj);

                if (DateTime.TryParse(value, out DateTime date))
                    return DateTime.Now;

                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        public static string ConvertStringValue(object obj)
        {
            try
            {
                if (obj == null)
                    return string.Empty;

                string value = Convert.ToString(obj);

                if (string.IsNullOrEmpty(value))
                    return string.Empty;

                return value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static decimal ConvertDecimalValue(object obj)
        {
            try
            {
                if (obj == null)
                    return 0;

                string value = Convert.ToString(obj);

                if (!decimal.TryParse(value, out decimal num))
                    return 0;

                return num;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int ConvertIntValue(object obj)
        {
            try
            {
                if (obj == null)
                    return 0;

                string value = Convert.ToString(obj);

                if (!int.TryParse(value, out int num))
                    return 0;

                return num;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool ValidateJsonFormat(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                return true;
            }
            catch (Exception)
            {

            }

            try
            {
                JToken obj = JToken.Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static double ConvertDouble(object obj)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool ConvertBoolean(object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string TimeSpanToString(TimeSpan hour, string format)
        {
            string tiempo;
            DateTime date = new DateTime().Add(hour);
            tiempo = date.ToString(format);

            return tiempo;
        }
    }
}
