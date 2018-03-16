using System;

using Prism.Windows.Mvvm;
using WebApi.Security;

namespace WebApi.Client.ViewModels
{
    public class RSAViewModel : ViewModelBase
    {
        private string _input;
        private string _encrypted;
        private string _decrypted;
        private ICryptoService _cryptoService;
        private string _publicKey;
        private string _privateKey;
        private bool _canEncrypt;
        private bool _canDecrypt;

        public RSAViewModel(ICryptoService cryptoService)
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
            var keys = await _cryptoService.GenerateRSAKeyPairAsync("");
            _publicKey = keys.Item1;
            _privateKey = keys.Item2;
            CanEncrypt = true;
        }

        public async void Encrypt()
        {
            var enc = await _cryptoService.EncryptRSAAsync(Input, _publicKey);
            Encrypted = Convert.ToBase64String(enc);
            CanDecrypt = true;
            Decrypted = "";
        }

        public async void Decrypt()
        {
            var dec = await _cryptoService.DecryptRSAAsync(Convert.FromBase64String(Encrypted), _privateKey);
            Decrypted = dec;
        }
    }
}
