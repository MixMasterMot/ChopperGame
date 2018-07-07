using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chopper
{
    public partial class Form1 : Form
    {
        //game variables
        bool goUp; //boolean to aloow player to go up
        bool goDown; //boolean to allow player to go down
        int score = 0; // player score
        int speed = 8; // speed of pbstacales and ufos
        Random rand = new Random(); // generate random number
        int playerSpeed = 8; //how fast the player moves
        int index; // empty int to change ufos
        List<PictureBox> bullets = new List<PictureBox>(); //list of fired bullets
        int tickCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void gameTick(object sender, EventArgs e)
        {
            tickCount = tickCount + 1;
            if (tickCount >= 100)
            {
                tickCount = 0;
                speed = speed + 1;
            }

            pillar1.Left -= speed;
            pillar2.Left -= speed;
            ufo.Left -= speed;
            label1.Text = "Score: " + score;

            if (goUp && player.Top > 0)
            {
                player.Top -= playerSpeed;
            }

            if (goDown && player.Bottom < 400)
            {
                player.Top += playerSpeed;
            }

            if (pillar1.Left < -150)
            {
                setPillar(pillar1);
            }

            if (pillar2.Left < -150)
            {
                setPillar(pillar2);
            }

            if(ufo.Left<-5||
                player.Bounds.IntersectsWith(ufo.Bounds) ||
                player.Bounds.IntersectsWith(pillar1.Bounds) ||
                player.Bounds.IntersectsWith(pillar2.Bounds))
            {
                gameTimer.Stop();
                DialogResult dialogResult = MessageBox.Show("You got a score of: " + score + Environment.NewLine + "Retry ?", "You Died", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Environment.Exit(0);
                }
            }

            for(int i = bullets.Count-1; i >= 0; i--)
            {
                PictureBox bullet = bullets[i];
                checkBullets(bullet);
            }
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if(e.KeyData == Keys.Space && bullets.Count<3)
            {
                makeBullet();
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void changeUfo()
        {
            index += 1;
            if (index > 3)
            {
                index = 1;
            }
            switch (index)
            {
                case 1:
                    ufo.Image = Properties.Resources.alien1;
                    break;
                case 2:
                    ufo.Image = Properties.Resources.alien2;
                    break;
                case 3:
                    ufo.Image = Properties.Resources.alien3;
                    break;
            }
        }

        private void makeBullet()
        {
            PictureBox bullet = new PictureBox();
            bullet.BackColor = Color.DarkOrange;
            bullet.Height = 5;
            bullet.Width = 10;
            bullet.Left = player.Left + player.Width;
            bullet.Top = player.Top + player.Height / 2;
            bullet.Tag = "Bullet";
            this.Controls.Add(bullet);
            bullets.Add(bullet);
        }

        private void removeBullet(PictureBox bullet)
        {
            this.Controls.Remove(bullet);
            bullet.Dispose();
            bullets.Remove(bullet);
        }

        private void resetUFO()
        {
            ufo.Left = 1000;
            ufo.Top = rand.Next(50, 330) - ufo.Height;
            changeUfo();
        }

        private void restart()
        {
            score = 0;
            speed = 8;
            tickCount = 0;
            foreach(PictureBox bullet in bullets)
            {
                removeBullet(bullet);
            }
            player.Top = 146;
            setPillar(pillar1);
            setPillar(pillar2);
            resetUFO();
            gameTimer.Start();
            goUp = false;
            goDown = false;
        }

        private void setPillar(PictureBox pillar)
        {
            pillar.Left = rand.Next(800, 1200);
        }

        private void checkBullets(PictureBox bullet)
        {
            bullet.Left += 15;
            if (bullet.Left > 900)
            {
                removeBullet(bullet);
            }

            if (bullet.Bounds.IntersectsWith(ufo.Bounds))
            {
                score += 1;
                removeBullet(bullet);
                resetUFO();
            }
        }
    }
}
