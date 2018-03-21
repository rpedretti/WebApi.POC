using System;
using System.Windows.Input;
using WebApi.Security;
using Xamarin.Forms;

namespace WebApi.Cross.ViewModels
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
            GenKeyCommand = new Command(GenKey, () => !IsBusy);
            EncryptCommand = new Command(Encrypt, () => !IsBusy && CanEncrypt);
            DecryptCommand = new Command(Decrypt, () => !IsBusy && CanDecrypt);
        }

        public bool CanEncrypt
        {
            get { return _canEncrypt; }
            set
            {
                SetProperty(ref _canEncrypt, value);
                (EncryptCommand as Command).ChangeCanExecute();
            }
        }

        public bool CanDecrypt
        {
            get { return _canDecrypt; }
            set
            {
                SetProperty(ref _canDecrypt, value);
                (DecryptCommand as Command).ChangeCanExecute();
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
                (EncryptCommand as Command).ChangeCanExecute();
                (DecryptCommand as Command).ChangeCanExecute();
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
            (GenKeyCommand as Command).ChangeCanExecute();
            (EncryptCommand as Command).ChangeCanExecute();
            (DecryptCommand as Command).ChangeCanExecute();
            var keys = await _cryptoService.GenerateRSAKeyPairAsync("");
            _publicKey = keys.Item1;
            _privateKey = keys.Item2;
            CanEncrypt = true;
            IsBusy = false;
            (GenKeyCommand as Command).ChangeCanExecute();
            (EncryptCommand as Command).ChangeCanExecute();
            (DecryptCommand as Command).ChangeCanExecute();

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
