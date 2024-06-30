//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SasaLib
//{
//    /// <summary>
//    ///  目的の値に最も近い値を返します
//    ///  使い方
//    /// var list = new[] { 1, 2, 4, 8, 16 };
//    /// Debug.Log(list.Nearest( 10 ) ); // 8
//    /// </summary>
//    public static class IEnumerableExtensions
//    {
//        /// <summary>
//        /// 目的の値に最も近い値を返します
//        /// </summary>
//        public static int Nearest(
//            this IEnumerable<int> self,
//            int target
//        )
//        {
//            var min = self.Min(c => Math.Abs(c - target));
//            return self.First(c => Math.Abs(c - target) == min);
//        }
//    }
//}
