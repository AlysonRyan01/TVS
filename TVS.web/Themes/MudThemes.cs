using MudBlazor;

namespace TVS.web.Themes;

public static class MudThemes
{
    public static MudTheme MyThemes = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#FF3130",
            Secondary = "#FFFFFF",
            Tertiary = "#000000",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Open Sans", "sans-serif"]
            }
        }
    };
}