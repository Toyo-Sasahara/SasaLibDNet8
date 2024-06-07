using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using SasaLibDummy;
using SasaLib;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    public class JSonUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public void test1()
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

        /// <summary>
        /// 
        /// </summary>
        public void test2()
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

            var weatherForecast = new WeatherForecast
            {
                Date = DateTime.Parse("2019-08-01"),
                TemperatureCelsius = 25,
                Summary = "Hot"
            };

            string jsonString = JsonSerializer.Serialize(weatherForecast, options);

            Console.WriteLine(jsonString);
        }
    }

    class Person
    {
        // get; set; でないと Json化できなかった・・・
        public string FullName { get; set; }
        public int Age { get; set; }
        public string FavoriteThings { get; set; }
        public string Memo { get; set; }
    }

    class WeatherForecast
    {
        // get; set; でないと Json化できなかった・・・
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
    }

}
