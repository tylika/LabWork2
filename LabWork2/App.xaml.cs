﻿// App.xaml.cs
using Microsoft.Maui.Controls;
using System;
using LabWork2.Views;
namespace LabWork2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);
            if (window != null) { window.Title = "LabWork2"; }
#if WINDOWS
        window.Created += (s, e) =>
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window.Handler.PlatformView);
            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

            appWindow.Closing += async (s, e) =>
            {
                e.Cancel = true;
                bool result = await App.Current.MainPage.DisplayAlert(
                    "Підтвердження",
                    "Ви впевнені, що хочете завершити?",
                    "Так",
                    "Ні");

                if (result)
                {
                    App.Current.Quit();
                }
            };
        };
#endif
            return window;
        }


    }

}



