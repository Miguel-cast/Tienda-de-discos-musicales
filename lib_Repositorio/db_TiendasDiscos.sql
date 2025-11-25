CREATE DATABASE db_TiendaDiscos;
GO

USE db_TiendaDiscos;
GO

CREATE TABLE [Clientes](
    [ClienteId] INT IDENTITY (1,1) PRIMARY KEY,
    [Nombre] NVARCHAR(50) NOT NULL,
    [Apellido] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100),
    [Telefono] NVARCHAR(20),
    [Direccion] NVARCHAR(200),
    [Ciudad] NVARCHAR(50),
    [Pais] NVARCHAR(50)
);
GO

CREATE TABLE [Empleados](
    [EmpleadoId] INT IDENTITY (1,1) PRIMARY KEY,
    [Nombre] NVARCHAR(50) NOT NULL,
    [Apellido] NVARCHAR(50) NOT NULL,
    [Cargo] NVARCHAR(50),
    [Email] NVARCHAR(100),
    [Telefono] NVARCHAR(20)
);
GO

CREATE TABLE [Proveedores](
	[ProveedoresId] INT IDENTITY (1,1) PRIMARY KEY,
	[NombreEmpresa] NVARCHAR (100) NOT NULL,
	[Contacto] NVARCHAR (50) NOT NULL,
	[Telefono] NVARCHAR (20) NOT NULL,
	[Direccion] NVARCHAR (200),
);

GO

CREATE TABLE [Artistas](
    [ArtistaId] INT IDENTITY (1,1) PRIMARY KEY,
    [NombreArtista] NVARCHAR(100) NOT NULL,
    [Nacionalidad] NVARCHAR(50)
);
GO

CREATE TABLE [Generos](
	[GenerosId] INT IDENTITY (1,1) PRIMARY KEY,
	[NombreGenero] NVARCHAR (50) NOT NULL,
	[Descripcion]  NVARCHAR (500) 
);

GO

CREATE TABLE [Discos] (
    [DiscoId] INT IDENTITY(1,1) PRIMARY KEY,
    [Titulo] NVARCHAR(50) NOT NULL,
    [AñoLanzamiento] INT,
    [Precio] DECIMAL(10,2) NOT NULL,
    [ArtistaId] INT CONSTRAINT FK_Discos_Artistas 
        FOREIGN KEY REFERENCES [Artistas]([ArtistaId]),
    [GenerosId] INT CONSTRAINT FK_Discos_Generos 
        FOREIGN KEY REFERENCES [Generos]([GenerosId]),
    [ProveedoresId] INT CONSTRAINT FK_Discos_Proveedores 
        FOREIGN KEY REFERENCES [Proveedores]([ProveedoresId])
);
GO

CREATE TABLE [Canciones](
	[CancionId] INT IDENTITY(1,1) PRIMARY KEY,
	[Titulo] NVARCHAR(100) NOT NULL,
	[Duracion] TIME NOT NULL,
	[DiscoId] INT NOT NULL,
	CONSTRAINT FK_Canciones_Discos 
		FOREIGN KEY ([DiscoId]) REFERENCES [Discos]([DiscoId])
);
GO

CREATE TABLE [Pedidos] (
    [PedidoID] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaPedido] DATE NOT NULL,
    [Estado] NVARCHAR(50) NOT NULL,
    [ClienteID] INT NOT NULL,
    [EmpleadoID] INT NOT NULL,
    CONSTRAINT FK_Pedidos_Clientes
        FOREIGN KEY ([ClienteID]) REFERENCES [Clientes]([ClienteID]),
    CONSTRAINT FK_Pedidos_Empleados
        FOREIGN KEY ([EmpleadoID]) REFERENCES [Empleados]([EmpleadoID])
);
GO

CREATE TABLE [DetallePedidos](
	[DetallesId] INT IDENTITY(1,1) PRIMARY KEY,
	[Cantidad] INT NOT NULL,
	[PrecioUnitario] DECIMAL(10,2) NOT NULL,
	[PedidoId] INT NOT NULL,
	CONSTRAINT FK_DetallePedidos_Pedidos 
		FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos]([PedidoID])
);
GO

CREATE TABLE [Facturas] (
    [FacturaID] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaFactura] DATE NOT NULL,
    [Total] DECIMAL(10,2) NOT NULL,
    [PedidoID] INT NOT NULL,
    CONSTRAINT FK_Facturas_Pedidos
        FOREIGN KEY ([PedidoID]) REFERENCES [Pedidos]([PedidoID])
);
GO

