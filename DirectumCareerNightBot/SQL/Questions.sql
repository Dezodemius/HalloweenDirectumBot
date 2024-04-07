create table Questions
(
    Id      INTEGER not null
        constraint PK_Questions
            primary key autoincrement,
    Text    TEXT    not null,
    CorrectChoiceId INTEGER not null
);

INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (1, 'Кто основал Microsoft?', 1);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (2, 'Лблпк тегйд нь гьвсбмй ема щйхспгбойа юупдп гпрсптб щйхспн Чёибса?', 5);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (3, 'Какой алгоритм поиска запрещён на территории России?', 12);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (4, 'Какая аббревиатура используется для обозначения Интернета вещей?', 15);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (5, 'На production-е обнаружен критичный баг. Кто первый получит по шапке?', 20);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (6, 'Какое число нужно вписать в пропуск? 89, 144, 233, 377, ___', 22);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (7, 'Какую игру можно запустить почти на любом устройстве?', 26);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (8, 'В дверь постучали 1024 раз... Сколько осьминогов и сороконожек постучали?', 30);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (9, 'Как назвать человека, который портит жизнь разработчикам?', 36);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (10, '킒⃐뫐냐뫐뻐뤠킺킾킴킸톀킾킲킺킵⃐병謠킾톂킾킱톀킰킷킸킻킸⃑跑苐뻑舠톂킵킺톁톂�', 37);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (11, 'Crjkmrj gfkmwtd yf ktdjq hert&', 43);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (12, 'Crjkmrj gfkmwtd yf lde[ herf[&', 47);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (13, 'Как можно записать 1 Тбайт памяти:', 49);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (14, 'Кто создал ОС Linux?', 54);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (15, 'Какого языка программирования не существует?', 60);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (16, 'Вычислите выражение ''1 2 + 3 * 100 -'':', 61);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (17, 'Все мужики в зале знают, что 40 + 40 будет равно...', 65);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (18, 'Что Senior .Net разработчица наносит на губы перед тем, как залить фичу?', 70);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (19, 'Что это? ''/@([A-Za-z0-9_]{1,15})/ ''', 75);
INSERT INTO Questions (Id, Text, CorrectChoiceId)VALUES (20, 'Один из ответов верный', 77);
