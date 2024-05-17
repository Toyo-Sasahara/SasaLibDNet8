using Connectivity.InventorAddin.EdmAddin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.AutodeskVault
{
    public static class EdmAddin
    {

        public static object GetInventorEdmAddinVaultConnection()
        {
            Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections.Connection connection = Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance.VaultConnection;

            return connection;
        }

        public static object GetInventorEdmAddinEdmSecurityInstance()
        {

            return Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance;

        }
    }
}