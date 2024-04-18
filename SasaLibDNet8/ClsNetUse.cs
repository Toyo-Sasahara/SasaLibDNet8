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

        public enum ResourceScope
        {
            RESOURCE_CONNECTED = 1,
            RESOURCE_GLOBALNET,
            RESOURCE_REMEMBERED,
            RESOURCE_RECENT,
            RESOURCE_CONTEXT
        };

        public enum ResourceType
        {
            RESOURCETYPE_ANY,
            RESOURCETYPE_DISK,
            RESOURCETYPE_PRINT,
            RESOURCETYPE_RESERVED = 8
        };

        [Flags]
        public enum ResourceUsage
        {
            RESOURCEUSAGE_CONNECTABLE = 0x00000001,
            RESOURCEUSAGE_CONTAINER = 0x00000002,
            RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
            RESOURCEUSAGE_SIBLING = 0x00000008,
            RESOURCEUSAGE_ATTACHED = 0x00000010,
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE |
                                 RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        };

        public enum ResourceDisplayType
        {
            RESOURCEDISPLAYTYPE_GENERIC,
            RESOURCEDISPLAYTYPE_DOMAIN,
            RESOURCEDISPLAYTYPE_SERVER,
            RESOURCEDISPLAYTYPE_SHARE,
            RESOURCEDISPLAYTYPE_FILE,
            RESOURCEDISPLAYTYPE_GROUP,
            RESOURCEDISPLAYTYPE_NETWORK,
            RESOURCEDISPLAYTYPE_ROOT,
            RESOURCEDISPLAYTYPE_SHAREADMIN,
            RESOURCEDISPLAYTYPE_DIRECTORY,
            RESOURCEDISPLAYTYPE_TREE,
            RESOURCEDISPLAYTYPE_NDSCONTAINER
        };

        [Flags]
        public enum AddConnectionOptions
        {
            CONNECT_UPDATE_PROFILE = 0x00000001,
            CONNECT_UPDATE_RECENT = 0x00000002,
            CONNECT_TEMPORARY = 0x00000004,
            CONNECT_INTERACTIVE = 0x00000008,
            CONNECT_PROMPT = 0x00000010,
            CONNECT_NEED_DRIVE = 0x00000020,
            CONNECT_REFCOUNT = 0x00000040,
            CONNECT_REDIRECT = 0x00000080,
            CONNECT_LOCALDRIVE = 0x00000100,
            CONNECT_CURRENT_MEDIA = 0x00000200,
            CONNECT_DEFERRED = 0x00000400,
            CONNECT_RESERVED = unchecked((int)0xFF000000),
            CONNECT_COMMANDLINE = 0x00000800,
            CONNECT_CMD_SAVECRED = 0x00001000,
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

        public List<int> listNetUseOkErrNo { get { return _listNetUseOkErrNo; } set { _listNetUseOkErrNo = value; } }
        public String strPathDShare { get { return _strPathDShare; } set { _strPathDShare = value; } }
        public String strDriveName { get { return _strDriveName; } set { _strDriveName = value; } }
        public String strUserName { get { return _strUserName; } set { _strUserName = value; } }
        public String strPassword { get { return _strPassword; } set { _strPassword = value; } }
        public String strDomainName { get { return _strDomainName; } set { _strDomainName = value; } }
        public String strMessage { get { return _strMessage; } set { _strMessage = value; } }
        public Boolean blnIsLogonAlwaysOk { get { return _blnIsLogonAlwaysOk; } set { _blnIsLogonAlwaysOk = value; } }

        public ClsNetUse()
        {
        }

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