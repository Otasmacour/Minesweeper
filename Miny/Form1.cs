using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Miny
{
    public partial class Form1 : Form
    {
        Game game;
        public Form1()
        {
            InitializeGame();
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            using (var dimensionsForm = new DimensionsForm())
            {
                if (dimensionsForm.ShowDialog() == DialogResult.OK)
                {
                    int width = dimensionsForm.SelectedWidth;
                    int height = dimensionsForm.SelectedHeight;
                    int percentOfMines = dimensionsForm.SelectedPercentOfMines;
                    game = new Game(width, height,10);
                    InitializeMyComponents(game.twoDArray, game.minesLeft);
                    this.Controls.Remove(startButton);
                }
            }
        }
        private void VictoryCheck()
        {
            (bool end, bool victory) result = game.VictoryCheck();
            if(result.end == false) { return; }
            if(result.victory)
            {
                resetButton.Text = "You won! \n Reset";
            }
            else
            {
                resetButton.Text = "You lost... \n Reset";

            }
            InitializeResetButton();
            resetButton.Location = new Point((game.twoDArrayWidth * labels[0].Size.Width / 2) - 75, this.Size.Height / 2 - 75);
            resetButton.BringToFront();
        }
        private void GameReset(object sender, EventArgs e)
        {
            foreach(Label label in labels)
            {
                this.Controls.Remove(label);
            }
            labels.Clear();
            game = new Game(game.twoDArrayHeight, game.twoDArrayWidth, game.percentOfMines);
            InitializeLabels(game.twoDArray);
            //ReSize();
            game.run = true;
            this.Controls.Remove(resetButton);
        }
        protected void ReSize(object sender, EventArgs e)
        {
            int x = this.Size.Width - 10;
            int y = this.Size.Height - 60;
            int sizeOfLabel = labels[0].Size.Width;
            int newSizeOfLabel = sizeOfLabel;
            if (((sizeOfLabel) * game.twoDArrayHeight < y  && (sizeOfLabel) * game.twoDArrayWidth < x) || ((sizeOfLabel ) * game.twoDArrayHeight > y || (sizeOfLabel ) * game.twoDArrayWidth > x))
            {
                if(y > x)
                {
                    newSizeOfLabel = x / game.twoDArrayHeight;
                }
                else
                {
                    newSizeOfLabel = y / game.twoDArrayHeight;
                }     
            }
            LabelsSizeUpdate(newSizeOfLabel, sizeOfLabel, game.twoDArrayHeight, game.twoDArrayWidth);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Image ResizeImage(Image image, int width, int height)
        {
            Bitmap newImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return newImage;
        }
        private void label1_Click(object sender, MouseEventArgs e)
        {
            if (game.run == false) { return; }
            Label l = (Label)sender;
            Node node = game.GetNode(int.Parse(l.Tag.ToString()));
            if (e.Button == MouseButtons.Left)
            {
                if(node.marked)
                {
                    node.marked = false;
                    l.Image = null;
                    game.minesLeft++;
                }
                if (node.mine)
                {
                    l.Image = Properties.Resources.mine;
                    l.Image = ResizeImage(Properties.Resources.mine, l.Width, l.Height);
                    game.someMineExpoloded = true;
                }
                else
                {
                    l.BackColor = Color.White;
                    l.Text = node.numberOfMinesAround.ToString();
                    //if(node.numberOfMinesAround == 0)
                    //{
                    //    game.Wawe(node);
                    //}
                }
                if(node.exposed == false)
                {
                    node.exposed = true;
                    game.numberOfExposed++;
                }
            }
            else if (e.Button == MouseButtons.Right && node.marked == false && node.exposed == false)
            {
                node.marked = true;
                l.Image = Properties.Resources.flag;
                l.Image = ResizeImage(Properties.Resources.flag, l.Width, l.Height);
                game.minesLeft--;
            }
            bombCountLabel.Text = "Mines left: " + game.minesLeft.ToString();
            //debugLabel.Text = "Exposed: " + game.numberOfExposed.ToString();
            VictoryCheck();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}