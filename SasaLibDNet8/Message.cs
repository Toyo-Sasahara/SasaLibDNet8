using System;

// テスト用・これをオブジェクトとしてパイプを通す
namespace SasaLibDNet8.PIPE
{
    // メッセージ
    [Serializable]
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $@"{{ {nameof(Id)} = {Id}, {nameof(Content)} = ""{Content}"" }}";
        }
    }
}