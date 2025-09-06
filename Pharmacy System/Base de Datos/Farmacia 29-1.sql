-- Si la base de datos existe, la borro. --
Use master
go

if exists(Select * FROM SysDataBases WHERE name='Farmacia')
BEGIN
	DROP DATABASE Farmacia
END
go

-- Creo la base de datos
Create database Farmacia
go

-- Pongo en uso la base de datos
Use Farmacia
go

---------- TABLAS -------------------------------------------------------------------

Create table Categoria
(
	CodCat varchar(6) primary key check (CodCat  like '%[A-Z]%[A-Z]%[A-Z]%'  AND 
                                    CodCat like '%[0-9]%[0-9]%[0-9]%'), 
	NomCat varchar (50) not null check(len(NomCat)>2),
	Activo bit not null Default (1)
)
go

Create table Articulo
(
	CodArt varchar(10) primary key check(CodArt  like '%[A-Z]%[A-Z]%[A-Z]%[A-Z]%[A-Z]%'  AND 
                                    CodArt like '%[0-9]%[0-9]%[0-9]%[0-9]%[0-9]%'),
	NomArt varchar(25) not null check(len(NomArt)>2),
	PrecioArt int not null check(PrecioArt > 0),
	FechaVtoArt date not null check(FechaVtoArt > getdate()),
	TipoPArt varchar(10) NOT NULL CHECK(TipoPArt IN ('Unidad', 'Blister', 'Sobre', 'Frasco')),
	TamArt int not null check(TamArt > 0),
	CodCat varchar(6) not null foreign key references Categoria(CodCat),
	Activo bit not null Default (1)
	)
go

Create table Cliente
(
	CiCli varchar (9) primary key check (CiCli like '[1-6][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9]' or CiCli like '[1-9][0-9][0-9][0-9][0-9][0-9]-[0-9]'),
	NomCli varchar(20) not null check(len(NomCli)>2),
	NumTarjCli varchar (16) not null check (NumTarjCli like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'), --16 digitos
	TelCli varchar (9)
)
go

Create table Venta
(
	NumVent int identity primary key,
	FechaVent date default getdate() not null,
	DirEnvIoVent varchar(50) not null check(len(DirEnvIoVent)>4),
	TotalVent int not null check (TotalVent > 0),
	CiCli varchar(9) not null foreign key references Cliente (CiCli)
)
go

Create table Linea
(
	NumVent int not null foreign key references Venta (NumVent),
    CodArt varchar(10) not null foreign key references Articulo(CodArt), 
    Cant int not null check(Cant > 0),
    primary key (NumVent, CodArt)
)
go

Create table Estado
(
	NumEst int primary key,
	NomEst varchar(50) not null check(len(NomEst)>2)
)
go

Create table Asignacion
(
    NumVent int not null foreign key references Venta(NumVent),
    NumEst int not null foreign key references Estado(NumEst),
	primary key (NumVent, NumEst),
    FyHEst datetime default getdate()
)
go

Create table Empleado
(
	UsuEmp varchar(50) primary key check(len(UsuEmp)>2),
	NomEmp varchar(50) not null check(len(NomEmp)>2), 
	PassEmp varchar(50) not null check(len(PassEmp)>4)
)
go

----------------------------------------- SP--------------------------------------------------------

------Articulo-------------------------------

--Buscar Art (Activo)
Create Procedure ArticuloBuscarActivo
	@CodArt varchar(10)
As
Begin
	Select * From Articulo
	Where CodArt = @CodArt AND Activo = 1
End
Go

--Buscar Art (todos)
Create Procedure ArticuloBuscarTodos
	@CodArt varchar(10)
As
Begin
	Select * From Articulo
	Where CodArt = @CodArt
End
Go

--Alta Art
Create Procedure AltaArticulo
    @CodArt varchar(10), @NomArt varchar(25), @PrecioArt int, @FechaVtoArt date, @TipoPArt varchar(10), @TamArt int, @CodCat varchar(6)
As
Begin

-- Verifica que la categoria exista y activa
    If Not Exists (Select * From Categoria Where CodCat = @CodCat and Activo=1)
    Begin
        Return -1 -- No existe Cat
    End

    -- Verifica el Art ya existe y si esta activo
    If Exists (Select * From Articulo Where CodArt = @CodArt AND Activo = 1)
    Begin
        Return -2 -- Existe art y esta activo
    End

    -- Verifica si el art existe pero está inactivo
    If Exists (Select * From Articulo Where CodArt = @CodArt AND Activo = 0)
    Begin
        Update Articulo
        Set NomArt = @NomArt, PrecioArt=@PrecioArt, FechaVtoArt=@FechaVtoArt, TipoPArt=@TipoPArt, TamArt=@TamArt, CodCat=@CodCat, Activo = 1
        Where CodArt = @CodArt
        Return 1 -- art reactivado
    End

    -- Inserta el nuevo Articulo
    Insert Into Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) --activo es Default asi que no hay que asignarlo no ?
    Values (@CodArt, @NomArt, @PrecioArt, @FechaVtoArt, @TipoPArt, @TamArt, @CodCat)

    If @@ERROR <> 0
    Begin
        Return -3 -- Error, no se pudo dar de alta
    End

End
Go

--Eliminar Art
create procedure BajaArticulo
	@CodArt varchar(10)
As
Begin
	If Not Exists (Select * From Articulo Where CodArt = @CodArt)
		Return -1 -- No existe Art

	-- Si tiene Lineas asociadas hace baja lógica
	If (Exists (Select * From Linea Where CodArt = @CodArt))
		Begin
			Update Articulo
			Set Activo = 0
			Where CodArt = @CodArt
			Return 1
		End
	Else
		Begin
			-- Sino, elimina
			Delete From Articulo
			Where CodArt = @CodArt 
				If (@@ERROR = 0) 
					Return 1
				Else 
					Return -2
		End
End
Go

--Modificar Art
Create Procedure ModificarArticulo
	 @CodArt varchar(10), @NomArt varchar(25), @PrecioArt int, @FechaVtoArt date, @TipoPArt varchar(10), @TamArt int, @CodCat varchar(6)
As
Begin

-- Verifica que la categoria exista y activa
    If Not Exists (Select * From Categoria Where CodCat = @CodCat and Activo=1)
    Begin
        Return -1 -- No existe Cat o no está activa
    End

	If Not exists (Select * From Articulo Where  CodArt = @CodArt AND Activo = 1)
		Return -2 -- No existe art o no está activo

	Update Articulo
	Set NomArt=@NomArt, PrecioArt=@PrecioArt, FechaVtoArt=@FechaVtoArt, TipoPArt=@TipoPArt, TamArt=@TamArt, CodCat=@CodCat
	Where CodArt = @CodArt

	If @@ERROR <> 0
		Return -2 -- Error, no se modifica
End
Go

--Listo Art (activos)
Create Procedure ListarTodosArticulos
As
Begin
	Select *
	From Articulo
	where Activo=1
End
Go

--Listo Art no Vencidos (activos)
Create Procedure ListarArticulosNoVencidos
As
Begin
	Select *
	From Articulo
	where Activo=1 AND FechaVtoArt > getdate()
	order by NomArt
End
Go



------- Categoria --------------------------------------------
--Buscar Cat (Activo)
create procedure BuscarCategoriaActivo
	@CodCat varchar(6)
As
Begin
	Select * From Categoria
	Where CodCat = @CodCat AND Activo = 1
End
Go

--Buscar Cat (todos)
create procedure BuscarCategoriaTodos
	@CodCat varchar(6)
