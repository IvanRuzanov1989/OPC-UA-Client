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

namespace OPC_UA
{
    public partial class WriteValueForm : Form
    {
        OPCServer _server;
        string _strNodeId;

        public WriteValueForm()
        {
            InitializeComponent();
        }

        public OPCServer server
        {
            set { _server = value; }
        }

        public string strNodeId
        {
            set { _strNodeId = value; }
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            byte value = Convert.ToByte(txb_value.Text);
            _server.WriteValues(_strNodeId, value);
            this.Close();
            
        }

        private void b_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
