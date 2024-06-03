using Inventor;

namespace SasaLib.InventorAPI
{
    public class ErrorMangerDialog
    {
        ErrorManager ErrorManager;
        Application Application;

        public ErrorMangerDialog(Application inventorApplication)
        {
            Application = inventorApplication;

            ErrorManager = Application.ErrorManager;
        }

        public void Test1()
        {

            MessageSection oMsgSection1 = ErrorManager.StartMessageSection();

            MessageSection oMsgSection2 = ErrorManager.StartMessageSection();

            ErrorManager.AddMessage("3つ目のエラー", true);

            oMsgSection2.AdoptMessages("2つ目のエラー", true);

            oMsgSection1.AdoptMessages("最初のエラー", true);



            ErrorManager.Show("エラーダイアログテスト1", false, false);

        }

        public void Test2()
        {
            MessageSection oMsgSection1 = ErrorManager.StartMessageSection();
            oMsgSection1.AdoptMessages("最初のエラー", true);

            MessageSection oMsgSection2 = ErrorManager.StartMessageSection();
            oMsgSection2.AdoptMessages("2つ目のエラー", true);

            ErrorManager.AddMessage("3つ目のエラー", true);


            ErrorManager.Show("エラーダイアログテスト1", false, false);
        }

        public void Test3()
        {
            ErrorManager.AddMessage("ABC", true);
            ErrorManager.AddMessage("DEF", true);
            ErrorManager.AddMessage("GHI", true);


            ErrorManager.Show("エラーダイアログテスト3", false, false);
        }

        public void Test4()
        {
            ErrorManager.AddMessage("AllowAccept = false, AllowEdit = true", false);

            ErrorManager.Show("エラーダイアログテスト4", false, true);

        }

        public void Test5()
        {
            ErrorManager.AddMessage("AllowAccept = true, AllowEdit = false", false);

            ErrorManager.Show("エラーダイアログテスト5", true, false);

        }

        public void Test6()
        {
            ErrorManager.AddMessage("AllowAccept = true, AllowEdit = true", false);

            ErrorManager.Show("エラーダイアログテスト6", true, true);

        }


        public ButtonTypeEnum Show(string Title,bool AllowAccept, bool AllowEdit)
        {
            ButtonTypeEnum ans = ErrorManager.Show(Title, AllowAccept, AllowEdit);

            return ans;
        }

    }
}
