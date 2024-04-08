using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.Scenarios;
using DirectumCareerNightBot.Quiz;
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
        await using var botDbContext = new BotDbContext();
        var botUser = botDbContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);
        var userQuestions = botDbContext.UserQuestions.Where(q => q.UserId == botUser.Id);
        if (userQuestions.Any())
            botDbContext.UserQuestions.RemoveRange(userQuestions);
        
        var question = GetQuizQuestion(botDbContext, new Random().Next(1, 5));

        botDbContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await botDbContext.SaveChangesAsync();
        
        var questionChoices = GetQuestionChoices(botDbContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId, 
        $"{BotMessages.QuizStartMessage}\n{question.Text}", 
            replyMarkup: replyMarkup);
    }

    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var quizContext = new BotDbContext();
        var botUser = quizContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);

        var userChoiceId = quizContext.Choices
            .Single(c => c.Id == int.Parse(update.CallbackQuery.Data));
        var lastUserQuestion = quizContext.UserQuestions
            .Where(c => c.User.Id == botUser.Id)
            .OrderByDescending(i => i.Id)
            .FirstOrDefault();
        var quizQuestion = quizContext.Questions.SingleOrDefault(q => lastUserQuestion.QuestionId == q.Id);
        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, quizQuestion, 
            quizQuestion.CorrectChoiceId == userChoiceId.Id));

        var question = GetQuizQuestion(quizContext, new Random().Next(5, 9));


        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await quizContext.SaveChangesAsync();

        var questionChoices = GetQuestionChoices(quizContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId,
            text: question.Text,
            replyMarkup: replyMarkup);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var quizContext = new BotDbContext();
        var botUser = quizContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);

        var userChoiceId = quizContext.Choices
            .Single(c => c.Id == int.Parse(update.CallbackQuery.Data));
        var lastUserQuestion = quizContext.UserQuestions
            .Where(c => c.User.Id == botUser.Id)
            .OrderByDescending(i => i.Id)
            .FirstOrDefault();
        var quizQuestion = quizContext.Questions.SingleOrDefault(q => lastUserQuestion.QuestionId == q.Id);
        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, quizQuestion, 
            quizQuestion.CorrectChoiceId == userChoiceId.Id));

        var question = GetQuizQuestion(quizContext, new Random().Next(9, 13));


        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await quizContext.SaveChangesAsync();

        var questionChoices = GetQuestionChoices(quizContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId,
            text: question.Text,
            replyMarkup: replyMarkup);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var quizContext = new BotDbContext();
        var botUser = quizContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);

        var userChoiceId = quizContext.Choices
            .Single(c => c.Id == int.Parse(update.CallbackQuery.Data));
        var lastUserQuestion = quizContext.UserQuestions
            .Where(c => c.User.Id == botUser.Id)
            .OrderByDescending(i => i.Id)
            .FirstOrDefault();
        var quizQuestion = quizContext.Questions.SingleOrDefault(q => lastUserQuestion.QuestionId == q.Id);
        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, quizQuestion, 
            quizQuestion.CorrectChoiceId == userChoiceId.Id));

        var question = GetQuizQuestion(quizContext, new Random().Next(13, 17));


        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await quizContext.SaveChangesAsync();

        var questionChoices = GetQuestionChoices(quizContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId,
            text: question.Text,
            replyMarkup: replyMarkup);
    }
    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var quizContext = new BotDbContext();
        var botUser = quizContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);

        var userChoiceId = quizContext.Choices
            .Single(c => c.Id == int.Parse(update.CallbackQuery.Data));
        var lastUserQuestion = quizContext.UserQuestions
            .Where(c => c.User.Id == botUser.Id)
            .OrderByDescending(i => i.Id)
            .FirstOrDefault();
        var quizQuestion = quizContext.Questions.SingleOrDefault(q => lastUserQuestion.QuestionId == q.Id);
        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, quizQuestion, 
            quizQuestion.CorrectChoiceId == userChoiceId.Id));

        var question = GetQuizQuestion(quizContext, new Random().Next(17, 21));


        quizContext.UserQuestions.Add(new QuizUserQuestion(botUser, question, null));
        await quizContext.SaveChangesAsync();

        var questionChoices = GetQuestionChoices(quizContext, question);
        var replyMarkup = GetChoicesReplyMarkup(questionChoices);
        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId,
            text: question.Text,
            replyMarkup: replyMarkup);
    }
    private async Task StepAction6(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var botDbContext = new BotDbContext();

        var botUser = botDbContext.BotUsers.First(u => u.Id == BotHelper.GetUserInfo(update).Id);

        var userChoiceId = botDbContext.Choices
            .Single(c => c.Id == int.Parse(update.CallbackQuery.Data));
        var lastUserQuestion = botDbContext.UserQuestions
            .Where(c => c.User.Id == botUser.Id)
            .OrderByDescending(i => i.Id)
            .FirstOrDefault();
        var quizQuestion = botDbContext.Questions.SingleOrDefault(q => lastUserQuestion.QuestionId == q.Id);
        botDbContext.UserQuestions.Add(new QuizUserQuestion(botUser, quizQuestion, 
            quizQuestion.CorrectChoiceId == userChoiceId.Id));
        await botDbContext.SaveChangesAsync();

        var userQuestions = botDbContext.UserQuestions.Where(q => q.UserId == botUser.Id);
        var quizResult = userQuestions.Count(q => q.IsCorrectAnswered.Value);
        
        botDbContext.UserQuestions.RemoveRange(userQuestions);
        
        var quizIsDone = quizResult > 0;
        
        var resultMessage = string.Empty;
        var botGiftMessage = "Держи код для бота @nochit2024_bot: **p1bzk**";
        if (quizResult == 0)
            resultMessage = "Кажется ты подустал, зарядись и пройди тест ещё раз.";
        else if (quizResult == 1)
            resultMessage = $"Not good, not terrible. {botGiftMessage}";
        else if (quizResult > 1 && quizResult < 5)
            resultMessage = $"Поздравляем! {botGiftMessage}";
        else if (quizResult == 5)
            resultMessage = $"Балдею от твоих ответов! Ты такой умный Дядька (Тётька)! {botGiftMessage}";
        
        var userResult = botDbContext.UserResults.SingleOrDefault(r => r.UserId == botUser.Id);
        await botDbContext.SaveChangesAsync();

        if (userResult == null)
        {
            botDbContext.UserResults.Add(new QuizUserResult(botUser.Id, quizIsDone));
        }
        else
        {
            userResult.IsQuizDone = quizIsDone;
            if (quizIsDone)
                resultMessage = string.Empty;
        }
        await botDbContext.SaveChangesAsync();

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.EditMessageTextAsync(
            chatId, 
            update.CallbackQuery.Message.MessageId,
            $"Твой результат {quizResult} из 5.\n{resultMessage}", 
            replyMarkup: markup);
    }

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
        this.steps = new List<BotCommandScenarioStep>
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),
            new (StepAction5),
            new (StepAction6),

        }.GetEnumerator();
    }
}