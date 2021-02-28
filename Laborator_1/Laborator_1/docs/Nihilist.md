# Cifrul Nihilist

Pentru a cripta un mesaj avem nevoie de text clar, cheie al alfabetului și cheie de criptare. În primul rând formăm careul Polybios 5x5 sau 6x6, folosind cheia alfabetului ca primele caractere. Mai departe convertim caracterele cheii de criptare în numere, folosind numărele de ordine a coloanelor și rândurilor în alfabet. Astfel, în cazul cheii de alfabet SIMPLE și cheii de criptare EASY obținem convertirea:

`E -> 21, `
`A -> 22, `
`S -> 11, `
`Y -> 54`

În continuare convertim astfel textul clar și îl aranjăm într-un tabel de la al doilea rând, lățimea căruia este egală cu lungimea cheii de criptare, ocupând însă prima linie cu numerele cheii de criptare. În final, mărim valoarea fiecărei casete din a 2-lea rând și mai departe cu valoarea casetei din primul rând și coloana corespunzătoare, și returnăm aceste numere ca text criptat.

Pentru a decripta executăm algoritmul invers: formăm alfabetul Polybios și convertim cheia de criptare, apoi scădem din fiecare număr valoarea numărului corespunzător din cheie, și convertim seria obținută înapoi în caractere.