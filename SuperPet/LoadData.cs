using System.IO;
using System.Reflection;

namespace SuperPet
{
    public class LoadData
    {
        public static bool LoadDataFromFile(bool defaultValue)
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
                                if (int.TryParse(s, out int result))
                                {
                                    if (result == 0)
                                    {
                                        return false;
                                    }

                                    if (result >= 1)
                                    {
                                        return true;
                                    }
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