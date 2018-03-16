using System;

using Prism.Windows.Mvvm;
using WebApi.Security;

namespace WebApi.Client.ViewModels
{
    public class TrippleDESViewModel : ViewModelBase
    {
        private string _input;
        private string _encrypted;
        private string _decrypted;
        private ICryptoService _cryptoService;
        private byte[] _key;
        private bool _canEncrypt;
        private bool _canDecrypt;

        public TrippleDESViewModel(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        public bool CanEncrypt
        {
            get { return _canEncrypt; }
            set { SetProperty(ref _canEncrypt, value); }
        }

        public bool CanDecrypt
        {
            get { return _canDecrypt; }
            set { SetProperty(ref _canDecrypt, value); }
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
            _key = await _cryptoService.GenerateTripleDESKeyAsync();
            CanEncrypt = true;
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
