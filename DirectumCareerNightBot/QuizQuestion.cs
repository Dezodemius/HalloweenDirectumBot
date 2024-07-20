using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BotCommon.Repository;

namespace DirectumCareerNightBot;

public class QuizQuestion
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    public List<QuizPossibleAnswer> Choices { get; set; }
    public int CorrectChoiceId { get; set; }
    public QuizPossibleAnswer CorrectChoice { get; set; }
}

public class QuizPossibleAnswer
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    
    public int QuestionId { get; set; }
    public QuizQuestion Question { get; set; }
}

public record QuizUserQuestion
{
    [Key]
    public int Id { get; set; }
    public long UserId { get; set; }
    public BotUser User { get; init; }

    public int QuestionId { get; set; }
    public QuizQuestion Question { get; init; }
    public bool? IsCorrectAnswered { get; init; }

    public QuizUserQuestion()
    {
        
    }
    
    public QuizUserQuestion(BotUser User, QuizQuestion Question, bool? IsCorrectAnswered)
    {
        this.User = User;
        this.Question = Question;
        this.IsCorrectAnswered = IsCorrectAnswered;
    }
}

public record QuizUserResult
{
    [Key]
    public int Id { get; set; }
    public long UserId { get; set; }
    public BotUser BotUser { get; set; }
    public bool IsQuizDone { get; set; }
    public QuizUserResult() { }

    public QuizUserResult(long userId, bool isQuizDone)
    {
        this.UserId = userId;
        this.IsQuizDone = isQuizDone;
    }
}

public record UserData
{
    [Key]
    public int Id { get; set; }
    public long UserId { get; set; }
    public BotUser BotUser { get; set; }
    [AllowNull]
    public string Fullname { get; set; }
    [AllowNull]
    public string Contact { get; set; }
    public string TelegramName { get; set; }
    [AllowNull]
    public string SomeField { get; set; }
    [AllowNull]
    public string Experience { get; set; }
}