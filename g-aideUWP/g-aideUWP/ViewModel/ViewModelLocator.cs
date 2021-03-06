﻿using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace g_aideUWP.ViewModel
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ConnectionPageModel>();
            SimpleIoc.Default.Register<ListServiceModel>();
            SimpleIoc.Default.Register<EditCategoryModel>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            NavigationService navigationPages = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationPages);

            navigationPages.Configure("ConnectionPage", typeof(ConnectionPage));
            navigationPages.Configure("ListService", typeof(ListService));
            navigationPages.Configure("EditCategory", typeof(EditCategory));
        }

        public ConnectionPageModel connectionPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConnectionPageModel>();
            }
        }

        public ListServiceModel listService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListServiceModel>();
            }
        }

        public EditCategoryModel editCategory
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditCategoryModel>();
            }
        }
    }
}
