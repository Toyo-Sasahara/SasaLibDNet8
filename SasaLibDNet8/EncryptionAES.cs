using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static System.Net.Mime.MediaTypeNames;

namespace SasaLib
{

    /// <summary>
    /// AES方式で文字列を暗号化／復号する
    /// https://atmarkit.itmedia.co.jp/ait/articles/1709/06/news020.html
    /// </summary>
    public static class EncryptionAES
    {

        /// <summary>
        /// 入力文字列をAES暗号化してBase64形式で返すメソッド(同期版)
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static async Task<string> EncryptToBase64(string plainText, byte[] key, byte[] iv)
        {
            try
            {
                // 入力文字列をバイト型配列に変換
                byte[] src = Encoding.Unicode.GetBytes(plainText);
                DebugConsole.WriteLine($"EncryptionAES.EncryptToBase64(..) 平文のバイト型配列の長さ={src.Length}");
                // 出力例：平文のバイト型配列の長さ=60

                // Encryptor（暗号化器）を用意する
                using (var am = new AesManaged())
                using (var encryptor = am.CreateEncryptor(key, iv))
                // ファイルを入力とするなら、ここでファイルを開く
                // using (FileStream inStream = new FileStream(FilePath, ……省略……
                // 出力ストリームを用意する
                using (var outStream = new MemoryStream())
                {
                    // 暗号化して書き出す
                    using (var cs = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(src, 0, src.Length);
                        // 入力がファイルなら、inStreamから一定量ずつバイトバッファーに読み込んで
                        // cse.Writeで書き込む処理を繰り返す（復号のサンプルコードを参照）
                    }
                    // 出力がファイルなら、以上で完了

                    // Base64文字列に変換して返す
                    byte[] result = outStream.ToArray();
                    DebugConsole.WriteLine($"EncryptionAES.EncryptToBase64(..) 暗号のバイト型配列の長さ={result.Length}");
                    // 出力例：暗号のバイト型配列の長さ=64
                    // 出力サイズはBlockSize（既定値16バイト）の倍数になる
                    return Convert.ToBase64String(result);
                }

            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.EncryptionAES", EventLogEntryType.Error, 0, $"※\"SasaLib.EncryptionAES.EncryptToBase64()にて例外 {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 入力文字列をAES暗号化してBase64形式で返すメソッド(非同期版)
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string EncryptToBase64NoAsync(string plainText, byte[] key, byte[] iv)
        {
            try
            {

                // 入力文字列をバイト型配列に変換
                byte[] src = Encoding.Unicode.GetBytes(plainText);
                DebugConsole.WriteLine($"EncryptionAES.EncryptToBase64(..) 平文のバイト型配列の長さ={src.Length}");
                // 出力例：平文のバイト型配列の長さ=60

                // Encryptor（暗号化器）を用意する
                using (var am = new AesManaged())
                using (var encryptor = am.CreateEncryptor(key, iv))
                // ファイルを入力とするなら、ここでファイルを開く
                // using (FileStream inStream = new FileStream(FilePath, ……省略……
                // 出力ストリームを用意する
                using (var outStream = new MemoryStream())
                {
                    // 暗号化して書き出す
                    using (var cs = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(src, 0, src.Length);
                        // 入力がファイルなら、inStreamから一定量ずつバイトバッファーに読み込んで
                        // cse.Writeで書き込む処理を繰り返す（復号のサンプルコードを参照）
                    }
                    // 出力がファイルなら、以上で完了

                    // Base64文字列に変換して返す
                    byte[] result = outStream.ToArray();
                    DebugConsole.WriteLine($"EncryptionAES.EncryptToBase64(..) 暗号のバイト型配列の長さ={result.Length}");
                    // 出力例：暗号のバイト型配列の長さ=64
                    // 出力サイズはBlockSize（既定値16バイト）の倍数になる
                    return Convert.ToBase64String(result);
                }

            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.EncryptionAES", EventLogEntryType.Error, 0, $"※\"SasaLib.EncryptionAES.EncryptToBase64NoAsync()にて例外 {ex.Message}");
                return null;
            }

        }

        /// <summary>
        /// key と iv を自動生成する
        /// </summary>
        /// <param name="ivByteArray"></param>
        /// <param name="ivBase64"></param>
        /// <param name="keyByteArray"></param>
        /// <param name="keyBase64"></param>
        public static void AutoCreateKey128bit(out byte[] ivByteArray, out string ivBase64, out byte[] keyByteArray, out string keyBase64)
        {
            try
            {

                var BLOCK_SIZE = 128;   // 128bit 固定
                var KEY_SIZE = 128;     // 128/192/256bit から選択

                // AES暗号サービスを生成
                var csp = new AesCryptoServiceProvider();
                csp.BlockSize = BLOCK_SIZE;
                csp.KeySize = KEY_SIZE;
                csp.Mode = CipherMode.CBC;
                csp.Padding = PaddingMode.PKCS7;

                // IV および 鍵 を自動生成
                csp.GenerateIV();
                csp.GenerateKey();

                // 鍵を出力；
                ivBase64 = Convert.ToBase64String(csp.IV);
                keyBase64 = Convert.ToBase64String(csp.Key);

                ivByteArray = csp.IV;
                keyByteArray = csp.Key;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.EncryptionAES", EventLogEntryType.Error, 0, $"※\"SasaLib.EncryptionAES.AutoCreateKey128bit()にて例外 {ex.Message}");
                ivBase64 = null;
                keyBase64 = null;
                ivByteArray = null;
                keyByteArray = null;
                return;
            }

        }

