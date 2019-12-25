using CoreEngine.APIHandlers;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;
        private readonly IPlatformService _platformService;
        public bool StepBox1 => RegistrationState >= (int)RegistrationState.Roll;
        public bool StepBox2 => (int)RegistrationState >= (int)RegistrationState.Mobile;
        public bool StepBox3 => (int)RegistrationState >= (int)RegistrationState.Password;
        public bool ShowRoll => RegistrationState == RegistrationState.Roll;
        public bool ShowMobile => RegistrationState == RegistrationState.Mobile;
        public bool ShowPassword => RegistrationState == RegistrationState.Password;

        public RegistrationState RegistrationState { get; private set; }
        public string OTPToken { get; set; }
        public string MobileNo { get; set; }
        public string RollNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private string VerificationId;

        public RegistrationViewModel(IMemberHandler memberHandler, IPlatformService platformService)
        {
            _memberHandler = memberHandler;
            _platformService = platformService;
        }

        public ICommand PhoneSubmitCommand => new RelayCommand(PhoneSubmitAction);
        ICommand CodeSentCommand => new RelayCommand<string>(CodeSentAction);
        ICommand VerifyCompleteCommand => new RelayCommand<string>(VerifyComplete);
        ICommand VerifyFailedCommand => new RelayCommand(VerifyFailed);
        public ICommand VerifyCommand => new RelayCommand(VerifyAction);
        public ICommand RegisterCommand => new RelayCommand(RegisterAction);

        private async void RegisterAction()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                _dialog.ShowMessage("Error", "Invalid Password");
            }
            else if (Password.Length < 8)
            {
                _dialog.ShowMessage("Error", "Password must be at least 8digit");
            }
            else if (Password != ConfirmPassword)
            {
                _dialog.ShowMessage("Error", "Password does not match");
            }
            else
            {
                var res = await _memberHandler.Register(RollNo, MobileNo, Password);
                if (res != null)
                {
                    _dialog.ShowToastMessage(res.Message);
                    _nav.GoBack();
                }
            }
        }

        private async void VerifyAction()
        {
            IsBusy = true;
            var res = await _platformService.VerifyOTP(VerificationId, OTPToken);
            IsBusy = false;
            if (res)
            {
                RegistrationState = RegistrationState.Password;
            }
        }

        private async void PhoneSubmitAction()
        {
            if (string.IsNullOrWhiteSpace(RollNo))
            {
                _dialog.ShowToastMessage("Invalid Roll Number");
            }
            else if (string.IsNullOrWhiteSpace(MobileNo))
            {
                _dialog.ShowToastMessage("Invalid Mobile Number");
            }
            var response = await _memberHandler.VerifyPhoneNo(RollNo, MobileNo);
            if (response != null && response.Actionstatus)
            {
                RegistrationState = RegistrationState.Mobile;
                _platformService.VerifyPhoneNumber(MobileNo,
                    VerifyCompleteCommand, VerifyFailedCommand, CodeSentCommand);
            }
            else
            {
                _dialog.ShowMessage("Error", response?.Message);
            }
        }

        private void VerifyComplete(string smsCode)
        {
            OTPToken = smsCode;
        }

        private void VerifyFailed()
        {
            IsBusy = false;
        }

        private void CodeSentAction(string verifyId)
        {
            VerificationId = verifyId;
        }
    }
    public enum RegistrationState
    {
        Roll,
        Mobile,
        Password
    }
}
