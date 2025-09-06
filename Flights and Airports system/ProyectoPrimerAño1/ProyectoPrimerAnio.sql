------------------------------------ PROYECTO PRIMER AÑO------------------------------------
------------------------------------------------------------------------------------------------------

--BORRAR LA BD SI EXISTE PARA PODER CREARLA
use master
go

if (exists (select * from sysdatabases where name = 'ProyectoPrimerAnio'))
begin
	drop database ProyectoPrimerAnio
end
go
--crear nueva bd
create database ProyectoPrimerAnio
go
use ProyectoPrimerAnio
go

-----------------------------------CREACION DE TABLAS ----------------------------------
Create table Ciudad
(
CodigoC varchar (6) primary key not null,
NombreC varchar (20) not null,
Pais varchar (20) not null
)
go

Create table Aeropuerto
(
CodigoA varchar(3) Primary key not null,
CodigoC varchar (6) foreign key references Ciudad (CodigoC) not null,   --ultima mod
NombreA varchar(40) not null,                                              
Direccion varchar(40) not null,
ImpuestoOri int not null,
ImpuestoDes int not null
)
go

Create table Vuelo
(
CodigoV varchar(15) primary key not null, --fecha y hora de partida + cod aeropuerto. Formato yyyyMMddHHmmAer (202401202050MVD)
CodigoA1Salida varchar(3) foreign key references Aeropuerto(CodigoA)not null, --estoy en duda si esto se puede repetir, esta bien asi? como indico si es del aeropuerto 1 o 2
CodigoA2Llegada varchar(3) foreign key references Aeropuerto(CodigoA)not null,
FechaPartida datetime not null,
FechaLlegada datetime not null,
Precio int not null,
CantAsientos int not null,
)
go

Create table Cliente
(
Pasaporte bigint Primary key not null,
NombreCompC Varchar(30) not null,
ContraC varchar (20) not null,
NumeroTarj bigint not null --(16 numeros) 
)
go

Create table Pasaje
(
NumV int identity primary key not null,
CodigoV varchar(15) foreign key references Vuelo(CodigoV) not null,
Pasaporte bigint foreign key references Cliente(Pasaporte) not null,
FechaVenta datetime not null,
PrecioTot int not null --Precio pasaje + impuestoOr + ImpuestoDes
)
go

create table Empleado
(
Usuario varchar (30) primary key not null,
NombreCompE Varchar(30) not null,
ContraE varchar (20) not null,
Cargo varchar (10) not null -- valor pre cargado (Gerente – Vendedor – Admin)
)
go

set dateformat ymd --setea el formato correcto 
------------------------------------- INSERCION DE DATOS --------------------------------

--CIUDADES
Insert Ciudad values ('MVDURU', 'Montevideo', 'Uruguay')
Insert Ciudad values ('BSSARG', 'Buenos Aires', 'Argentina')
Insert Ciudad values ('SNPBRA', 'San Pablo', 'Brasil')
Insert Ciudad values ('SANCHI', 'Santiago de Chile', 'Chile')
Insert Ciudad values ('LAPBOL', 'La Paz', 'Bolivia')
Insert Ciudad values ('ASUPAR', 'Asuncion', 'Paraguay')
Insert Ciudad values ('LIMPER', 'Lima', 'Peru')
Insert Ciudad values ('BOGCOL', 'Bogota', 'Comombia')
Insert Ciudad values ('CARVEN', 'Caracas', 'Venezuela')
Insert Ciudad values ('BRABRA', 'Brasilia', 'Brasil')
Insert Ciudad values ('MADESP', 'Madrid', 'España')
Insert Ciudad values ('LISPOR', 'Lisboa', 'Portugal')
Insert Ciudad values ('PARFRA', 'Paris', 'Ferancia')
Insert Ciudad values ('ROMITA', 'Roma', 'Italia')
Insert Ciudad values ('BERALE', 'Berlin', 'Alemania')
go

