create database sanpatricio
go
use sanpatricio
create table alumno
(
id varchar(12),
dni char(8),
nombre varchar (100),
apepat varchar (100),
apetmat varchar (100),
foto image,
fecreg date,
fecupd date
)
go
create procedure sp_listaralumno
as
begin
select id as 'CODIGO',dni AS 'DNI',nombre AS 'NOMBRES',apepat AS 'PATERNO',apetmat AS 'MATERNO',fecreg AS 'FEC. INGRESO', foto as 'FOTO' from alumno
order by fecreg desc
end
go
create procedure sp_listaralumnoxdniocod
@filtro varchar(11)
as
begin
select id as 'CODIGO',dni AS 'DNI',nombre AS 'NOMBRES',apepat AS 'PATERNO',apetmat AS 'MATERNO',fecreg AS 'FEC. INGRESO', foto as 'FOTO' from alumno
where id like '%'+@filtro+'%' or dni like '%'+@filtro+'%'
order by fecreg desc
end
go
create procedure sp_registraralumno
@dni int,
@nombre varchar (100),
@apepat varchar (100),
@apetmat varchar (100),
@foto image
as
begin
insert into alumno values ((select 'i'+convert(char(4),year(GETDATE()))+(select right('00000'+convert(varchar(20),(select isnull((substring((select max(id) from alumno),6,5)+1),'1'))),5))
),@dni,@nombre,@apepat,@apetmat,@foto,GETDATE(),GETDATE())
end
go
create procedure sp_actualizaralumno
@id varchar(12),
@dni int,
@nombre varchar (100),
@apepat varchar (100),
@apetmat varchar (100),
@foto image
as
begin
update alumno set dni = @dni , nombre=@nombre,apepat=@apepat,apetmat=@apetmat,foto=@foto,fecupd=GETDATE()
where id = @id
end
go
