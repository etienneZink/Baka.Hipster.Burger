using System.ComponentModel;
using System.Runtime.CompilerServices;
using Baka.Hipster.Burger.Client.Annotations;

namespace Baka.Hipster.Burger.Client.Framework
{
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}