--AEROPUERTO  
Insert Aeropuerto values ('MVD','MVDURU', 'Aeropuerto Campeon del siglo', 'Av. Peñarol 1111', 300, 0)
Insert Aeropuerto values ('BSS','BSSARG', 'Aeropuerto de Aires Buenos', 'Av. Allala 1212', 150,0)
Insert Aeropuerto values ('SNP', 'SNPBRA', 'Aeroporto Beleza', 'Av. Vinicious Jr. careca 3434', 200, 0)
Insert Aeropuerto values ('SAN', 'SANCHI', 'Aeropuerto Weon', 'Calle C.CTM 1234', 500, 0)
Insert Aeropuerto values ('LAP', 'LAPBOL', 'Puerto Aereo porque mar no tienen', 'Av. nida 4569',250, 0)	
Insert Aeropuerto values ('ASU','ASUPAR', 'Aeropuerto Paragua', 'Dr Bufarreti 7868', 235,0)
Insert Aeropuerto values ('LIM','LIMPER', 'ElPisco NoPega Airport', 'Av. EstaNo LaSiguiente 3333',400,0)
Insert Aeropuerto values ('BOG', 'BOGCOL', 'Balderrama Aeropuerto', 'Ave Nida 4102', 175,0)
Insert Aeropuerto values ('CAR','CARVEN', 'Aeropuerto Vinotinto', 'Estrasulas 6767',10,0)
Insert Aeropuerto values ('BRA','BRABRA', 'James salada di fruta Aeroporto', 'Olhea esa habilidachi 1072',200, 0)
Insert Aeropuerto values ('MAD','MADESP', 'Aeropuerto Ostia tio','Av. Madrid 231',600,0)                            
Insert Aeropuerto values ('LIS','LISPOR', 'Aeropuerto CR7', 'Portu Gal 1700', 500,0)
Insert Aeropuerto values ('PAR','PARFRA', 'Gui Aeroport', 'Interbaalnearea km 2',600,0)--aeropuerto sin vuelo
Insert Aeropuerto values ('ROM','ROMITA', 'Aeroporto Italiani','AV. En italia 4455',450,0)--aeropuerto sin vuelo
Insert Aeropuerto values ('BER','BERALE','Aeropuerto Aleman', 'Av. Aleman 1087', 650,0)--aeropuerto sin vuelo
go

--VUELO
Insert Vuelo values ('202403152000BSS','MVD','BSS','2024-03-15 20:00','2024-03-15 21:00',4500, 150) --formato de Codigo V yyyyMMddHHmmAer. otro formato para insertar fechas  yyyyMMdd HH:mm
Insert Vuelo values ('202404031800MVD','BSS','MVD','2024-04-03 18:00','2024-04-03 21:00',3500, 200)
Insert Vuelo values ('202404011700MVD','SNP','MVD','2024-04-01 17:00','2024-04-01 22:03',9000, 232)
Insert Vuelo values ('202404071100SAN','BSS','SAN','2024-04-07 11:00','2024-04-07 17:45',6000, 241)
Insert Vuelo values ('202404112100ASU','LIM','ASU','2024-04-11 21:00','2024-04-12 01:00',3200, 152)
Insert Vuelo values ('202404101330LIM','MVD','LIM','2024-04-10 13:30','2024-04-10 17:30',7600,222)
Insert Vuelo values ('202405010930BOG','SNP','BOG','2024-05-01 09:30','2024-05-01 14:00',7800,281)
Insert Vuelo values ('202405031900BOG','MVD','BOG','2024-05-03 19:00','2024-05-03 23:30',9300,291)
Insert Vuelo values ('202405100800MVD','BRA','MVD','2024-05-10 08:00','2024-05-10 12:00',8000,165)
Insert Vuelo values ('202405110900CAR','MVD','CAR','2024-05-11 09:00','2024-05-11 12:30',1000,101)
Insert Vuelo values ('202405121900BRA','LIS','BRA','2024-05-12 19:00','2024-05-13 04:00',1100,249)
Insert Vuelo values ('202405130200LIM','MAD','LIM','2024-05-13 02:00','2024-05-13 13:00',1100,249)-- sin pasajes
Insert Vuelo values ('202405131500BOG','ASU','BOG','2024-05-13 15:00','2024-05-13 19:00',9200,225)-- sin pasajes
Insert Vuelo values ('202405131800MAD','BSS','MAD','2024-05-13 18:00','2024-05-14 01:30',8000,225)-- sin pasajes
Insert Vuelo values ('202405151800SAN','BSS','SAN','2024-05-15 18:00','2024-05-16 13:30',8000,225)-- sin pasajes
go

