using MvvmCross.Core.ViewModels;
using System;
using System.Windows.Input;
using WebApi.Security;

namespace WebApi.Client.Shared.ViewModels
{
    public class TrippleDESPageViewModel : BaseViewModel
    {
        private string _input;
        private string _encrypted;
        private string _decrypted;
        private ICryptoService _cryptoService;
        private byte[] _key;
        private bool _canEncrypt;
        private bool _canDecrypt;

        public ICommand GenKeyCommand { get; private set; }
        public ICommand EncryptCommand { get; private set; }
        public ICommand DecryptCommand { get; private set; }

        public TrippleDESPageViewModel(ICryptoService cryptoService)
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

        public async void GenKey()
        {
            IsBusy = true;
            CanEncrypt = false;
            CanDecrypt = false;
            (GenKeyCommand as MvxCommand).RaiseCanExecuteChanged();
            (EncryptCommand as MvxCommand).RaiseCanExecuteChanged();
            (DecryptCommand as MvxCommand).RaiseCanExecuteChanged();
            _key = await _cryptoService.GenerateTripleDESKeyAsync();
            CanEncrypt = true;
            IsBusy = false;
            (GenKeyCommand as MvxCommand).RaiseCanExecuteChanged();
            (EncryptCommand as MvxCommand).RaiseCanExecuteChanged();
            (DecryptCommand as MvxCommand).RaiseCanExecuteChanged();
        }

        public async void Encrypt()
        {
            var enc = await _cryptoService.EncryptTripleDESAsync(Input, _key);
            Encrypted = Convert.ToBase64String(enc);
            CanDecrypt = true;
            Decrypted = "";
        }

        public async void Decrypt()
        {
            var dec = await _cryptoService.DecryptTripleDESAsync(Convert.FromBase64String(Encrypted), _key);
            Decrypted = dec;
        }
    }
}
