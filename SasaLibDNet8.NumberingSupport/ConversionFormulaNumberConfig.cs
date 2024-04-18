using System.Collections.Generic;

namespace SasaLib.NumberingSupport
{
    public struct ConversionFormula
    {
        public string Comment;
        public string Pattern;
        public string Replacement;
    }

    /// <summary>
    /// 図番ﾊﾟﾀｰﾝとArcSuite登録図面番号対応表ﾌｧｲﾙ
    /// </summary>
    public class ConversionFormulaNumberConfig
    {
        public static ConversionFormulaNumberConfig Config { get; set; }

        //public double VERSION;

        public double VersionNumber;
        public string VersionComment;

        public string COMMNENT01;
        public string COMMNENT02;
        public string COMMNENT03;

        public List<ConversionFormula> ConversionFormulas { get; set; }
        
        //シリアライズのためにはコンストラクタは必要
        public ConversionFormulaNumberConfig() { }
    }
}
