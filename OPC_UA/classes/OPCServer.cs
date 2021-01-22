using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc;
using Opc.Ua;
using Opc.Ua.Client;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace OPC_UA.classes
{
    public class OPCServer
    {
        #region fields

        private ApplicationConfiguration _applicationConfig = null;
        private Session _session = null;
        private CertificateValidationEventHandler CertificateValidationNotification = null;
        private KeepAliveEventHandler KeepAliveNotification = null;

        #endregion

        #region constructor

        public OPCServer()
        {
            _applicationConfig = CreateClientConfiguration();
        }

        #endregion

        #region properties


        public MonitoredItemNotificationEventHandler ItemChangedNotification = null;

        #endregion

        #region public methods

        public void Connect(EndpointDescription endpointDescription, bool userAuth, string userName, string password)
        {
            try
            {


                //Secify application configuration
                ApplicationConfiguration ApplicationConfig = _applicationConfig;

                //Hook up a validator function for a CertificateValidation event
                ApplicationConfig.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;

                //Create EndPoint configuration
                EndpointConfiguration EndpointConfiguration = EndpointConfiguration.Create(ApplicationConfig);

                //Connect to server and get endpoints
                ConfiguredEndpoint mEndpoint = new ConfiguredEndpoint(null, endpointDescription, EndpointConfiguration);

                //Create the binding factory.
                //BindingFactory bindingFactory = BindingFactory.Create(mApplicationConfig, ServiceMessageContext.GlobalContext);

                //Creat a session name
                String sessionName = "MySession";


                //Create user identity
                UserIdentity UserIdentity;
                if (userAuth)
                {
                    UserIdentity = new UserIdentity(userName, password);
                }
                else
                {
                    UserIdentity = new UserIdentity();
                }

                //Update certificate store before connection attempt
                ApplicationConfig.CertificateValidator.Update(ApplicationConfig);

                //Create and connect session
                Task<Session> ss = Session.Create(ApplicationConfig, mEndpoint, true, sessionName, 60000, UserIdentity, null);

                _session = ss.Result;

                _session.KeepAlive += new KeepAliveEventHandler(Notification_KeepAlive);

            }
            catch (Exception e)
            {
                //handle Exception here
                MessageBox.Show(e.Message);
            }
        }

        public ReferenceDescriptionCollection Browse()
        {
            //Create a collection for the browse results
            ReferenceDescriptionCollection referenceDescriptionCollection;
            //Create a continuationPoint
            byte[] continuationPoint;
            try
            {
                //Browse the RootFolder for variables, objects and methods
                _session.Browse(null, null, ObjectIds.RootFolder, 0u, BrowseDirection.Forward,
                    ReferenceTypeIds.HierarchicalReferences, true,
                    (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method, out continuationPoint,
                    out referenceDescriptionCollection);
                return referenceDescriptionCollection;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        public ReferenceDescriptionCollection Browse(ReferenceDescription refDesc)
        {
            //Create a collection for the browse results
            ReferenceDescriptionCollection referenceDescriptionCollection;
            ReferenceDescriptionCollection nextreferenceDescriptionCollection;
            //Create a continuationPoint
            byte[] continuationPoint;
            byte[] revisedContinuationPoint;
            //Create a NodeId using the selected ReferenceDescription as browsing starting point
            NodeId nodeId = ExpandedNodeId.ToNodeId(refDesc.NodeId, null);
            try
            {
                //Browse from starting point for all object types
                _session.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, 0, out continuationPoint, out referenceDescriptionCollection);

                while (continuationPoint != null)
                {
                    _session.BrowseNext(null, false, continuationPoint, out revisedContinuationPoint, out nextreferenceDescriptionCollection);
                    referenceDescriptionCollection.AddRange(nextreferenceDescriptionCollection);
                    continuationPoint = revisedContinuationPoint;
                }

                return referenceDescriptionCollection;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        public ReferenceDescriptionCollection Browse(ReferenceDescription refDesc, NodeId refTypeId)
        {
            //Create a collection for the browse results
            ReferenceDescriptionCollection referenceDescriptionCollection;
            ReferenceDescriptionCollection nextreferenceDescriptionCollection;
            //Create a continuationPoint
            byte[] continuationPoint;
            byte[] revisedContinuationPoint;
            //Create a NodeId using the selected ReferenceDescription as browsing starting point
            NodeId nodeId = ExpandedNodeId.ToNodeId(refDesc.NodeId, null);
            try
            {
                //Browse from starting point for all object types
                _session.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, refTypeId, true, 0, out continuationPoint, out referenceDescriptionCollection);

                while (continuationPoint != null)
                {
                    _session.BrowseNext(null, false, continuationPoint, out revisedContinuationPoint, out nextreferenceDescriptionCollection);
                    referenceDescriptionCollection.AddRange(nextreferenceDescriptionCollection);
                    continuationPoint = revisedContinuationPoint;
                }

                return referenceDescriptionCollection;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        public void readAttributes(ReadValueIdCollection nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diag)
        {
            _session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out results, out diag);
        }

        public Subscription CreateSubscription()
        {
            Subscription subscription = new Subscription(_session.DefaultSubscription);

            subscription.DisplayName = "My subsription";
            subscription.PublishingEnabled = true;
            subscription.PublishingInterval = 100;
            subscription.KeepAliveCount = 10;
            subscription.LifetimeCount = 100;
            subscription.MaxNotificationsPerPublish = 100;
            _session.AddSubscription(subscription);

            try
            {
                subscription.Create();
                return subscription;
            }
            catch   (Exception e)
            {
                throw e;
            }



        }

        public MonitoredItem MonitorItem(Subscription subscription, string nodeIdString, string itemName)
        {

            MonitoredItem item = new MonitoredItem(subscription.DefaultItem);
            item.DisplayName = itemName;
            item.StartNodeId = nodeIdString;
            item.AttributeId = Attributes.Value;
            item.MonitoringMode = MonitoringMode.Reporting;
            item.SamplingInterval = 100;
            item.QueueSize = 1;
            item.DiscardOldest = false;

            item.Notification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem);
            try
            {
                subscription.AddItem(item);
                subscription.ApplyChanges();
                return item;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }           

        }

        public void WriteValues(string strNodeId ,byte value)
        {
            StatusCodeCollection result=null;
            DiagnosticInfoCollection diagnosticInfo = null;
            DataValue DV;

            Variant variant = new Variant(Type.GetType("Byte"));
            DV = new DataValue(variant);
            DV.Value = value;
            WriteValueCollection valuesToWrite = new WriteValueCollection();
            WriteValue attributeToWrite = new WriteValue();
            
            attributeToWrite.NodeId = (NodeId)strNodeId;
            attributeToWrite.AttributeId = Attributes.Value;
            attributeToWrite.Value = DV;
            valuesToWrite.Add(attributeToWrite);


            _session.Write(null,valuesToWrite,out result,out diagnosticInfo);
        }

        #endregion


        #region events


        private void Notification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            ItemChangedNotification(monitoredItem, e);
        }

        private void Notification_KeepAlive(Session session, KeepAliveEventArgs e)
        {

        }

        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }

        #endregion


        #region private methods

        private static ApplicationConfiguration CreateClientConfiguration()
        {
            // The application configuration can be loaded from any file.
            // ApplicationConfiguration.Load() method loads configuration by looking up a file path in the App.config.
            // This approach allows applications to share configuration files and to update them.
            // This example creates a minimum ApplicationConfiguration using its default constructor

            ApplicationConfiguration configuration = new ApplicationConfiguration();
            X509Certificate2 newclientCertificate = null;

            // Step 1 - Specify the client identity.
            configuration.ApplicationName = "UAClient";
            configuration.ApplicationType = ApplicationType.Client;
            configuration.ApplicationUri = "http://Localhost/UAClient"; //Kepp this syntax
            configuration.ProductUri = "http://www.oxymat.ru";

            // Step 2 - Specify the client's application instance certificate.
            // Application instance certificates must be placed in a windows certficate store because that is 
            // the best way to protect the private key. Certificates in a store are identified with 4 parameters:
            // StoreLocation, StoreName, SubjectName and Thumbprint.
            // When using StoreType = Directory you need to have the opc.ua.certificategenerator.exe installed on your machine

            configuration.SecurityConfiguration = new SecurityConfiguration();
            configuration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier();
            configuration.SecurityConfiguration.ApplicationCertificate.StoreType = CertificateStoreType.X509Store;
            configuration.SecurityConfiguration.ApplicationCertificate.StorePath = "CurrentUser\\My";
            configuration.SecurityConfiguration.ApplicationCertificate.SubjectName = configuration.ApplicationName;

            // Define trusted root store for server certificate checks
            configuration.SecurityConfiguration.TrustedIssuerCertificates.StoreType = CertificateStoreType.X509Store;
            configuration.SecurityConfiguration.TrustedIssuerCertificates.StorePath = "CurrentUser\\Root";
            configuration.SecurityConfiguration.TrustedPeerCertificates.StoreType = CertificateStoreType.X509Store;
            configuration.SecurityConfiguration.TrustedPeerCertificates.StorePath = "CurrentUser\\Root";



            // find the client certificate in the store.
            Task<X509Certificate2> clientCertificate = configuration.SecurityConfiguration.ApplicationCertificate.Find(true);

            if (clientCertificate.Result == null)
            {

                // Get local interface ip addresses and DNS name
                List<string> localIps = GetLocalIpAddressAndDns();

                ushort keySize = 2048;                      //Size of the key (1024, 2048 or 4096).
                DateTime startTime = DateTime.Now;          //The start time.
                ushort lifeTime = 24;                       //The lifetime of the key in months.
                ushort hashSizeInBits = 256;                //The hash size in bits.
                bool isCA = false;                          //if set to true then a CA certificate is created.
                X509Certificate2 issuerCAKeyCert = null;    //The CA cert with the CA private key.
                byte[] publicKey = null;
                int pathLengthConstraint = 0;
                string passsword = "";                  //The password to use to protect the certificate.
                string subjectName = "UAClient";            //The subject used to create the certificate(optional if applicationName is specified).


                // this code would normally be called as part of the installer - called here to illustrate.
                // create a new certificate an place it in the current user certificate store.

                newclientCertificate = CertificateFactory.CreateCertificate(
                      configuration.SecurityConfiguration.ApplicationCertificate.StoreType,
                      configuration.SecurityConfiguration.ApplicationCertificate.StorePath,
                      passsword,
                      configuration.ApplicationUri,
                      configuration.ApplicationName,
                      subjectName, localIps, keySize, startTime, lifeTime, hashSizeInBits, isCA, issuerCAKeyCert, publicKey, pathLengthConstraint);


            }

            // Step 3 - Specify the supported transport quotas.
            // The transport quotas are used to set limits on the contents of messages and are
            // used to protect against DOS attacks and rogue clients. They should be set to
            // reasonable values.
            configuration.TransportQuotas = new TransportQuotas();
            configuration.TransportQuotas.OperationTimeout = 360000;
            configuration.TransportQuotas.MaxStringLength = 67108864;
            configuration.TransportQuotas.MaxByteStringLength = 16777216; //Needed, i.e. for large TypeDictionarys

            // Step 4 - Specify the client specific configuration.
            configuration.ClientConfiguration = new ClientConfiguration();
            configuration.ClientConfiguration.DefaultSessionTimeout = 360000;

            // Step 5 - Validate the configuration.
            // This step checks if the configuration is consistent and assigns a few internal variables
            // that are used by the SDK. This is called automatically if the configuration is loaded from
            // a file using the ApplicationConfiguration.Load() method.

            configuration.Validate(ApplicationType.Client);

            return configuration;

        }

        private static List<string> GetLocalIpAddressAndDns()
        {
            List<string> localIps = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIps.Add(ip.ToString());
                }
            }
            if (localIps.Count == 0)
            {
                throw new Exception("Local IP Address Not Found!");
            }
            localIps.Add(Dns.GetHostName());
            return localIps;
        }

        #endregion
    }
}
