using System.Text.RegularExpressions;

namespace generalStore.Extension
{
    public static class Extension
    {
        public static string ToVnd(this double dongGia)
        {
            return dongGia.ToString("#,##0") + " VND";
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if(s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }

        public static string ToUrlFriendly(this string url)
        {
            var result = url.ToLower().Trim();
            result = Regex.Replace(result, "àáãạảăắằẳẵặâấầẩẫậ", "a");
            result = Regex.Replace(result, "èéẹẻẽêềếểễệ", "e");
            result = Regex.Replace(result, "ìíĩỉị", "i");
            result = Regex.Replace(result, "òóõọỏôốồổỗộơớờởỡợ", "o");
            result = Regex.Replace(result, "ùúũụủưứừửữự", "u");
            result = Regex.Replace(result, "ỳỵỷỹý", "y");
            result = Regex.Replace(result, "đ", "d");
            result = Regex.Replace(result, "[^a-z0-9-]", "");
            result = Regex.Replace(result, "(-)+", "-");

            return result;
        }
    }
}
