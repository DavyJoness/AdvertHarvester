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
)
--drop table dbo.Advert
select iif(exists(select 1 from dbo.Advert where Adw_ForeignId = @Id),1,0) IsAdwExists

insert into dbo.Advert(Adw_ForeignId, Adw_Name, Adw_Category, Adw_Location, Adw_Date, Adw_Price, Adw_Url, Adw_IsPromoted)
values (@Id, @Name, @Category, @Location, @Date, @Price, @Url, @IsPromoted)
select 'Poprawnie dodano og³oszenie ' + Adw_ForeignId + ': ' + Adw_Name as Info from dbo.Advert where Adw_Id = SCOPE_IDENTITY()

select * from dbo.Advert order by Adw_Date desc
