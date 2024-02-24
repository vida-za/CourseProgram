using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using System;
using CourseProgram.DataClasses;

namespace CourseProgram.ViewModels
{
    public class BaseViewModel<N> : INotifyPropertyChanged
    {
        protected BaseViewModel()
        {
            IsBusy = false;
            Title = "Base";
            Commands = new();
        }

        public IDataService<N> Data = DependencyService.Get<IDataService<N>>();
        private readonly Dictionary<string, object> _properties = new();

        public Dictionary<string, ICommand>? Commands
        {
            get => GetValue<Dictionary<string, ICommand>>();
            private init => SetValue(value);
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set { SetProperty(ref isBusy, value); }
        }

        private string? title;
        public string? Title
        {
            get => title;
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void SetValue(object? value, [CallerMemberName] string? propertyName = null)
        {
            if (_properties.TryGetValue(propertyName!, out var item) && item == value)
            {
                return;
            }

#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            _properties[propertyName!] = value;
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            OnPropertyChanged(propertyName);
        }

        protected T? GetValue<T>([CallerMemberName] string? propertyName = null)
        {
            if (_properties.TryGetValue(propertyName!, out var value))
            {
                return (T)value;
            }

            return default;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}