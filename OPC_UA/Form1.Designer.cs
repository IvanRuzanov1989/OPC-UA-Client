namespace OPC_UA
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.b_browse = new System.Windows.Forms.Button();
            this.ServersViewer = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ItemsViewer = new System.Windows.Forms.TreeView();
            this.MonitoredItems = new System.Windows.Forms.ListView();
            this.NodeIdCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SamplingIntervalCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QualityCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimestampCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastOperationStatusCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttributesViewer = new System.Windows.Forms.ListView();
            this.Attributes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(93, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(350, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "opc.tcp://Ivan-PC:4861";
            // 
            // b_browse
            // 
            this.b_browse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.b_browse.Location = new System.Drawing.Point(12, 12);
            this.b_browse.Name = "b_browse";
            this.b_browse.Size = new System.Drawing.Size(75, 23);
            this.b_browse.TabIndex = 1;
            this.b_browse.Text = "Обзор";
            this.b_browse.UseVisualStyleBackColor = true;
            this.b_browse.Click += new System.EventHandler(this.b_browse_Click);
            // 
            // ServersViewer
            // 
            this.ServersViewer.ImageIndex = 0;
            this.ServersViewer.ImageList = this.imageList1;
            this.ServersViewer.Location = new System.Drawing.Point(12, 41);
            this.ServersViewer.Name = "ServersViewer";
            this.ServersViewer.SelectedImageIndex = 0;
            this.ServersViewer.Size = new System.Drawing.Size(431, 136);
            this.ServersViewer.TabIndex = 4;
            this.ServersViewer.DoubleClick += new System.EventHandler(this.ServersViewer_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            this.imageList1.Images.SetKeyName(1, "object");
            this.imageList1.Images.SetKeyName(2, "getpoint");
            this.imageList1.Images.SetKeyName(3, "property");
            this.imageList1.Images.SetKeyName(4, "variable");
            this.imageList1.Images.SetKeyName(5, "view");
            this.imageList1.Images.SetKeyName(6, "browse");
            this.imageList1.Images.SetKeyName(7, "method");
            this.imageList1.Images.SetKeyName(8, "objecttype.png");
            this.imageList1.Images.SetKeyName(9, "type.png");
            this.imageList1.Images.SetKeyName(10, "reftype.png");
            this.imageList1.Images.SetKeyName(11, "error");
            // 
            // ItemsViewer
            // 
            this.ItemsViewer.AllowDrop = true;
            this.ItemsViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ItemsViewer.ImageIndex = 0;
            this.ItemsViewer.ImageList = this.imageList1;
            this.ItemsViewer.Location = new System.Drawing.Point(12, 183);
            this.ItemsViewer.Name = "ItemsViewer";
            this.ItemsViewer.SelectedImageIndex = 0;
            this.ItemsViewer.Size = new System.Drawing.Size(431, 109);
            this.ItemsViewer.TabIndex = 5;
            this.ItemsViewer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ItemsViewer_BeforeExpand);
            this.ItemsViewer.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ItemsViewer_ItemDrag);
            this.ItemsViewer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ItemsViewer_AfterSelect);
            // 
            // MonitoredItems
            // 
            this.MonitoredItems.AllowDrop = true;
            this.MonitoredItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MonitoredItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NodeIdCH,
            this.SamplingIntervalCH,
            this.ValueCH,
            this.QualityCH,
            this.TimestampCH,
            this.LastOperationStatusCH});
            this.MonitoredItems.Location = new System.Drawing.Point(12, 298);
            this.MonitoredItems.Name = "MonitoredItems";
            this.MonitoredItems.Size = new System.Drawing.Size(853, 258);
            this.MonitoredItems.TabIndex = 6;
            this.MonitoredItems.UseCompatibleStateImageBehavior = false;
            this.MonitoredItems.View = System.Windows.Forms.View.Details;
            this.MonitoredItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.MonitoredItems.DragOver += new System.Windows.Forms.DragEventHandler(this.MonitoredItems_DragOver);
            this.MonitoredItems.DoubleClick += new System.EventHandler(this.MonitoredItems_DoubleClick);
            // 
            // NodeIdCH
            // 
            this.NodeIdCH.Text = "NodeId";
            this.NodeIdCH.Width = 100;
            // 
            // SamplingIntervalCH
            // 
            this.SamplingIntervalCH.Text = "Sampling";
            this.SamplingIntervalCH.Width = 100;
            // 
            // ValueCH
            // 
            this.ValueCH.Text = "Value";
            this.ValueCH.Width = 100;
            // 
            // QualityCH
            // 
            this.QualityCH.Text = "Quality";
            this.QualityCH.Width = 100;
            // 
            // TimestampCH
            // 
            this.TimestampCH.Text = "Timestamp";
            this.TimestampCH.Width = 100;
            // 
            // LastOperationStatusCH
            // 
            this.LastOperationStatusCH.Text = "Last Error";
            this.LastOperationStatusCH.Width = 100;
            // 
            // AttributesViewer
            // 
            this.AttributesViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributesViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Attributes,
            this.Value});
            this.AttributesViewer.Location = new System.Drawing.Point(449, 13);
            this.AttributesViewer.Name = "AttributesViewer";
            this.AttributesViewer.Size = new System.Drawing.Size(416, 280);
            this.AttributesViewer.TabIndex = 7;
            this.AttributesViewer.UseCompatibleStateImageBehavior = false;
            this.AttributesViewer.View = System.Windows.Forms.View.Details;
            // 
            // Attributes
            // 
            this.Attributes.Text = "Attributes";
            this.Attributes.Width = 250;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.Width = 150;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox2.Location = new System.Drawing.Point(306, 562);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(474, 20);
            this.textBox2.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 594);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.ServersViewer);
            this.Controls.Add(this.AttributesViewer);
            this.Controls.Add(this.MonitoredItems);
            this.Controls.Add(this.ItemsViewer);
            this.Controls.Add(this.b_browse);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OPC UA Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button b_browse;
        private System.Windows.Forms.TreeView ServersViewer;
        private System.Windows.Forms.TreeView ItemsViewer;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView MonitoredItems;
        private System.Windows.Forms.ListView AttributesViewer;
        private System.Windows.Forms.ColumnHeader Attributes;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ColumnHeader NodeIdCH;
        private System.Windows.Forms.ColumnHeader SamplingIntervalCH;
        private System.Windows.Forms.ColumnHeader ValueCH;
        private System.Windows.Forms.ColumnHeader QualityCH;
        private System.Windows.Forms.ColumnHeader TimestampCH;
        private System.Windows.Forms.ColumnHeader LastOperationStatusCH;
        private System.Windows.Forms.TextBox textBox2;
    }
}

