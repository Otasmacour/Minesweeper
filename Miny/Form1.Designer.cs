using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Miny
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void LabelsSizeUpdate(int newSize, int previousSize,  int twoDArrayHeight, int twoDArrayWidth)
        {
            if(newSize == previousSize) { return; }
            foreach(Label label in labels)
            {
                int enlarge = newSize / previousSize;
                label.Font = new Font("Arial", newSize/3, FontStyle.Bold);
                int labelOrder = int.Parse(label.Tag.ToString());
                int y = labelOrder / twoDArrayHeight;
                int x = labelOrder - y * twoDArrayWidth;
                int newY = x * newSize;
                int newX = y * newSize;
                label.Location = new System.Drawing.Point(newY, newX);
                label.Size = new System.Drawing.Size(newSize, newSize);
            }
        }
        private void InitializeResetButton()
        {
            resetButton.Text = "Reset";
            resetButton.Size = new Size(150, 60);
            resetButton.BackColor = Color.White; 
            resetButton.ForeColor = Color.Black; 
            resetButton.Font = new Font("Arial", 12, FontStyle.Bold); 
            resetButton.FlatAppearance.BorderColor = Color.Black;
            resetButton.FlatAppearance.BorderSize = 2;
            resetButton.Click += new EventHandler(GameReset);
            this.Controls.Add(resetButton);
        }
        private void InitializeMyComponents(Node[,] twoDArray, int minesNumber)
        {
            InitializeLabels(game.twoDArray);
            InitializeComponent(game.minesLeft);
        }
        private void InitializeLabels(Node[,] twoDArray) 
        {
            int labelNumber = 0;
            for (int y = 0; y < twoDArray.GetLength(0); y++)
            {
                for (int x = 0; x < twoDArray.GetLength(1); x++)
                { 
                    Label l = new Label();
                    labels.Add(l);
                    twoDArray[y,x].label = l;
                    this.Controls.Add(l);
                    l.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    l.Font = new Font("Arial", 16, FontStyle.Bold);
                    l.Location = new System.Drawing.Point(y * 50, x * 50);
                    l.Size = new System.Drawing.Size(50, 50);
                    l.TabIndex = 1;
                    l.Tag = labelNumber;
                    l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    l.Text = "";
                    l.Click += new EventHandler((sender, e) => label1_Click(sender, (MouseEventArgs)e));
                    labelNumber++;
                }
            }
        }
        private void InitializeComponent(int minesNumber)
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(500, 520);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.Resize += new System.EventHandler(this.ReSize);

            bombCountLabel.Text = "Mines left: "+minesNumber.ToString();
            bombCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            statusStrip.Items.Add(bombCountLabel);
            statusStrip.Dock = DockStyle.Bottom;
            this.Controls.Add(statusStrip);

            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(40, 40); 
            pictureBox.Size = new Size(100, 100); 
            this.ResumeLayout(false);
        }
        private void InitializeGame()
        {
            startButton.Text = "Start Game";
            startButton.Location = new System.Drawing.Point(100, 100);
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        #endregion
        private Button startButton = new Button();
        private Button resetButton = new Button();
        private StatusStrip statusStrip = new StatusStrip();
        private ToolStripStatusLabel bombCountLabel = new ToolStripStatusLabel();
        private List<Label> labels = new List<Label>();
    }
}

