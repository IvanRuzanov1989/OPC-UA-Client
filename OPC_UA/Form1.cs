using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Opc;
using Opc.Ua;
using Opc.Ua.Client;
using OPC_UA.classes;
using System.Data.SqlClient;
using System.Data.Sql;

namespace OPC_UA
{
    public partial class Form1 : Form
    {
        #region fields

        EndpointDescriptionCollection endpoints;
        DiscoveryClient client;
        Uri discoveryUrl;
        OPCServer server;
        string uriString;
        AttributesReader attriburesReader;
        Subscription subscription = null;

        SqlConnection connection;

        #endregion

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            //OpenSqlConnection();
        }
        #region sql

        public void OpenSqlConnection()
        {
            string ConnectionString= @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\User\Documents\Visual Studio 2015\Projects\OPC_UA\OPC_UA\LocalDB.mdf; Integrated Security = True";
            connection = new SqlConnection(ConnectionString);
            connection.OpenAsync();
            connection.StateChange += Connection_StateChange;
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if(e.CurrentState==ConnectionState.Open)
            {
                MessageBox.Show("connected");
            }
        }

        public

        #endregion

        void CustomizeTreeNode(ref TreeNode node)
        {
            // Get the data about the tree node.
            ReferenceDescription refDescr = (ReferenceDescription)node.Tag;

            if (refDescr == null)
            {
                // Error case.
                return;
            }

            // Check for folder.
            if (ExpandedNodeId.ToNodeId(refDescr.TypeDefinition, null) == ObjectTypes.FolderType)
            {
                node.ImageIndex = 0;
            }
            // Check for property.
            else if (ExpandedNodeId.ToNodeId(refDescr.ReferenceTypeId, null) == ReferenceTypes.HasProperty)
            {
                node.ImageIndex = 3;
            }
            // Check node class.
            else
            {
                switch (refDescr.NodeClass)
                {
                    case NodeClass.Object:
                        node.ImageIndex = 1;
                        break;
                    case NodeClass.Variable:
                        node.ImageIndex = 4;
                        break;
                    case NodeClass.Method:
                        node.ImageIndex = 7;
                        break;
                    case NodeClass.ObjectType:
                        node.ImageIndex = 8;
                        break;
                    case NodeClass.VariableType:
                        node.ImageIndex = 4;
                        break;
                    case NodeClass.ReferenceType:
                        node.ImageIndex = 10;
                        break;
                    case NodeClass.DataType:
                        node.ImageIndex = 10;
                        break;
                    case NodeClass.View:
                        node.ImageIndex = 5;
                        break;
                    default:
                        node.ImageIndex = 11;
                        break;
                }
            }

            // Set the key of the image.
            node.SelectedImageIndex = node.ImageIndex;
            node.StateImageKey = node.ImageKey;
        }

        private void b_browse_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ServersViewer.Nodes.Clear();
            uriString = textBox1.Text;
            discoveryUrl = new Uri(uriString);
            client = DiscoveryClient.Create(discoveryUrl);
            endpoints = client.GetEndpoints(null);



            for (int i=0;i< endpoints.Count;i++)
            {
                string key = endpoints[i].Server.ApplicationName.Text;
                string text = endpoints[i].Server.ApplicationName.Text;
                int imageIndex = 0;
                int selectedImageindex = 0;

                if (i == 0)
                {

                    ServersViewer.Nodes.Add(key, text, imageIndex, selectedImageindex);
                }
                else
                {
                    for (int j = 0; j < ServersViewer.Nodes.Count; j++)
                    {

                        if (ServersViewer.Nodes[j].Text != text)
                        {
                            ServersViewer.Nodes.Add(key, text, imageIndex, selectedImageindex);
                        }

                    }
                }
               
            }

            for (int i = 0; i < endpoints.Count; i++)
            {
                
                string key = endpoints[i].Server.ApplicationName.Text;
                string text = endpoints[i].Server.ApplicationName.Text;
                int imageIndex = 2;
                int selectedImageindex = 2;

                string[] collection = endpoints[i].SecurityPolicyUri.Split('#');
                string SecurityMode = endpoints[i].SecurityMode.ToString();
                string name = collection[1] + ", " + SecurityMode;

                TreeNode node = new TreeNode(name, imageIndex, selectedImageindex);
                node.Tag = endpoints[i];

                ServersViewer.Nodes[key].Nodes.Add(node);

            }

