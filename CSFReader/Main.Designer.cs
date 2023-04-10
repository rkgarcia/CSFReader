namespace CSFReader
{
    partial class Main
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
            if (disposing && (components != null))
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
            button1 = new Button();
            ResultsTable = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)ResultsTable).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 21);
            button1.Name = "button1";
            button1.Size = new Size(224, 23);
            button1.TabIndex = 0;
            button1.Text = "Selecciona y procesa directorio";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Execute;
            // 
            // ResultsTable
            // 
            ResultsTable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ResultsTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ResultsTable.Location = new Point(1, 72);
            ResultsTable.Name = "ResultsTable";
            ResultsTable.RowTemplate.Height = 25;
            ResultsTable.Size = new Size(402, 228);
            ResultsTable.TabIndex = 1;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(403, 299);
            Controls.Add(ResultsTable);
            Controls.Add(button1);
            Name = "Main";
            Text = "CSF Reader";
            WindowState = FormWindowState.Maximized;
            Load += Main_Load;
            SizeChanged += Main_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)ResultsTable).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private DataGridView ResultsTable;
    }
}