--CLIENTE
Insert Cliente values (48700655, 'Nicolas Dagys', 'seniacontra!1', 1234432156788910)
Insert Cliente values (46677788, 'Omar Peralta', 'seniacontra!2', 4444888866662222)
Insert Cliente values (58921452, 'Esteban Perinaglia','seniacontra!3', 1461234571243756)
Insert Cliente values (20318401, 'Juan Ramirez','seniacontra!4', 8130100590763557)
Insert Cliente values (10435767, 'Marco Polo', 'seniacontra!5', 1025100369197356)
Insert Cliente values (22340973, 'Rodrigo Igo', 'seniacontra!6',7300551799124675)
Insert Cliente values (25468466, 'Tomas Mas','seniacontra!7', 3270003715455661)
Insert Cliente values (75168556, 'Roberto Berto', 'seniacontra!8', 1308788663315776)
Insert Cliente values (70887317, 'David Vid', 'seniacontra!9',1097511338860473)
Insert Cliente values (40088773, 'Manuel Nuel', 'seniacontra!10', 9722430884667200)
go

--PASAJE
Insert Pasaje values ('202403152000BSS',48700655,'2024-01-01 08:00', 4800)
Insert Pasaje values ('202404031800MVD',46677788,'2024-01-02 06:00', 3650)
Insert Pasaje values ('202404011700MVD',58921452,'2024-02-12 07:00',9200)
Insert Pasaje values ('202404071100SAN',20318401,'2024-02-13 12:00',6150)
Insert Pasaje values ('202404112100ASU',10435767,'2024-02-15 12:00',3600)
Insert Pasaje values ('202404101330LIM',22340973,'2024-02-15 15:00',7900)
Insert Pasaje values ('202405010930BOG',25468466,'2024-02-15 16:20',8000)
Insert Pasaje values ('202405031900BOG',75168556,'2024-03-01 18:20',9600)
Insert Pasaje values ('202405100800MVD',70887317,'2024-03-05 10:00',8200)
Insert Pasaje values ('202405110900CAR',40088773,'2024-03-12 21:40',1300)
Insert Pasaje values ('202405121900BRA',48700655,'2024-03-12 21:50',1600)
Insert Pasaje values ('202403152000BSS',46677788,'2024-03-13 05:00',4800)
Insert Pasaje values ('202404031800MVD',58921452,'2024-03-13 07:00',3650)
Insert Pasaje values ('202404112100ASU',20318401,'2024-03-13 07:00',3600)
Insert Pasaje values ('202405100800MVD',48700655,'2024-03-13 07:30',8200)
go


--EMPLEADO
Insert Empleado values('RobertoCarlos','Carlos Roberto', 'Fulbo123@', 'Vendedor') --(Gerente – Vendedor – Admin)
Insert Empleado values('WiliamsonWiliam','Wiliam Wiliamson','Fulbo1234@','Vendedor')
Insert Empleado values('MolinaManuel','Manuel Molina','Fulbo125@','Vendedor')
Insert Empleado values('PerezAgustin','Agustin Perez','Fulbo126@','Vendedor')
Insert Empleado values('SalasaSamuel','Samuel Salasa','Fulbo127@','Vendedor')
Insert Empleado values('MartinezStefani','Stefani Martinez','Fulbo128@','Vendedor')
Insert Empleado values('LueMonica','Monica Lue','Fulbo129@','Vendedor')
Insert Empleado values('GonzalesJosefina','Josefina Gonzales','Fulbo122@','Gerente')
Insert Empleado values('MarxCarlos','Crlos Marx','Fulbo211@','Gerente')
Insert Empleado values('MendiettaNicolas','Nicolas Mendietta','Fulbo112@','Admin')
go

------------------------------------- PROCEDIMIENTOS ------------------------------------
---ABM CIUDADES
-------------------------------------------------------------------------------------
--Buscar Ciudad
Create Procedure BuscarCiudad
@codigoC varchar(6)
AS
Begin
	Select * From Ciudad Where CodigoC = @codigoC
