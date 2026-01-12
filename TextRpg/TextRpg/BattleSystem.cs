using System;
using System.Collections.Generic;

static class BattleSystem
{
    static Random r = new Random();

    const int LogHeight = 10;

    const int RowPlayer = 0;
    const int RowEnemy  = 1;
    const int RowLog    = 2;
    const int RowMenu   = 12;

    public static bool Start(Player player, Enemy enemy, int floor, int fightIndex, int fightsThisFloor)
    {
        var log = new List<string>();
        AddLog(log, $"=== {floor}층 전투 {fightIndex}/{fightsThisFloor} 시작 ===");

        while (player.Hp > 0 && enemy.Hp > 0)
        {
            ApplyStatus(player.Statuses, isPlayer: true, player: player, enemy: null, log: log);
            if (player.Hp <= 0) break;

            Render(player, enemy, log);

            string[] actions = { "공격", "스킬", "도주" };
            int actionIndex = SelectMenu(actions, RowMenu);

            if (actionIndex == 0)
            {
                PlayerAttack(player, enemy, log);
            }
            else if (actionIndex == 1)
            {
                PlayerUseSkill(player, enemy, log);
            }
            else
            {
                if (r.Next(100) < 30)
                {
                    AddLog(log, "도주 성공!");
                    Render(player, enemy, log);
                    Console.ReadKey(true);
                    return true;
                }
                AddLog(log, "도주 실패!");
            }

            if (enemy.Hp <= 0) break;

            ApplyStatus(enemy.Statuses, isPlayer: false, player: null, enemy: enemy, log: log);
            if (enemy.Hp <= 0) break;

            MonsterAction(player, enemy, log);
        }

        if (player.Hp <= 0)
        {
            Console.Clear();
            Console.WriteLine("패배...");
            Environment.Exit(0);
        }

        if (enemy.Hp <= 0)
        {
            enemy.Hp = 0;
            AddLog(log, $"{enemy.Name} 처치!");
            player.GainExp(enemy.Exp, log);
            Render(player, enemy, log);
            Console.ReadKey(true);
        }

        return false;
    }

    static void Render(Player player, Enemy enemy, List<string> log)
    {
        int width = SafeWidth();
        Console.Clear();

        WriteLineAt(RowPlayer, $"플레이어: {player.Name}  HP: {player.Hp}/{player.MaxHp}  ATK: {player.Attack}", width);
        WriteLineAt(RowEnemy,  $"{enemy.Name}  HP: {enemy.Hp}/{enemy.MaxHp}", width);

        int start = Math.Max(0, log.Count - LogHeight);
        for (int i = 0; i < LogHeight; i++)
        {
            int row = RowLog + i;
            if (start + i < log.Count) WriteLineAt(row, log[start + i], width);
            else WriteLineAt(row, "", width);
        }

        for (int i = 0; i < 3; i++)
            WriteLineAt(RowMenu + i, "", width);
    }

    static int SelectMenu(string[] options, int startRow)
    {
        int index = 0;
        int width = SafeWidth();

        while (true)
        {
            for (int i = 0; i < options.Length; i++)
            {
                string line = (i == index) ? $"> {options[i]}" : $"  {options[i]}";
                WriteLineAt(startRow + i, line, width);
            }

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.DownArrow) index = (index + 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.Enter) return index;
        }
    }

    static void PlayerAttack(Player player, Enemy enemy, List<string> log)
    {
        int dmg = player.TotalAttack();
        enemy.Hp -= dmg;
        AddLog(log, $"플레이어 기본 공격! -{dmg} 피해");
    }

    static void PlayerUseSkill(Player player, Enemy enemy, List<string> log)
    {
        int width = SafeWidth();
        int index = 0;
        int menuTop = RowMenu;

        while (true)
        {
            int lines = 1 + player.Skills.Count;

            for (int i = 0; i < lines; i++)
                WriteLineAt(menuTop + i, "", width);

            WriteLineAt(menuTop, "스킬 선택:", width);

            for (int i = 0; i < player.Skills.Count; i++)
            {
                Skill sk = player.Skills[i];
                string line = (i == index)
                    ? $"> {sk.Name} ({sk.PP}/{sk.MaxPP})"
                    : $"  {sk.Name} ({sk.PP}/{sk.MaxPP})";
                WriteLineAt(menuTop + 1 + i, line, width);
            }

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) index = (index - 1 + player.Skills.Count) % player.Skills.Count;
            else if (key == ConsoleKey.DownArrow) index = (index + 1) % player.Skills.Count;
            else if (key == ConsoleKey.Enter)
            {
                Skill sk = player.Skills[index];

                if (sk.PP <= 0)
                {
                    AddLog(log, "PP 부족!");
                }
                else
                {
                    sk.PP--;
                    int dmg = (int)(player.TotalAttack() * sk.Rate);
                    enemy.Hp -= dmg;
                    AddLog(log, $"플레이어가 {sk.Name} 사용! -{dmg} 피해");

                    if (sk.Effect != null && r.Next(100) < sk.Chance)
                    {
                        enemy.Statuses.Add(new Status(sk.Effect.Value, 3));
                        AddLog(log, $"{enemy.Name} 상태이상! {sk.Effect}");
                    }
                }

                for (int i = 0; i < lines; i++)
                    WriteLineAt(menuTop + i, "", width);

                return;
            }
        }
    }

    static void MonsterAction(Player player, Enemy enemy, List<string> log)
    {
        if (enemy is Boss b)
        {
            if (r.Next(100) < 40)
            {
                b.FireBreath(player);
                AddLog(log, "드래곤 화염 브레스!");
            }
            else
            {
                b.DarkStrike(player);
                AddLog(log, "드래곤 피어!");
            }
        }
        else
        {
            player.Hp -= enemy.Attack;
            AddLog(log, $"{enemy.Name} 공격 -{enemy.Attack}");
        }
    }

    static void ApplyStatus(List<Status> statuses, bool isPlayer, Player player, Enemy enemy, List<string> log)
    {
        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            Status s = statuses[i];
            int dmg = s.GetDamage();

            if (isPlayer) player.Hp -= dmg;
            else enemy.Hp -= dmg;

            AddLog(log, $"{s.Effect} 피해 -{dmg}");

            s.Turns--;
            if (s.Turns <= 0) statuses.RemoveAt(i);
        }
    }

    static void AddLog(List<string> log, string msg)
    {
        log.Add(msg);
        if (log.Count > 200) log.RemoveAt(0);
    }

    static void WriteLineAt(int row, string text, int width)
    {
        Console.SetCursorPosition(0, row);
        Console.Write(text.PadRight(width));
    }

    static int SafeWidth()
    {
        return Math.Max(1, Console.WindowWidth);
    }
}
