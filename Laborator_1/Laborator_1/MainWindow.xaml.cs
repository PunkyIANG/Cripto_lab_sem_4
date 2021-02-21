using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private int _myszkowskiRowCount;

        public int MyszkowskiRowCount
        {
            get => _myszkowskiRowCount;
            set
            {
                if (_myszkowskiRowCount == value) return;
                _myszkowskiRowCount = value;
                OnPropertyChanged();
            }
        }
        
        private int _myszkowskiColumnCount;

        public int MyszkowskiColumnCount
        {
            get => _myszkowskiColumnCount;
            set
            {
                if (_myszkowskiColumnCount == value) return;
                _myszkowskiColumnCount = value;
                OnPropertyChanged();
            }
        }
        
        private int _nihilistRowCount;

        public int NihilistRowCount
        {
            get => _nihilistRowCount;
            set
            {
                if (_nihilistRowCount == value) return;
                _nihilistRowCount = value;
                OnPropertyChanged();
            }
        }
        
        private int _nihilistColumnCount;

        public int NihilistColumnCount
        {
            get => _nihilistColumnCount;
            set
            {
                if (_nihilistColumnCount == value) return;
                _nihilistColumnCount = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                
                MyszkowskiEncryptedText.Text = Myszkowski.Encrypt(clearText, key, out var charTable);
                SetMyszkowskiTable(charTable, Myszkowski.LetterMap(key));
                
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
                MyszkowskiClearText.Text = Myszkowski.Decrypt(encryptedText, key, out var charTable);
                SetMyszkowskiTable(charTable, Myszkowski.LetterMap(key));
                _triggerMyszkowski = true;
            }
        }

        private void SetMyszkowskiTable(char[][] table, int[] letterMap)
        {
            var fullTable = new char[table.Length + 1][];
            for (int i = 0; i < fullTable.Length; i++)
            {
                fullTable[i] = new char[table[0].Length];

                if (i == 0)
                {
                    for (int j = 0; j < fullTable[i].Length; j++)
                    {
                        fullTable[i][j] = (char)('0' + letterMap[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < fullTable[i].Length; j++)
                    {
                        fullTable[i][j] = table[i - 1][j];
                    }
                }
            }

            MyszkowskiRowCount = fullTable.Length;
            MyszkowskiColumnCount = fullTable[0].Length;
            MyszkowskiGrid.Children.Clear();

            for (var i = 0; i < fullTable.Length; i++)
            {
                for (var j = 0; j < fullTable[i].Length; j++)
                {
                    var textBlock = new TextBlock
                    {
                        Text = "" + fullTable[i][j], Style = (Style) Resources["Character"]
                    };

                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);

                    MyszkowskiGrid.Children.Add(textBlock);
                }
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
                NihilistEncrypted.Text = Nihilist.Encrypt(clearText, cryptKey, alphabetKey, out var charGrid, out var numberGrid);
                SetNihilistAlphabet(Nihilist.GetAlphabetTable(alphabetKey));
                SetNihilistEncryptedGrid(charGrid, numberGrid);
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
                NihilistClearText.Text = Nihilist.Decrypt(encryptedText, cryptKey, alphabetKey, out var charGrid, out var numberGrid);
                SetNihilistAlphabet(Nihilist.GetAlphabetTable(alphabetKey));
                SetNihilistEncryptedGrid(charGrid, numberGrid);
                _triggerNihilist = true;
            }
        }

        private void SetNihilistAlphabet(char[][] alphabetTable)
        {
            NihilistAlphabetGrid.Children.Clear();

            for (var i = 0; i < alphabetTable.Length; i++)
            {
                for (var j = 0; j < alphabetTable[i].Length; j++)
                {
                    var textBlock = new TextBlock
                    {
                        Text = "" + alphabetTable[i][j], Style = (Style) Resources["Character"]
                    };

                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);

                    NihilistAlphabetGrid.Children.Add(textBlock);
                }
            }
        }

        private void SetNihilistEncryptedGrid(char[][] charGrid, int[][] numberGrid)
        {
            NihilistCharGrid.Children.Clear();
            NihilistNumberGrid.Children.Clear();

            NihilistRowCount = charGrid.Length;
            NihilistColumnCount = charGrid[0].Length;
            
            for (var i = 0; i < charGrid.Length; i++)
            {
                for (var j = 0; j < charGrid[i].Length; j++)
                {
                    var charBlock = new TextBlock
                    {
                        Text = "" + charGrid[i][j], Style = (Style) Resources["Character"]
                    };

                    Grid.SetRow(charBlock, i);
                    Grid.SetColumn(charBlock, j);
                    
                    
                    
                    var numBlock = new TextBlock
                    {
                        Text = "" + numberGrid[i][j] + " ", Style = (Style) Resources["Character"]
                    };

                    Grid.SetRow(numBlock, i);
                    Grid.SetColumn(numBlock, j);

                    if (numBlock.Text != "100 ")
                    {
                        NihilistCharGrid.Children.Add(charBlock);
                        NihilistNumberGrid.Children.Add(numBlock);
                    }
                }
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