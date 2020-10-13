using Microsoft.Extensions.DependencyInjection;

namespace MailSenderLeonidPetrov.ViewModels
{
    class ViewModelLocator
    {
        public MainWindowsViewModel MyWindowsModel => App.Services.GetRequiredService<MainWindowsViewModel>();
    }
}