            ServersViewer.ExpandAll();
            Cursor = Cursors.Default;


        }

        private void ServersViewer_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            server = new OPCServer();
            EndpointDescription endpoint = (EndpointDescription)ServersViewer.SelectedNode.Tag;
            try
            {
                server.Connect(endpoint, false, "", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ReferenceDescriptionCollection refDescr = server.Browse();

            foreach(ReferenceDescription root in refDescr)
            {
                string name = root.BrowseName.ToString();
                TreeNode node = new TreeNode(name,0,0);
                node.Tag = root;

                TreeNode dummy = new TreeNode();
                node.Nodes.Add(dummy);

                CustomizeTreeNode(ref node);

                ItemsViewer.Nodes.Add(node);
            }

            Cursor = Cursors.Default;
            b_browse.Enabled = false;
            textBox1.Enabled = false;
            ServersViewer.Enabled = false;

        }

        private void ItemsViewer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            e.Node.Nodes.Clear();
            ReferenceDescription refDescr = (ReferenceDescription)e.Node.Tag;

            ReferenceDescriptionCollection items = server.Browse(refDescr);

            foreach(ReferenceDescription item in items)
            {
                string name = item.BrowseName.ToString();
                int imageIndex = 4;
                int selectedImageindex = 4;
                TreeNode node = new TreeNode(name, imageIndex, selectedImageindex);
                node.Tag = item;

                TreeNode dummy = new TreeNode();
                node.Nodes.Add(dummy);

                CustomizeTreeNode(ref node);
                e.Node.Nodes.Add(node);
            }


            Cursor = Cursors.Default;
        }

        private void ItemsViewer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            attriburesReader = new AttributesReader();
            attriburesReader.Server = server;

            List<ListViewItem> attr = attriburesReader.getAttributesList(e.Node);



            AttributesViewer.Items.Clear();
            foreach (ListViewItem item in attr)
            {
                AttributesViewer.Items.Add(item);
            }
        }

        private void ItemsViewer_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (((TreeNode)e.Item).Tag.GetType() == typeof(ReferenceDescription))
            {
                // Get the data about the tree node.
                ReferenceDescription reference = (ReferenceDescription)((TreeNode)e.Item).Tag;

                // Allow only variables drag and drop.
                if (reference.NodeId.IsAbsolute || reference.NodeClass != NodeClass.Variable)
                {
                    return;
                }

                // Start the drag and drop action.
                // We have to copy serializable data like strings.
                String sNodeId = reference.NodeId.ToString();

                // The data is copied to the target control.
                DragDropEffects dde = ItemsViewer.DoDragDrop(sNodeId, DragDropEffects.Copy);
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                String sNodeId = (String)e.Data.GetData(typeof(System.String));

                if (subscription==null)
                {
                    subscription = server.CreateSubscription();
                    server.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ClientApi_ValueChanged);
                }


                ListViewItem item = new ListViewItem(sNodeId);
                object serverHandle = null;

                // Prepare further columns.
                item.SubItems.Add("100"); // Sampling interval by default.
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);

                try
                {
                    MonitoredItem tempMonitoredItem = server.MonitorItem(subscription, sNodeId, "item1");
                    tempMonitoredItem.Handle = item;
                    serverHandle = tempMonitoredItem;
                }
                catch(ServiceResultException monitoredItemResult)
                {
                    item.SubItems[5].Text= monitoredItemResult.StatusCode.ToString();

                }
                item.Tag = serverHandle;
                MonitoredItems.Items.Add(item);

                // Fit column width to the longest item and add a few pixel:
                MonitoredItems.Columns[0].Width = -1;
                MonitoredItems.Columns[0].Width += 15;
                // Fit column width to the column content:
                MonitoredItems.Columns[1].Width = -2;
                MonitoredItems.Columns[5].Width = -2;
                // Fix settings:
                MonitoredItems.Columns[2].Width = 95;
                MonitoredItems.Columns[3].Width = 75;
                MonitoredItems.Columns[4].Width = 75;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ClientApi_ValueChanged(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            // We have to call an invoke method. 
            if (this.InvokeRequired)
            {
                // Asynchronous execution of the valueChanged delegate.
                this.BeginInvoke(new MonitoredItemNotificationEventHandler(ClientApi_ValueChanged), monitoredItem, e);
                return;
            }

            // Extract notification from event
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }

            object value = notification.Value.WrappedValue.Value;

            // Get the according item.
            ListViewItem item = (ListViewItem)monitoredItem.Handle;

            // Set current value, status code and timestamp.
            item.SubItems[2].Text = Utils.Format("{0}", notification.Value.Value);
            item.SubItems[3].Text = Utils.Format("{0}", notification.Value.StatusCode);
            item.SubItems[4].Text = Utils.Format("{0:HH:mm:ss.fff}", notification.Value.SourceTimestamp.ToLocalTime());
           
        }

        private void MonitoredItems_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MonitoredItems_DoubleClick(object sender, EventArgs e)
        {
            WriteValueForm WriteValueForm = new WriteValueForm();
            WriteValueForm.server = server;
            WriteValueForm.strNodeId = MonitoredItems.SelectedItems[0].SubItems[0].Text;
            WriteValueForm.ShowDialog();

        }
    }
}
