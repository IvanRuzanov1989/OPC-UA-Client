using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Opc.Ua;

namespace OPC_UA.classes
{
    public class AttributesHashtable
    {
        #region construcor
        public AttributesHashtable()
        {
            Initialization();
        }
        #endregion

        #region privae fields
        private Hashtable hashAttributeNames;
        #endregion

        #region public properties
        /// <summary>
        /// Hashtable with attributes
        /// </summary>

        public Hashtable hashtable
        {
            get { return hashAttributeNames; }
        }
        #endregion

        #region private methods
        private void Initialization()
        {
            hashAttributeNames = new Hashtable();
            hashAttributeNames.Add(Attributes.AccessLevel, "AccessLevel");
            hashAttributeNames.Add(Attributes.ArrayDimensions, "ArrayDimensions");
            hashAttributeNames.Add(Attributes.BrowseName, "BrowseName");
            hashAttributeNames.Add(Attributes.ContainsNoLoops, "ContainsNoLoops");
            hashAttributeNames.Add(Attributes.DataType, "DataType");
            hashAttributeNames.Add(Attributes.Description, "Description");
            hashAttributeNames.Add(Attributes.DisplayName, "DisplayName");
            hashAttributeNames.Add(Attributes.EventNotifier, "EventNotifier");
            hashAttributeNames.Add(Attributes.Executable, "Executable");
            hashAttributeNames.Add(Attributes.Historizing, "Historizing");
            hashAttributeNames.Add(Attributes.InverseName, "InverseName");
            hashAttributeNames.Add(Attributes.IsAbstract, "IsAbstract");
            hashAttributeNames.Add(Attributes.MinimumSamplingInterval, "MinimumSamplingInterval");
            hashAttributeNames.Add(Attributes.NodeClass, "NodeClass");
            hashAttributeNames.Add(Attributes.NodeId, "NodeId");
            hashAttributeNames.Add(Attributes.Symmetric, "Symmetric");
            hashAttributeNames.Add(Attributes.UserAccessLevel, "UserAccessLevel");
            hashAttributeNames.Add(Attributes.UserExecutable, "UserExecutable");
            hashAttributeNames.Add(Attributes.UserWriteMask, "UserWriteMask");
            hashAttributeNames.Add(Attributes.Value, "Value");
            hashAttributeNames.Add(Attributes.ValueRank, "ValueRank");
            hashAttributeNames.Add(Attributes.WriteMask, "WriteMask");
        }
        #endregion
    }
}
