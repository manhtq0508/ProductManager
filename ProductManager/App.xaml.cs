﻿using ProductManager.Services;

namespace ProductManager;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
