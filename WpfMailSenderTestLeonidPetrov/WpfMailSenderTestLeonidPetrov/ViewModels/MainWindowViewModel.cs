using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using WpfMailSenderTestLeonidPetrov.ViewModels.Base;

namespace WpfMailSenderTestLeonidPetrov.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private string _Title = "Тестовое окно";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
            //{
            //    if(_Title == value) return;
            //    _Title = value;
            //    //OnPropertyChanged("Title");
            //    //OnPropertyChanged(nameof(Title));
            //    OnPropertyChanged();
            //}
        }
        public  DateTime CurrentTime => DateTime.Now;
        private bool _timerEnabled = true;

        public bool TimerEnabled
        {
            get => _timerEnabled;
            set
            {
                if(!Set(ref _timerEnabled, value)) return;
                _timer.Enabled = value;
            }
        }
        private readonly Timer _timer;

        public MainWindowViewModel()
        {
            _timer = new Timer(100);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs E)
        {
            OnPropertyChanged(nameof(CurrentTime));
        }
    }
}
