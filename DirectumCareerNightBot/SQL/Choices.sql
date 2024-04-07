create table Choices
(
    Id               INTEGER not null
        constraint PK_Choices
            primary key autoincrement,
    Text             TEXT    not null,
    QuizQuestion INTEGER
        constraint FK_Choices_Questions_QuizQuestion
            references Questions
);

create index IX_Choices_QuizQuestion
    on Choices (QuestionId);

INSERT INTO Choices (Id, Text, QuestionId)VALUES (1, 'con', 1);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (2, 'git', 1);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (3, 'BDFL', 1);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (4, 'notch', 1);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (5, '1', 2);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (6, '2', 2);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (7, '3', 2);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (8, '4', 2);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (9, 'Экспоненциальный поиск', 3);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (10, 'Линейный поиск', 3);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (11, 'Бинарный поиск', 3);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (12, 'Небинарный поиск', 3);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (13, 'DI', 4);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (14, 'IoC', 4);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (15, 'IoT', 4);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (16, 'IdIoT', 4);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (17, 'Разработчик', 5);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (18, 'Тестировщик', 5);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (19, 'Владелец продукта', 5);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (20, 'Никто, так как в IT все дружелюбные :)', 5);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (21, '169', 6);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (22, '610', 6);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (23, '458', 6);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (24, '925', 6);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (25, 'Super Mario Bros.', 7);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (26, 'DOOM', 7);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (27, 'Смута', 7);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (28, 'Бесконечное лето', 7);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (29, '96 осьминог и 6 сороконожек! - догадался Штирлиц', 8);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (30, '100 осьминог и 5 сороконожек! - догадался Штирлиц +', 8);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (31, '10 осьминогов и 25 сороконожек! - догадался Штирлиц', 8);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (32, 'Штирлиц не догадался...', 8);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (33, 'QA', 9);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (34, 'Тестировщик', 9);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (35, 'Пользователь', 9);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (36, 'Никто, ведь мы живём счастливо и богато :)', 9);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (37, 'UTF-16BE', 10);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (38, 'UTF-8', 10);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (39, 'Windows1251', 10);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (40, 'UTF-6', 10);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (41, '1', 11);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (42, '3', 11);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (43, '5', 11);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (44, '7', 11);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (45, '2', 12);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (46, '6', 12);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (47, '10', 12);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (48, '14', 12);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (49, '1000000000 Кбайт', 13);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (50, '1000000000 МБайт', 13);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (51, '10000 Гбайт', 13);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (52, '100000000000 байт', 13);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (53, 'Господь Бог', 14);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (54, 'Линус Торвальдс', 14);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (55, 'Бьерн Страуструп', 14);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (56, 'Блез Паскаль', 14);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (57, 'Йоба', 15);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (58, 'Brainfuck', 15);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (59, 'Chicken', 15);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (60, 'Cock', 15);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (61, '-91', 16);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (62, '500', 16);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (63, '312', 16);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (64, '42', 16);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (65, '100', 17);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (66, '80', 17);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (67, '92.5', 17);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (68, '105', 17);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (69, 'Спонж', 18);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (70, 'Тинт', 18);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (71, 'Бронзер', 18);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (72, 'Праймер', 18);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (73, 'Набор неподдерживаемых символов', 19);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (74, 'Белиберда', 19);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (75, 'Регулярное выражение', 19);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (76, 'Пароль', 19);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (77, 'ВЕРНЫЙ ОТВЕT', 20);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (78, 'ВЕРНЫЙ 0ТВЕТ', 20);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (79, 'ВЕРНЫЙ  ОТВЕТ', 20);
INSERT INTO Choices (Id, Text, QuestionId)VALUES (80, 'ВЕРНЬIЙ ОТВЕТ', 20);
