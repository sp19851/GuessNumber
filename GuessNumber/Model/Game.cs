using System;
using System.Diagnostics;
using System.IO;

namespace GuessNumber.Model
{
    public class Game
    {
        private readonly Random random = new Random();
        private int Attempts { get; set; }
        private int Number { get; set; }
        private bool IsNewPlayer { get; set; } 
        private string[] games; //"player: Ваня games: game1: win/2 game2: faild/5;"
        private string[] players; //"player: Ваня games: game1: win/2 game2: faild/5;"
        private string path = @"games.txt";
        private Player player { get; set; }

        public Game(Player player)
        {
            this.player = player;
            IsNewPlayer = false;
            if (File.Exists(path)) games = File.ReadAllLines(path);
            else games = new string[3];
        }
        public bool Wellcome()
        {
            var playersLength = 0;
            foreach (var g in games)
            {
                Debug.WriteLine($"Wellcome g {g}");
                if (g != null && g.StartsWith("player:")) playersLength++;
            }
            if (playersLength > 0)
            {
                players = new string[playersLength];
                var playersIndex = 0;
                for (int i = 0; i < games.Length; i++)
                {
                    if (games[i].StartsWith("player:"))
                    {
                        players[playersIndex++] = games[i].Remove(0, 7);
                        Debug.WriteLine($"player {players[playersIndex-1]}");
                    }
                }

            } else players = new string[1];
            
            //Debug.WriteLine($"player 0 {players[0]}");
            if (players[0] != null)
            {
                foreach (var p in players)
                {
                    if (p.Trim() == player.Name)  return true;
                }                
            }
            IsNewPlayer = true;
            return false;
        }

        public void SetAttempts(int value)
        {
            Attempts = value;
        }
        public void StartGame()
        {
            Number = random.Next(1, 101);
            Debug.WriteLine($"Number - {Number}");
        }

        public void FinishGame(bool value)
        {
            Debug.WriteLine($"FinishGame - {value}");
            
            //новый файл
            if (players[0] == null)
            {
                Debug.WriteLine($"FinishGame - новый файл");
                //Array.Resize(ref games, games.Length + 3);
                games[games.Length - 3] = $"player: {player.Name}";
                games[games.Length - 2] = $"games:";
                games[games.Length - 1] = $"game: {value}/{player.Attempt}";
                

                File.WriteAllLines(path, games);
                Debug.WriteLine($"WriteAllLines1 - {games}");
                //Ваня games: game1: win/2 game2: faild/5;"
            }
            else
            {
                if (IsNewPlayer)
                {
                    Array.Resize(ref games, games.Length + 3);
                    games[games.Length - 3] = $"player: {player.Name}";
                    games[games.Length - 2] = $"games:";
                    games[games.Length - 1] = $"game: {value}/{player.Attempt}";
                    
                }
                else
                {
                    var startPos = 0;
                    var recordAdded = false;
                    for (int i = 0; i < games.Length; i++)
                    {
                        if (games[i].StartsWith("player") && games[i].Remove(0, 7).Trim() == player.Name)
                        {
                            Debug.WriteLine($"WriteAllLines ++++");
                            if (startPos == 0) startPos = i;                          
                        }
                        else if (games[i].StartsWith("player") && startPos>0)
                        {
                            Debug.WriteLine($"startPos {startPos}");
                            recordAdded = AddRecord(value, i-1);
                            break;
                        }
                    }

                    if (!recordAdded) AddRecord(value, games.Length);
                }
                foreach (var g in games)
                {
                    Debug.WriteLine($"WriteAllLines2 - {g}");
                }
                File.WriteAllLines(path, games);
                
            }
        }

        private bool AddRecord(bool value, int index)
        {            
            Debug.WriteLine($"AddRecord {value} {index}");
            Array.Resize(ref games, games.Length + 1);
            games[index] = $"game: {value}/{player.Attempt}";
            return true;
        }

        public (bool, string) CheckNumber(int value, int attempt)
        {
            Debug.WriteLine($"Попытка {attempt} из {Attempts}");
            if (value > Number)
            {
                if (Attempts > attempt) return (true, "Меньше");
                if (Attempts == attempt)
                {
                    FinishGame(false);
                    return (false, "Попытки закончены, Вы проиграли!");
                }
            }
            if (value < Number)
            {
                if (Attempts > attempt) return (true, "Больше");
                if (Attempts == attempt)
                {
                    FinishGame(false); 
                    return (false, "Попытки закончены, Вы проиграли!");
                }
            }
            if (Attempts < attempt) 
            {
                FinishGame(false);
                return (false, "Попытки закончены, Вы проиграли!");
            }
            FinishGame(true);
            return (false, $"В точку!{Environment.NewLine}Вы выйграли!!!");
        }
    }
}