End
go

--Agregar Ciudad
Create Procedure AgregarCiudad
@codigoC varchar (6), @nombreC varchar (20), @pais varchar (20)
as
Begin
	if (EXISTS(Select * From Ciudad Where CodigoC = @codigoC))
		return 0 

		Insert Ciudad (CodigoC,NombreC,Pais) values (@codigoC,@nombreC,@pais)
		IF(@@Error=0)
		RETURN 1
	ELSE
		RETURN -1
end
go

--Modificar Ciudad 
Create Procedure ModificarCiudad
@codigoC varchar (6), @nombreC varchar (20), @pais varchar (20)
as
begin
	if (not exists(select CodigoC from Ciudad  where CodigoC=@codigoC ))
		return -1

		update Ciudad set NombreC=@nombreC, Pais=@pais
		where CodigoC=@codigoC
		return 1
end
go

--Eliminar Ciudad
Create Procedure EliminarCiudad
@codigoC varchar (6)
as
begin
	--valido si existe la Ciudad
	if(not exists (select * from Ciudad where CodigoC=@codigoC))
		return -1 

	--valido si tiene Aeropuertos asociados
	if(exists (select * from Aeropuerto where CodigoC=@codigoC))
		return -2 

		Delete Ciudad where CodigoC=@codigoC
	return 1
end
go

--ABM AEROPUERTOS
-------------------------------------------------------------------------------------
--Buscar Aeropuerto
Create Procedure BuscarAeropuerto
@codigoA varchar(3)
as
begin
	Select * from Aeropuerto where CodigoA=@codigoA
end
go

--Agregar Aeropuerto
Create Procedure AgregarAeropuerto
@codigoA varchar(3), @codigoC varchar (6), @nombreA varchar(20), @direccion varchar(40), @impuestoOri int, @impuestoDes int
as
begin
	-- valido existencia de PK
	if(exists(select CodigoA from Aeropuerto  where CodigoA=@codigoA ))
		return 0

	--valido si exite la ciudad
	if(not exists(select CodigoC from Ciudad  where CodigoC=@codigoC))
		return - 1

		insert Aeropuerto values (@codigoA,@codigoC,@nombreA,@direccion,@impuestoOri,@impuestoDes)
	IF(@@Error=0)
		RETURN 1
	ELSE
		RETURN -2
end
go

--Modificar Aeropuerto 
Create Procedure MofificarAeropuerto
@codigoA varchar(3), @codigoC varchar (6), @nombreA varchar(20), @direccion varchar(40), @impuestoOri int, @impuestoDes int
as
begin
	-- valido existencia de PK
	if(not exists(select CodigoA from Aeropuerto  where CodigoA=@codigoA ))
		return 0

	--valido si exite la ciudad
	if(not exists(select CodigoC from Ciudad  where CodigoC=@codigoC))
		return - 1

	update Aeropuerto set CodigoC=@codigoC, NombreA=@nombreA, Direccion=@direccion, ImpuestoOri=@impuestoOri, ImpuestoDes=@impuestoDes
		where CodigoA=@codigoA
		return 1
end
go

--Eliminar Aeropuerto
Create Procedure EliminarAeropuerto
@codigoA varchar(3)
As
begin
	-- valido existencia de PK
	if(not exists(select * from Aeropuerto  where CodigoA=@codigoA ))
		return 0

		--valido si tiene Vuelos asociados
	if(exists (select * from Vuelo where CodigoA1Salida=@codigoA or CodigoA2Llegada=@codigoA))
		return -1 

		Delete Aeropuerto where CodigoA=@codigoA
	return 1
end
go

--Listado Aeropuertos
Create Procedure ListarAeropuertos
as
begin
select * From Aeropuerto
end
go

--ABM CLIENTES
----------------------------------------------------------------------------------------
--Buscar Cliente
Create Procedure BuscarCliente
@pasaporte bigint
as
begin
	Select * From Cliente where Pasaporte=@pasaporte
end
go

