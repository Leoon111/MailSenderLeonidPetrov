using System;
using System.ComponentModel;
using MailSender.lib.Models.Base;

namespace MailSender.lib.Models
{
    public class Recipient : Person, IDataErrorInfo//, INotifyDataErrorInfo
    {
        #region Overrides of NamedEntity

        public override string Name
        {
            get => base.Name;
            set
            {
                //if (value is null)
                //    throw new ArgumentException(nameof(value));

                if (value == "qwe")
                    throw new ArgumentException("Запрещено вводить qwe", nameof(value));
                //if (value == "")
                //    throw new ArgumentException("Имя не может быть пустой строкой", nameof(value));
                base.Name = value;
            }
        }

        #endregion

        #region Implementation of IDataErrorInfo

        /// <summary>Gets an error message indicating what is wrong with this object.</summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        string IDataErrorInfo.Error { get; } = null;

        /// <summary>Gets the error message for the property with the given name.</summary>
        /// <param name="columnName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        string IDataErrorInfo.this[string PropertyName]
        {
            get
            {
                switch (PropertyName)
                {
                    case nameof(Name):
                        var name = Name;
                        if (name is null) return "Имя не может быть пустым";
                        if (name.Length < 3) return "Имя не должно быть короче двух символов";
                        return null;
                    case nameof(Address):

                        return null;
                        default: return null;
                }
            }
        }

        #endregion
    }
}
