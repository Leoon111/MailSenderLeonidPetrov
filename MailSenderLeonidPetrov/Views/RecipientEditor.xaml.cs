using System.Windows.Controls;

namespace MailSenderLeonidPetrov.Views
{
    /// <summary>
    /// Логика взаимодействия для RecipientEditor.xaml
    /// </summary>
    public partial class RecipientEditor : UserControl
    {
        public RecipientEditor() => InitializeComponent();

        private void OnDataValidationError(object? sender, ValidationErrorEventArgs e)
        {
            // заменен всплывающей подсказкой из xaml
            //var control = (Control) e.OriginalSource;
            //if (e.Action == ValidationErrorEventAction.Added)
            //    control.ToolTip = e.Error.ErrorContent.ToString();
            //else
            //    control.ClearValue(ToolTipProperty);
        }
    }
}