--Agregar Cliente
Create Procedure AgregarCliente
@pasaporte bigint, @nombreCompC Varchar(30), @contraC varchar (20), @numeroTarj bigint
as
begin
	if (EXISTS(Select * From Cliente Where Pasaporte = @pasaporte))
		return -1
	
	INSERT Cliente VALUES(@pasaporte, @nombreCompC, @contraC, @numeroTarj) 
	
	IF(@@Error=0)
		RETURN 1
	else
		RETURN -2
end
go


--Modificar Cliente 
Create Procedure ModificarCliente
@pasaporte bigint, @nombreCompC Varchar(30), @contraC varchar (20), @numeroTarj bigint
as
begin
	If(not exists(select * from Cliente where Pasaporte=@pasaporte))
	return 0

	update Cliente set NombreCompC=@nombreCompC, ContraC=@contraC, NumeroTarj=@numeroTarj
	where Pasaporte=@pasaporte
	return 1
end
go

--Eliminar Cliente
Create Procedure EliminarCliente
@pasaporte bigint
as
begin
	-- valido existencia de PK
	if(not exists(select * from Cliente where Pasaporte=@pasaporte))
		return 0

		if( exists(select * from Pasaje where Pasaporte=@pasaporte))
		return -1

		Delete Cliente where Pasaporte=@pasaporte
	return 1
end
go

--VUELOS
----------------------------------------------------------------------------------------
--Buscar Vuelo
Create Procedure BuscarVuelo
@codigoV varchar(15)
AS
Begin
	Select * From Vuelo Where CodigoV=@codigoV
End
go

--Alta Vuelo
Create Procedure AgregarVuelo
@codigoV varchar(15), @codigoA1 varchar(3), @codigoA2 varchar(3), @fechaPartida datetime, @fechaLlegada datetime, @precio int, @cantAsientos int
as
begin
	--valido PK 
	if (exists (select * from vuelo where CodigoV = @codigoV))
		return 0

	--valido Aeropuerto Salida
	if (not exists (select * from Aeropuerto where CodigoA = @codigoA1))
		return -1

	--valido Aeropuerto Llegada
	if (not exists (select * from Aeropuerto where CodigoA=@codigoA2))
		return -2

insert Vuelo values (@codigoV,@codigoA1,@codigoA2,@fechaPartida,@fechaLlegada,@precio,@cantAsientos)	
	IF(@@Error=0)
		RETURN 1
	else
		RETURN -3
end
go

--Listado Partidas
Create Procedure ListarPartidas 
@codigoA1 varchar(3)
as
begin
	Select * from Vuelo where CodigoA1Salida=@codigoA1 and FechaPartida> GETDATE()
end
go

--Listado Arribos
Create Procedure ListarArribos
@codigoA2 varchar(3)
as
begin
	Select * from Vuelo where CodigoA2Llegada=@codigoA2 and FechaLlegada > GETDATE()
end
go

--PASAJES
----------------------------------------------------------------------------------------
--Alta pasaje
Create Procedure AgregarPasaje
@codigoV varchar(15), @pasaportre bigint, @fechaVenta datetime, @precioTot int ----Precio pasaje + impuestoOr + ImpuestoDes (hago el calculo en c# y lo paso al parametro @precioTot)
as
begin
	--valido PK 
	if (not exists (select * from Vuelo where CodigoV = @codigoV))
		return -1 --ya existe la PK

	--valido PK 
	if (not exists (select * from Cliente where Pasaporte = @pasaportre))
		return -2 

insert Pasaje values(@codigoV,@pasaportre,@fechaVenta,@precioTot)
	IF(@@Error=0)
		RETURN 1
	ELSE
		RETURN -3
end
go

--Pasajes por Cliente
Create Procedure PasajeXCliente
@pasaporte bigint
as
begin
	Select * from Pasaje  where Pasaporte=@pasaporte
end
go
--USUARIOS
----------------------------------------------------------------------------------------
--Logeo Empleado
Create procedure LogeoEmpleado
@usuario varchar (30), @pass varchar (20)
as
begin
	Select * From Empleado Where Empleado.Usuario = @usuario AND Empleado.ContraE = @pass
end
go

--Logeo Cliente
Create procedure LogeoCliente
@pasaporte bigint, @pass varchar (20)
as
begin
	Select * From Cliente Where Cliente.Pasaporte = @pasaporte and Cliente.ContraC=@pass
end
go