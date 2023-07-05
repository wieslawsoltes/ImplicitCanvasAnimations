﻿using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace ImplicitCanvasAnimations.Android;

[Activity(Label = "ImplicitCanvasAnimations", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
}
