using System;
using System.Windows.Forms;

public class DimensionsForm : Form
{
    private TextBox widthTextBox;
    private TextBox heightTextBox;
    private TextBox percentOfMinesTextBox;

    public DimensionsForm()
    {
        InitializeForm();
    }

    private void InitializeForm()
    {
        this.Text = "Choose Dimensions";
        this.Size = new System.Drawing.Size(300, 180);

        Label widthLabel = new Label();
        widthLabel.Text = "Width:";
        widthLabel.Location = new System.Drawing.Point(20, 20);
        this.Controls.Add(widthLabel);

        widthTextBox = new TextBox();
        widthTextBox.Location = new System.Drawing.Point(120, 20);
        this.Controls.Add(widthTextBox);

        Label heightLabel = new Label();
        heightLabel.Text = "Height:";
        heightLabel.Location = new System.Drawing.Point(20, 50);
        this.Controls.Add(heightLabel);

        heightTextBox = new TextBox();
        heightTextBox.Location = new System.Drawing.Point(120, 50);
        this.Controls.Add(heightTextBox);

        Label percentOfMinesLabel = new Label();
        percentOfMinesLabel.Text = "Percent of mines:";
        percentOfMinesLabel.Location = new System.Drawing.Point(20, 80);
        this.Controls.Add(percentOfMinesLabel);

        percentOfMinesTextBox = new TextBox();
        percentOfMinesTextBox.Location = new System.Drawing.Point(120, 80);
        this.Controls.Add(percentOfMinesTextBox);

        Button okButton = new Button();
        okButton.Text = "OK";
        okButton.Location = new System.Drawing.Point(100, 110);
        okButton.Click += OkButton_Click;
        this.Controls.Add(okButton);
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
        if (int.TryParse(widthTextBox.Text, out int width) && int.TryParse(heightTextBox.Text, out int height) && int.TryParse(percentOfMinesTextBox.Text, out int percentOfMines))
        {
            if (percentOfMines >= 0 && percentOfMines <= 100)
            {
                SelectedWidth = width;
                SelectedHeight = height;
                SelectedPercentOfMines = percentOfMines;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Percent of mines must be between 0 and 100.");
            }
        }
        else
        {
            MessageBox.Show("Invalid dimensions, enter valid numbers.");
        }
    }

    public int SelectedWidth { get; private set; }
    public int SelectedHeight { get; private set; }
    public int SelectedPercentOfMines { get; private set; }
}
