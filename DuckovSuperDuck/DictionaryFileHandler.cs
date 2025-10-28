using System;
using System.Collections.Generic;
using System.IO;

namespace DuckovSuperDuck
{
    public class DictionaryFileHandler
    {
        private readonly string _filePath;
 
        public DictionaryFileHandler(string filePath)
        {
            _filePath = filePath;
        }
 
        // 保存字典（每行存储一个 Key=Value）
        public void SaveDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                foreach (var kvp in dictionary)
                {
                    writer.WriteLine($"{kvp.Key}={kvp.Value}");
                }
            }
        }
 
        // 从文件读取字典
        public Dictionary<TKey, TValue> LoadDictionary<TKey, TValue>()
        {
            var dict = new Dictionary<TKey, TValue>();
            if (!File.Exists(_filePath))
                return dict;
 
            foreach (string line in File.ReadLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
 
                string[] parts = line.Split(new[] { '=' }, 2);
                if (parts.Length == 2)
                {
                    TKey key = (TKey)Convert.ChangeType(parts[0], typeof(TKey));
                    TValue value = (TValue)Convert.ChangeType(parts[1], typeof(TValue));
                    dict[key] = value;
                }
            }
 
            return dict;
        }
    }
}