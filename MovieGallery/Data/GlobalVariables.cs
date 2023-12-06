﻿using System.Collections.Generic;

public static class GlobalVariables
{
    public static List<string> Genres { get; } = new List<string>();
    public static List<string> GenreList { get; } = Genres;
    public static List<string> RatingValues { get; } = new List<string>();
    static GlobalVariables() {

        Genres.Add("Action");
        Genres.Add("Drama");
        Genres.Add("Horror");
        Genres.Add("Comedy");
        Genres.Add("Animated");

        RatingValues.Add("Random");
        RatingValues.Add("1");
        RatingValues.Add("2");
        RatingValues.Add("3");
        RatingValues.Add("4");
        RatingValues.Add("5");

        GenreList.Add("All");
        GenreList.Reverse();

    }
}