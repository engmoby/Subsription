using System;
using AutoMapper;
using MSharp.Framework;
using Repository.Pattern.UnitOfWork;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.Interfaces;
using Subscription.BLL.Services.ManageStorage;
using Subscription.DAL.Entities.Model;
using Subscription.Common.CustomException;

namespace Subscription.BLL.Services
{
    public class BackgroundFacade : BaseFacade, IBackgroundFacade
    {
        private readonly IBackgroundService _backgroundService;
        private readonly IManageStorage _manageStorage;

        public BackgroundFacade(IBackgroundService backgroundService, IManageStorage manageStorage, IUnitOfWorkAsync unitOfWork) : base(unitOfWork)
        {
            _backgroundService = backgroundService;
            _manageStorage = manageStorage;
        }

        public BackgroundFacade(IBackgroundService backgroundService, IManageStorage manageStorage)
        {
            _backgroundService = backgroundService;
            _manageStorage = manageStorage;
        }

        public void AddBackground(BackgroundDto backgroundDto, string path)
        {
            var background = Mapper.Map<Background>(backgroundDto);

            background.CreationTime = DateTime.Now;
            background.LastModificationTime = DateTime.Now;
            background.DeletionTime = DateTime.Now;
            background.IsActive = true;
            _backgroundService.Insert(background);
            SaveChanges();
            _manageStorage.UploadImage(path + "\\" + "Background", backgroundDto.Image, background.BackgroundId.ToString());
        }

        public BackgroundDto GetBackground(long backgroundId)
        {
            var background = _backgroundService.Find(backgroundId);
            if (background == null) throw new NotFoundException(ErrorCodes.BackgroundNotFound);
            if (background.IsDeleted) throw new NotFoundException(ErrorCodes.BackgroundDeleted);
            return Mapper.Map<Background, BackgroundDto>(background);
        }

        public PagedResultsDto GetAllBackgrounds(int page, int pageSize, long userId)
        {
            var backgroundObj = _backgroundService.GetAllBackgrounds(page, pageSize, userId);
            if (backgroundObj == null) throw new NotFoundException(ErrorCodes.BackgroundNotFound);


            var results = backgroundObj;
            return results;
        }

        public PagedResultsDto GetActivatedBackgroundByUserId(long userId, int page, int pageSize)
        {
            var backgroundObj = _backgroundService.Find(userId);
            if (backgroundObj == null) throw new NotFoundException(ErrorCodes.BackgroundNotFound);
            if (backgroundObj.IsDeleted) throw new Common.CustomException.ValidationException(ErrorCodes.BackgroundDeleted);
            var results = _backgroundService.GetAllBackgrounds(page, pageSize,userId);
            return results;
        }


        public void DeleteBackground(long backgroundId)
        {
            var background = _backgroundService.Find(backgroundId);
            if (background == null) throw new NotFoundException(ErrorCodes.BackgroundNotFound);
            background.IsDeleted = true;
            background.IsActive = false;
            _backgroundService.Update(background);

            SaveChanges();
        }

        public void UpdateBackground(BackgroundDto backgroundDto, string path)
        {
            var background = _backgroundService.Find(backgroundDto.BackgroundId);
            if (background == null) throw new NotFoundException(ErrorCodes.BackgroundNotFound);

            _backgroundService.Update(background);
            SaveChanges();
            if (backgroundDto.IsImageChange)
                _manageStorage.UploadImage(path + "\\" + "Background", backgroundDto.Image, background.BackgroundId.ToString());
        }
    }
}
