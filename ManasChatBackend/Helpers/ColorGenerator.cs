namespace ManasChatBackend.Helpers;

public static class ColorGenerator
{
    private static List<string> _colors = new List<string>()
    {
        "red", "blue", "green", "yellow", "black", "orange", "gray", "brown", "violet", "pink",
        "lime", "pale-green", "pale-yellow", "pale-blue", "pale-red", "purple", "indigo", "amber", "teal"
    };
    
    public static string GenerateColor()
    {
        var random = new Random();
        return _colors.ElementAt(random.Next(1, _colors.Count()));
    }
}