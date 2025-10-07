namespace Elte.Amoba.View
{
    partial class GameForm1
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
            menuStrip1 = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuFileNewGame = new ToolStripMenuItem();
            _game10x10button = new ToolStripMenuItem();
            _game20x20Button = new ToolStripMenuItem();
            _game30x30Button = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            _menuFileLoadGame = new ToolStripMenuItem();
            _menuFileSaveGame = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            _pauseButton = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            _actualPlayer = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            _xTime = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            _OTime = new ToolStripStatusLabel();
            field = new Panel();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { _menuFile, _pauseButton });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(762, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuFileNewGame, toolStripSeparator1, _menuFileLoadGame, _menuFileSaveGame, toolStripSeparator2 });
            _menuFile.Name = "_menuFile";
            _menuFile.Size = new Size(46, 24);
            _menuFile.Text = "File";
            // 
            // _menuFileNewGame
            // 
            _menuFileNewGame.DropDownItems.AddRange(new ToolStripItem[] { _game10x10button, _game20x20Button, _game30x30Button });
            _menuFileNewGame.Name = "_menuFileNewGame";
            _menuFileNewGame.Size = new Size(224, 26);
            _menuFileNewGame.Text = "Új játék";
            // 
            // _game10x10button
            // 
            _game10x10button.Name = "_game10x10button";
            _game10x10button.Size = new Size(224, 26);
            _game10x10button.Text = "10x10";
            // 
            // _game20x20Button
            // 
            _game20x20Button.Name = "_game20x20Button";
            _game20x20Button.Size = new Size(224, 26);
            _game20x20Button.Text = "20x20";
            // 
            // _game30x30Button
            // 
            _game30x30Button.Name = "_game30x30Button";
            _game30x30Button.Size = new Size(224, 26);
            _game30x30Button.Text = "30x30";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(221, 6);
            // 
            // _menuFileLoadGame
            // 
            _menuFileLoadGame.Name = "_menuFileLoadGame";
            _menuFileLoadGame.Size = new Size(224, 26);
            _menuFileLoadGame.Text = "Játék betöltése...";
            // 
            // _menuFileSaveGame
            // 
            _menuFileSaveGame.Name = "_menuFileSaveGame";
            _menuFileSaveGame.Size = new Size(224, 26);
            _menuFileSaveGame.Text = "Játék mentése...";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(221, 6);
            // 
            // _pauseButton
            // 
            _pauseButton.Name = "_pauseButton";
            _pauseButton.Size = new Size(67, 24);
            _pauseButton.Text = "Szünet";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, _actualPlayer, toolStripStatusLabel2, _xTime, toolStripStatusLabel3, _OTime });
            statusStrip1.Location = new Point(0, 261);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(762, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(64, 20);
            toolStripStatusLabel1.Text = "Aktuális:";
            // 
            // _actualPlayer
            // 
            _actualPlayer.Name = "_actualPlayer";
            _actualPlayer.Size = new Size(18, 20);
            _actualPlayer.Text = "X";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(47, 20);
            toolStripStatusLabel2.Text = "X Idő:";
            // 
            // _xTime
            // 
            _xTime.Name = "_xTime";
            _xTime.Size = new Size(55, 20);
            _xTime.Text = "0:00:00";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(49, 20);
            toolStripStatusLabel3.Text = "O Idő:";
            // 
            // _OTime
            // 
            _OTime.Name = "_OTime";
            _OTime.Size = new Size(55, 20);
            _OTime.Text = "0:00:00";
            // 
            // field
            // 
            field.AutoSize = true;
            field.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            field.Dock = DockStyle.Fill;
            field.Location = new Point(0, 28);
            field.Name = "field";
            field.Size = new Size(762, 233);
            field.TabIndex = 2;
            // 
            // _openFileDialog
            // 
            _openFileDialog.DefaultExt = "amoba";
            _openFileDialog.Filter = "Amőba fájl|*.amoba";
            _openFileDialog.Title = "Játék betöltése..";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.DefaultExt = "amoba";
            _saveFileDialog.Filter = "Amőba fájl|*.amoba";
            _saveFileDialog.Title = "Játék mentése..";
            // 
            // GameForm1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(762, 287);
            Controls.Add(field);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimumSize = new Size(150, 150);
            Name = "GameForm1";
            Text = "Amőba";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuFileNewGame;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem _menuFileLoadGame;
        private ToolStripMenuItem _menuFileSaveGame;
        private ToolStripSeparator toolStripSeparator2;
        private StatusStrip statusStrip1;
        private Panel field;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel _actualPlayer;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel _xTime;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripMenuItem _game10x10button;
        private ToolStripMenuItem _game20x20Button;
        private ToolStripMenuItem _game30x30Button;
        private ToolStripStatusLabel _OTime;
        private ToolStripMenuItem _pauseButton;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
    }
}