        /// <summary>
        /// key と iv を それぞれパスワードで生成する
        /// </summary>
        /// <param name="ivPassword"></param>
        /// <param name="ivByteArray"></param>
        /// <param name="ivBase64"></param>
        /// <param name="keyPassword"></param>
        /// <param name="keyByteArray"></param>
        /// <param name="keyBase64"></param>
        public static void ManualCreateKey128bit(string ivPassword, out byte[] ivByteArray, out string ivBase64,
                                                         string keyPassword, out byte[] keyByteArray, out string keyBase64)
        {
            try
            {


                var BLOCK_SIZE = 128;   // 128bit 固定
                var KEY_SIZE = 128;     // 128/192/256bit から選択

                // IV を生成
                var rfcBlock = new Rfc2898DeriveBytes(ivPassword, BLOCK_SIZE / 8);
                var arrBlock = rfcBlock.GetBytes(BLOCK_SIZE / 8);

                ivByteArray = arrBlock;
                ivBase64 = Convert.ToBase64String(arrBlock);

                // Key を生成
                var rfcKey = new Rfc2898DeriveBytes(keyPassword, KEY_SIZE / 8);
                var arrKey = rfcKey.GetBytes(KEY_SIZE / 8);

                keyByteArray = arrKey;
                keyBase64 = Convert.ToBase64String(arrKey);
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.EncryptionAES", EventLogEntryType.Error, 0, $"※\"SasaLib.EncryptionAES.ManualCreateKey128bit()にて例外 {ex.Message}");
                ivBase64 = null;
                keyBase64 = null;
                ivByteArray = null;
                keyByteArray = null;
                return;
            }

        }


