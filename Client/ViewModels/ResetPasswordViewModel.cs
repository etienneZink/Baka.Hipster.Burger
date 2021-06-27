using Baka.Hipster.Burger.Client.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class ResetPasswordViewModel: ViewModelBase
    {
        private string _oldPassword;

        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                if (_oldPassword == value) return;
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        private string _newPasswordOne;

        public string NewPasswordOne
        {
            get => _newPasswordOne;
            set
            {
                if (_newPasswordOne == value) return;
                _newPasswordOne = value;
                OnPropertyChanged(nameof(NewPasswordOne));
            }
        }

        private string _newPasswordTwo;

        public string NewPasswordTwo
        {
            get => _newPasswordTwo;
            set
            {
                if (_newPasswordTwo == value) return;
                _newPasswordTwo = value;
                OnPropertyChanged(nameof(NewPasswordTwo));
            }
        }

        public ICommand SubmitPasswordResetCommand { get; set; }
    }
}
