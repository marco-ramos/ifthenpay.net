using System;
using System.Globalization;
using Ifthenpay.Domain;

namespace Ifthenpay.Services
{
    public class Generator
    {
        private readonly Configuration _configuration;

        public Generator(Configuration configuration)
        {
            _configuration = configuration;
        }

        public static string GetReference(Configuration configuration, int identifier, decimal amount)
        {
            return new Generator(configuration).CalculateReference(identifier, amount);
        }

        public string CalculateReference(int identifier, decimal amount)
        {
            string strValue = string.Empty;
            string s = FormatValue(amount);
            decimal.TryParse(s, out var result);
            decimal.TryParse($"{(object)result:f}", out result);

            if (_configuration.SubEntity.Length == 3)
                strValue = _configuration.SubEntity + RightEx(identifier.ToString("0000"), 4) + decimal.Multiply(result, new Decimal(100L)).ToString("00000000");
            else if (_configuration.SubEntity.Length == 2)
                strValue = _configuration.SubEntity + RightEx(identifier.ToString("00000"), 5) + decimal.Multiply(result, new Decimal(100L)).ToString("00000000");
            else if (_configuration.SubEntity.Length == 1)
                strValue = _configuration.SubEntity + RightEx(identifier.ToString("000000"), 6) + decimal.Multiply(result, new Decimal(100L)).ToString("00000000");

            int checkDigit = CalculateCheckDigit(_configuration.Entity, strValue);
            var paymentRef = $"{(object)_configuration.SubEntity} {(object)RightEx(identifier.ToString("000 0"), 5)}{(object)checkDigit:00}";
            if (_configuration.SubEntity.Length == 3)
                paymentRef = $"{(object)_configuration.SubEntity} {(object)RightEx(identifier.ToString("000 0"), 5)}{(object)checkDigit:00}";
            else if (_configuration.SubEntity.Length == 2)
                paymentRef = $"{(object)_configuration.SubEntity}{(object)RightEx(identifier.ToString("0 000 0"), 7)}{(object)checkDigit:00}";
            else if (_configuration.SubEntity.Length == 1)
                paymentRef = $"{(object)_configuration.SubEntity}{(object)RightEx(identifier.ToString("00 000 0"), 8)}{(object)checkDigit:00}";
            return paymentRef;
        }

        private int GetCheckDigit(string entity, int reference, Decimal amount)
        {
            string StrValue = MidEx(reference.ToString("000000000"), 0, 3) + MidEx(reference.ToString("000000000"), 3, 4) + Decimal.Multiply(amount, new Decimal(100L)).ToString("00000000");
            return CalculateCheckDigit(entity, StrValue);
        }

        private int GetCheckDigitEx(string entity, string reference, decimal amount)
        {
            string s = FormatValue(amount);
            Decimal.TryParse(s, out var result);
            Decimal.TryParse($"{(object)result:f}", out result);
            string strValue = $"{(object)reference:000000000}".Substring(0, 7) + Decimal.Multiply(result, new Decimal(100L)).ToString("00000000");
            return Convert.ToInt32($"{(object)CalculateCheckDigit(entity, strValue):00}");
        }

        private int CalculateCheckDigit(string entity, string strValue)
        {
            return checked(98 - checked(51 * int.Parse(MidEx(entity, 0, 1)) + 73 * int.Parse(MidEx(entity, 1, 1)) + 17 * int.Parse(MidEx(entity, 2, 1)) + 89 * int.Parse(MidEx(entity, 3, 1)) + 38 * int.Parse(MidEx(entity, 4, 1)) + 3 * int.Parse(MidEx(strValue, 14, 1)) + 30 * int.Parse(MidEx(strValue, 13, 1)) + 9 * int.Parse(MidEx(strValue, 12, 1)) + 90 * int.Parse(MidEx(strValue, 11, 1)) + 27 * int.Parse(MidEx(strValue, 10, 1)) + 76 * int.Parse(MidEx(strValue, 9, 1)) + 81 * int.Parse(MidEx(strValue, 8, 1)) + 34 * int.Parse(MidEx(strValue, 7, 1)) + 49 * int.Parse(MidEx(strValue, 6, 1)) + 5 * int.Parse(MidEx(strValue, 5, 1)) + 50 * int.Parse(MidEx(strValue, 4, 1)) + 15 * int.Parse(MidEx(strValue, 3, 1)) + 53 * int.Parse(MidEx(strValue, 2, 1)) + 45 * int.Parse(MidEx(strValue, 1, 1)) + 62 * int.Parse(MidEx(strValue, 0, 1))) % 97);
        }

