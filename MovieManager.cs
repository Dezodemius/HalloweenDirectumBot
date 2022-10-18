using System;
using System.Collections.Generic;
using System.IO;

namespace HalloweenDirectumBot;

public struct Movie
{
  public string Description { get; }

  public Stream MoviePosterPathStream { get; }

  public Movie(string description, Stream moviePosterPathStream)
  {
    Description = description;
    MoviePosterPathStream = moviePosterPathStream;
  }
}

public class MovieManager
{
  private LinkedList<string> movieFolders;

  public LinkedListNode<string> currentMovieFolder;

  private static string GetMovieDescription(string movieFolder)
  {
    var movieDescriptionPath = Path.Combine(movieFolder, "description");
    return File.ReadAllText(movieDescriptionPath);
  }

  private static Stream? GetMoviePoster(string movieFolder)
  {
    var moviePosterPath = Path.Combine(movieFolder, "poster.png");
    return File.OpenRead(moviePosterPath);
  }

  public Movie GetNextMovie()
  {
    if (currentMovieFolder == null)
      currentMovieFolder = movieFolders.First;

    var movie = GetMovie();
    currentMovieFolder = currentMovieFolder.Next;
    return movie;
  }

  public Movie GetPreviousMovie()
  {
    if (currentMovieFolder.Value == null)
      currentMovieFolder = movieFolders.Last;

    var movie = GetMovie();
    currentMovieFolder = currentMovieFolder.Previous;
    return movie;
  }

  private Movie GetMovie()
  {
    var movieDescription = GetMovieDescription(currentMovieFolder.Value);
    var moviePosterStream = GetMoviePoster(currentMovieFolder.Value) ?? Stream.Null;

    return new Movie(movieDescription, moviePosterStream);
  }

  public MovieManager()
  {
    const string MoviesDirectory = "movies";
    movieFolders = new LinkedList<string>(Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MoviesDirectory)));
  }
}