using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MailSenderLeonidPetrov.Infrastructure.ValidationRules
{
    public class RegExValidation : ValidationRule
    {
        private Regex _Regex;

        public string Pattern
        {
            get => _Regex?.ToString();
            set => _Regex = string.IsNullOrEmpty(value) ? null : new Regex(value);
        }
        public bool AllowNull { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>When overridden in a derived class, performs validation checks on a value.</summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //return ValidationResult.ValidResult;
            //return new ValidationResult(false, "Сведения о возникшей ошибке");

            if (value is null)
                return AllowNull
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, ErrorMessage ?? "Отсутствует ссылка на строку");

            if (_Regex is null) return ValidationResult.ValidResult;

            if (!(value is string str))
                str = value.ToString();

            return _Regex.IsMatch(str)
                ? ValidationResult.ValidResult
                : new ValidationResult(false,
                    ErrorMessage ?? $"Строка не удовлетворяет требованиям выражения {Pattern}");
        }
    }
}
