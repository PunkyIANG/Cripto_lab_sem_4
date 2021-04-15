using System;
using System.Printing;

namespace Laborator_2
{
    public abstract class Fighter
    {
        public int HitPoints;

        public virtual void PrintStats()
        {
            Console.WriteLine("HitPoints: " + HitPoints + '\n');
        }
    }

    public class Warrior : Fighter
    {
        public int Fury;

        public override void PrintStats()
        {
            base.PrintStats();
            
            Console.WriteLine("Fury: " + Fury + '\n');
        }
    }
    
    public class Mage : Fighter
    {
        public int Mana;

        public override void PrintStats()
        {
            base.PrintStats();
            
            Console.WriteLine("Mana: " + Mana + '\n');
        }
    }
}