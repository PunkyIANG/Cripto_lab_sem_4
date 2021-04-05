using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
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

namespace Laborator_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool encryptSalsa20 = true;
        private bool encryptTrivium = true;
        private Salsa20.ESalsa20 selectedSalsa20Method = Salsa20.ESalsa20.Bit16;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Salsa20Encrypt(object sender, TextChangedEventArgs e)
        {
            Salsa20Encrypt();
        }

        private void Salsa20Encrypt()
        {
            string keyText = Salsa20Key.Text;
            string nonceText = Salsa20Nonce.Text;
            string clearText = Salsa20Clear.Text;

            GetTrimmedBase64(ref clearText);

            if (keyText != string.Empty
                && nonceText != string.Empty
                && clearText != string.Empty
                && encryptSalsa20)
            {
                encryptSalsa20 = false;
                byte[] key = Encoding.UTF8.GetBytes(keyText);
                byte[] nonce = Encoding.UTF8.GetBytes(nonceText);
                byte[] text = Convert.FromBase64String(clearText);
                
                Salsa20.Encrypt(key, nonce, text, selectedSalsa20Method);

                Salsa20Encrypted.Text = Convert.ToBase64String(text);
                encryptSalsa20 = true;
            }
        }


        private void Salsa20Decrypt(object sender, TextChangedEventArgs e)
        {
            string keyText = Salsa20Key.Text;
            string nonceText = Salsa20Nonce.Text;
            string encryptedText = Salsa20Encrypted.Text;

            GetTrimmedBase64(ref encryptedText);

            // if (encryptedText.Length != 0)
            // {
            //     encryptedText = encryptedText.Remove(4 * (encryptedText.Length / 4));
            // }


            if (keyText != string.Empty
                && nonceText != string.Empty
                && encryptedText != string.Empty
                && encryptSalsa20)
            {
                encryptSalsa20 = false;
                byte[] key = Encoding.UTF8.GetBytes(keyText);
                byte[] nonce = Encoding.UTF8.GetBytes(nonceText);
                //byte[] text = Encoding.ASCII.GetBytes(encryptedText);
                byte[] text = Convert.FromBase64String(encryptedText);
                
                Salsa20.Encrypt(key, nonce, text, selectedSalsa20Method);

                Salsa20Clear.Text = Convert.ToBase64String(text);
                encryptSalsa20 = true;
            }
        }

        private void Salsa20Check(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Name == "Salsa32BitRadio")
            {
                selectedSalsa20Method = Salsa20.ESalsa20.Bit32;
            }
            else
            {
                selectedSalsa20Method = Salsa20.ESalsa20.Bit16;
            }
            
            Salsa20Encrypt();
        }

        private void GetTrimmedBase64(ref string input)
        {
            if (input.Length % 4 != 0)
            {
                input = input.Remove(4 * (input.Length / 4));
            }
        }

        private void TriviumEncrypt_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string keyText = TriviumKey.Text;
            string ivText = TriviumIV.Text;
            string clearText = TriviumClear.Text;

            GetTrimmedBase64(ref clearText);
            
            if (keyText != string.Empty
                && ivText != string.Empty
                && clearText != string.Empty
                && encryptTrivium)
            {
                encryptTrivium = false;
                byte[] key = Encoding.UTF8.GetBytes(keyText);
                byte[] iv = Encoding.UTF8.GetBytes(ivText);
                byte[] text = Convert.FromBase64String(clearText);
                
                var state = new Trivium(key, iv);
                state.Encrypt(text);
                
                TriviumEncrypted.Text = Convert.ToBase64String(text);
                encryptTrivium = true;
            }
        }

        private void TriviumDecrypt_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string keyText = TriviumKey.Text;
            string ivText = TriviumIV.Text;
            string encryptedText = TriviumEncrypted.Text;

            GetTrimmedBase64(ref encryptedText);
            
            if (keyText != string.Empty
                && ivText != string.Empty
                && encryptedText != string.Empty
                && encryptTrivium)
            {
                encryptTrivium = false;
                byte[] key = Encoding.UTF8.GetBytes(keyText);
                byte[] iv = Encoding.UTF8.GetBytes(ivText);
                byte[] text = Convert.FromBase64String(encryptedText);
                
                var state = new Trivium(key, iv);
                state.Encrypt(text);
                
                TriviumClear.Text = Convert.ToBase64String(text);
                encryptTrivium = true;
            }
        }
    }
}