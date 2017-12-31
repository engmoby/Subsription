using Subscription.BLL.DTOs;

namespace Subscription.BLL.Services.Interfaces
{
    public interface IBackgroundFacade
    {
        void AddBackground(BackgroundDto backgroundDto, string path);
        BackgroundDto GetBackground(long backgroundId);
        PagedResultsDto GetAllBackgrounds(int page, int pageSize, long userId); 
      
        void UpdateBackground(BackgroundDto backgroundDto, string path);
        PagedResultsDto GetActivatedBackgroundByUserId(long userId, int page, int pageSize);
    }
}
