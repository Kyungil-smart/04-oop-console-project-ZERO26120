using System;

class Program
{
    static void Main()
    {
        while(true)
        {
            int sel=Title();
            if(sel==0)
            {
                Player p=JobSelect();
                Town.Enter(p);
            }
            else break;
        }
    }

    static int Title()
    {
        string[] menu={ "게임 시작","게임 종료" };
        int index=0;

        while(true)
        {
            Console.Clear();
            Console.ForegroundColor=ConsoleColor.Yellow;
            Console.WriteLine(@"
 ████████╗██████╗ ███████╗████████╗
 ╚══██╔══╝██╔══██╗██╔════╝╚══██╔══╝
    ██║   ██████╔╝█████╗     ██║   
    ██║   ██╔══██╗██╔══╝     ██║   
    ██║   ██║  ██║███████╗   ██║   
    ╚═╝   ╚═╝  ╚═╝╚══════╝   ╚═╝   

 ██████╗ ██████╗  ██████╗ 
 ██╔══██╗██╔══██╗██╔════╝ 
 ██████╔╝██████╔╝██║  ███╗
 ██╔══██╗██╔═══╝ ██║   ██║
 ██║  ██║██║     ╚██████╔╝
 ╚═╝  ╚═╝╚═╝      ╚═════╝ 
");
            Console.ResetColor();

            for(int i=0;i<menu.Length;i++)
            {
                if(i==index)
                { Console.ForegroundColor=ConsoleColor.Cyan; Console.WriteLine($"> {menu[i]}"); Console.ResetColor(); }
                else Console.WriteLine($"  {menu[i]}");
            }

            Console.WriteLine("\n↑ ↓ 이동  Enter 선택");
            var key=Console.ReadKey(true).Key;
            if(key==ConsoleKey.UpArrow) index--;
            if(key==ConsoleKey.DownArrow) index++;
            if(index<0) index=menu.Length-1;
            if(index>=menu.Length) index=0;
            if(key==ConsoleKey.Enter) return index;
        }
    }

    static Player JobSelect()
    {
        Console.Clear();
        Console.WriteLine("직업 선택:");
        Console.WriteLine("1. 전사  2. 마법사");

        ConsoleKeyInfo key = Console.ReadKey(true);
        if(key.KeyChar=='2') return new Player(PlayerJob.Mage);
        return new Player(PlayerJob.Warrior);
    }
}