As
Begin
	Select * From Categoria
	Where CodCat = @CodCat
End
Go

--Alta Cat
create procedure AltaCategoria
	@CodCat varchar(6), @NomCat varchar (50)
As
Begin
	-- Verifica si la Categoría existe y si está activa
	If Exists (Select * From Categoria Where CodCat =  @CodCat AND Activo = 1)
	Begin
		Return -1 -- Existe Cat y está activa
	End
	
	-- Verifica si la Cat existe pero está inactiva
	If Exists (Select * From Articulo Where CodCat = @CodCat AND Activo = 0)
	Begin
		Update Categoria
		Set NomCat = @NomCat, Activo = 1
		Where CodCat = @CodCat
		Return 1 -- Categoria reactivada
	End

	-- Inserta la Categoría
	Insert Into Categoria(CodCat, NomCat)
	Values (@CodCat, @NomCat) -- Activo default

	If @@ERROR <> 0
	Begin
		Return -2 -- Error, no se pudo dar de alta
	End
End
Go

--Eliminar Cat
Create procedure BajaCategoria
	@CodCat varchar(6)
As
Begin
	If Not Exists (Select * From Categoria Where CodCat = @CodCat)
		Return -1 -- No existe Categoría

	-- Si tiene Artículos asociados hace baja lógica
	If (Exists (Select * From Articulo Where CodCat = @CodCat))
		Begin
			Update Categoria
			Set Activo = 0
			Where CodCat = @CodCat
			Return 1
		End
	Else
		Begin
			-- Sino, elimina
			Delete From Categoria
			Where CodCat = @CodCat 
				If (@@ERROR = 0) 
					Return 1
				Else 
					Return -2
		End
End
Go

--Modificar Cat 

Create Procedure ModificarCategoria  
@CodCat varchar(6), @NomCat varchar (50)
As 
Begin
	if not Exists (Select * From Categoria where CodCat = @CodCat AND Activo = 1)
		Return -1 --No existe categoría o no está activa
	Update Categoria
	Set NomCat = @NomCat Where CodCat = @CodCat
	if @@ERROR <> 0
		Return -2 --Error, no se modifica
end
go

--ListarCat
Create Procedure ListarCategorias
As
Begin
	Select *
	From Categoria
End
Go

--ListarCatActivas
Create Procedure ListarCategoriasActivas
As
Begin
	Select *
	From Categoria 
	where Activo=1
End
Go


-- Empleados  --------------------------------------------
-- Logueo Empleado
Create Procedure LogueoEmpleado
	@UsuEmp varchar(50), @PassEmp varchar(50)
As
Begin
	Select * From Empleado
	Where UsuEmp = @UsuEmp AND PassEmp = @PassEmp
End
Go




-- Clientes ----------------------------------------------
--Buscar Cliente
Create Procedure BuscarCliente
	@CiCli varchar (9)
As
Begin
	Select * from Cliente
	Where CiCli = @CiCli
end
Go
	
--Alta Cliente
Create Procedure AltaCliente
	@CiCli varchar (9), @NomCli varchar (20), @NumTarjCli bigint, @TelCli bigint
As
Begin
	-- Verifico si existe el cliente
	If Exists (Select * from Cliente Where CiCli = @CiCli)
		return -1 -- No se puede dar de alta, ya existe el cliente 
	-- Inserto el cliente
	Insert Into Cliente (CiCli, NomCli, NumTarjCli, TelCli) 
	values (@CiCli, @NomCli, @NumTarjCli, @TelCli) 
	If (@@ERROR = 0)
			return 1
		Else
			return -2
end
Go
 
--Modificar Clientes
Create Procedure ModificarCliente
@CiCli varchar (9), @NomCli varchar (20), @NumTarjCli bigint, @TelCli bigint
As
Begin
	-- Verifico si existe ciente 
	If Not Exists (Select * from Cliente Where CiCli = @CiCli)
		return -1 -- No se puede modificar, no existe el cliente 
	-- modifico el cliente
	Update Cliente 
	Set  NomCli=@NomCli , NumTarjCli=@NumTarjCli , TelCli=@TelCli where CiCli=@CiCli
	If (@@ERROR = 0)
			return 1
		Else
			return -2
end
Go
	
	
--Listar Clientes
Create Procedure ListarClientes
As
Begin
	Select *
	From Cliente
End
Go


-- Líneas ----------------------------------------------
--Alta Línea
Create Procedure AltaLinea

@NumVent int, @CodArt varchar (10), @Cant int
As
Begin
	--verifico que exista el artículo
	if Not Exists (Select * from Articulo Where CodArt = @CodArt)
		return -1 -- No existe artículo
	--verifico que exista venta con ese número


	if Not Exists (Select * from Venta where NumVent = @NumVent)
    return -2 -- No existe venta 

	--valido la pk	
	if Exists (Select * from Linea Where NumVent = @NumVent AND CodArt = @CodArt)
		return -3 --Ya existe esta línea
		
	--doy de alta la línea
	Insert Into Linea (NumVent, CodArt, Cant) 
	values (@NumVent, @CodArt, @Cant) 	
	if (@@ERROR = 0)
		return 1
	else
		return -4	
end
Go

--Listar Líneas
Create Procedure ListarLineas
@NumVent int
As
Begin
	Select * from Linea where NumVent = @NumVent
end
Go 


-- Ventas ----------------------------------------------
--Alta Venta
create Procedure AltaVenta
@DirEnvioVent varchar (50), @TotalVent int, @CiCli varchar (9) 
As
Begin
	-- verifico que exista cliente
	if Not Exists (Select * from Cliente where CiCli=@CiCli)
		return -1 -- no se puede dar de alta, no existe cliente
	--doy de alta a la venta
	Insert Into Venta (DirEnvIoVent, TotalVent, CiCli) 
		values (@DirEnvioVent, @TotalVent, @CiCli)
	if (@@ERROR = 0)
		return @@Identity
	else
		return -2
end
Go


--Listar Todas las Ventas
Create Procedure ListarVentas
As
Begin
	Select * from Venta
end
Go


-- Asignación ----------------------------------------------

--Cambio de Estado una Venta - Asignación
Create Procedure AsignoEstadoVenta
@NumVent int
As
Begin
	Declare @EstAsignar int
	-- verifico que exista una venta con ese número
	if Not Exists (Select * from Venta where NumVent=@NumVent)
		return -1 --no se puede modificar una venta que no existe
		
	-- si la venta tiene el estado 4 (último), no puedo cambiarle el estado
	if Exists (Select * from Asignacion where  NumVent = @NumVent AND NumEst = 4)
		return -2 -- Estado último - Devuelto
	
		-- si no se le ha asignado un estado a la venta, le asigno el 1
	if Not Exists (Select * from Asignacion where NumVent=@NumVent)
		insert Asignacion (NumVent, NumEst) values (@NumVent,1)	
	
	Set @EstAsignar = (select MAX(NumEst) from Asignacion where NumVent = @NumVent) 
	
	insert Asignacion (NumVent, NumEst) values (@NumVent,@EstAsignar+1)
	
	if (@@ERROR = 0)
		return 1
	else
		return -3
end
Go 

Create Procedure ListarEstadosVenta
@NumVent int
As
Begin
	select * from Asignacion where NumVent = @NumVent
end
go




-- Estado ----------------------------------------------
Create Procedure BuscarEstado
@NumEst int
As
Begin
	Select * from Estado
	Where NumEst = @NumEst
