using System;
using Inventor;
using SasaLib.InventorAPI;
using Uno.UI.Xaml;
using Windows.UI.Xaml;
using Application = Inventor.Application;

namespace SsasaLib.InventorAPI
{
    public static class ContentCenterLibraryTest
    {
        public static void AddMemberTest(Application inventorApp)
        {

            // Content Centerライブラリの取得
            ContentCenter contentCenter = inventorApp.ContentCenter;

            // 丸棒ファミリを検索
            string familyName = "Round Bars"; // コンテンツセンター内のファミリ名を指定
                                              //ContentFamily roundBarFamily = FindFamily(contentCenter, ContentIdentifier: "v3#330ae6e6-eeb7-4353-b38a-0fd76739d23b#", familyName);
            ContentFamily roundBarFamily = (ContentFamily)contentCenter.GetContentObject("v3#330ae6e6-eeb7-4353-b38a-0fd76739d23b#");
            if (roundBarFamily != null)
            {
                // 新しいメンバを追加
                AddNewMember(roundBarFamily, 50); // 50mmの長さを例として追加
                Console.WriteLine("新しいメンバを追加しました。");
            }
            else
            {
                Console.WriteLine("指定したファミリが見つかりません。");
            }
        }

        static ContentFamily FindFamily(ContentCenter contentCenter, string ContentIdentifier, string familyName)
        {


            ContentFamily contentObject = (ContentFamily)contentCenter.GetContentObject(ContentIdentifier);

            ObjectTypeEnum objType = (Inventor.ObjectTypeEnum)InventorControl.GetComObjectMember(contentObject, "Type");


            //// コンテンツファミリーを特定しメンバファイルを作成
            //ContentTableRow oContentTableRow = (ContentTableRow)contentObject;

            var familyType = contentObject.FamilyType;

            //foreach (ContentFamily family in oContentTableRow)
            //{
            //    if (family.DisplayName == familyName)
            //    {
            //        return family;
            //    }
            //}
            return null;
        }

        static void AddNewMember(ContentFamily family, double newLength)
        {
            string[] rowData = default;

            // 新しいメンバのパラメータを設定
            ContentTableRow newRow = family.TableRows.Add(ref rowData, -1);
            ContentTableColumn lengthColumn = family.TableColumns["Length"];

            if (lengthColumn != null)
            {
                //newRow.SetValue(lengthColumn, newLength);
            }

            // 必要に応じて他のパラメータも設定
            // newRow.SetValue(family.TableColumns["他のパラメータ名"], 値);
        }



        //static void ContentQueryTest(Application inventorApp)
        //{
        //    ContentCenter contentCenter = inventorApp.ContentCenter;

        //    string familyName = "Round Bars"; // 検索するファミリ名
        //    double targetLength = 50; // 検索する長さ（例）

        //    ContentFamily roundBarFamily = FindFamily(contentCenter, familyName);
        //    if (roundBarFamily != null)
        //    {
        //        ContentTableRow foundRow = FindMemberByLength(roundBarFamily, targetLength);
        //        if (foundRow != null)
        //        {
        //            Console.WriteLine($"長さ {targetLength}mm のメンバが見つかりました。");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"長さ {targetLength}mm のメンバは存在しません。");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("指定したファミリが見つかりません。");
        //    }
        //}

        //static ContentFamily FindFamily(ContentCenter contentCenter, string familyName)
        //{
        //    foreach (ContentFamily family in contentCenter.Families)
        //    {
        //        if (family.DisplayName == familyName)
        //        {
        //            return family;
        //        }
        //    }
        //    return null;
        //}

        //static ContentTableRow FindMemberByLength(ContentFamily family, double length)
        //{
        //    // ContentQueryオブジェクトを作成
        //    ContentQuery query = family.CreateContentQuery();
        //    query.RegisterDisposablePropertyChangedCallback
        //    // 長さに基づく検索条件を設定
        //    query.AddCondition("Length", ConditionEnum.kEqualCondition, length);

        //    // クエリを実行して結果を取得
        //    ContentTableRows results = family.TableRows.Query(query);

        //    // 最初に見つかった行を返す
        //    return results.Count > 0 ? results[1] : null; // インデックスは1から始まる
        //}
    }
}
