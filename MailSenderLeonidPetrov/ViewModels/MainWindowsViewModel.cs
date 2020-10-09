using MailSenderLeonidPetrov.ViewModels.Base;

namespace MailSenderLeonidPetrov.ViewModels
{
    class MainWindowsViewModel : ViewModel
    {
        private string _Title = "Тестовое окно";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
           
        }
    }
}
