create table Choices
(
    ChoiceId               INTEGER not null
        constraint PK_Choices
            primary key autoincrement,
    ChoiceText             TEXT    not null,
    QuizQuestion INTEGER
        constraint FK_Choices_Questions_QuizQuestion
            references Questions
);

create index IX_Choices_QuizQuestion
    on Choices (QuizQuestionQuestionId);

INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (1, 'con', 1);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (2, 'git', 1);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (3, 'BDFL', 1);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (4, 'notch', 1);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (5, '1', 2);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (6, '2', 2);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (7, '3', 2);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (8, '4', 2);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (9, 'Экспоненциальный поиск', 3);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (10, 'Линейный поиск', 3);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (11, 'Бинарный поиск', 3);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (12, 'Небинарный поиск', 3);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (13, 'DI', 4);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (14, 'IoC', 4);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (15, 'IoT', 4);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (16, 'IdIoT', 4);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (17, 'Разработчик', 5);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (18, 'Тестировщик', 5);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (19, 'Владелец продукта', 5);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (20, 'Никто, так как в IT все дружелюбные :)', 5);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (21, '169', 6);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (22, '610', 6);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (23, '458', 6);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (24, '925', 6);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (25, 'Super Mario Bros.', 7);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (26, 'DOOM', 7);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (27, 'Смута', 7);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (28, 'Бесконечное лето', 7);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (29, '96 осьминог и 6 сороконожек! - догадался Штирлиц', 8);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (30, '100 осьминог и 5 сороконожек! - догадался Штирлиц +', 8);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (31, '10 осьминогов и 25 сороконожек! - догадался Штирлиц', 8);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (32, 'Штирлиц не догадался...', 8);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (33, 'QA', 9);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (34, 'Тестировщик', 9);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (35, 'Пользователь', 9);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (36, 'Никто, ведь мы живём счастливо и богато :)', 9);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (37, 'UTF-16BE', 10);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (38, 'UTF-8', 10);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (39, 'Windows1251', 10);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (40, 'UTF-6', 10);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (41, '1', 11);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (42, '3', 11);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (43, '5', 11);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (44, '7', 11);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (45, '2', 12);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (46, '6', 12);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (47, '10', 12);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (48, '14', 12);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (49, '1000000000 Кбайт', 13);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (50, '1000000000 МБайт', 13);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (51, '10000 Гбайт', 13);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (52, '100000000000 байт', 13);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (53, 'Господь Бог', 14);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (54, 'Линус Торвальдс', 14);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (55, 'Бьерн Страуструп', 14);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (56, 'Блез Паскаль', 14);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (57, 'Йоба', 15);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (58, 'Brainfuck', 15);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (59, 'Chicken', 15);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (60, 'Cock', 15);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (61, '-91', 16);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (62, '500', 16);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (63, '312', 16);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (64, '42', 16);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (65, '100', 17);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (66, '80', 17);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (67, '92.5', 17);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (68, '105', 17);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (69, 'Спонж', 18);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (70, 'Тинт', 18);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (71, 'Бронзер', 18);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (72, 'Праймер', 18);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (73, 'Набор неподдерживаемых символов', 19);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (74, 'Белиберда', 19);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (75, 'Регулярное выражение', 19);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (76, 'Пароль', 19);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (77, 'ВЕРНЫЙ ОТВЕT', 20);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (78, 'ВЕРНЫЙ 0ТВЕТ', 20);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (79, 'ВЕРНЫЙ  ОТВЕТ', 20);
INSERT INTO Choices (ChoiceId, ChoiceText, QuizQuestionQuestionId) VALUES (80, 'ВЕРНЬIЙ ОТВЕТ', 20);
