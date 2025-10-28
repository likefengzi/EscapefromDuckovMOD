using System.IO;
using System.Reflection;

namespace WeakerEnemy
{
    public class LoadData
    {
        public static float LoadDataFromFile(float defaultValue)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string assemblyPath = executingAssembly.Location; // 包含文件名，如 "MyAssembly.dll"
            string directory = Path.GetDirectoryName(assemblyPath); // 获取所在目录
            if (directory != null)
            {
                string dataPath = Path.Combine(directory, "Data.txt"); // 正确拼接路径
                if (File.Exists(dataPath))
                {
                    using (StreamReader sr = new StreamReader(dataPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            if (!string.IsNullOrEmpty(s))
                            {
                                if (float.TryParse(s, out float result))
                                {
                                    return result;
                                }
                                return defaultValue;
                            }
                        }
                    }
                }
                else
                {
                    File.Create(dataPath).Close();
                }
            }

            return defaultValue;
        }
    }
}