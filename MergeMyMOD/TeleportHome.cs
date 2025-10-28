using Cysharp.Threading.Tasks;
using Duckov.Utilities;

namespace MergeMyMOD
{
    public class TeleportHome
    {
        public static void TeleportStart()
        {
            SceneLoader.Instance.LoadBaseScene(
                    GameplayDataSettings.SceneManagement.EvacuateScreenScene, true
                )
                .Forget();
        }
    }
}