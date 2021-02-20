﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Laborator_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //TODO: add uppercase vaidation

        private bool _triggerMyszkowski = true;
        private bool _triggerNihilist = true;
        private bool _triggerPlayfair = true;
        private bool _triggerVigenere = true;
        private bool _triggerNicodemus = true;

        private void MyszkowskiEncrypt(object sender, TextChangedEventArgs e)
        {
            var clearText = MyszkowskiClearText.Text;
            var key = MyszkowskiKey.Text;
            if (clearText != string.Empty
                && key != string.Empty
                && _triggerMyszkowski)
            {
                _triggerMyszkowski = false;
                MyszkowskiEncryptedText.Text = Myszkowski.Encrypt(clearText, key);
                _triggerMyszkowski = true;
            }
        }

        private void MyszkowskiDecrypt(object sender, TextChangedEventArgs e)
        {
            var encryptedText = MyszkowskiEncryptedText.Text;
            var key = MyszkowskiKey.Text;
            if (encryptedText != string.Empty
                && key != string.Empty
                && _triggerMyszkowski)
            {
                _triggerMyszkowski = false;
                MyszkowskiClearText.Text = Myszkowski.Decrypt(encryptedText, key);
                _triggerMyszkowski = true;
            }
        }

        private void NihilistEncrypt(object sender, TextChangedEventArgs e)
        {
            var clearText = RemoveSpaces(NihilistClearText.Text);
            var alphabetKey = NihilistAlphabetKey.Text;
            var cryptKey = NihilistCryptKey.Text;

            if (clearText != string.Empty
                && alphabetKey != string.Empty
                && cryptKey != string.Empty
                && _triggerNihilist)
            {
                _triggerNihilist = false;
                NihilistEncrypted.Text = Nihilist.Encrypt(clearText, cryptKey, alphabetKey);
                _triggerNihilist = true;
            }
        }

        private void NihilistDecrypt(object sender, TextChangedEventArgs e)
        {
            var encryptedText = NihilistEncrypted.Text;
            var alphabetKey = NihilistAlphabetKey.Text;
            var cryptKey = NihilistCryptKey.Text;

            if (encryptedText != string.Empty
                && alphabetKey != string.Empty
                && cryptKey != string.Empty
                && _triggerNihilist)
            {
                _triggerNihilist = false;
                NihilistClearText.Text = Nihilist.Decrypt(encryptedText, cryptKey, alphabetKey);
                _triggerNihilist = true;
            }
        }

        private void PlayfairEncrypt(object sender, TextChangedEventArgs e)
        {
            var clearText = RemoveSpaces(PlayfairClearText.Text);
            var alphabetKey = PlayfairAlphabetKey.Text;

            if (clearText != string.Empty
                && alphabetKey != string.Empty
                && _triggerPlayfair)
            {
                _triggerPlayfair = false;
                PlayfairEncrypted.Text = Playfair.Encrypt(clearText, alphabetKey);
                _triggerPlayfair = true;
            }
        }

        private void PlayfairDecrypt(object sender, TextChangedEventArgs e)
        {
            var encryptedText = RemoveSpaces(PlayfairEncrypted.Text);
            var alphabetKey = PlayfairAlphabetKey.Text;

            if (encryptedText != string.Empty
                && alphabetKey != string.Empty
                && _triggerPlayfair)
            {
                _triggerPlayfair = false;
                PlayfairClearText.Text = Playfair.Decrypt(encryptedText, alphabetKey);
                _triggerPlayfair = true;
            }
        }

        private void VigenereEncrypt(object sender, TextChangedEventArgs e)
        {
            var key = VigenereAlphabetKey.Text;
            var clearText = VigenereClearText.Text;

            if (key != string.Empty
                && clearText != string.Empty
                && _triggerVigenere)
            {
                _triggerVigenere = false;
                VigenereEncrypted.Text = Vigenere.Encrypt(clearText, key);
                _triggerVigenere = true;
            }
        }

        private void VigenereDecrypt(object sender, TextChangedEventArgs e)
        {
            var key = VigenereAlphabetKey.Text;
            var encryptedText = VigenereEncrypted.Text;

            if (key != string.Empty
                && encryptedText != string.Empty
                && _triggerVigenere)
            {
                _triggerVigenere = false;
                VigenereClearText.Text = Vigenere.Decrypt(encryptedText, key);
                _triggerVigenere = true;
            }
        }

        private void NicodemusEncrypt(object sender, TextChangedEventArgs e)
        {
            var key = NicodemusAlphabetKey.Text;
            var clearText = RemoveSpaces(NicodemusClearText.Text);
            
            if (key != string.Empty
                && clearText != string.Empty
                && _triggerNicodemus)
            {
                _triggerNicodemus = false;
                NicodemusEncrypted.Text = Nicodemus.Encrypt(clearText, key);
                _triggerNicodemus = true;

            }
        }
        
        private void NicodemusDecrypt(object sender, TextChangedEventArgs e)
        {
            var key = NicodemusAlphabetKey.Text;
            var encryptedText = RemoveSpaces(NicodemusEncrypted.Text);
            
            if (key != string.Empty
                && encryptedText != string.Empty
                && _triggerNicodemus)
            {
                _triggerNicodemus = false;
                NicodemusClearText.Text = Nicodemus.Decrypt(encryptedText, key);
                _triggerNicodemus = true;
            }
        }
        
        private void RestrictAlphabetSpace(object sender, TextCompositionEventArgs e)
        {
            var alphabetRegex = new Regex("[a-zA-Z ]");
            e.Handled = !alphabetRegex.IsMatch(e.Text);
        }
        
        private void RestrictAlphabet(object sender, TextCompositionEventArgs e)
        {
            var alphabetRegex = new Regex("[a-zA-Z]");
            e.Handled = !alphabetRegex.IsMatch(e.Text);
        }
        
        private string RemoveSpaces(string s)
        {
            return s.Replace(" ", "");
        }
    }
}