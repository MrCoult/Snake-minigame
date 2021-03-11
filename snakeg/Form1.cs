using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snakeg
{
    public partial class Form1 : Form
    {

        private List<Cerc> Snake = new List<Cerc>();
        private Cerc food = new Cerc();




        public Form1()
        {
            InitializeComponent();

            //setările default
            new Setari();
            // setare viteză joc şi timer start

            gameTimer.Interval = 1000 / Setari.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            // start joc nou
            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;
            //caracter jucător nou
            new Setari();

            Snake.Clear();
            Cerc head = new Cerc();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);


            lblScore.Text = Setari.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            //plasează random în joc "mâncarea"

            int maxPosX = sgCanvas.Size.Width / Setari.Width;
            int maxPosY = sgCanvas.Size.Height / Setari.Height;

            Random random = new Random();
            food = new Cerc();
            food.X = random.Next(0, maxPosX);
            food.Y = random.Next(0, maxPosY);

        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            // verifică dacă jocul s-a terminat

            if (Setari.GameOver)
            {
                //verifică dacă enter e apăsat
                if (Inputs.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }

            else
            {
                if (Inputs.KeyPressed(Keys.Right) && Setari.direction != Directions.Left)
              
                    Setari.direction = Directions.Right;

                 else if (Inputs.KeyPressed(Keys.Left) && Setari.direction != Directions.Right)
               
                    Setari.direction = Directions.Left;
                
                else if (Inputs.KeyPressed(Keys.Up) && Setari.direction != Directions.Down)
                
                    Setari.direction = Directions.Up;
                
                else if (Inputs.KeyPressed(Keys.Down) && Setari.direction != Directions.Up)
                
                    Setari.direction = Directions.Down;

                    MovePlayer();
                }

            //de fiecare dată când UpdateScreen() = call, sgCanvas = refresh

            sgCanvas.Invalidate();
            }

        private void Form1_Load(object sender, System.EventArgs e)
        {
           
        }

        private void SgCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

           

            if (Setari.GameOver != true)
            {
                //Setează culoarea şarpelui
                Brush snakeColor;
                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.Black; // capul

                    else
                        snakeColor = Brushes.Green; // restul corpului

                    //desenează şarpele

                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Setari.Width,
                        Snake[i].Y * Setari.Height,
                        Setari.Width, Setari.Height));

                    // desenează mâncarea

                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Setari.Width,
                        food.Y * Setari.Height, Setari.Width, Setari.Height));

                }

            }
            else
            {

                string gameOver = "Game over \nYour final score is: " + Setari.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                // move head
                if(i == 0)
                {
                    switch (Setari.direction)
                    {
                        case Directions.Right:
                        Snake[i].X++;
                        break;

                        case Directions.Left:
                            Snake[i].X--;
                            break;

                        case Directions.Up:
                            Snake[i].Y--;
                            break;

                        case Directions.Down:
                            Snake[i].Y++;
                            break;



                    }

                    int maxPosX = sgCanvas.Size.Width / Setari.Width;
                    int maxPosY = sgCanvas.Size.Height / Setari.Height;

                    // Detectează coleziunea cu marginile jocului
                    //dacă iese în afara minimului/maximului marginilor, moare

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxPosX || Snake[i].Y >= maxPosY)
                    {
                       Die();
                    }

                    // detectează coleziunea cu corpul
                    // dacă orice cerc din corpul şarpelui este în aceeaşi poziţie X/Y cu alt cerc, moare

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                           Die();
                        }
                    }

                    // detectează coleziunea cu mâncarea 
                    // dacă capul şarpelui e în aceeaşi poziţie X/Y ca şi mâncarea, va mânca 
                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                      Eat();
                    }

                }
                else
                {
                    //move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            //adaugă cerc în corpul şarpelui
            Cerc food = new Cerc();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            //modifică scorul
            // de fiecare dată când cercul e mâncat, scorul creşte cu 30p
            Setari.Score += Setari.Points;
            lblScore.Text = Setari.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Setari.GameOver = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Inputs.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Inputs.ChangeState(e.KeyCode, false);
        }

        private void sgCanvas_Click(object sender, EventArgs e)
        {

        }
    }
  }