        /// <summary>
        /// 暗号化されたBase64形式の入力文字列をAES復号して平文の文字列を返すメソッド(非同期版)
        /// </summary>
        /// <param name="base64Text"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static async Task<string> DecryptFromBase64(string base64Text, byte[] key, byte[] iv)
        {
            try
            {

                // Base64文字列をバイト型配列に変換
                byte[] src = Convert.FromBase64String(base64Text);

                // Decryptor（復号器）を用意する
                using (var am = new AesManaged())
                using (var decryptor = am.CreateDecryptor(key, iv))
                // 入力ストリームを開く
                using (var inStream = new MemoryStream(src, false))
                // 出力ストリームを用意する
                using (var outStream = new MemoryStream())
                {
                    // 復号して一定量ずつ読み出し、それを出力ストリームに書き出す
                    using (var cs = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[4096]; // バッファーサイズはBlockSizeの倍数にする
                        int len = 0;
                        while ((len = await cs.ReadAsync(buffer, 0, 4096)) > 0)
                            outStream.Write(buffer, 0, len);
                    }
                    // 出力がファイルなら、以上で完了

                    // 文字列に変換して返す
                    byte[] result = outStream.ToArray();
                    return Encoding.Unicode.GetString(result);
                }

            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.EncryptionAES", EventLogEntryType.Error, 0, $"※\"SasaLib.EncryptionAES.DecryptFromBase64()にて例外 {ex.Message}");
                return null;
            }

        }

        /// <summary>
        /// 暗号化されたBase64形式の入力文字列をAES復号して平文の文字列を返すメソッド(同期版)
        /// </summary>
        /// <param name="base64Text"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static string DecryptFromBase64NoAsync(string base64Text, byte[] key, byte[] iv)
        {
            try
            {


                // Base64文字列をバイト型配列に変換
                byte[] src = Convert.FromBase64String(base64Text);

                // Decryptor（復号器）を用意する
                using (var am = new AesManaged())
                using (var decryptor = am.CreateDecryptor(key, iv))
                // 入力ストリームを開く
                using (var inStream = new MemoryStream(src, false))
                // 出力ストリームを用意する
                using (var outStream = new MemoryStream())
                {
                    // 復号して一定量ずつ読み出し、それを出力ストリームに書き出す
                    using (var cs = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[4096]; // バッファーサイズはBlockSizeの倍数にする
                        int len = 0;
                        while ((len = cs.Read(buffer, 0, 4096)) > 0)
                            outStream.Write(buffer, 0, len);
                    }
                    // 出力がファイルなら、以上で完了

                    // 文字列に変換して返す
                    byte[] result = outStream.ToArray();
                    return Encoding.Unicode.GetString(result);
                }

            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.EncryptionAES", EventLogEntryType.Error, 0, $"※\"SasaLib.EncryptionAES.DecryptFromBase64NoAsync()にて例外 {ex.Message}");
                return null;
            }

        }

        /// <summary>
        /// byte[]をBase64エンコード文字列似て返す
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string CovertFromByteArrayToBase64String(byte[] bytes)
        {
            ///Bas64にして返す
            string str = Convert.ToBase64String(bytes);
            return str;
        }

        /// <summary>
        /// Base64文字列をbyte配列にデコード
        /// </summary>
        /// <param name="base64Text"></param>
        /// <returns></returns>
        public static byte[] ConvertFromBase64StringToByteArray(string base64Text)
        {
            if (string.IsNullOrWhiteSpace(base64Text) == false)
            {
                // Base64文字列をバイト型配列に変換
                byte[] bytes = Convert.FromBase64String(base64Text);
                return bytes;
            }
            else
                return null;
        }

        /// <summary>
        /// 上記の暗号化／復号メソッドを使う例
        /// </summary>
        public static async void EncryptionAES_Sample()
        {
            // KeyとIV（一例）
            byte[] key //  Key は暗号化を行うときの共通鍵
              = {
                0xED, 0x0B, 0x56, 0xAF, 0x61, 0xA2, 0x71, 0x39,
                0xE0, 0x4B, 0xDC, 0xC9, 0x23, 0x69, 0x8C, 0xBD,
                0xB9, 0x86, 0x98, 0x28, 0xC8, 0x3E, 0x62, 0xA7,
                0xFA, 0x17, 0xC1, 0x33, 0x64, 0xBF, 0x96, 0x24
            };

            byte[] iv //（Initialization Vector） ASEのアルゴリズムの初期のトランスフォーメーションの初期値
              = {
                0x6F, 0xDF, 0x98, 0x00, 0x67, 0x36, 0x7D, 0x3B,
                0xFF, 0xC9, 0x3B, 0x79, 0x4D, 0xD4, 0x81, 0x72
            };

            // 暗号化したい平文
            string plainText = "業務アプリInsiderは業務アプリ開発者のためのサイトです";

            // 暗号化
            string encrypted = await EncryptToBase64(plainText, key, iv);
            DebugConsole.WriteLine($"暗号化の結果：'{encrypted}'");

            // 出力：'CUiUxGqvU1az7THf81lpEmijZrIn……省略……9Vr38ouENvfcaV0en5uw=='

            // 復号
            string decrypted = await DecryptFromBase64(encrypted, key, iv);
            DebugConsole.WriteLine($"複合化の結果：'{decrypted}'");
            // 出力：'業務アプリInsiderは業務アプリ開発者のためのサイトです'
        }

    }
}
