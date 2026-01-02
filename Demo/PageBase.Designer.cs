namespace Demo;

partial class PageBase
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        button1 = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        button1.BackColor = SystemColors.ControlLight;
        button1.FlatAppearance.BorderColor = Color.Black;
        button1.FlatAppearance.BorderSize = 4;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Font = new Font("Verdana", 14F, FontStyle.Bold);
        button1.Location = new Point(272, 13);
        button1.Name = "button1";
        button1.Size = new Size(187, 62);
        button1.TabIndex = 0;
        button1.Text = "Home";
        button1.UseVisualStyleBackColor = false;
        button1.Click += button1_Click;
        // 
        // PageBase
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(button1);
        Name = "PageBase";
        Size = new Size(472, 407);
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
}
