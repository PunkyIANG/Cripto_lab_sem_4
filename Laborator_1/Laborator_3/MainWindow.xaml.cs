using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laborator_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AES.Mode aesMode = AES.Mode.Bit128;
        private bool encryptAES = true;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void GetTrimmedBase64(ref string input)
        {
            if (input.Length % 4 != 0)
            {
                input = input.Remove(4 * (input.Length / 4));
            }
        }

        private void AES128BitRadio_OnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            switch (rb.Name)
            {
                case "AES128BitRadio":
                    aesMode = AES.Mode.Bit128;
                    break;
                
                case "AES192BitRadio":
                    aesMode = AES.Mode.Bit192;
                    break;
                
                case "AES256BitRadio":
                    aesMode = AES.Mode.Bit256;
                    break;
            }
            
            StartEncryption();
        }

        private void StartEncryption()
        {
            var keyText = AESKey.Text;
            var clearText = AESClear.Text;
            
            GetTrimmedBase64(ref clearText);

            if (encryptAES
                && keyText != string.Empty
                && clearText != string.Empty)
            {
                encryptAES = false;
                
                var key = new byte[AES.key_words[(int) aesMode] * 4];
                var tempKey = Encoding.UTF8.GetBytes(keyText);

                var minLength = key.Length < tempKey.Length ? key.Length : tempKey.Length;
                
                Array.Copy(tempKey, 0, key, 0, minLength);
                
                var tempText = Convert.FromBase64String(clearText);
                
                var text = new byte[(int)Math.Ceiling((float)tempText.Length / 16) * 16];
                
                Array.Copy(tempText, 0, text, 0, tempText.Length);
                
                AES.Encrypt(ref text, key, (int) aesMode);
                AES.PrintBlock(text);

                AESEncrypted.Text = Convert.ToBase64String(text);

                encryptAES = true;
            }
        }

        private void AESKey_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            StartEncryption();
        }

        private void AESEncrypted_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var keyText = AESKey.Text;
            var encryptedText = AESEncrypted.Text;
            
            GetTrimmedBase64(ref encryptedText);

            if (encryptAES
                && keyText != string.Empty
                && encryptedText != string.Empty)
            {
                encryptAES = false;
                
                var key = new byte[AES.key_words[(int) aesMode] * 4];
                var tempKey = Encoding.UTF8.GetBytes(keyText);

                var minLength = key.Length < tempKey.Length ? key.Length : tempKey.Length;
                
                Array.Copy(tempKey, 0, key, 0, minLength);
                
                var tempText = Convert.FromBase64String(encryptedText);
                
                var text = new byte[(int)Math.Ceiling((float)tempText.Length / 16) * 16];
                
                Array.Copy(tempText, 0, text, 0, tempText.Length);
                
                AES.Decrypt(ref text, key, (int) aesMode);
                AES.PrintBlock(text);

                AESClear.Text = Convert.ToBase64String(text);

                encryptAES = true;
            }
        }
    }
}