        private string FormatValue(decimal amount)
        {
            var value = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", amount);
            string str1 = $"{(object)99:n}";
            string str2 = value.ToString();
            string str3 = str1.Substring(2, 1);
            int num1 = checked(str2.Length - 1);
            bool flag = false;
            while (num1 >= 0)
            {
                if (string.CompareOrdinal(str2.Substring(num1, 1), ".") == 0 | string.CompareOrdinal(str2.Substring(num1, 1), ",") == 0)
                {
                    flag = true;
                    str2 = $"{(object)str2.Trim().Substring(0, num1)}@{(object)str2.Trim().Substring(checked(num1 + 1))}";
                    break;
                }
                checked { num1 += -1; }
            }
            if (!flag)
            {
                str2 = $"{(object)str2:n}";
                int num2 = checked(str2.Length - 1);
                while (num2 >= 1)
                {
                    if (string.CompareOrdinal(str2.Substring(num2, 1), ".") == 0 | string.CompareOrdinal(str2.Substring(num2, 1), ",") == 0)
                    {
                        str2 = $"{(object)str2.Trim().Substring(0, num2)}@{(object)str2.Trim().Substring(checked(num2 + 1))}";
                        break;
                    }
                    checked { num2 += -1; }
                }
            }
            int num3 = checked(str2.Length - 1);
            int num4 = 1;
            while (num4 <= num3)
            {
                if (string.CompareOrdinal(str2.Substring(num4, 1), ".") == 0 | string.CompareOrdinal(str2.Substring(num4, 1), ",") == 0)
                {
                    str2 = $"{(object)str2.Trim().Substring(0, num4)}{(object)str2.Trim().Substring(checked(num4 + 1))}";
                    break;
                }
                checked { ++num4; }
            }
            if (str2.Contains("@"))
                str2 = $"{(object)str2.Trim().Substring(0, str2.IndexOf("@", StringComparison.Ordinal))}{(object)str3.Trim()}{(object)str2.Trim().Substring(checked(str2.IndexOf("@", StringComparison.Ordinal) + 1))}";
            return str2;
        }

        private string RightEx(string param, int length)
        {
            return param.Substring(checked(param.Length - length), length);
        }

        private string MidEx(string param, int startIndex, int length)
        {
            return param.Substring(startIndex, length);
        }

        private bool IsNumber(string value)
        {
            return int.TryParse(value, out int res);
        }

        private bool IsDecimal(string value)
        {
            return decimal.TryParse(value, out decimal res);
        }

        private string StringVerify(string value)
        {
            int num = 0;
            string str1 = string.Empty;
            string str2 = value;
            int index1 = 0;
            int length1 = str2.Length;
            while (index1 < length1)
            {
                char c = str2[index1];
                if (char.IsPunctuation(c) || char.IsSeparator(c))
                    checked { ++num; }
                checked { ++index1; }
            }
            if (num > 1)
            {
                string str3 = value;
                int index2 = 0;
                int length2 = str3.Length;
                while (index2 < length2)
                {
                    char c = str3[index2];
                    if (num > 1)
                    {
                        if (char.IsPunctuation(c) || char.IsSeparator(c))
                            checked { --num; }
                        else
                            str1 += Convert.ToString(c);
                    }
                    else
                        str1 += Convert.ToString(c);
                    checked { ++index2; }
                }
            }
            else
                str1 = value;
            return str1.Replace(",", ".");
        }

    }
}
