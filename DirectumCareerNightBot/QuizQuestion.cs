using System;
using System.Collections.Generic;

namespace DirectumCareerNightBot;

public class QuizQuestion
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; }
    public List<QuizChoice> Choices { get; set; }
    public Guid CorrectChoiceId { get; set; }
}

public class QuizChoice
{
    public Guid ChoiceId { get; set; }
    public string ChoiceText { get; set; }
}