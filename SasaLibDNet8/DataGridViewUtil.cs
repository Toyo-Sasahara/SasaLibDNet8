using System.Windows.Forms;

namespace SasaLib
{
    public static class DataGridViewUtil
    {
        /// <summary>
        /// DataGridViewでクリックしたセル情報を取得
        /// </summary>
        public static void ShowCellContentClickHandler(object sender, DataGridViewCellEventArgs args)
        {
            DataGridView g = sender as DataGridView;

            if (g != null)
            {
                int col = args.ColumnIndex;
                int row = args.RowIndex;

                //
                // クリックがヘッダー部分などの場合は、どちらかの
                // インデックスが-1となります。
                //
                if (col >= 0 && row >= 0)
                {
                    MessageBox.Show($"行：{row}, 列：{col}, 値：{g[col, row].Value}");
                }
            }
        }

    }
}
