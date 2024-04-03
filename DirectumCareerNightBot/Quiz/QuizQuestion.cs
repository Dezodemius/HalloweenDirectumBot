using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DirectumCareerNightBot;

[PrimaryKey(nameof(QuestionId))]
public class QuizQuestion
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public List<QuizChoice> Choices { get; set; }
    public int CorrectChoiceId { get; set; }
}

[PrimaryKey(nameof(ChoiceId))]
public class QuizChoice
{
    public int ChoiceId { get; set; }
    public string ChoiceText { get; set; }
}