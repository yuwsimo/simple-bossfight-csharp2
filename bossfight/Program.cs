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
    knight player = new knight (name, 100, 100, 100, 100, 10, 40);
    knight boss = new knight ("The Shadow",150, 100, 150, 100, 10, 40);
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
                bool successfullCast = false;
                while (successfullCast == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"{name} chose magic. What spell will you use?");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{name} has {player.SP} SP!");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("1 = ziodyne(electro damage) - 10sp \n2 = agidyne(fire damage) - 15sp \n3 = bufudyne(ice damage) - 20sp  \n4 = diarama(heal) - 15sp \nb = back");
                    Console.ResetColor();
                    var spell = Console.ReadLine()?.ToLower().Trim();

                    if (spell == "b" || spell == "back")
                    {
                        player.LastDamageDealt = 0;
                        player.LastHealAmount = 0;
                        player.LastDamageTaken = 0;
                        break;
                    }
                    string spellCast = spell switch
                    {
                        "1" => "ziodyne",
                        "2" => "agidyne",
                        "3" => "bufudyne",
                        "4" => "diarama",
                        _ => "invalid"
                    };
                    if (spellCast == "invalid")
                    {
                        Console.WriteLine("Invalid input. Try again!");
                        continue;
                    }
                    successfullCast = player.CastSpell(spellCast, boss);
                    if (!successfullCast)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Not enough SP to cast that spell. Try again!");
                        Console.ResetColor();
                    }
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


    if (boss.HP <= 0 || player.HP <= 0)
    {
        Displayhp(name, boss.Name, player.HP, boss.HP, player.LastDamageDealt, player.LastDamageTaken, player.LastHealAmount);
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
    else if (playerdamage == 0 && bossdamage == 0)
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{name} decided to return to attack selection.");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.WriteLine($"How will {name} attack?\n1 = magic\n2 = melee");
    }
    else if (playerHP <= 0 && bossHP <= 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{name} dealt {playerdamage}. {bossname} dealt {bossdamage} in exchange");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("-----------------");
        Console.WriteLine($"{name} managed to deal a finishing blow right before death! {bossname} is heroically defeated by {name}!");
        Console.WriteLine("-----------------");
        Console.ResetColor();
    }
    else if (bossHP <= 0)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{name} dealt {playerdamage}. {bossname} dealt {bossdamage} in exchange");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("-----------------");
        Console.WriteLine("Congrats you win!");
        Console.WriteLine("-----------------");
        Console.ResetColor();
    }
    else if (playerHP <= 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{name} dealt {playerdamage}. {bossname} dealt {bossdamage} in exchange");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine($"{name} hp is {playerHP}. {bossname} hp is {bossHP}");
        Console.WriteLine("-------------------------------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("-----------------");
        Console.WriteLine("You lose! Better luck next time!");
        Console.WriteLine("-----------------");
        Console.ResetColor();
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
    public int SP;
    public int MaxSP = 100;
    public int MaxHP;
    public int MinDamage = 10;
    public int MaxDamage = 40;
    private Random rng = new Random();
    public int LastDamageDealt = 0 ;
    public int LastDamageTaken = 0;
    public int LastHealAmount = 0;
    public knight(string name, int hp, int sp, int maxhp, int maxsp, int MinD, int MaxD)
    {
        Name = name;
        HP = hp;
        SP = sp;
        MaxHP = maxhp;
        MaxSP = maxsp;
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
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        return HP;
    }
    public bool CastSpell(string spellname, knight target)
    {
        int damage = 0;
        int damage1 = 0;
        int healAmount = 0;
        int SPCost = 0;  
        
        if (spellname == "ziodyne")
        {
            SPCost = 10;
        }
        else if (spellname == "agidyne")
        {
            SPCost = 15;
        }
        else if (spellname == "bufudyne")
        {
            SPCost = 20;
        }
        else if (spellname == "diarama")
        {
            SPCost = 15;
        }
        if (this.SP < SPCost)
        {
            return false;
        }
        this.SP -= SPCost;
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
                damage1 = (int)(target.CalculateAttack() * 0.5);
                this.TakeDamage(damage1);
                break;
            default:
                return false;        
        }
        this.LastDamageDealt = damage;
        this.LastDamageTaken = damage1;
        this.LastHealAmount = healAmount;
        return true;
    }
    public void PerformMelee(knight target)
    {
        LastHealAmount = 0;
        int damage = (int)(CalculateAttack() * 0.5);
        target.TakeDamage(damage);
        int damage1 = target.CalculateAttack();
        this.TakeDamage(damage1);
        LastDamageDealt = damage;
        LastDamageTaken = damage1;
        this.SP += 10;
        if (MaxSP < this.SP)
        {
            this.SP = MaxSP;
        }
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