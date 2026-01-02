namespace Demo;

partial class PageE
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
        label1 = new Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Verdana", 32F, FontStyle.Bold);
        label1.Location = new Point(0, 0);
        label1.Name = "label1";
        label1.Size = new Size(216, 65);
        label1.TabIndex = 2;
        label1.Text = "Page5";
        // 
        // PageE
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Orchid;
        Controls.Add(label1);
        Name = "PageE";
        Controls.SetChildIndex(label1, 0);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
}
