using System;
using System.Collections.Generic;

namespace Laborator_2
{
    public class Stuff
    {
        public void TestStuff()
        {
            string[] fullStringArr = new string[]
            {
                "раз",
                "первый класс"
                //ну и остальные строки сюда
            };
            
            var rnd = new Random();
            var copyList = new List<string>();    
            copyList.CopyTo(fullStringArr);    //копируем элементы в список

            for (int i = 0; i < 5; i++)    //фор тут просто так сидит, в твоём случае может быть while true пока инпут не придёт
            {
                while (copyList.Count != 0)    
                {
                    var elementId = rnd.Next(copyList.Count);
                    var extractedElement = copyList[elementId];
                    copyList.RemoveAt(elementId);
                    
                    //уже делаешь что хочешь с элементом
                }
                
                copyList.CopyTo(fullStringArr);    //заполняем пустой список
            }
        }
    }
}