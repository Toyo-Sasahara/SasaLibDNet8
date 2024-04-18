using System;
using System.Collections.Generic;

namespace SasaLib.NumberingSupport
{
    public class PurchasingManufacturerCodeConfig
    {
        public static PurchasingManufacturerCodeConfig Config { get; set; }

        public double VERSION;

        public double VersionNumber;
        public string VersionComment;

        public DateTime DATETIME = DateTime.MinValue;

        public struct PurchasingManufacturer
        {
            public string Pattern;
            public string NAME;
            public string CODE;
        }

        public  List<PurchasingManufacturer> PurchasingManufacturerDatas { get; set; }
        
        //シリアライズのためにはコンストラクタは必要
        public PurchasingManufacturerCodeConfig()
        {
            // デシリアライズする前に参照される場合のために初期化
            PurchasingManufacturerDatas = new List<PurchasingManufacturerCodeConfig.PurchasingManufacturer>();
        }
    }
}
