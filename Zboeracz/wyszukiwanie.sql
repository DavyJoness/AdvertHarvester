alter procedure dbo.SendMailHotDeals(@searchId int)
as

declare @Ids table (Id int)
declare @Message varchar(4096)
declare @Title varchar(256) = 'Nowe oferty mieszkañ: '

declare @Nazwa varchar(1024)
declare @Url varchar(512)
declare @Lokalizacja varchar(128)
declare @Czynsz decimal(8,2)
declare @CzynszRzeczywisty decimal(8,2)

set nocount on;

insert into @Ids
select Adw_Id as Id
from dbo.Advert
inner join dbo.Attributes with(nolock) on Atr_AdwId = Adw_Id
inner join dbo.SearchLink with(nolock) on Sel_Id = Adw_SeLId
where Atr_Name in ('Czynsz - dodatkowo','Czynsz (dodatkowo)') and Sel_Id = @searchId and
Adw_Sent = 0 and
--warunki
Adw_Price + cast(replace(replace(Atr_Value,' z³',''),' ','') as decimal(8,2)) <= 1500
and cast(Adw_Date as date) = cast(GETDATE() as date)

set @Message = '<h1>Nowe oferty na rejonie!</h1>
'

declare kursor cursor read_only fast_forward
for
select Adw_Name, Adw_Url, Adw_Location, Adw_Price,
Adw_Price + cast(replace(replace(Atr_Value,' z³',''),' ','') as decimal(8,2)) Kwota
from dbo.Advert
inner join dbo.Attributes with(nolock) on Atr_AdwId = Adw_Id
where Adw_Id in (select Id from @Ids)
and Atr_Name in ('Czynsz - dodatkowo','Czynsz (dodatkowo)')

open kursor
fetch next from kursor 
into @Nazwa, @Url, @Lokalizacja, @Czynsz, @CzynszRzeczywisty
set @Title = @Title + @Lokalizacja	
while @@FETCH_STATUS =0
begin
	set @Message = @Message + '<h3 style=''margin-top: 15px''>' + @Nazwa + '</h3>
<p>Link do <a href=''' + @Url + '''>oferty</a></p>
<p>Wartoœæ og³oszenia: ' + cast(@Czynsz as varchar(12)) + 'z³</p>
<p>Wartoœæ rzeczywista: ' + cast(@CzynszRzeczywisty as varchar(12)) + 'z³</p>
'

fetch next from kursor 
into @Nazwa, @Url, @Lokalizacja, @Czynsz, @CzynszRzeczywisty
end
close kursor
deallocate kursor

if exists(select 1 from @Ids)
begin
	EXEC msdb.dbo.sp_send_dbmail  
	@profile_name = 'Gmail',  
	@recipients = 'matikielpinski@gmail.com',  
	@subject = @Title,  
	@body = @Message,
	@body_format = 'HTML'
end

update dbo.Advert set Adw_Sent = 1 where Adw_Id in (select Id from @Ids)

set nocount off;