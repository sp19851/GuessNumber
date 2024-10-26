using GuessNumber.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuessNumber
{
    public partial class Form1 : Form
    {
        public Game game;
        public Player player;

        public Form1()
        {
            InitializeComponent();
            ResetGame();
        }

        private void ResetGame()
        {
            NameLabel.Text = "Введите Ваше имя";
            textBoxName.Text = "";
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            switch (NameLabel.Text)
            {
                case "Введите Ваше имя":
                    player = new Player(textBoxName.Text);
                    game = new Game(player);
                    if (game.Wellcome())
                    {
                        MessageBox.Show($"{player.Name}, Вы снова с нами!{Environment.NewLine}Добро пожаловать в игру!", "Давай поиграем!", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show($"{player.Name}, добро пожаловать в игру!", "Давай поиграем!", MessageBoxButtons.OK);
                    };
                    textBoxName.Text = "1";
                    NameLabel.Text = "Введите количество попыток";
                    break;
                case "Введите количество попыток":
                    game.SetAttempts(int.Parse(textBoxName.Text));
                    textBoxName.Text = "1";
                    NameLabel.Text = "Ваш Вариант";
                    game.StartGame();
                    MessageBox.Show($"Число загадано, Ваш ход", "Давай поиграем!", MessageBoxButtons.OK);
                    break;
                case "Ваш Вариант":
                    var result = game.CheckNumber(int.Parse(textBoxName.Text), ++player.Attempt);
                    MessageBox.Show($"{result.Item2}!", "Ваш ход!", MessageBoxButtons.OK);
                    if (!result.Item1) ResetGame();
                    
                    break;
                default:
                    break;
            }

           
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (NameLabel.Text != "Введите Ваше имя")
            {
                try
                {
                    int.Parse(textBoxName.Text);
                }
                catch
                {
                    MessageBox.Show($"{player.Name}, вводить нужно только положительное число!", "Ошибка!", MessageBoxButtons.OK);
                    textBoxName.Text = "1";
                }
            }
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) buttonStart_Click(sender, e); 
        }
    }
}
