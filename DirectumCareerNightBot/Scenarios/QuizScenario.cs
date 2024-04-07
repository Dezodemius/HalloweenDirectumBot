using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.Repository;
using BotCommon.Scenarios;
using DirectumCareerNightBot.Quiz;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCareerNightBot.Scenarios;

public class QuizScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("C2181931-E2A2-409D-9E9B-220177C40556");
    public override string ScenarioCommand { get; }
    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.QuizStartMessage, ParseMode.Markdown);

        await using var quizContext = new BotDbContext();
        await using var userContext = new UserDbContext();
        
        var question = GetQuizQuestion(quizContext, new Random().Next(1, 5));

        var botUser = userContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);
        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await quizContext.SaveChangesAsync();
        
        var questionChoices = GetQuestionChoices(quizContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.SendTextMessageAsync(
            chatId,
            text: question.Text,
            replyMarkup: replyMarkup);
    }

    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id, "Ответ принят!");

        await using var quizContext = new BotDbContext();
        var botUser = quizContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);

        var userChoiceId = quizContext.Choices
            .Single(c => c.Id == int.Parse(update.CallbackQuery.Message.Text));
        var lastUserQuestion = quizContext.UserQuestions
            .Include(quizUserQuestion => quizUserQuestion.Question)
            .ThenInclude(quizQuestion => quizQuestion.CorrectChoice)
            .Last(q => q.User.Id == botUser.Id);
        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, lastUserQuestion.Question, 
            lastUserQuestion.Question.CorrectChoice.Id == userChoiceId.Id));

        var question = GetQuizQuestion(quizContext, new Random().Next(5, 9));


        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await quizContext.SaveChangesAsync();

        var questionChoices = GetQuestionChoices(quizContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.SendTextMessageAsync(
            chatId,
            text: question.Text,
            replyMarkup: replyMarkup);
    }
    // private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    // {
    //     await using var quizContext = new QuizContext();
    //     var randomQuestionId = new Random().Next(9, 12);
    //     var question = quizContext.Questions.Single(q => q.QuestionId == randomQuestionId);
    //     var questionChoices = quizContext.Choices.Where(c => c.QuizQuestion.QuestionId == question.QuestionId).ToList();
    //     var choicesKeyboard = new List<InlineKeyboardButton[]>
    //     {
    //         new []
    //         {
    //             InlineKeyboardButton.WithCallbackData(questionChoices[0].ChoiceText, "/1" ),
    //             InlineKeyboardButton.WithCallbackData(questionChoices[1].ChoiceText, "/2" ),
    //         },new []
    //         {
    //             InlineKeyboardButton.WithCallbackData(questionChoices[2].ChoiceText, "/3" ),
    //             InlineKeyboardButton.WithCallbackData(questionChoices[3].ChoiceText, "/4" ),
    //         },
    //     };
    //     var replyMarkup = new InlineKeyboardMarkup(choicesKeyboard);
    //     await bot.EditMessageTextAsync(
    //         chatId,
    //         update.CallbackQuery.Message.MessageId, 
    //         text: question.QuestionText,
    //         replyMarkup: replyMarkup);
    // }
    // private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    // {
    //     await using var quizContext = new QuizContext();
    //     var randomQuestionId = new Random().Next(13, 16);
    //     var question = quizContext.Questions.Single(q => q.QuestionId == randomQuestionId);
    //     var questionChoices = quizContext.Choices.Where(c => c.QuizQuestion.QuestionId == question.QuestionId).ToList();
    //     var choicesKeyboard = new List<InlineKeyboardButton[]>
    //     {
    //         new []
    //         {
    //             InlineKeyboardButton.WithCallbackData(questionChoices[0].ChoiceText, "/1" ),
    //             InlineKeyboardButton.WithCallbackData(questionChoices[1].ChoiceText, "/2" ),
    //         },new []
    //         {
    //             InlineKeyboardButton.WithCallbackData(questionChoices[2].ChoiceText, "/3" ),
    //             InlineKeyboardButton.WithCallbackData(questionChoices[3].ChoiceText, "/4" ),
    //         },
    //     };
    //     var replyMarkup = new InlineKeyboardMarkup(choicesKeyboard);
    //     await bot.EditMessageTextAsync(
    //         chatId,
    //         update.CallbackQuery.Message.MessageId, 
    //         text: question.QuestionText,
    //         replyMarkup: replyMarkup);
    // }
    // private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    // {
    //     await using var quizContext = new QuizContext();
    //     var randomQuestionId = new Random().Next(17, 20);
    //     var question = quizContext.Questions.Single(q => q.QuestionId == randomQuestionId);
    //     var questionChoices = quizContext.Choices.Where(c => c.QuizQuestion.QuestionId == question.QuestionId).ToList();
    //     var choicesKeyboard = new List<InlineKeyboardButton[]>
    //     {
    //         new []
    //         {
    //             InlineKeyboardButton.WithCallbackData(questionChoices[0].ChoiceText, "/1" ),
    //             InlineKeyboardButton.WithCallbackData(questionChoices[1].ChoiceText, "/2" ),
    //         },new []
    //         {
    //             InlineKeyboardButton.WithCallbackData(questionChoices[2].ChoiceText, "/3" ),
    //             InlineKeyboardButton.WithCallbackData(questionChoices[3].ChoiceText, "/4" ),
    //         },
    //     };
    //     var replyMarkup = new InlineKeyboardMarkup(choicesKeyboard);
    //     await bot.EditMessageTextAsync(
    //         chatId,
    //         update.CallbackQuery.Message.MessageId, 
    //         text: question.QuestionText,
    //         replyMarkup: replyMarkup);
    // }
    // private async Task StepAction6(ITelegramBotClient bot, Update update, long chatId)
    // {
    //     await bot.SendTextMessageAsync(chatId, "Ура!", ParseMode.Markdown);
    // }

    private static QuizQuestion GetQuizQuestion(BotDbContext botDbContext, int questionId)
    {
        return botDbContext.Questions.First(q => q.Id == questionId);
    }

    private static List<QuizPossibleAnswer> GetQuestionChoices(BotDbContext botDbContext, QuizQuestion question)
    {
        return botDbContext.Choices.Where(c => c.Question.Id == question.Id).ToList();
    }

    private static InlineKeyboardMarkup GetChoicesReplyMarkup(List<QuizPossibleAnswer> questionChoices)
    {
        var choicesKeyboard = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(questionChoices[0].Text, questionChoices[0].Id.ToString()),
                InlineKeyboardButton.WithCallbackData(questionChoices[1].Text, questionChoices[1].Id.ToString()),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(questionChoices[2].Text, questionChoices[2].Id.ToString()),
                InlineKeyboardButton.WithCallbackData(questionChoices[3].Text, questionChoices[3].Id.ToString()),
            },
        };
        var replyMarkup = new InlineKeyboardMarkup(choicesKeyboard);
        return replyMarkup;
    }

    public QuizScenario()
    {
        this.steps = new List<BotCommandScenarioStep>()
        {
            new (StepAction1),
            new (StepAction2),
            // new (StepAction3),
            // new (StepAction4),
            // new (StepAction5),
            // new (StepAction6),

        }.GetEnumerator();
    }
}