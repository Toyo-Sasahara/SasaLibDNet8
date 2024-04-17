using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace SasaLibDNet8
{
    public class JSonUtils
    {
        public void test()
        {
            // オプション設定
            var options = new JsonSerializerOptions
            {
                // 日本語を変換するためのエンコード設定
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

                // プロパティ名をキャメルケースに変換
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                // プロパティ名をスネークケースに変換（自作ポリシーの適用）
                //PropertyNamingPolicy = new SnakeCaseNamingPolicy(),

                // インデントを付ける
                WriteIndented = true
            };

            var person = new Person
            {
                FullName = "田中太郎",
                Age = 30,
                FavoriteThings = "読書",
                Memo = "こんにちは"
            };

            // シリアライズ
            var jsonString = JsonSerializer.Serialize(person, options);
            Console.WriteLine(jsonString);

            // デシリアライズ
            var person2 = JsonSerializer.Deserialize<Person>(jsonString, options);
            Console.WriteLine($"{person2?.FullName} {person2?.Age} {person2?.FavoriteThings}");

        }
    }
}
