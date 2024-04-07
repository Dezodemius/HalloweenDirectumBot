using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BotCommon.Repository;

namespace DirectumCareerNightBot.Quiz;

public class QuizQuestion
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    public List<QuizPossibleAnswer> Choices { get; set; }
    // [ForeignKey(nameof(CorrectChoiceId))]
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