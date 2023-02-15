create table main.UserContents
(
    Id      TEXT    not null
        constraint PK_UserContents
            primary key,
    UserId  INTEGER not null,
    Content TEXT,
    Type    INTEGER not null,
    WishDay TEXT
);

