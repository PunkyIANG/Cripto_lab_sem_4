﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private void MyszkowskiClearText_OnTextChanged(object sender, TextChangedEventArgs e)
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

        private void MyszkowskiEncryptedText_OnTextChanged(object sender, TextChangedEventArgs e)
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

        private void NihilistAlphabetKey_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var clearText = NihilistClearText.Text;
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

        private void NihilistEncrypted_OnTextChanged(object sender, TextChangedEventArgs e)
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

        private void PlayfairAlphabetKey_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var clearText = PlayfairClearText.Text;
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

        private void PlayfairEncrypted_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var encryptedText = PlayfairEncrypted.Text;
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
    }
}