CREATE TABLE [Pagos] (
    [PagoID] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaPago] DATE NOT NULL,
    [Monto] DECIMAL(10,2) NOT NULL,
    [MetodoPago] NVARCHAR(50) NOT NULL,
    [FacturaID] INT NOT NULL,
    CONSTRAINT FK_Pagos_Facturas
        FOREIGN KEY ([FacturaID]) REFERENCES [Facturas]([FacturaID])
);
GO

CREATE TABLE [Envios] (
    [EnvioID] INT IDENTITY(1,1) PRIMARY KEY,
    [DireccionEntrega] NVARCHAR(200) NOT NULL,
    [CiudadEntrega] NVARCHAR(100) NOT NULL,
    [PaisEntrega] NVARCHAR(100) NOT NULL,
    [FechaEnvio] DATE NOT NULL,
    [PedidoID] INT NOT NULL,
    CONSTRAINT FK_Envios_Pedidos
        FOREIGN KEY ([PedidoID]) REFERENCES [Pedidos]([PedidoID])
);
GO
CREATE TABLE [Roles] (
	[RolId] INT NOT NULL IDENTITY (1,1) PRIMARY KEY,
	[Nombre] NVARCHAR (50),
	[Descripcion] NVARCHAR (70)
);
GO
GO

CREATE TABLE [Usuarios](
    [UsuarioId] INT IDENTITY (1,1) PRIMARY KEY,
    [Email] NVARCHAR(50) NOT NULL,
    [Contraseña] NVARCHAR(200) NOT NULL,
    [RolID] INT NOT NULL,
    [EmpleadoId] INT NOT NULL,
    CONSTRAINT FK_Usuarios_Empleados 
        FOREIGN KEY ([EmpleadoId]) REFERENCES [Empleados]([EmpleadoId]),
    CONSTRAINT FK_Usuarios_Roles 
            FOREIGN KEY ([RolID]) REFERENCES [Roles]([RolId])
);
GO

CREATE TABLE [InventarioMovimientos](
    [MovimientoId] INT IDENTITY (1,1) PRIMARY KEY,
    [FechaMovimiento] DATE NOT NULL,
    [TipoMovimiento] NVARCHAR(20) NOT NULL, -- Entrada / Salida
    [Cantidad] INT NOT NULL,
    [DiscoId] INT NOT NULL,
    [EmpleadoId] INT NOT NULL,
    CONSTRAINT FK_InventarioMovimientos_Discos 
        FOREIGN KEY ([DiscoId]) REFERENCES [Discos]([DiscoId]),
    CONSTRAINT FK_InventarioMovimientos_Empleados 
        FOREIGN KEY ([EmpleadoId]) REFERENCES [Empleados]([EmpleadoId])
);
GO

CREATE TABLE [ReseñasClientes] (
    [ReseñaID] INT IDENTITY(1,1) PRIMARY KEY,
    [Comentario] NVARCHAR(500) NOT NULL,
    [Calificacion] INT CHECK ([Calificacion] BETWEEN 1 AND 5),
    [Fecha] DATE NOT NULL,
    [ClienteId] INT NOT NULL,
    [DiscoId] INT NOT NULL,
    CONSTRAINT FK_ReseñasClientes_Clientes
        FOREIGN KEY ([ClienteId]) REFERENCES [Clientes]([ClienteId]),
    CONSTRAINT FK_ReseñasClientes_Discos
        FOREIGN KEY ([DiscoId]) REFERENCES [Discos]([DiscoId])
);
GO



CREATE TABLE [Auditorias](
    [AuditoriaId] INT IDENTITY(1,1) PRIMARY KEY,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE(),
    [Usuario] NVARCHAR(100) NOT NULL,
    [Accion] NVARCHAR(100) NOT NULL, -- INSERT, UPDATE, DELETE, LOGIN, etc.
    [Tabla] NVARCHAR(100), -- Tabla     afectada
    [Descripcion] NVARCHAR(100), -- Tabla     afectada
    );
GO



