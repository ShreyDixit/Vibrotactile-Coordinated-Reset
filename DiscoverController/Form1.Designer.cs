namespace DiscoverController
{
    partial class DiscoverControllerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PulseTactor1Button = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DiscoverButton = new System.Windows.Forms.Button();
            this.ComPortLabel = new System.Windows.Forms.Label();
            this.ComPortComboBox = new System.Windows.Forms.ComboBox();
            this.ConsoleOutputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SimulationStartButton = new System.Windows.Forms.Button();
            this.GainTrackBar = new System.Windows.Forms.TrackBar();
            this.MirrorHands = new System.Windows.Forms.CheckBox();
            this.RandomizGain = new System.Windows.Forms.CheckBox();
            this.GainValueLabel = new System.Windows.Forms.Label();
            this.GainValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.vCRDuration = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GainTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // PulseTactor1Button
            // 
            this.PulseTactor1Button.Enabled = false;
            this.PulseTactor1Button.Location = new System.Drawing.Point(22, 214);
            this.PulseTactor1Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PulseTactor1Button.Name = "PulseTactor1Button";
            this.PulseTactor1Button.Size = new System.Drawing.Size(386, 68);
            this.PulseTactor1Button.TabIndex = 9;
            this.PulseTactor1Button.Text = "Pulse Tactor 1";
            this.PulseTactor1Button.UseVisualStyleBackColor = true;
            this.PulseTactor1Button.Click += new System.EventHandler(this.PulseTactor1Button_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(22, 137);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(386, 68);
            this.ConnectButton.TabIndex = 8;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // DiscoverButton
            // 
            this.DiscoverButton.Location = new System.Drawing.Point(22, 60);
            this.DiscoverButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DiscoverButton.Name = "DiscoverButton";
            this.DiscoverButton.Size = new System.Drawing.Size(386, 68);
            this.DiscoverButton.TabIndex = 7;
            this.DiscoverButton.Text = "Discover";
            this.DiscoverButton.UseVisualStyleBackColor = true;
            this.DiscoverButton.Click += new System.EventHandler(this.DiscoverButton_Click);
            // 
            // ComPortLabel
            // 
            this.ComPortLabel.AutoSize = true;
            this.ComPortLabel.Location = new System.Drawing.Point(18, 23);
            this.ComPortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComPortLabel.Name = "ComPortLabel";
            this.ComPortLabel.Size = new System.Drawing.Size(75, 20);
            this.ComPortLabel.TabIndex = 6;
            this.ComPortLabel.Text = "Com Port";
            // 
            // ComPortComboBox
            // 
            this.ComPortComboBox.FormattingEnabled = true;
            this.ComPortComboBox.Location = new System.Drawing.Point(102, 18);
            this.ComPortComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ComPortComboBox.Name = "ComPortComboBox";
            this.ComPortComboBox.Size = new System.Drawing.Size(304, 28);
            this.ComPortComboBox.TabIndex = 5;
            // 
            // ConsoleOutputRichTextBox
            // 
            this.ConsoleOutputRichTextBox.Location = new System.Drawing.Point(22, 651);
            this.ConsoleOutputRichTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConsoleOutputRichTextBox.Name = "ConsoleOutputRichTextBox";
            this.ConsoleOutputRichTextBox.ReadOnly = true;
            this.ConsoleOutputRichTextBox.Size = new System.Drawing.Size(386, 182);
            this.ConsoleOutputRichTextBox.TabIndex = 10;
            this.ConsoleOutputRichTextBox.Text = "";
            // 
            // SimulationStartButton
            // 
            this.SimulationStartButton.Enabled = false;
            this.SimulationStartButton.Location = new System.Drawing.Point(20, 361);
            this.SimulationStartButton.Name = "SimulationStartButton";
            this.SimulationStartButton.Size = new System.Drawing.Size(386, 67);
            this.SimulationStartButton.TabIndex = 11;
            this.SimulationStartButton.Text = "Start";
            this.SimulationStartButton.UseVisualStyleBackColor = true;
            this.SimulationStartButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // GainTrackBar
            // 
            this.GainTrackBar.Enabled = false;
            this.GainTrackBar.Location = new System.Drawing.Point(20, 523);
            this.GainTrackBar.Maximum = 255;
            this.GainTrackBar.Minimum = 1;
            this.GainTrackBar.Name = "GainTrackBar";
            this.GainTrackBar.Size = new System.Drawing.Size(386, 69);
            this.GainTrackBar.TabIndex = 12;
            this.GainTrackBar.Value = 150;
            this.GainTrackBar.Scroll += new System.EventHandler(this.GainTrackBar_Scroll);
            // 
            // MirrorHands
            // 
            this.MirrorHands.Appearance = System.Windows.Forms.Appearance.Button;
            this.MirrorHands.AutoSize = true;
            this.MirrorHands.Enabled = false;
            this.MirrorHands.Location = new System.Drawing.Point(20, 454);
            this.MirrorHands.MaximumSize = new System.Drawing.Size(455, 455);
            this.MirrorHands.Name = "MirrorHands";
            this.MirrorHands.Size = new System.Drawing.Size(110, 30);
            this.MirrorHands.TabIndex = 13;
            this.MirrorHands.Text = "Mirror Hands";
            this.MirrorHands.UseVisualStyleBackColor = true;
            // 
            // RandomizGain
            // 
            this.RandomizGain.Appearance = System.Windows.Forms.Appearance.Button;
            this.RandomizGain.AutoSize = true;
            this.RandomizGain.Enabled = false;
            this.RandomizGain.Location = new System.Drawing.Point(268, 454);
            this.RandomizGain.Name = "RandomizGain";
            this.RandomizGain.Size = new System.Drawing.Size(138, 30);
            this.RandomizGain.TabIndex = 14;
            this.RandomizGain.Text = "Randomize Gain";
            this.RandomizGain.UseVisualStyleBackColor = true;
            this.RandomizGain.CheckedChanged += new System.EventHandler(this.RandomizGain_CheckedChanged);
            // 
            // GainValueLabel
            // 
            this.GainValueLabel.AutoSize = true;
            this.GainValueLabel.Location = new System.Drawing.Point(70, 595);
            this.GainValueLabel.Name = "GainValueLabel";
            this.GainValueLabel.Size = new System.Drawing.Size(88, 20);
            this.GainValueLabel.TabIndex = 15;
            this.GainValueLabel.Text = "Gain Value";
            // 
            // GainValue
            // 
            this.GainValue.Enabled = false;
            this.GainValue.Location = new System.Drawing.Point(205, 592);
            this.GainValue.Name = "GainValue";
            this.GainValue.ReadOnly = true;
            this.GainValue.Size = new System.Drawing.Size(181, 26);
            this.GainValue.TabIndex = 16;
            this.GainValue.Text = "150";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 313);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Simulation Duration (in mins.)";
            // 
            // vCRDuration
            // 
            this.vCRDuration.Enabled = false;
            this.vCRDuration.Location = new System.Drawing.Point(268, 310);
            this.vCRDuration.Name = "vCRDuration";
            this.vCRDuration.Size = new System.Drawing.Size(138, 26);
            this.vCRDuration.TabIndex = 18;
            this.vCRDuration.Text = "1";
            // 
            // DiscoverControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 866);
            this.Controls.Add(this.vCRDuration);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GainValue);
            this.Controls.Add(this.GainValueLabel);
            this.Controls.Add(this.RandomizGain);
            this.Controls.Add(this.MirrorHands);
            this.Controls.Add(this.GainTrackBar);
            this.Controls.Add(this.SimulationStartButton);
            this.Controls.Add(this.ConsoleOutputRichTextBox);
            this.Controls.Add(this.PulseTactor1Button);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.DiscoverButton);
            this.Controls.Add(this.ComPortLabel);
            this.Controls.Add(this.ComPortComboBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DiscoverControllerForm";
            this.Text = "Discover Controller Tutorial";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DiscoverControllerForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.GainTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PulseTactor1Button;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DiscoverButton;
        private System.Windows.Forms.Label ComPortLabel;
        private System.Windows.Forms.ComboBox ComPortComboBox;
        private System.Windows.Forms.RichTextBox ConsoleOutputRichTextBox;
        private System.Windows.Forms.Button SimulationStartButton;
        private System.Windows.Forms.TrackBar GainTrackBar;
        private System.Windows.Forms.CheckBox MirrorHands;
        private System.Windows.Forms.CheckBox RandomizGain;
        private System.Windows.Forms.Label GainValueLabel;
        private System.Windows.Forms.TextBox GainValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox vCRDuration;
    }
}