end
Go


Create Procedure ListarEstados
As
Begin
	Select * from Estado
end
Go


-------------------------------------- USUARIO DE IIS ------------------------------
Use master
go
 
Create login [IIS APPPOOL\DefaultAppPool] from windows
go
 
Use Farmacia
go
 
Create user [IIS APPPOOL\DefaultAppPool] for login [IIS APPPOOL\DefaultAppPool]
go

Exec sys.sp_addrolemember 'db_owner', [IIS APPPOOL\DefaultAppPool]

go




---------------------------------- USUARIO SQL PARA EMPLEADOS, PERMISOS----------------------

Create Procedure NuevoUsuarioEmpleado
@UsuEmp varchar(50),
@NomEmp varchar(50), 
@PassEmp varchar(50) 
As
Begin
	Declare @variable varchar (200)
	if (Exists (Select * from empleado where UsuEmp = @UsuEmp))
		return -1 -- Error - No puedo ingresar. Ya existe ese usuario
	
	Begin transaction
	Begin try
			-- 1. Inserto Empleado en la tabla
			Insert Empleado Values (@UsuEmp, @NomEmp, @PassEmp)
			
			
			-- 2. Creo Usuario de Logueo
			Set @variable = 'CREATE LOGIN [' + @UsuEmp + '] WITH PASSWORD = ' + QUOTENAME (@PassEmp,'''')
			Exec (@variable)
			
			-- 3. Creo Usuario de BD
			Set @variable = 'CREATE USER [' + @UsuEmp + '] FROM LOGIN [' + @UsuEmp + ']' 
			Exec (@variable)
			
			-- 4. Asigno rol 
			Set @variable = 'GRANT EXECUTE TO [' + @UsuEmp + ']'
			Exec (@variable)		
		Commit		
	End try
	
	Begin catch
		RollBack
		return -2
	End catch
End
Go



/*
Create Procedure EliminarUsuarioSql @nombre varchar(10) As
Begin
	Declare @VarSentencia varchar(200)
	Set @VarSentencia = 'Drop Login [' +  @nombre + ']'
	Exec (@VarSentencia)
	if (@@ERROR = 0)
		return 1
	else
		return -1
End
go
*/


----------------------------------------  DATOS DE PRUEBA -------------------------------------------
--Categorias 10
Insert Categoria (CodCat, NomCat) Values ('A11MU2', 'Anagesico Muscular')
Insert Categoria (CodCat, NomCat) Values ('A22GR9', 'Antigripal')
Insert Categoria (CodCat, NomCat) Values ('A03AL3', 'Antialérgico')
Insert Categoria (CodCat, NomCat) Values ('P08AL6', 'Presion Alta')
Insert Categoria (CodCat, NomCat) Values ('JP1T54', 'Jarabe Tos')
Insert Categoria (CodCat, NomCat) Values ('ANS444', 'Ansiedad')
Insert Categoria (CodCat, NomCat) Values ('CAL214', 'Calmante')
Insert Categoria (CodCat, NomCat) Values ('CA44B7', 'Jaqueca')
Insert Categoria (CodCat, NomCat) Values ('REL000', 'Relajante Muscular')
Insert Categoria (CodCat, NomCat) Values ('PR25T6', 'Proteinas')
Insert Categoria (CodCat, NomCat) Values ('AHI245', 'Articulos de Hijiene')
Insert Categoria (CodCat, NomCat) Values ('CRE531', 'Crema')
go
 
 --art 80
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ANGRI00001','Anti Gripal Diurno',400,'20300215','Sobre',100, 'A22GR9')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ANGRI00002','Anti Gripal Nocturno',350,'20290319','Sobre',100, 'A22GR9')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ANGRI00003','Baja Fiebre General',600,'20280617','Sobre',200, 'A22GR9')    
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('MUSCU11111', 'Calma Dolores Fuerte',650,'20280518','Blister',100, 'A11MU2')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('MUSCU11112', 'Sin Dolores Light',350,'20280518','Blister',200, 'A11MU2')	     
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ALERG22221', 'Alergia General',850,'20280318','Blister',20, 'A03AL3')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ALERG22222', 'Alergia Pediatrica',550,'20290712','Frasco',100, 'A03AL3')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ALERG22223', 'Alergia Leve',250,'20261212','Blister',10, 'A03AL3')  
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TOSIN22222', 'Para Tos',690,'20260919','Frasco',200, 'JP1T54')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TOSIN22225', 'Tos Aguda',590,'20270817','Frasco',150, 'JP1T54')
--10
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PRESI33331', 'BajaPresion Concentrado',690,'20281119','Unidad',5, 'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PRESI33332', 'BajaPresion General',990,'20260212','Blister',20, 'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PRESI33334', 'Baja Presion Leve',560,'20271019','Unidad',5, 'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PROTE44441', 'Complemento vitaminico',990,'20291012','Unidad',5, 'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PROTE44442', 'Proteinas polvo',690,'20301112','Sobre',150, 'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PROTE44443', 'Batido Proteico',590,'20261019','Frasco',200, 'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('SEPIL44444', 'Sepillo de dientes',50,'20281012','Unidad',3, 'AHI245')		     
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PERIF44444', 'perifar 400',250,'20281012','Sobre',10, 'A11MU2')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PERIF66666', 'perifar 600',500,'20281012','Sobre',10, 'A11MU2')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PERIF88888', 'perifar 800',990,'20281012','Sobre',10, 'A11MU2')
--20
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TRALE10101', 'Traler gotas',1000,'20281012','frasco ',100, 'CAL214')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('DEYTE10101', 'Detres capsulas',600,'20281012','Blister',20, 'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('LOVER10101', 'Louverina',2500,'20281012','Blister',15,'CA44B7')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TEOFI10101', 'teofilina 250 mg',350 ,'20281012','Blister ',15,'CA44B7')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('OROFG10101', 'Orofogol',500,'20281012','Frasco',20,'REL000')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('HISTA10101', 'Histaplen 5mg comprimidos',550,'20281012','Blister',20,'REL000')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('LORDE10101', 'Lordex comprimidos',400,'20281012','Blister',10 , 'REL000')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TIROX10101', 'Tirox comprimidos',300,'20281012','Sobre',20 ,'A03AL3')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('HISRA10101', 'Hisradin',350,'20281012','Sobre',15,'ANS444')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('KETOF10101', 'Ketofen 30mg', 200,'20281012','Unidad',3, 'A11MU2')
--30
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('FLEXO20202', 'Flexofenac',450,'20281012','Frasco',50, 'CRE531')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PREND20202', 'Prednicort 20 Mg',350,'20281012','Blister',20,'ANS444')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('AVEMU20202', 'Avemus íntimo', 600,'20281012','Frasco',100,'CRE531')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('COLOP20202', 'Colpoestriol',1500,'20281012','Frasco',150,'CRE531')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ANTRO20202', 'Antrofi',200,'20281012','Frasco', 300,'CRE531')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PROGE20202', 'Progelle 100 Mg',3000,'20281012','Blister',30,'A22GR9')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PROOO20202', 'Progelle 200 Mg',3300,'20281012','Blister',15,'A22GR9')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PREND20602', 'Prednisona Athena 5 Mg ',300,'20281012','Blister',20,'A03AL3')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TECUA20202', 'Tecuatro 25 Mcg',200,'20281012','Unidad',5, 'CAL214')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PRENN20202', 'Prednisolin 3 Mg',600,'20281012','Frasco',50,'A03AL3')
--40
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TECCU20202', 'Tecuatro 150 Mcg',650,'20281012','Sobre',10,'ANS444')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PREEN20202', 'Prednisolin 3 Mg ',600,'20281012','Frasco',50,'JP1T54')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TECCU20272', 'Tecuatro 150 Mcg ',400,'20281012','Sobre',15,'REL000')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('COROD20202', 'Corodex 20 Mg ',2200,'20281012','Blister',20,'REL000')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('LEVOT20202', 'Levotiroxina 100 Mcg',700,'20281012','Unidad',15,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('LEVVO20202', 'Levotiroxina 100 Mcg',1100,'20281012','Sobre',20,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('CORDE20202', 'Corodex 1 Mg',500,'20281012','Blister',30,'AHI245')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('COORO20202', 'Corodex 4 Mg',900,'20281012','Frasco',50,'AHI245')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('LIOTI20202', 'Liotironina Dispert',1300,'20281012','Sobre',30,'CA44B7')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('MENTI20202', 'Metidazol 5',760,'20281012','Frasco',100,'P08AL6')
--50
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('CORIP30303', 'Coripen 20',1000,'20281012','Unidad',10,'JP1T54')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ANDRO30303', 'Androgel 50 Mg',6000,'20281012','Sobre',30,'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TIRRO30303', 'Tirox 100 Mcg',400,'20281012','sobre',30,'CA44B7')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('HIDRO30303', 'Hidrocortisona',1500,'20281012','Frasco',50,'CRE531')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('CORRO30303', 'Corodex Solución 20 Ml',500,'20281012','Frasco',20,'JP1T54')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ACRYL30303', 'Acrylarm',1200,'20281012','Frasco',10,'CRE531')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('SOPHI30303', 'Sophixin Dx',1500,'20281012','Frasco',5,'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('HIPRO30303', 'Hiprocel Gotas',700,'20281012','Frasco',10,'A11MU2')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TALOF30303', 'Talof Gotas',1700,'20281012','Frasco',5,'A22GR9')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('SPLAS30303', 'Splash Tears',1100,'20281012','Frasco',15,'A03AL3')
--60
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('ISOPT30304', 'Isoptomax Suspensión',1000,'20281012','Unidad',5,'P08AL6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('GLUTA40404', 'Glautimol 0.50%',900,'20281012','Frasco',5,'JP1T54')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('EFATC40404', 'Efatracina',500,'20281012','Unidad',10,'ANS444')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('MOXOF40404', 'Moxof Solución',900,'20281012','Frasco',10,'ANS444')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('BEOFF40404', 'Beof ',1900,'20281012','Frasco',5,'CAL214')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('TIRRO40404', 'Tiof',1600,'20281012','Sobre',10,'CAL214')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('DIARI40404', 'Diaris',1800,'20281012','Blister',30,'CA44B7')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('KUARA40404', 'Kuara',1800,'20281012','Sobre',30,'REL000')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('BRINZ40404', 'Brinzoten',2200,'20281012','Unidad',10,'A03AL3')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('FLUME40404', 'Flumetol',1500,'20281012','Blister',250,'REL000')
--70
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('SOPHI40404', 'Sophixin',1800,'20281012','Sobre',25,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('LACRI40404', 'Lacrimax',800,'20281012','Blister',30,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('XEGEX40404', 'Xegrex ',2000,'20281012','Sobre',15,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('RENUP40404', 'Renu Plus',1900,'20281012','Unidad',20,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('MUROO40404', 'Muro 128',1300,'20281012','Frasco',20,'PR25T6')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('PREDO50505', 'Predol',1600,'20281012','Frasco',50, 'AHI245')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('KRYTO50505', 'Krytantek',2300,'20281012','Unidad',5,'AHI245')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('GAAPO50505', 'Gaap Ofteno',1900,'20281012','Unidad',8,'AHI245')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('GATIO50505', 'Gatidex',1700,'20281012','Sobre',10,'AHI245')
Insert Articulo (CodArt, NomArt, PrecioArt, FechaVtoArt, TipoPArt, TamArt, CodCat) Values('BELTO60505', 'Beltears',500,'20281012','Blister',20 ,'JP1T54')
--80
go

--Clientes 30
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4221999-4', 'Ignacio Martínez', '1234567890123456','099111111')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5220333-1', 'Mathias Techera', '6123456789234568', '098123456')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5990223-2', 'Lorenzo Lopez', '0012345678923456', '098123459')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4556737-6', 'Sabrina Caceres', '0001234567892345', '097123455')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('3460999-3', 'Jovanna Gonzalez', '0001234567892340', '095123454')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5013111-7', 'Alexis Sanchez', '0000234567892300', '091100000')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4980245-2', 'Marcelo Benitez', '1111234567892311', '098108888')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4998334-7', 'Laura Rodríguez','1222234588892311', '094444444')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4786890-3', 'Maria Machado','1255534588888311', '094466471')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4955545-2', 'Sebastian Benitez', '2159999582577311', '094999971')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4998919-0', 'Laura Morales',  '9159922008773117', '096699555')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4690890-3', 'Martin Machado', '8889922008555554', '096625632')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4226333-2', 'Gimena Lopez', '6549922008773113', '098646282')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5993878-1', 'Adolfo Martinez', '6549756987777787', '098656987')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4302737-6', 'Sabrina Gonzalez', '1258722008785689', '098648656')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4761098-7', 'Andrea Machado', '7856925693573411', '094569872')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4981212-1', 'Valentina Barboza', '7569853647177311', '091236875')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5004213-2', 'Washington Mendez', '6789645782658485', '098569325')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('2334567-6', 'Ana Morel', '6569874436598741', '098785695')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('3190871-2', 'Jonathan Fernandez', '6485693245845655', '094856982')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5121992-1', 'Ricardo Candiota', '1452346598745678', '098456987')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5851958-3', 'Noelia Olivera', '7896243658974365', '098125862')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4912744-2', 'Thiago Barrios', '7569845632145203', '097859648')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4008187-5', 'Martin Fonseca', '4785469325632589', '091236547')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4798122-6', 'German Rodriguez','6445678925896321', '097896542')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('1344654-2', 'Rodrigo Viera', '1235896452145876', '097896526')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4665987-0', 'Ruben Gimenez', '7456321458964541', '097896582')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('3887443-1', 'Marcos Araujo', '1452658947456322', '091478568')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5221987-2', 'Rodrigo Cabrera', '8965243145698745', '094569852')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5332561-3', 'Eduardo Cabrera', '1445896325896547', '097569856')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5192341-2', 'Romina Vaz', '4125896325742365', '097856325')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4710872-3', 'Mary Suarez', '2364598745632145', '091258965')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('2903180-0', 'Marcio Leites', '7896523654142587', '098145698')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('3469826-1', 'Carlos Perez', '7896523645555478', '091234564')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4512962-0', 'Facundo Moreira', '0000040236547896', '098252525')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('3980011-2', 'Silvia Moreira', '0241365478963214', '093256589')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4556012-3', 'Luana Espino', '1456589643214785', '098222222')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('2332767-7', 'Diego Lopez', '1452346541236985', '097858888')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4122765-1', 'Alan Dominguez', '1459487456963214', '098111111')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5018990-7', 'Sabrina Lima', '1234567489654123', '098555555')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4530190-3', 'Ana Machado', '1458963254896521', '091222555')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('3450908-1', 'Julio Noblia', '7894561423321456', '099999999')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4122334-1', 'Rodrigo Arevalo', '7778564666333255', '094558886')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5019172-4', 'Bruno Martinez', '7778889949666555', '094556332')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5430969-8', 'Robert Dominguez', '1234561444444555', '098000000')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('5112099-2', 'Melina Olivera', '9996665552224445', '094888555')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4915092-2', 'Micaela Morales','5554555666666222', '098666444')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4132956-7', 'Cristian Morel', '8456987423654185', '094565858')
Insert Cliente (CiCli, NomCli, NumTarjCli, TelCli) values('4762109-8', 'Gaston Soria', '0022343664455221', '094856999')				
go

-- Ventas 100
--10 con 1 linea y est armado:
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250101','Av. nida 1477',400,'4221999-4')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250102','Av. coso 1427',700,'5220333-1')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250103','Av. alla 1177',600,'5990223-2')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250103','Av. justo 1877',1300,'4556737-6')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250105','Av. napolitana 1427',350,'3460999-3')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250106','Av. fritas 1077',1700,'5013111-7')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250107','Av. franc 1877',550 ,'4980245-2')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250106','Av. sip 2477',500,'4762109-8')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250109','Av. donde 1474',1380 ,'4132956-7')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250110','Av. pro ahi 7477',560,'4915092-2')
--10 con 1 linea y est Enviado (hay que hacer la asignacion de Armado)
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250111','Av. ahi 1427',1380,'5019172-4')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250109','Av. atawalpa 1437',1920,'4122334-1')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250102','Av. yupanqui 1427',690,'3450908-1')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250104','Av. firu 1471',1180,'4530190-3')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250111','Av. cande 5477',990,'4556012-3')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250112','Av. tamontina 8477',1180,'5430969-8')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250113','Av. comarca 1377',990,'5851958-3')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250114','Av. filadelphia 1474',800,'4912744-2')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250115','Av. dublas 2477',350,'4998919-0')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250116','Av. san justo 147',1200,'5192341-2')
--10 con 1 linea y est entregado (hay que hacer los estados anteriores a entregado)
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250101','Av. nida 1477',400,'4221999-4')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250102','Av. coso 1427',700,'5220333-1')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250103','Av. alla 1177',600,'5990223-2')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250103','Av. justo 1877',1300,'4556737-6')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250105','Av. napolitana 1427',350,'3460999-3')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250106','Av. fritas 1077',1700,'5013111-7')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250107','Av. franc 1877',550 ,'4980245-2')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250106','Av. sip 2477',500,'4762109-8')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250109','Av. donde 1474',1380 ,'4132956-7')
insert Venta (FechaVent, DirEnvIoVent, TotalVent, CiCli) values ('20250110','Av. pro ahi 7477',560,'4915092-2')
INSERT INTO [Venta] (FechaVent,DirEnvIoVent,TotalVent,CiCli) --queda arreglar el TotalVent
VALUES
  ('20250909','6766 Malesuada. St.',2039,'4221999-4'),
  ('20250423','613-2820 Euismod Av.',745,'5220333-1'),
  ('20260108','517-2506 Aliquet. Avenue',2962,'5990223-2'),
  ('20240722','298-1572 Suspendisse Street',819,'4556737-6'),
  ('20240502','P.O. Box 151, 5498 Diam Av.',606,'3460999-3'),
  ('20250419','275-3219 Cursus Avenue',1868,'5013111-7'),
  ('20241209','2279 Sed, St.',1820,'4980245-2'),
  ('20240805','820-1008 Nunc St.',1182,'4998334-7'),
  ('20250816','6269 Sit Av.',664,'4786890-3'),
  ('20240124','Ap #381-4929 Nec Rd.',2093,'4955545-2'), --40
  ('20250916','Ap #526-814 Ultrices Street',2962,'4998919-0'),
  ('20250331','Ap #766-9666 Ut, Street',886,'4690890-3'),
  ('20240208','P.O. Box 989, 7031 Proin St.',1490,'4226333-2'),
  ('20240303','P.O. Box 773, 1859 Sed Rd.',1743,'5993878-1'),
  ('20250123','3692 Hendrerit. St.',1206,'4302737-6'),
  ('20240831','1903 Eget Street',445,'4761098-7'),
  ('20250109','P.O. Box 873, 1043 Imperdiet Ave',928,'4981212-1'),
  ('20250822','P.O. Box 741, 8260 Vel Ave',2087,'5004213-2'),
  ('20251011','Ap #141-9604 Cum Ave',1212,'2334567-6'),
  ('20240130','766-9230 Amet Avenue',1373,'3190871-2'),--50
  ('20241001','Ap #380-7887 Massa. St.',932,'5121992-1'),
  ('20251211','2581 Commodo St.',2042,'5851958-3'),
  ('20250330','Ap #199-6283 At Ave',1446,'4912744-2'),
  ('20250227','Ap #713-5149 Mollis Ave',2311,'4008187-5'),
  ('20250725','Ap #111-4075 Libero Av.',1651,'4798122-6'),
  ('20251026','887-3635 Nisi St.',1140,'1344654-2'),
  ('20250309','688-9528 Sapien Rd.',1240,'4665987-0'),
  ('20240830','Ap #298-8643 Orci. Avenue',2424,'3887443-1'),
  ('20240222','536 Id, St.',2278,'5221987-2'),
  ('20260112','476-7288 Venenatis Rd.',1926,'5332561-3'),--60
  ('20250413','4598 Accumsan Av.',557,'5192341-2'),
  ('20240127','Ap #392-6115 Mauris. Av.',1636,'4710872-3'),
  ('20241112','8883 Nonummy Rd.',2570,'2903180-0'),
  ('20250601','928-9003 Convallis, Ave',1185,'3469826-1'),
  ('20250313','8015 Gravida St.',2100,'4512962-0'),
  ('20240322','989-9080 In Ave',2895,'3980011-2'),
  ('20250923','427-2118 Nunc Road',1200,'4556012-3'),
  ('20250902','Ap #803-4581 Semper Ave',1426,'2332767-7'),
  ('20240402','619-8884 Non St.',470,'4122765-1'),
  ('20250411','Ap #839-271 Rhoncus. St.',1251,'5018990-7'),--70
  ('20240608','Ap #836-7876 Posuere Rd.',1100,'4530190-3'),
  ('20240725','Ap #551-8568 Nibh Rd.',1082,'3450908-1'),
  ('20250901','7947 Pede. St.',2303,'4122334-1'),
  ('20240220','P.O. Box 338, 4575 Venenatis Road',2879,'5019172-4'),
  ('20250713','103-8192 Faucibus Rd.',1969,'5430969-8'),
  ('20250131','2216 Erat, St.',2780,'5112099-2'),
  ('20240817','Ap #233-7707 Dapibus Avenue',1274,'4915092-2'),
  ('20240713','Ap #656-4458 Quam. Road',2555,'4132956-7'),
  ('20250720','Ap #886-9614 Id Ave',745,'4762109-8'),
  ('20240820','Ap #755-1856 Nonummy St.',1750,'4132956-7'),--80
  ('20250517','578-4651 Eu, St.',2421,'5004213-2'),
  ('20250407','Ap #687-1999 Ipsum. Ave',857,'2334567-6'),
  ('20250630','Ap #473-8015 A Rd.',636,'3190871-2'),
  ('20250311','423-7902 Fringilla, Avenue',1835,'5121992-1'),
  ('20250718','Ap #497-907 Arcu. Rd.',1600,'5851958-3'),
  ('20250314','1455 Nunc Av.',858,'4912744-2'),
  ('20250324','P.O. Box 380, 9179 Et, Ave',1095,'4008187-5'),
  ('20250120','2122 Arcu St.',1040,'4798122-6'),
  ('20250606','853-8791 Nec Ave',2662,'1344654-2'),
  ('20250126','4393 Tristique St.',2683,'4665987-0'),
  ('20250331','P.O. Box 731, 1160 Consectetuer St.',2917,'3887443-1'),
  ('20250423','625-2726 Lectus. St.',664,'5221987-2'),
  ('20250110','Ap #945-9854 Phasellus Street',2955,'5332561-3'),
  ('20250719','485-8900 Dui, Road',1796,'4710872-3'),
  ('20250620','P.O. Box 350, 1523 Et St.',1300,'2903180-0'),
  ('20250603','Ap #914-7634 Massa. Avenue',2376,'3469826-1'),
  ('20250304','Ap #305-8829 Ornare Avenue',1297,'4512962-0'),
  ('20250512','118-9774 Sed Rd.',1361,'3980011-2'),
  ('20250716','325-7064 Laoreet Av.',2763,'4556012-3'),
  ('20250504','697-4702 Ante Street',2968,'2332767-7');
go

--10 con 1 linea y est armado:
insert Linea (NumVent, CodArt, Cant) values (1,'ANGRI00001',1)
insert Linea (NumVent, CodArt, Cant) values (2,'ANGRI00002',1)
insert Linea (NumVent, CodArt, Cant) values (3,'ANGRI00003',1)
insert Linea (NumVent, CodArt, Cant) values (4,'MUSCU11111',1)
insert Linea (NumVent, CodArt, Cant) values (5,'MUSCU11112',1)
insert Linea (NumVent, CodArt, Cant) values (6,'ALERG22221',1)
insert Linea (NumVent, CodArt, Cant) values (7,'ALERG22222',1)
insert Linea (NumVent, CodArt, Cant) values (8,'ALERG22223',2)
insert Linea (NumVent, CodArt, Cant) values (9,'TOSIN22222',1)
insert Linea (NumVent, CodArt, Cant) values (10,'PRESI33331',1)
--10 con 1 linea y est Enviado (hay que hacer la asignacion de Armado)
insert Linea (NumVent, CodArt, Cant) values (11,'PRESI33331',1)
insert Linea (NumVent, CodArt, Cant) values (12,'PRESI33332',1)
insert Linea (NumVent, CodArt, Cant) values (13,'PRESI33334',1)
insert Linea (NumVent, CodArt, Cant) values (14,'PROTE44441',1)
insert Linea (NumVent, CodArt, Cant) values (15,'PROTE44442',1)
insert Linea (NumVent, CodArt, Cant) values (16,'PROTE44443',1)
insert Linea (NumVent, CodArt, Cant) values (17,'PROTE44443',1)
insert Linea (NumVent, CodArt, Cant) values (18,'ANGRI00001',1)
insert Linea (NumVent, CodArt, Cant) values (19,'ANGRI00002',1)
insert Linea (NumVent, CodArt, Cant) values (20,'ANGRI00003',1)
--10 con 1 linea y est entregado (hay que hacer antes la asignacion de Armado, Envio), 
INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (21,'ANGRI00001',1),
  (22,'ANGRI00002',1),
  (23,'ANGRI00003',1),
  (24,'MUSCU11111',1),
  (25,'MUSCU11112',1),
  (26,'ALERG22221',1),
  (27,'ALERG22222',1),
  (28,'ALERG22223',1),
  (29,'TOSIN22222',1),
  (30,'TOSIN22225',1);
--10 con 2 lineas y est armado, 
INSERT INTO [Linea] (NumVent,CodArt,Cant)  --aca da error
VALUES
  (31,'PRESI33331',1),
  (31,'ANGRI00001',1),
  (32,'PRESI33332',1),
  (32,'PRESI33331',1),
  (33,'PRESI33334',1),
  (33,'PROTE44441',1),
  (34,'PROTE44441',1),
  (34,'PRESI33334',1),
  (35,'PROTE44442',1),
  (35,'PROTE44441',1),
  (36,'PROTE44443',1),
  (36,'PRESI33334',1),
  (37,'SEPIL44444',1),
  (37,'PROTE44442',1),
  (38,'PERIF44444',1),
  (38,'PROTE44443',1),
  (39,'PERIF66666',1),
  (39,'SEPIL44444',1),
  (40,'PERIF88888',1),
  (40,'PERIF44444',1);
--10 con 2 lineas y est Enviado, 
INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (41,'PERIF44444',1),
  (41,'TRALE10101',1),
  (42,'PRESI33334',1),
  (42,'DEYTE10101',1),
  (43,'PRESI33334',1),
  (43,'LOVER10101',1),
  (44,'PROTE44441',1),
  (44,'TEOFI10101',1),
  (45,'PROTE44442',1),
  (45,'OROFG10101',1),
  (46,'PROTE44443',1),
  (46,'HISTA10101',1),
  (47,'SEPIL44444',1),
  (47,'LORDE10101',1),
  (48,'PERIF44444',1),
  (48,'TIROX10101',1),
  (49,'PERIF66666',1),
  (49,'HISRA10101',1),
  (50,'PERIF88888',1),
  (50,'KETOF10101',1);
--10 con 2 lineas y est Entregado,
INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (51,'FLEXO20202',1),
  (51,'TECCU20202',1),
  (52,'PREND20202',1),
  (52,'PREEN20202',1),
  (53,'AVEMU20202',1),
  (53,'TECCU20272',1),
  (54,'COLOP20202',1),
  (54,'LEVOT20202',1),
  (55,'ANTRO20202',1),
  (55,'LEVVO20202',1),
  (56,'PROGE20202',1),
  (56,'CORDE20202',1),
  (57,'PROOO20202',1),
  (57,'COORO20202',1),
  (58,'PREND20602',1),
  (58,'LIOTI20202',1),
  (59,'TECUA20202',1),
  (59,'MENTI20202',1),
  (60,'PRENN20202',1),
  (60,'CORDE20202',1);
--10 con 3 lineas y est armado
INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (61,'COORO20202',1),
  (62,'LIOTI20202',1),
  (63,'LIOTI20202',1),
  (64,'MENTI20202',1),
  (65,'CORIP30303',1),
  (66,'ANDRO30303',1),
  (67,'TIRRO30303',1),
  (68,'HIDRO30303',1),
  (69,'CORRO30303',1),
  (70,'ACRYL30303',1),
  (61,'SOPHI30303',1),
  (62,'HIPRO30303',1),
  (63,'TALOF30303',1),
  (64,'SPLAS30303',1),
  (65,'ISOPT30304',1),
  (66,'GLUTA40404',1),
  (67,'EFATC40404',1),
  (68,'MOXOF40404',1),
  (69,'BEOFF40404',1),
  (70,'TIRRO40404',1),
  (61,'DIARI40404',1),
  (62,'KUARA40404',1),
  (63,'BRINZ40404',1),
  (64,'FLUME40404',1),
  (65,'SOPHI40404',1),
  (66,'LACRI40404',1),
  (67,'XEGEX40404',1),
  (68,'RENUP40404',1),
  (69,'MUROO40404',1),
  (70,'PREDO50505',1);
 --10 con 3 lineas y est Enviado,
 INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (71,'ANGRI00001',1),
  (72,'ANGRI00002',1),
  (73,'ANGRI00003',1),
  (74,'MUSCU11111',1),
  (75,'MUSCU11112',1),
  (76,'ALERG22221',1),
  (77,'ALERG22223',1),
  (78,'TOSIN22222',1),
  (79,'TOSIN22225',1),
  (80,'PRESI33331',1),
  (71,'PRESI33334',1),
  (72,'PROTE44441',1),
  (73,'PROTE44442',1),
  (74,'PROTE44443',1),
  (75,'SEPIL44444',1),
  (76,'PERIF44444',1),
  (77,'PERIF66666',1),
  (78,'PERIF88888',1),
  (79,'TRALE10101',1),
  (80,'DEYTE10101',1),
  (71,'LOVER10101',1),
  (72,'OROFG10101',1),
  (73,'TEOFI10101',1),
  (74,'HISTA10101',1),
  (75,'LORDE10101',1),
  (76,'HISRA10101',1),
  (77,'TIROX10101',1),
  (78,'KETOF10101',1),
  (79,'FLEXO20202',1),
  (80,'PREND20202',1);
 --10 con 3 lineas y est entregado
 INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (81,'KRYTO50505',1),
  (82,'GAAPO50505',1),
  (83,'GATIO50505',1),
  (84,'BELTO60505',1),
  (85,'ANGRI00001',1),
  (86,'ANGRI00002',1),
  (87,'ANGRI00003',1),
  (88,'MUSCU11111',1),
  (89,'MUSCU11112',1),
  (90,'ALERG22221',1),
  (81,'ALERG22222',1),
  (82,'ALERG22223',1),
  (83,'TOSIN22222',1),
  (84,'TOSIN22225',1),
  (85,'PRESI33331',1),
  (86,'PRESI33332',1),
  (87,'PRESI33334',1),
  (88,'PROTE44441',1),
  (89,'PROTE44442',1),
  (90,'PROTE44443',1),
  (81,'SEPIL44444',1),
  (82,'PERIF44444',1),
  (83,'PERIF66666',1),
  (84,'PERIF88888',1),
  (85,'TRALE10101',1),
  (86,'DEYTE10101',1),
  (87,'LOVER10101',1),
  (88,'TEOFI10101',1),
  (89,'OROFG10101',1),
  (90,'HISTA10101',1);
--10 con 4 lineas y est armado
INSERT INTO [Linea] (NumVent,CodArt,Cant)
VALUES
  (91,'PREDO50505',1),
  (92,'COLOP20202',1),
  (93,'ANTRO20202',1),
  (94,'PROGE20202',1),
  (95,'PROOO20202',1),
  (96,'PREND20602',1),
  (97,'TECUA20202',1),
  (98,'PRENN20202',1),
  (99,'TECCU20202',1),
  (100,'PREEN20202',1),
  (91,'TECCU20272',1),
  (92,'COROD20202',1),
  (93,'LEVOT20202',1),
  (94,'LEVVO20202',1),
  (95,'CORDE20202',1),
  (96,'COORO20202',1),
  (97,'LIOTI20202',1),
  (98,'MENTI20202',1),
  (99,'CORIP30303',1),
  (100,'ANDRO30303',1),
  (91,'TIRRO30303',1),
  (92,'HIDRO30303',1),
  (93,'CORRO30303',1),
  (94,'ACRYL30303',1),
  (95,'SOPHI30303',1),
  (96,'HIPRO30303',1),
  (97,'TALOF30303',1),
  (98,'SPLAS30303',1),
  (99,'ISOPT30304',1),
  (100,'GLUTA40404',1),
  (91,'EFATC40404',1),
  (92,'MOXOF40404',1),
  (93,'BEOFF40404',1),
  (94,'TIRRO40404',1),
  (95,'DIARI40404',1),
  (96,'KUARA40404',1),
  (97,'BRINZ40404',1),
  (98,'FLUME40404',1),
  (99,'SOPHI40404',1),
  (100,'LACRI40404',1),
  (91,'XEGEX40404',1),
  (92,'RENUP40404',1),
  (93,'MUROO40404',1),
  (94,'PREDO50505',1),
  (95,'KRYTO50505',1),
  (96,'GAAPO50505',1),
  (97,'GATIO50505',1),
  (98,'BELTO60505',1),
  (99,'SPLAS30303',1),
  (100,'XEGEX40404',1);
go

insert Estado (NumEst, NomEst) values (1, 'Armado')
insert Estado (NumEst, NomEst) values (2, 'Envio')
insert Estado (NumEst, NomEst) values (3, 'Entregado')
insert Estado (NumEst, NomEst) values (4, 'Devuelto')
go

--10 con 1 linea y est armado:
insert Asignacion (NumVent,NumEst,FyHEst) values (1,1,'20250101')
insert Asignacion (NumVent,NumEst,FyHEst) values (2,2,'20250102')
insert Asignacion (NumVent,NumEst,FyHEst) values (3,3, '20250103')
insert Asignacion (NumVent,NumEst,FyHEst) values (4,4, '20250104')
insert Asignacion (NumVent,NumEst,FyHEst) values (5,1, '20250105')
insert Asignacion (NumVent,NumEst,FyHEst) values (6,2, '20250106')
insert Asignacion (NumVent,NumEst,FyHEst) values (7,3, '20250117')
insert Asignacion (NumVent,NumEst,FyHEst) values (8,4, '20250108')
insert Asignacion (NumVent,NumEst,FyHEst) values (9,1, '20250109')
insert Asignacion (NumVent,NumEst,FyHEst) values (10,2, '20250110')
--10 con 1 linea y est Envio (hay que hacer la asignacion de Armado)
--Armado
insert Asignacion (NumVent,NumEst,FyHEst) values (11,1, '20250111')
insert Asignacion (NumVent,NumEst,FyHEst) values (12,1, '20250112')
insert Asignacion (NumVent,NumEst,FyHEst) values (13,1, '20250113')
insert Asignacion (NumVent,NumEst,FyHEst) values (14,1, '20250114')
insert Asignacion (NumVent,NumEst,FyHEst) values (15,1, '20250115')
insert Asignacion (NumVent,NumEst,FyHEst) values (16,1, '20250116')
insert Asignacion (NumVent,NumEst,FyHEst) values (17,1, '20241216')
insert Asignacion (NumVent,NumEst,FyHEst) values (18,1, '20241217')
insert Asignacion (NumVent,NumEst,FyHEst) values (19,1, '20241218')
insert Asignacion (NumVent,NumEst,FyHEst) values (20,1, '20241229')
--envio
insert Asignacion (NumVent,NumEst,FyHEst) values (11,2, '20250111')
insert Asignacion (NumVent,NumEst,FyHEst) values (12,2, '20250112')
insert Asignacion (NumVent,NumEst,FyHEst) values (13,2, '20250113')
insert Asignacion (NumVent,NumEst,FyHEst) values (14,2, '20250114')
insert Asignacion (NumVent,NumEst,FyHEst) values (15,2, '20250115')
insert Asignacion (NumVent,NumEst,FyHEst) values (16,2, '20250116')
insert Asignacion (NumVent,NumEst,FyHEst) values (17,2, '20241216')
insert Asignacion (NumVent,NumEst,FyHEst) values (18,2, '20241217')
insert Asignacion (NumVent,NumEst,FyHEst) values (19,2, '20241218')
insert Asignacion (NumVent,NumEst,FyHEst) values (20,2, '20241229')

-- 10 con 1 linea y est entregado
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--armado
  (21,1,'20260315'),
  (22,1,'20250105'),
  (23,1,'20250520'),
  (24,1,'20240108'),
  (25,1,'20240306'),
  (26,1,'20240106'),
  (27,1,'20240226'),
  (28,1,'20240108'),
  (29,1,'20241013'),
  (30,1,'20241118'),
--envio
  (21,2,'20260315'),
  (22,2,'20250105'),
  (23,2,'20250520'),
  (24,2,'20240108'),
  (25,2,'20240306'),
  (26,2,'20240106'),
  (27,2,'20240226'),
  (28,2,'20240108'),
  (29,2,'20241013'),
  (30,2,'20241118'),
  --entregado 3
  (21,3,'20260315'),
  (22,3,'20250105'),
  (23,3,'20250520'),
  (24,3,'20240108'),
  (25,3,'20240306'),
  (26,3,'20240106'),
  (27,3,'20240226'),
  (28,3,'20240108'),
  (29,3,'20241013'),
  (30,3,'20241118');
--10 con 2 lineas y est armado
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--armado
  (31,1,'20250818'),
  (32,1,'20250112'),
  (33,1,'20250416'),
  (34,1,'20250310'),
  (35,1,'20250930'),
  (36,1,'20251116'),
  (37,1,'20251223'),
  (38,1,'20250406'),
  (39,1,'20251019'),
  (40,1,'20251025');
--10 con 2 lineas y est Envio
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--Armado
  (41,1,'20250414'),
  (42,1,'20250428'),
  (43,1,'20250706'),
  (44,1,'20250616'),
  (45,1,'20250407'),
  (46,1,'20250718'),
  (47,1,'20250822'),
  (48,1,'20251125'),
  (49,1,'20251127'),
  (50,1,'20250317'),
--Envio
  (41,2,'20250414'),
  (42,2,'20250428'),
  (43,2,'20250706'),
  (44,2,'20250616'),
  (45,2,'20250407'),
  (46,2,'20250718'),
  (47,2,'20250822'),
  (48,2,'20251125'),
  (49,2,'20251127'),
  (50,2,'20250315');
--10 con 2 lineas y est Entregado, 
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--armado
  (51,1,'20250423'),
  (52,1,'20250307'),
  (53,1,'20250430'),
  (54,1,'20250203'),
  (55,1,'20250320'),
  (56,1,'20250111'),
  (57,1,'20250215'),
  (58,1,'20250304'),
  (59,1,'20250116'),
  (60,1,'20250319'),
 --Envio
  (51,2,'20250423'),
  (52,2,'20250307'),
  (53,2,'20250430'),
  (54,2,'20250203'),
  (55,2,'20250320'),
  (56,2,'20250111'),
  (57,2,'20250215'),
  (58,2,'20250304'),
  (59,2,'20250116'),
  (60,2,'20250319'),
  --Entregado
  (51,3,'20250423'),
  (52,3,'20250307'),
  (53,3,'20250430'),
  (54,3,'20250203'),
  (55,3,'20250320'),
  (56,3,'20250111'),
  (57,3,'20250215'),
  (58,3,'20250304'),
  (59,3,'20250116'),
  (60,3,'20250319');
--10 con 3 lineas y est armado,
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
  (61,1,'20250325'),
  (62,1,'20250326'),
  (63,1,'20250704'),
  (64,1,'20250118'),
  (65,1,'20251225'),
  (66,1,'20250218'),
  (67,1,'20250424'),
  (68,1,'20250422'),
  (69,1,'20250502'),
  (70,1,'20250422');
--10 con 3 lineas y est Enviado, 
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--Armado
  (71,1,'20250608'),
  (72,1,'20250415'),
  (73,1,'20250810'),
  (74,1,'20250214'),
  (75,1,'20250215'),
  (76,1,'20250323'),
  (77,1,'20250419'),
  (78,1,'20250208'),
  (79,1,'20250217'),
  (80,1,'20250321'),
--Envio
  (71,2,'20251008'),
  (72,2,'20250415'),
  (73,2,'20250810'),
  (74,2,'20250314'),
  (75,2,'20250215'),
  (76,2,'20250823'),
  (77,2,'20250919'),
  (78,2,'20250708'),
  (79,2,'20250717'),
  (80,2,'20250621');
  --10 con 3 lineas y est entregado
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--Armado
  (81,1,'20250412'),
  (82,1,'20250719'),
  (83,1,'20250307'),
  (84,1,'20250718'),
  (85,1,'20251111'),
  (86,1,'20250912'),
  (87,1,'20250311'),
  (88,1,'20250529'),
  (89,1,'20250503'),
  (90,1,'20251204'),
--envio
  (81,2,'20250412'),
  (82,2,'20250719'),
  (83,2,'20251007'),
  (84,2,'20250718'),
  (85,2,'20251211'),
  (86,2,'20250912'),
  (87,2,'20250311'),
  (88,2,'20250529'),
  (89,2,'20250503'),
  (90,2,'20251204'),
--Entregado
  (81,3,'20250412'),
  (82,3,'20250719'),
  (83,3,'20250307'),
  (84,3,'20250718'),
  (85,3,'20251211'),
  (86,3,'20250912'),
  (87,3,'20250311'),
  (88,3,'20250529'),
  (89,3,'20250503'),
  (90,3,'20251204');
--10 con 4 lineas y est armado
INSERT INTO [Asignacion] (NumVent,NumEst,FyHEst)
VALUES
--Armado
  (91,1,'20251209'),
  (92,1,'20250515'),
  (93,1,'20250225'),
  (94,1,'20250102'),
  (95,1,'20250605'),
  (96,1,'20251125'),
  (97,1,'20250225'),
  (98,1,'20250409'),
  (99,1,'20251125'),
  (100,1,'20251205');
go



Exec NuevoUsuarioEmpleado 'pepito01', 'Jose Alvarez', 'ABC121'
go
Exec NuevoUsuarioEmpleado 'pepito02', 'Jose Perez','ABC122'
go
Exec NuevoUsuarioEmpleado 'raul03', 'Raul Rodriguez','ABC123'
go
Exec NuevoUsuarioEmpleado 'sivana04', 'Silvana Zalazar','ABC124'
go
Exec NuevoUsuarioEmpleado 'Marce05', 'Marcelina Martinez','ABC125'
go
Exec NuevoUsuarioEmpleado 'Caro06', 'Carolina Rostand','ABC126'
go
Exec NuevoUsuarioEmpleado 'Pau07', 'Paulina Nieva','ABC127'
go
Exec NuevoUsuarioEmpleado 'Maurito08', 'Mauro Pascale','ABC128'
go
Exec NuevoUsuarioEmpleado 'Marti09', 'Martina Vazquez','ABC129'
go
Exec NuevoUsuarioEmpleado 'Jose10', 'Josefina Monsecchi','ABC110'
go
Exec NuevoUsuarioEmpleado 'Sele22', 'Selena Gonzalez','ABC069'
go





/*
Exec EliminarUsuarioSql 'pepito01'
go
Exec EliminarUsuarioSql 'pepito02'
go
Exec EliminarUsuarioSql 'raul03'
go
Exec EliminarUsuarioSql 'sivana04'
go
Exec EliminarUsuarioSql 'Marce05'
go
Exec EliminarUsuarioSql 'Caro06'
go
Exec EliminarUsuarioSql 'Pau07'
go
Exec EliminarUsuarioSql 'Maurito08'
go
Exec EliminarUsuarioSql 'Marti09'
go
Exec EliminarUsuarioSql 'Jose10'
go
Exec EliminarUsuarioSql 'Sele22'
go*/

select * from Empleado

select *
from Cliente