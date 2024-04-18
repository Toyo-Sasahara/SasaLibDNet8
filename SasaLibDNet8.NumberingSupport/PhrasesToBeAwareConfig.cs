using System;
using System.Collections.Generic;

namespace SasaLib.NumberingSupport
{
    public class PhrasesToBeAwareConfig
    {
        public static PhrasesToBeAwareConfig Config { get; set; }

        public double VERSION;

        public double VersionNumber;
        public string VersionComment;

        public DateTime DATETIME = DateTime.MinValue;

        public struct PhrasesToBeAwareDataSet
        {
            public string Comment;
            public string Pattern; // 注意すべき文字列を正規表現で指定
            public string Replacement;
            public string Evidence; // 理由を説明
            public string HowToDeal; // 対処方法を説明
        }

        public List<PhrasesToBeAwareDataSet> PhrasesToBeAwareDataSets { get; set; }
        
        //シリアライズのためにはコンストラクタは必要
        public PhrasesToBeAwareConfig() { }
    }
}
