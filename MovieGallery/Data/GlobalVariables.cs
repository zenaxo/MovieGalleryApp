using System.Collections.Generic;

public static class GlobalVariables
{
    public static List<string> Genres { get; set; } = new List<string>();

    static GlobalVariables() {
        Genres.Add("Action");
        Genres.Add("Drama");
        Genres.Add("Horror");
        Genres.Add("Comedy");
    }
}