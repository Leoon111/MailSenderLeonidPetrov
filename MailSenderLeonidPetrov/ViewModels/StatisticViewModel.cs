using MailSenderLeonidPetrov.ViewModels.Base;

namespace MailSenderLeonidPetrov.ViewModels
{
    class StatisticViewModel : ViewModel
    {
        #region SentMessagesCount

        private int _SendMessagesCount;

        public int SendMessagesCount
        {
            get => _SendMessagesCount;
            private set => Set(ref _SendMessagesCount, value);
        }

        #endregion

        public void MessagesSended() => SendMessagesCount++;
    }
}
