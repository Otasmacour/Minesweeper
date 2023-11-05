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
                    game = new Game(width, height,percentOfMines);
                    InitializeMyComponents(game.twoDArray, game.minesLeft);
                    this.Controls.Remove(startButton);
                    AdjustLabelsSize();
                    this.BackgroundImage = null;
                }
            }
        }
        private void RevealingAllMines()
        {
            foreach (Label label in labels)
            {
                Node node = game.GetNode(int.Parse(label.Tag.ToString()));
                if (node.mine)
                {
                    label.Image = Properties.Resources.mine;
                    label.Image = ResizeImage(Properties.Resources.mine, label.Width, label.Height);
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
                RevealingAllMines();
                resetButton.Text = "You lost... \n Reset";

            }
            InitializeResetButton();
            resetButton.Location = new Point((game.twoDArrayWidth * labels[0].Size.Width / 2) - 75, this.Size.Height / 2 - 75);
            resetButton.BringToFront();
        }
        private void GameReset(object sender, EventArgs e)
        {
            
            foreach (Label label in labels)
            {
                this.Controls.Remove(label);
            }
            labels.Clear();
            game = new Game(game.twoDArrayHeight, game.twoDArrayWidth, game.percentOfMines);
            InitializeLabels(game.twoDArray);
            game.run = true;
            this.Controls.Remove(resetButton);
            AdjustLabelsSize();
            bombCountLabel.Text = "Mines left: " + game.minesLeft.ToString();
            
        }
        private void AdjustLabelsSize()
        {
            int x = this.Size.Width - 10;
            int y = this.Size.Height - 60;
            int sizeOfLabel = labels[0].Size.Width;
            int newSizeOfLabel = sizeOfLabel;
            if (((sizeOfLabel) * game.twoDArrayHeight < y && (sizeOfLabel) * game.twoDArrayWidth < x) || ((sizeOfLabel) * game.twoDArrayHeight > y || (sizeOfLabel) * game.twoDArrayWidth > x))
            {
                if (y > x)
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
        protected void ReSize(object sender, EventArgs e)
        {
            AdjustLabelsSize();
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
                    game.someMineExpoloded = true;
                }
                else if(node.exposed == false)
                {
                    if (node.numberOfMinesAround == 0)
                    {
                        List<Node> nodesToExpose = game.Wawe(node);
                        foreach (Node nodeToExpose in nodesToExpose)
                        {
                            nodeToExpose.exposed = true;
                            game.numberOfExposed++;
                            nodeToExpose.label.Text = nodeToExpose.numberOfMinesAround.ToString();
                            l.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        node.exposed = true;
                        game.numberOfExposed++;
                        l.Text = node.numberOfMinesAround.ToString();
                        l.BackColor = Color.White;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right && node.exposed == false)
            {
                if(node.marked)
                {
                    node.marked = false;
                    l.Image = null;
                    game.minesLeft++;
                }
                else
                {
                    node.marked = true;
                    l.Image = Properties.Resources.flag;
                    l.Image = ResizeImage(Properties.Resources.flag, l.Width, l.Height);
                    game.minesLeft--;
                }
            }
            bombCountLabel.Text = "Mines left: " + game.minesLeft.ToString();
            //debugLabel.Text = game.numberOfExposed.ToString();
            VictoryCheck();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}