using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DuckovSuperDuck
{
    public class LoadData
    {
        public static Dictionary<string, float> LoadDataFromFile()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string assemblyPath = executingAssembly.Location; // 包含文件名，如 "MyAssembly.dll"
            string directory = Path.GetDirectoryName(assemblyPath); // 获取所在目录
            if (directory != null)
            {
                string dataPath = Path.Combine(directory, "Data.txt"); // 正确拼接路径
                if (File.Exists(dataPath))
                {
                    DictionaryFileHandler handler = new DictionaryFileHandler(dataPath);
                    Dictionary<string, float> loadedDict = handler.LoadDictionary<string, float>();
                    return loadedDict;
                }
                else
                {
                    File.Create(dataPath).Close();
                }
            }

            return null;
        }
    }
}