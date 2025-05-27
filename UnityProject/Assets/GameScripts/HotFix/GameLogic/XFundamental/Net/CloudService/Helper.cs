using System;
using System.Collections.Generic;

namespace GameScripts.Net
{
    public static class Helper
    {
        private static readonly Random Random = new();
        private static readonly List<string> Emojis = new() { "😀", "😂", "😎" };
        
        public static string GetRandomEmoji()
        {
            return Emojis[Random.Next(Emojis.Count)];
        }

    }
}