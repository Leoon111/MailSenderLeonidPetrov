using System.Linq;
using System.Windows;

namespace MailSenderLeonidPetrov.Infrastructure.Commands.Base
{
    class CloseWindowsCommand : Command
    {
        protected override void Execute(object p)
        {
            var window = p as Window;
            if (window is null)
                window = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);
            if (window is null)
                window = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);
            window?.Close();
        }
    }
}
