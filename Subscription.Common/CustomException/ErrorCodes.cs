﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.Common.CustomException
{
    public enum ErrorCodes
    {
        UserNotFound,
        UnSupportedLanguage,
        EmptyRestaurantType,
        RestaurantTypeExceedLength,
        RestaurantTypeAlreadyExist,
        EmptyRestaurantName,
        RestaurantTypeNotFound,
        RestaurantNameAlreadyExist,
        RestaurantNameExceedLength,
        EmptyRestaurantDescription,
        EmptyRestaurantAdminUserName,
        EmptyRestaurantAdminPassword,
        RestaurantAdminUserNameAlreadyExist,
        RestaurantAdminPasswordLengthNotMatched,
        RestaurantNotFound,
        RestaurantMenuDoesNotActivated,
        RestaurantDeleted,
        EmptyMenuName,
        MenuNameExceedLength,
        MenuNameAlreadyExist,
        MenuNotFound,
        MenuDeleted,
        MenuCategoriesDoesNotActivated,
        EmptyCategoryName,
        CategoryNameExceedLength,
        CategoryNameAlreadyExist,
        CategoryNotFound,
        CategoryDeleted,
        CategoryItemsDoesNotActivated,
        EmptyItemName,
        EmptyItemImage,
        ItemNameExceedLength,
        ItemNameAlreadyExist,
        ItemNotFound,
        ItemDeleted,
        EmptyItemDescription,
        EmptyItemPrice,
        InvalidItemPrice,
        RestaurantAdminNotFound,
        EmptyRestaurantLogo,
        ImageExceedSize,
        InvalidImageType,
        EmptyCategoryImage,
        EmptySizeName,
        SizeNameExceedLength,
        SizeNameMinimumLength,
        SizeNameAlreadyExist,
        SizeDeleted,
        SizeNotFound,
        SizeHasItems,
        EmptySideItemName,
        SideItemNameExceedLength,
        SideItemNameMinimumLength,
        SideItemNameAlreadyExist,
        SideItemDeleted,
        SideItemNotFound,
        SideHasItems,
        InvalidSideItemValue,
        SizeIsNotTranslated,
        SideItemIsNotTranslated,
        MenuIsNotTranslated,
        CategoryIsNotTranslated,
        RestaurantIsNotTranslated,
        ItemIsNotTranslated,
        RestaurantIsNotReady,
        RestaurantTypeIsNotTranslated,
        EmptyRestaurantWaiterUserName,
        RestaurantWaiterNameExceedLength,
        BackgroundNotFound,
        BackgroundDeleted,
        BackgroundItemsDoesNotActivated,
        EmptyBackgroundImage,
        TemplateNotFound,
        CategoryTemplatesInvalid,
        MailExist,
        PhoneExist,
        ProductNotFound
    }
}
