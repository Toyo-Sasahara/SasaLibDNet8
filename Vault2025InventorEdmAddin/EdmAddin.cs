using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.AutodeskVault
{
    public static class EdmAddin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object GetInventorEdmAddinVaultConnection()
        {
            Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections.Connection connection = Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance.VaultConnection;

            return connection;
        }

        //public static object GetInventorEdmAddinEdmSecurityInstance()
        //{

        //    return Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsSignedIn()
        {
            if (Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance == null)
                return false;
            return Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance.IsSignedIn();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vaultLogin"></param>
        /// <returns></returns>
        public static bool OnLoginButtonExecute(bool vaultLogin)
        {
            if (Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance == null)
                return false;

            Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance.OnLoginButtonExecute(vaultLogin);

            return Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance.IsSignedIn();
        }
    }
}