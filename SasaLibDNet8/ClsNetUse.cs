using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SasaLib
{
    /// <summary>
    /// Code from https://qiita.com/h-ymmr/items/48aa308b8219f35f7255
    /// </summary>
    public class ClsNetUse
    {
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int WNetCancelConnection2(string lpName, Int32 dwFlags, bool fForce);

        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, Int32 dwFlags);

        /// <summary>
        /// 
        /// </summary>
        public enum ResourceScope
        {
            /// <summary>
            /// 
            /// </summary>
            RESOURCE_CONNECTED = 1,
            /// <summary>
            /// 
            /// </summary>
            RESOURCE_GLOBALNET,
            /// <summary>
            /// 
            /// </summary>
            RESOURCE_REMEMBERED,
            /// <summary>
            /// 
            /// </summary>
            RESOURCE_RECENT,
            /// <summary>
            /// 
            /// </summary>
            RESOURCE_CONTEXT
        };

        /// <summary>
        /// 
        /// </summary>
        public enum ResourceType
        {
            /// <summary>
            /// 
            /// </summary>
            RESOURCETYPE_ANY,
            /// <summary>
            /// 
            /// </summary>
            RESOURCETYPE_DISK,
            /// <summary>
            /// 
            /// </summary>
            RESOURCETYPE_PRINT,
            /// <summary>
            /// 
            /// </summary>
            RESOURCETYPE_RESERVED = 8
        };

        /// <summary>
        /// 
        /// </summary>
        [Flags]        
        public enum ResourceUsage
        {
            /// <summary>
            /// 
            /// </summary>
            RESOURCEUSAGE_CONNECTABLE = 0x00000001,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEUSAGE_CONTAINER = 0x00000002,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEUSAGE_SIBLING = 0x00000008,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEUSAGE_ATTACHED = 0x00000010,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE |
                                 RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        };

        /// <summary>
        /// 
        /// </summary>
        public enum ResourceDisplayType
        {
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_GENERIC,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_DOMAIN,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_SERVER,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_SHARE,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_FILE,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_GROUP,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_NETWORK,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_ROOT,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_SHAREADMIN,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_DIRECTORY,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_TREE,
            /// <summary>
            /// 
            /// </summary>
            RESOURCEDISPLAYTYPE_NDSCONTAINER
        };

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum AddConnectionOptions
        {
            /// <summary>
            /// 
            /// </summary>
            CONNECT_UPDATE_PROFILE = 0x00000001,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_UPDATE_RECENT = 0x00000002,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_TEMPORARY = 0x00000004,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_INTERACTIVE = 0x00000008,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_PROMPT = 0x00000010,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_NEED_DRIVE = 0x00000020,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_REFCOUNT = 0x00000040,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_REDIRECT = 0x00000080,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_LOCALDRIVE = 0x00000100,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_CURRENT_MEDIA = 0x00000200,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_DEFERRED = 0x00000400,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_RESERVED = unchecked((int)0xFF000000),
            /// <summary>
            /// 
            /// </summary>
            CONNECT_COMMANDLINE = 0x00000800,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_CMD_SAVECRED = 0x00001000,
            /// <summary>
            /// 
            /// </summary>
            CONNECT_CRED_RESET = 0x00002000
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NETRESOURCE
        {
            public ResourceScope dwScope;
            public ResourceType dwType;
            public ResourceDisplayType dwDisplayType;
            public ResourceUsage dwUsage;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpLocalName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpRemoteName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpComment;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpProvider;
        }

        List<int> _listNetUseOkErrNo = new List<int>();
        private String _strPathDShare = null;
        private String _strDriveName = null;
        private String _strDomainName = null;
        private String _strUserName = "";
        private String _strPassword = "";
        private Boolean _blnIsLogonAlwaysOk = false;
        private String _strMessage = "";

        /// <summary>
        /// 
        /// </summary>
        public List<int> listNetUseOkErrNo { get { return _listNetUseOkErrNo; } set { _listNetUseOkErrNo = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String strPathDShare { get { return _strPathDShare; } set { _strPathDShare = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String strDriveName { get { return _strDriveName; } set { _strDriveName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String strUserName { get { return _strUserName; } set { _strUserName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String strPassword { get { return _strPassword; } set { _strPassword = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String strDomainName { get { return _strDomainName; } set { _strDomainName = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String strMessage { get { return _strMessage; } set { _strMessage = value; } }
        /// <summary>
        /// 
        /// </summary>
        public Boolean blnIsLogonAlwaysOk { get { return _blnIsLogonAlwaysOk; } set { _blnIsLogonAlwaysOk = value; } }

        /// <summary>
        /// 
        /// </summary>
        public ClsNetUse()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean Connect()
        {
            ClsWinError objWinError = new ClsWinError();
            Boolean blnIsOk = true;
            _strMessage = "NG";
            if (String.IsNullOrEmpty(_strPathDShare)) _strPathDShare = "パスが指定されていません。";
            NETRESOURCE objNetResource = new NETRESOURCE();
            objNetResource.dwScope = 0;
            objNetResource.dwType = ResourceType.RESOURCETYPE_DISK;
            objNetResource.dwDisplayType = 0;
            objNetResource.dwUsage = 0;
            objNetResource.lpLocalName = (String.IsNullOrEmpty(_strDriveName) ? "" : _strDriveName + ":");
            objNetResource.lpRemoteName = _strPathDShare;
            objNetResource.lpProvider = "";
            try
            {
                String strLogonUser = _strUserName;
                if (!String.IsNullOrEmpty(_strDomainName)) strLogonUser = _strDomainName + @"\" + _strUserName;
                int intReturn = WNetAddConnection2(ref objNetResource, _strPassword, strLogonUser, 0);
                switch (intReturn)
                {
                    case 0:
                        _strMessage = "OK";
                        break;
                    default:
                        if (_blnIsLogonAlwaysOk)
                        {
                            _strMessage = "--";
                        }
                        else
                        {
                            if (_listNetUseOkErrNo.Count > 0)
                            {
                                if (_listNetUseOkErrNo.Contains(intReturn))
                                {
                                    _strMessage = "--";
                                }
                                else
                                {
                                    blnIsOk = false;
                                    _strMessage = "NG";
                                }
                            }
                            else
                            {
                                blnIsOk = false;
                                _strMessage = "NG";
                            }
                        }
                        break;
                }
                String strErrDesc = objWinError.GetWinErrMessage(intReturn);
                _strMessage += " : 接続(" + intReturn + ") => " + _strPathDShare;
                if (!String.IsNullOrEmpty(strErrDesc)) _strMessage += " : " + strErrDesc;
            }
            catch (Exception e)
            {
                if (_blnIsLogonAlwaysOk)
                {
                    _strMessage = "-- : 接続(EXCEPTION) => " + _strPathDShare + " : " + e.Message;
                }
                else
                {
                    blnIsOk = false;
                    _strMessage = "NG : 接続(EXCEPTION) => " + _strPathDShare + " : " + e.Message;
                }
            }
            return blnIsOk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean DisConnect()
        {
            ClsWinError objWinError = new ClsWinError();
            Boolean blnIsOk = true;
            _strMessage = "NG";
            if (String.IsNullOrEmpty(_strPathDShare)) _strPathDShare = "パスが指定されていません。";
            try
            {
                int intReturn = WNetCancelConnection2(_strPathDShare, 0, true);
                switch (intReturn)
                {
                    case 0:
                    case 2250:
                        _strMessage = "OK";
                        break;
                    default:
                        if (_blnIsLogonAlwaysOk)
                        {
                            _strMessage = "--";
                        }
                        else
                        {
                            if (_listNetUseOkErrNo.Count > 0)
                            {
                                if (_listNetUseOkErrNo.Contains(intReturn))
                                {
                                    _strMessage = "--";
                                }
                                else
                                {
                                    blnIsOk = false;
                                    _strMessage = "NG";
                                }
                            }
                            else
                            {
                                blnIsOk = false;
                                _strMessage = "NG";
                            }
                        }
                        break;
                }
                String strErrDesc = objWinError.GetWinErrMessage(intReturn);
                _strMessage += " : 切断(" + intReturn + ") => " + _strPathDShare;
                if (!String.IsNullOrEmpty(strErrDesc)) _strMessage += " : " + strErrDesc;
            }
            catch (Exception e)
            {
                if (_blnIsLogonAlwaysOk)
                {
                    _strMessage = "-- : 切断(EXCEPTION) => " + _strPathDShare + " : " + e.Message;
                }
                else
                {
                    blnIsOk = false;
                    _strMessage = "NG : 切断(EXCEPTION) => " + _strPathDShare + " : " + e.Message;
                }
            }
            return blnIsOk;
        }

    }
}