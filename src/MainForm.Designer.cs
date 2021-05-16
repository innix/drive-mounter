namespace DriveMounter
{
    partial class MainForm
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
            this.volumesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mountButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // volumesListView
            // 
            this.volumesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.volumesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.volumesListView.FullRowSelect = true;
            this.volumesListView.GridLines = true;
            this.volumesListView.HideSelection = false;
            this.volumesListView.Location = new System.Drawing.Point(12, 12);
            this.volumesListView.MultiSelect = false;
            this.volumesListView.Name = "volumesListView";
            this.volumesListView.Size = new System.Drawing.Size(989, 528);
            this.volumesListView.TabIndex = 4;
            this.volumesListView.UseCompatibleStateImageBehavior = false;
            this.volumesListView.View = System.Windows.Forms.View.Details;
            this.volumesListView.SelectedIndexChanged += new System.EventHandler(this.volumesListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Drive";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 121;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Mounted";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Free Space";
            this.columnHeader4.Width = 116;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Capacity";
            this.columnHeader5.Width = 89;
            // 
            // mountButton
            // 
            this.mountButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mountButton.Enabled = false;
            this.mountButton.Location = new System.Drawing.Point(12, 565);
            this.mountButton.Name = "mountButton";
            this.mountButton.Size = new System.Drawing.Size(105, 31);
            this.mountButton.TabIndex = 5;
            this.mountButton.Text = "Unmount";
            this.mountButton.UseVisualStyleBackColor = true;
            this.mountButton.Click += new System.EventHandler(this.mountButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 608);
            this.Controls.Add(this.mountButton);
            this.Controls.Add(this.volumesListView);
            this.Name = "MainForm";
            this.Text = "Drive Mounter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView volumesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button mountButton;
    }
}

