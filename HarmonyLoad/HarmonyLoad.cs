using System.IO;
using System.Reflection;

namespace HarmonyLoad
{
    public class HarmonyLoad
    {
        public static Assembly Load0Harmony()
        {
            // 在项目属性中设置 0Harmony.dll 的 "生成操作" 为 "嵌入的资源"
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            // 获取当前类的命名空间（假设当前类在 "DeadNoDrop" 命名空间下）
            string currentNamespace = typeof(HarmonyLoad).Namespace;
            using (Stream stream = executingAssembly.GetManifestResourceStream($"{currentNamespace}.0Harmony.dll"))
            using (MemoryStream ms = new MemoryStream())
            {
                stream?.CopyTo(ms);
                Assembly assembly = Assembly.Load(ms.ToArray());

                return assembly;
            }
        }
    }
}