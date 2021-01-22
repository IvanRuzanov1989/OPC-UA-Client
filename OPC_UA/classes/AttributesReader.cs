using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;
using System.Windows.Forms;


namespace OPC_UA.classes
{
    public class AttributesReader
    {

        #region private fields

        private AttributesHashtable m_hashtable;
        private OPCServer m_server = null;

        #endregion

        #region properties

        public OPCServer Server
        {
            set { m_server = value; }
        }

        #endregion

        #region public methods

        public List<ListViewItem> getAttributesList(TreeNode node)
        {


            List<ListViewItem> items = new List<ListViewItem>();
            ReferenceDescription refDescr = (ReferenceDescription)node.Tag;
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            m_hashtable = new AttributesHashtable();
            DataValueCollection results;
            DiagnosticInfoCollection diag;

            

            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.NodeId));
            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.NodeClass));
            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.BrowseName));
            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.DisplayName));
            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.Description));
            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.WriteMask));
            nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.UserWriteMask));


            switch (refDescr.NodeClass)
            {
                case NodeClass.Object:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.EventNotifier));
                    break;
                case NodeClass.Variable:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.Value));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.DataType));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.ValueRank));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.ArrayDimensions));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.AccessLevel));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.UserAccessLevel));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.MinimumSamplingInterval));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.Historizing));
                    break;
                case NodeClass.Method:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.Executable));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.UserExecutable));
                    break;
                case NodeClass.ObjectType:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.IsAbstract));
                    break;
                case NodeClass.VariableType:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.Value));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.DataType));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.ValueRank));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.ArrayDimensions));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.IsAbstract));
                    break;
                case NodeClass.ReferenceType:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.IsAbstract));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.Symmetric));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.InverseName) );
                    break;
                case NodeClass.DataType:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.IsAbstract));
                    break;
                case NodeClass.View:
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.ContainsNoLoops));
                    nodesToRead.Add(readAttribute(refDescr, m_hashtable, Attributes.EventNotifier));
                    break;
                default:
                    break;
            }

            m_server.readAttributes(nodesToRead, out results, out diag);


            for (int i = 0; i < nodesToRead.Count; i++)
            {
                string attributeName = (string)nodesToRead[i].Handle;
                ListViewItem item = new ListViewItem(attributeName);


                if (results[i].WrappedValue.Value != null)
                {
                    item.SubItems.Add(results[i].WrappedValue.Value.ToString());

                }
                else
                {
                    item.SubItems.Add("null");
                }

                items.Add(item);
            }

            return items;

        }

        #endregion

        #region private methods

        private ReadValueId readAttribute(ReferenceDescription refDescription, AttributesHashtable hashtable, uint attributeId)
        {
            ReadValueId attributeToRead = new ReadValueId();
            attributeToRead.NodeId = (NodeId)refDescription.NodeId;
            attributeToRead.AttributeId = attributeId;
            string ret = (string)hashtable.hashtable[attributeId];
            attributeToRead.Handle = ret;
            return attributeToRead;

        }

        #endregion

    }
}
