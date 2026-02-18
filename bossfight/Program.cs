// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel.Design;
using System.Data;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

var rng = new Random();
bool keepPlaying = true;
bool success = false;
string name = "";
while (success == false)
{
    Console.Write("Please write your name before we start:");
    name = Console.ReadLine()?.ToLower().Trim();
    if (name.Length <= 15 && !string.IsNullOrWhiteSpace(name) && name.Length >= 3)
    {
        Console.WriteLine("Your name is eligable. Success!");
        success = true;
        break;
    }
    else
    {
        Console.WriteLine("Inavalid name try again!");
    }
}
Console.WriteLine("Do you want to start the game?(y/n)");
string start = Console.ReadLine()?.ToLower().Trim();
if(start == "y" || start == "yes")
{
    keepPlaying = true;
}
else if (start == "n" || start == "no")
{
    keepPlaying = false;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Thanks for playing!");
    Console.ResetColor();
}
else
{
    Console.WriteLine("Invalid input, exiting the game.");
    keepPlaying = false;
}
while (keepPlaying == true)
{
    knight player = new knight (name, 100, 100, 10, 40);
    knight boss = new knight ("The Shadow",150, 150, 10, 40);
    Console.WriteLine("Okay we are starting the game!");
    Console.ForegroundColor= ConsoleColor.Yellow;
    Console.WriteLine($"While exploring the dungeon {name} encounters a strong enemy named: {boss.Name}.\n{name} is first to attack!");
    Console.ResetColor();
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine($"{name} hp is {player.HP}. {boss.Name} hp is {boss.HP}");
    Console.WriteLine("-------------------------------------------");
    Console.ResetColor();
    Console.WriteLine($"How will {name} attack?\n1 = magic\n2 = melee");
    bool win = false;
    while (win == false)
    {
        if (player.HP <= 0 || boss.HP <= 0)
        {
            win = true;
            break;
        }
        var attack = Console.ReadLine();
        switch (attack)
        {
            case "1":
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"{name} chose magic. What spell will you use?");
                Console.WriteLine("1 = ziodyne(high electro damage) \n2 = agidyne(high fire damage) \n3 = bufudyne(high ice damage)  \n4 = diarama(moderate heal)");
                Console.ResetColor();
                var spell = Console.ReadLine();
                switch (spell)
                {
                    case "1":
                        player.CastSpell("ziodyne", boss);
                        break;
                    case "2":
                        player.CastSpell("agidyne", boss);
                        break;
                    case "3":
                        player.CastSpell("bufudyne", boss);
                        break;
                    case "4":
                        player.CastSpell("diarama", boss);
                        break;
                    default:
                        player.CastSpell("invalid", boss);
                        break;
                }
                break;
            case "2":
                player.PerformMelee(boss);
                break;
            default:
                player.InvalidCase(boss);
                break;     
        }
        Displayhp(name, boss.Name, player.HP, boss.HP, player.LastDamageDealt, player.LastDamageTaken, player.LastHealAmount);
    }


    if (boss.HP <= 0)
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {player.HP}. Boss hp is 0");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Congrats you win!");
        Console.ResetColor();
    }
    else if (player.HP <= 0)
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is 0. Boss hp is {boss.HP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("You lose! Better luck next time!");
        Console.ResetColor();
    }
    Console.WriteLine("Do you want to start a new game?(y/n)");
    string answer = Console.ReadLine()?.ToLower().Trim();
    if (answer == "y" || answer == "yes")
    {
        keepPlaying = true;
        Console.Clear();
    }
    else if (answer == "n" || answer == "no")
    {
        keepPlaying = false;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Thanks for playing!");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("Invalid input, exiting the game.");
        keepPlaying = false;
    }
}
Console.ReadKey();
static void Displayhp(string name, string bossname, int playerHP, int bossHP, int playerdamage, int bossdamage, int heal)
{
    Console.Clear();
    if (heal > 0)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"{name} healed {heal}. {bossname} dealt {bossdamage} while {name} was healing.");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.WriteLine($"How will {name} attack?\n1 = magic\n2 = melee");
    }
    else if (playerdamage == 0)
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Invalid input. {bossname} dealt {bossdamage} while {name} was thinking.");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.WriteLine($"How will {name} attack?\n1 = magic\n2 = melee");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{name} dealt {playerdamage}. {bossname} dealt {bossdamage} in exchange");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.WriteLine($"How will {name} attack?\n1 = magic\n2 = melee");
    }
}

class knight
{
    public string Name;
    public int HP;
    public int MaxHP;
    public int MinDamage = 10;
    public int MaxDamage = 40;
    private Random rng = new Random();
    public int LastDamageDealt = 0 ;
    public int LastDamageTaken = 0;
    public int LastHealAmount = 0;
    public knight(string name, int hp,int maxhp, int MinD, int MaxD)
    {
        Name = name;
        HP = hp;
        MaxHP = maxhp;
        MinDamage = MinD;
        MaxDamage = MaxD;
    }
    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP < 0)
        {
            HP = 0;
        }
    }
    public int CalculateAttack()
    {
        return rng.Next(MinDamage, MaxDamage + 1);
    }
    public int Heal(int amount2)
    {
        HP += amount2;
        if (HP > 100)
        {
            HP = 100;
        }
        return HP;
    }
    public void CastSpell(string spellname, knight target)
    {
        int damage = 0;
        int damage1 = 0;
        int healAmount = 0;
        switch (spellname)
        {
            case "ziodyne":
                damage = CalculateAttack() + 5;
                target.TakeDamage(damage);
                damage1 = target.CalculateAttack() + 3;
                this.TakeDamage(damage1);
                break;
            case "agidyne":
                damage = CalculateAttack() + 8;
                target.TakeDamage(damage);
                damage1 = target.CalculateAttack();
                this.TakeDamage(damage1);
                break;
            case "bufudyne":
                damage = CalculateAttack() + 9;
                target.TakeDamage(damage);
                damage1 = target.CalculateAttack() + 6;
                this.TakeDamage(damage1);
                break;
            case "diarama":
                healAmount = rng.Next(15, 28);
                this.Heal(healAmount);
                damage1 = target.CalculateAttack() - 10;
                this.TakeDamage(damage1);
                break;
            default:
                damage = 0;
                damage1 = target.CalculateAttack() + 5;
                this.TakeDamage(damage1);
                break;
        }
        this.LastDamageDealt = damage;
        this.LastDamageTaken = damage1;
        this.LastHealAmount = healAmount;
    }
    public void PerformMelee(knight target)
    {
        LastHealAmount = 0;
        int damage = CalculateAttack() - 10;
        target.TakeDamage(damage);
        int damage1 = target.CalculateAttack();
        this.TakeDamage(damage1);
        LastDamageDealt = damage;
        LastDamageTaken = damage1;
    }
    public void InvalidCase(knight target)
    {
        LastDamageDealt = 0;
        LastHealAmount = 0;
        int damage1 = target.CalculateAttack();
        this.TakeDamage(damage1);
        LastDamageTaken = damage1;
    }
}