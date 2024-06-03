using Inventor;
using System;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// アプリケーションオプション に対する設定
    /// </summary>
    public class ApplicationOptions
    {

        /// <summary>
        /// Inventor.Application オブジェクト
        /// </summary>
        Application Application;
        /// <summary>
        /// Inventor.DrawingOptions オブジェクト
        /// </summary>
        DrawingOptions DrawingOptions;
        /// <summary>
        /// Inventor.PartOptions オブジェクト
        /// </summary>
        PartOptions PartOptions;
        /// <summary>
        /// Inventor.AssemblyOptions オブジェクト
        /// </summary>
        AssemblyOptions AssemblyOptions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="application"></param>
        public ApplicationOptions(Application application)
        {
            // フィールドへ挿入
            Application = application;
            DrawingOptions = Application.DrawingOptions;
            PartOptions = Application.PartOptions;
            AssemblyOptions = Application.AssemblyOptions;
        }

        /// <summary>
        /// アプリケーションオプション＞図面＞線幅表示＞線幅を表示 on/off を設定する
        /// </summary>
        /// <param name="use"></param>
        public void DrawingOption_SetDisplayLineWeights(bool use)
        {
            DrawingOptions.DisplayLineWeights = use;
            try
            {
                if (use == true)
                {
                    DrawingOptions.LineWeightType = LineWeightTypeEnum.kTrueLineWeight;
                }
                else
                {
                    DrawingOptions.LineWeightType = LineWeightTypeEnum.kRangeLineWeight;
                }
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"DrawingOption_SetDisplayLineWeights(..) 例外検知 {ex.Message}");
            }

        }

        /// <summary>
        /// アプリケーションオプション＞図面＞線幅表示＞線幅を表示 on/off 現在の設定を読み出す
        /// </summary>
        /// <returns></returns>
        public bool DrawingOption_GetDisplayLineWeights()
        {
            return DrawingOptions.DisplayLineWeights;
        }


        /// <summary>
        /// ブラウザ内のフィーチャ名の後に拡張情報を表示するかどうかを取得および設定する
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public void PartOptions_SetDisplayExtendedName(bool flag)
        {
            PartOptions.DisplayExtendedName = flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool PartOptions_GetDisplayExtendedName()
        {
            return PartOptions.DisplayExtendedName;
        }

        /// <summary>
        /// アクティブなコンポーネントの不透明性を設定します。
        /// </summary>
        /// <param name="flag"></param>
        public void AssemblyOptions_SetOnlyActiveComponentIsOpaque(bool flag)
        {
            AssemblyOptions.OnlyActiveComponentIsOpaque = flag;
        }
    }
}
