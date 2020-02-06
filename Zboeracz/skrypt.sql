create table dbo.SearchLink
(
	Sel_Id int identity(1,1) primary key,
	Sel_Url varchar(768) not null unique,
	Sel_Name varchar(512),
	Sel_Describe varchar(2048)
);

create table dbo.Advert
(
	Adw_Id int identity(1,1) primary key,
	Adw_ForeignId varchar(16) unique not null,
	Adw_Name varchar(1024) not null,
	Adw_Category varchar(128) not null,
	Adw_Location varchar(128) not null,
	Adw_Date datetime default(GETDATE()),
	Adw_Price decimal(8,2) not null,
	Adw_Url varchar(512) not null,
	Adw_IsPromoted bit default(0),
	Adw_CreateDate datetime default(GETDATE()),
	Adw_SeLId int foreign key references dbo.SearchLink (Sel_Id)
);

create table dbo.Attributes
(
	Atr_Id int identity(1,1) primary key,
	Atr_AdwId int foreign key references dbo.Advert (Adw_Id),
	Atr_Name varchar(255) not null,
	Atr_Value varchar(255) not null
);

create table dbo.Describe
(
	Des_Id int identity(1,1) primary key,
	Des_AdwId int foreign key references dbo.Advert (Adw_Id),
	Des_Describe varchar(8000),
	Des_Exposer varchar(32),
	Des_Tel varchar(9)
);

create table dbo.Pictures
(
	Pic_Id int identity(1,1) primary key,
	Pic_AdwId int foreign key references dbo.Advert (Adw_Id),
	Pic_Content binary
);

--drop table dbo.Advert
select iif(exists(select 1 from dbo.Advert where Adw_ForeignId = @Id),1,0) IsAdwExists

insert into dbo.Advert(Adw_ForeignId, Adw_Name, Adw_Category, Adw_Location, Adw_Date, Adw_Price, Adw_Url, Adw_IsPromoted)
values (@Id, @Name, @Category, @Location, @Date, @Price, @Url, @IsPromoted)
select 'Poprawnie dodano og³oszenie ' + Adw_ForeignId + ': ' + Adw_Name as Info from dbo.Advert where Adw_Id = SCOPE_IDENTITY()

select * from dbo.Advert order by Adw_Date desc
