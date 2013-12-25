# Cross-Platform Charting App using ShinobiControls

This project demonstrates an approach to building cross-platform apps using
Xamarin and ShinobiControls.

## Overview

The project uses the MVP pattern to allow extensive code sharing between the
Android and iOS solutions. The shared code is in a portable class library (PCL)
and there are 2 additional projects in the solution for the Android and iOS
apps.

## Getting Started

### Shinobi License Keys

This project has been built using the trial versions of ShinobiCharts for iOS
ShinobiCharts for Android - available from the Xamarin component store. Therefore,
when you open the solution file in Xamarin Studio, the components should be
magically installed for you.

Since we're using the trial versions you'll be issued with license keys for each
component when they are first downloaded from the component store. You can access
these licence keys by logging in to the component store via the web interface,
selecting your account and viewing 'My Components'. Form here you can select your
trial components from your list of components in order to find your trial keys.

There are 2 files which require license keys - `StockChartViewController.cs` for
the iOS project:

    _chart.LicenseKey = @"<PUT YOUR LICENSE KEY HERE>";

And `StockChartActivity.cs` in the Android project:

    _chart.SetLicenseKey ("<PUT YOUR LICENSE KEY HERE>");


### Minimum Versions

The iOS app has been designed to work with iOS7, and the Android with api-level
> 12 (it has been tested with Android 4.0.4, Ice Cream Sandwich (API Level 15)).


