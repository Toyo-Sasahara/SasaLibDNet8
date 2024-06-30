//using System;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class MemCheck
//    {
//        long beforeSize;
//        long afterSize;

//        /// <summary>
//        /// 
//        /// </summary>
//        public MemCheck()
//        {
//            beforeSize = GC.GetTotalMemory(false);
//            Console.WriteLine("現在のメモリ使用量 {0} Byte", beforeSize.ToString("N0"));
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void GetAfterMem()
//        {
//            afterSize = GC.GetTotalMemory(false);
//            long deltaSize =   afterSize - beforeSize;
//            Console.WriteLine("現在のメモリ使用量 {0} Byte,増減={1}Byte", afterSize.ToString("N0"),deltaSize.ToString("N0"));
//       }


//    }
//}
