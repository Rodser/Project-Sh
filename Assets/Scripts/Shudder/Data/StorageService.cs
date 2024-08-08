using YG;

namespace Shudder.Data
{
    public class StorageService
    {
        public void SaveProgress(PlayerProgress progress)
        {
            YandexGame.savesData.PlayerProgress = progress;
            YandexGame.SaveProgress();
        }

        public PlayerProgress LoadProgress()
        {
            return YandexGame.savesData.PlayerProgress;
        }
    }
}