CREATE TABLE [dbo].[ImagenPrincipal]
(

    [ImagenID] INT IDENTITY(1,1) PRIMARY KEY, 
    
    [NombreArchivo] NVARCHAR(255) NOT NULL,

    [TipoMime] NVARCHAR(50) NOT NULL,
    
    [ContenidoBytes] VARBINARY(MAX) NOT NULL,
    
    [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
----- Inserts -----
INSERT INTO [Clientes] (Nombre, Apellido, Email, Telefono, Direccion, Ciudad, Pais) VALUES
('Carlos', 'Ramírez', 'carlos.ramirez@mail.com', '3001234567', 'Cra 10 #20-30', 'Bogotá', 'Colombia'),
('Ana', 'García', 'ana.garcia@mail.com', '3157654321', 'Calle 45 #12-05', 'Medellín', 'Colombia'),
('Luis', 'Pérez', 'luis.perez@mail.com', '3109988776', 'Av. Siempre Viva 123', 'Cali', 'Colombia'),
('María', 'Lopez', 'maria.lopez@mail.com', '3001122334', 'Calle 8 #5-44', 'Barranquilla', 'Colombia'),
('Jorge', 'Martínez', 'jorge.martinez@mail.com', '3015566778', 'Carrera 15 #45-67', 'Cartagena', 'Colombia');
GO

INSERT INTO [Empleados] (Nombre, Apellido, Cargo, Email, Telefono) VALUES
('Andrés', 'Gómez', 'Vendedor', 'andres.gomez@tiendadiscos.com', '3201112233'),
('Claudia', 'Ruiz', 'Cajera', 'claudia.ruiz@tiendadiscos.com', '3202223344'),
('Felipe', 'Torres', 'Gerente', 'felipe.torres@tiendadiscos.com', '3203334455'),
('Laura', 'Mendoza', 'Bodeguera', 'laura.mendoza@tiendadiscos.com', '3204445566'),
('Santiago', 'Morales', 'Soporte TI', 'santiago.morales@tiendadiscos.com', '3205556677'),
('Santiagopro', 'Morales', 'Soporte TI', 'santiagopro.morales@tiendadiscos.com', '3205556677');
GO

INSERT INTO [Proveedores] (NombreEmpresa, Contacto, Telefono, Direccion) VALUES
('Sony Music Colombia', 'María Fernández', '3101234567', 'Zona Rosa, Bogotá'),
('Universal Music Group', 'Carlos Mendoza', '3157891234', 'El Poblado, Medellín'),
('Warner Music Latina', 'Ana Rodríguez', '3209876543', 'Zona T, Bogotá'),
('Codiscos S.A.', 'Felipe Vargas', '3186547890', 'Centro, Medellín'),
('Rimas Entertainment', 'Diego Santos', '3198765432', 'Laureles, Medellín'),
('Flow Music', 'Camila Torres', '3167894521', 'Norte, Barranquilla');
GO

INSERT INTO [Artistas] (NombreArtista, Nacionalidad) VALUES
('The Beatles', 'Reino Unido'),
('Shakira', 'Colombia'),
('Adele', 'Reino Unido'),
('Michael Jackson', 'Estados Unidos'),
('Hades66', 'Colombia');
GO

INSERT INTO [Generos] (NombreGenero, Descripcion) VALUES
('Reggaeton', 'Género musical urbano originario de Puerto Rico y Colombia'),
('Pop Latino', 'Música popular en español con influencias modernas'),
('Trap Latino', 'Subgénero del hip hop con influencias latinas'),
('R&B Contemporáneo', 'Rhythm and Blues moderno'),
('Indie Pop', 'Pop independiente con sonidos alternativos'),
('Hip Hop', 'Género urbano estadounidense');
GO

INSERT INTO [Discos] (Titulo, AñoLanzamiento, Precio, ArtistaId, GenerosId, ProveedoresId) VALUES
('Abbey Road', 2019, 55000.00, 1, 4, 2),           
('El Dorado', 2017, 48000.00, 2, 2, 1),         
('30', 2021, 52000.00, 3, 5, 2),                 
('Trap Cake Vol. 2', 2023, 42000.00, 5, 3, 5),   
('Las Que No Iban a Salir', 2020, 60000.00, 5, 1, 4); 
GO

INSERT INTO [Canciones] (Titulo, Duracion, DiscoID) VALUES

('Come Together', '00:04:19', 1),
('Something', '00:03:03', 1),
('Maxwell''s Silver Hammer', '00:03:27', 1),
('Oh! Darling', '00:03:26', 1),
('Octopus''s Garden', '00:02:51', 1),
('Here Comes the Sun', '00:03:05', 1),

('Me Enamoré', '00:03:27', 2),
('Nada', '00:03:23', 2),
('Chantaje', '00:03:16', 2),
('When a Woman', '00:03:07', 2),
('Amarillo', '00:03:40', 2),
('Perro Fiel', '00:03:25', 2),

('Strangers By Nature', '00:03:03', 2),
('Easy On Me', '00:03:44', 2),
('My Little Love', '00:06:29', 2),
('Cry Your Heart Out', '00:04:00', 2),
('Oh My God', '00:03:45', 2),
('Can I Get It', '00:03:30', 2),

('Billie Jean', '00:04:54', 3),
('Beat It', '00:04:18', 3),
('Thriller', '00:05:57', 3),
('Human Nature', '00:04:06', 3),
('P.Y.T. Pretty Young Thing', '00:03:58', 3),
('The Way You Make Me Feel', '00:04:58', 3),

('Creepy', '00:02:45', 4),
('Diabla', '00:03:12', 4),
('Bzrp Music Sessions 50', '00:02:58', 4),
('420', '00:03:25', 4),
('No Soy Yo', '00:02:52', 4),
('Lokotron', '00:03:15', 4),

('Whiskey', '00:03:08', 5),
('Mi Diablo', '00:02:47', 5),
('Hijo de la Noche', '00:03:22', 5),
('Pacto', '00:03:05', 5),
('Lukeando', '00:02:38', 5),
('Eclipse', '00:03:45', 5);
GO

INSERT INTO [Pedidos] (FechaPedido, Estado, ClienteID, EmpleadoID)
VALUES
('2025-09-01', 'Pendiente', 1, 2),
('2025-09-02', 'Completado', 2, 1),
('2025-09-03', 'En proceso', 3, 2),
('2025-09-04', 'Cancelado', 4, 3),
('2025-09-05', 'Pendiente', 5, 1);
GO

INSERT INTO [DetallePedidos] (Cantidad, PrecioUnitario, PedidoId) VALUES
(2, 65000.00, 1),
(1, 55000.00, 2), 
(1, 45000.00, 3), 
(1, 48000.00, 4), 
(2, 47000.00, 5); 
GO

INSERT INTO [Facturas] (FechaFactura, Total, PedidoID)
VALUES
('2025-09-01', 150000.00, 1),
('2025-09-02', 250000.00, 2),
('2025-09-03', 80000.00, 3),
('2025-09-04', 320000.00, 4),
('2025-09-05', 50000.00, 5);
GO

INSERT INTO [Pagos] (FechaPago, Monto, MetodoPago, FacturaID)
VALUES
('2025-09-01', 150000.00, 'Tarjeta de Crédito', 1),
('2025-09-02', 100000.00, 'Efectivo', 2),
('2025-09-02', 150000.00, 'Transferencia', 2),
('2025-09-03', 80000.00, 'Nequi', 3),
('2025-09-04', 320000.00, 'PSE', 4);
GO

INSERT INTO [Envios] (DireccionEntrega, CiudadEntrega, PaisEntrega, FechaEnvio, PedidoID)
VALUES
('Cra 45 #23-11', 'Medellín', 'Colombia', '2025-09-02', 1),
('Calle 10 #55-33', 'Bogotá', 'Colombia', '2025-09-03', 2),
('Av. Principal 123', 'Cali', 'Colombia', '2025-09-04', 3),
('Cl 80 #45-21', 'Barranquilla', 'Colombia', '2025-09-05', 4),
('Cl 20 #10-05', 'Cartagena', 'Colombia', '2025-09-06', 5);
GO

INSERT INTO [Roles] (Nombre, Descripcion) VALUES
('Administrador', 'Dueño de la tienda'),
('Cliente', 'Comprador'),
('Vendedor', 'Empleado');

GO

INSERT INTO [Usuarios] (Email, Contraseña, EmpleadoId, RolID) VALUES
('andresg@hotmail.com', 'hash123',  1,3),
('claudiar@gmail.com', 'hash234',  2,3),
('felipet@gmail.com', 'hash345',  3,2),
('lauram@gmail.com', 'hash456',  4,2),
('santiagom@gmail.com', 'hash567', 5,2),
('admin', 'admin', 6, 1);
GO

INSERT INTO [InventarioMovimientos] (FechaMovimiento, TipoMovimiento, Cantidad, DiscoId, EmpleadoId) VALUES
('2025-09-01', 'Entrada', 20, 1, 1),
('2025-09-02', 'Salida', 5, 1, 2),
('2025-09-03', 'Entrada', 15, 2, 3),
('2025-09-04', 'Salida', 2, 2, 4),
('2025-09-05', 'Entrada', 30, 3, 5);
GO

INSERT INTO [ReseñasClientes] (Comentario, Calificacion, Fecha, ClienteId, DiscoId)
VALUES
('Excelente disco, sonido impecable.', 5, '2025-09-05', 1, 1),
('Me gustó, pero esperaba más canciones.', 3, '2025-09-06', 2, 2),
('Buen precio y entrega rápida.', 4, '2025-09-07', 3, 3),
('El empaque llegó dañado, pero la música es buena.', 2, '2025-09-08', 4, 4),
('El mejor álbum del año, recomendado.', 5, '2025-09-09', 5, 5);
GO

INSERT INTO ImagenPrincipal (NombreArchivo, TipoMime, ContenidoBytes, FechaCreacion)
SELECT 
    'logo_principal.png', 
    'image/png',          
    BulkColumn,         
    GETDATE()
FROM OPENROWSET(BULK 'C:\Users\abela\Downloads\ImagenPrincipal.png', SINGLE_BLOB) AS x;
GO


SELECT * FROM ImagenPrincipal;
GO