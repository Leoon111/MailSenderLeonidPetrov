using System;
using MailSender.lib.Models.Base;

namespace MailSender.lib.Models
{
    public class Recipient : Person
    {
        #region Overrides of NamedEntity

        public override string Name
        {
            get => base.Name;
            set
            {
                if (value == "qwe")
                    throw new ArgumentException("Запрещено вводить qwe", nameof(value));
                if (value == "")
                    throw new ArgumentException("Имя не может быть пустой строкой", nameof(value));
                base.Name = value;
            }
        }

        #endregion
    }
}
