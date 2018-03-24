using MvvmCross.Core.ViewModels;
using System;
using System.Windows.Input;
using WebApi.Security;

namespace WebApi.Client.Shared.ViewModels
{
    public class RSAPageViewModel : BaseViewModel
    {
        private string _input;
        private string _encrypted;
        private string _decrypted;
        private ICryptoService _cryptoService;
        private string _publicKey;
        private string _privateKey;
        private bool _canEncrypt = false;
        private bool _canDecrypt = false;

        public ICommand GenKeyCommand { get; private set; }
        public ICommand EncryptCommand { get; private set; }
        public ICommand DecryptCommand { get; private set; }

        public RSAPageViewModel(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
            GenKeyCommand = new MvxCommand(GenKey, () => !IsBusy);
            EncryptCommand = new MvxCommand(Encrypt, () => !IsBusy && CanEncrypt);
            DecryptCommand = new MvxCommand(Decrypt, () => !IsBusy && CanDecrypt);
        }

        public bool CanEncrypt
        {
            get { return _canEncrypt; }
            set
            {
                SetProperty(ref _canEncrypt, value);
                (EncryptCommand as MvxCommand).RaiseCanExecuteChanged();
            }
        }

        public bool CanDecrypt
        {
            get { return _canDecrypt; }
            set
            {
                SetProperty(ref _canDecrypt, value);
                (DecryptCommand as MvxCommand).RaiseCanExecuteChanged();
            }
        }

        public string Input
        {
            get { return _input; }
            set
            {
                SetProperty(ref _input, value);
                CanDecrypt = false;
                Encrypted = "";
                Decrypted = "";
                (EncryptCommand as MvxCommand).RaiseCanExecuteChanged();
                (DecryptCommand as MvxCommand).RaiseCanExecuteChanged();
            }
        }

        public string Encrypted
        {
            get { return _encrypted; }
            set { SetProperty(ref _encrypted, value); }
        }

        public string Decrypted
        {
            get { return _decrypted; }
            set { SetProperty(ref _decrypted, value); }
        }

        private async void GenKey()
        {
            IsBusy = true;
            CanEncrypt = false;
            CanDecrypt = false;
            (GenKeyCommand as MvxCommand).RaiseCanExecuteChanged();
            (EncryptCommand as MvxCommand).RaiseCanExecuteChanged();
            (DecryptCommand as MvxCommand).RaiseCanExecuteChanged();
            var keys = await _cryptoService.GenerateRSAKeyPairAsync("");
            _publicKey = keys.Item1;
            _privateKey = keys.Item2;
            CanEncrypt = true;
            IsBusy = false;
            (GenKeyCommand as MvxCommand).RaiseCanExecuteChanged();
            (EncryptCommand as MvxCommand).RaiseCanExecuteChanged();
            (DecryptCommand as MvxCommand).RaiseCanExecuteChanged();

        }

        private async void Encrypt()
        {
            var enc = await _cryptoService.EncryptRSAAsync(Input, _publicKey);
            Encrypted = Convert.ToBase64String(enc);
            CanDecrypt = true;
            Decrypted = "";
        }

        private async void Decrypt()
        {
            var dec = await _cryptoService.DecryptRSAAsync(Convert.FromBase64String(Encrypted), _privateKey);
            Decrypted = dec;
        }
    }
}
