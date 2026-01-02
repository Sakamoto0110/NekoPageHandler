namespace Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            panel2 = new Panel();
            pnHost = new Panel();
            pnMenu = new Panel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button4 = new Button();
            button2 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button3 = new Button();
            btnMenu = new Button();
            tableLayoutPanel1.SuspendLayout();
            pnMenu.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Controls.Add(pnHost, 1, 1);
            tableLayoutPanel1.Controls.Add(pnMenu, 0, 1);
            tableLayoutPanel1.Controls.Add(btnMenu, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 433);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 14);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlDark;
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(98, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(699, 37);
            panel2.TabIndex = 2;
            // 
            // pnHost
            // 
            pnHost.Dock = DockStyle.Fill;
            pnHost.Location = new Point(98, 46);
            pnHost.Name = "pnHost";
            pnHost.Size = new Size(699, 381);
            pnHost.TabIndex = 3;
            // 
            // pnMenu
            // 
            pnMenu.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnMenu.Controls.Add(flowLayoutPanel2);
            pnMenu.Controls.Add(flowLayoutPanel1);
            pnMenu.Location = new Point(3, 46);
            pnMenu.Name = "pnMenu";
            pnMenu.Size = new Size(89, 381);
            pnMenu.TabIndex = 4;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.Controls.Add(button4);
            flowLayoutPanel2.Controls.Add(button2);
            flowLayoutPanel2.Dock = DockStyle.Bottom;
            flowLayoutPanel2.FlowDirection = FlowDirection.BottomUp;
            flowLayoutPanel2.Location = new Point(0, 319);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(89, 62);
            flowLayoutPanel2.TabIndex = 1;
            flowLayoutPanel2.WrapContents = false;
            // 
            // button4
            // 
            button4.AutoSize = true;
            button4.Dock = DockStyle.Bottom;
            button4.Location = new Point(3, 34);
            button4.Name = "button4";
            button4.Size = new Size(82, 25);
            button4.TabIndex = 2;
            button4.Text = "Marketplace";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button2
            // 
            button2.AutoSize = true;
            button2.Dock = DockStyle.Bottom;
            button2.Location = new Point(3, 3);
            button2.Name = "button2";
            button2.Size = new Size(82, 25);
            button2.TabIndex = 0;
            button2.Text = "Estoque";
            button2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(81, 31);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button3
            // 
            button3.AutoSize = true;
            button3.Dock = DockStyle.Top;
            button3.Location = new Point(3, 3);
            button3.Name = "button3";
            button3.Size = new Size(75, 25);
            button3.TabIndex = 1;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // btnMenu
            // 
            btnMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnMenu.Location = new Point(23, 3);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(48, 37);
            btnMenu.TabIndex = 0;
            btnMenu.Text = "☰";
            btnMenu.UseVisualStyleBackColor = true;
            btnMenu.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            pnMenu.ResumeLayout(false);
            pnMenu.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btnMenu;
        private Panel panel1;
        private Panel panel2;
        private Panel pnHost;
        private Panel pnMenu;
        private Button button4;
        private Button button3;
        private Button button2;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
