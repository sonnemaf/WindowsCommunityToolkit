﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.Styles
{
    public static class ThemeInjector
    {
        private static ResourceDictionary darkTheme;

        private static ResourceDictionary lightTheme;

        public static void InjectThemeResources(ResourceDictionary dict)
        {
            var themes = dict.MergedDictionaries[0];
            darkTheme = themes.ThemeDictionaries["Dark"] as ResourceDictionary;
            lightTheme = themes.ThemeDictionaries["Light"] as ResourceDictionary;

            if (VisualHelpers.SupportsFluentAcrylic)
            {
                var backingGrey = VisualHelpers.ColorFromHex("#FF111111");

                AddAcrylic(new ThemeAcrylic
                {
                    Name = "Background-01",
                    DarkAcrylic = new AcrylicBrush
                    {
                        TintColor = backingGrey,
                        TintOpacity = 0.8,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    },
                    LightAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.White,
                        TintOpacity = 0.7,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    }
                });

                AddAcrylic(new ThemeAcrylic
                {
                    Name = "Background-02",
                    DarkAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.Black,
                        TintOpacity = 0.5,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    },
                    LightAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.White,
                        TintOpacity = 0.5,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    }
                });
            }
        }

        private static void AddAcrylic(ThemeAcrylic resource)
        {
            var light = resource?.LightAcrylic;
            var dark = resource?.DarkAcrylic;

            if (light != null)
            {
                if (light.FallbackColor == null && lightTheme[resource.Name] is SolidColorBrush brush)
                {
                    light.FallbackColor = brush.Color;
                }

                lightTheme[resource.Name] = light;
            }

            if (dark != null)
            {
                if (dark.FallbackColor == null && darkTheme[resource.Name] is SolidColorBrush brush)
                {
                    dark.FallbackColor = brush.Color;
                }

                darkTheme[resource.Name] = dark;
            }
        }

        private class ThemeAcrylic
        {
            public string Name { get; set; }

            public AcrylicBrush LightAcrylic { get; set; }

            public AcrylicBrush DarkAcrylic { get; set; }
        }
    }
}