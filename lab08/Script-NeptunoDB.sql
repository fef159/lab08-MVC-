USE NeptunoDB;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Producto_Listar
    @Busqueda NVARCHAR(120) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, p.CategoriaID,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia,
           p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado,
           pr.CompaniaNombre AS NombreProveedor, c.NombreCategoria
    FROM dbo.Productos AS p
    LEFT JOIN dbo.Proveedores AS pr ON pr.ProveedorID = p.ProveedorID
    LEFT JOIN dbo.Categorias AS c ON c.CategoriaID = p.CategoriaID
    WHERE p.Activo = 1
      AND (NULLIF(LTRIM(RTRIM(@Busqueda)), N'') IS NULL
           OR p.NombreProducto LIKE N'%' + LTRIM(RTRIM(@Busqueda)) + N'%')
    ORDER BY p.NombreProducto;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Producto_ObtenerPorId
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, p.CategoriaID,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia,
           p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado,
           pr.CompaniaNombre AS NombreProveedor, c.NombreCategoria
    FROM dbo.Productos AS p
    LEFT JOIN dbo.Proveedores AS pr ON pr.ProveedorID = p.ProveedorID
    LEFT JOIN dbo.Categorias AS c ON c.CategoriaID = p.CategoriaID
    WHERE p.ProductoID = @ProductoID AND p.Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Producto_Insertar
    @NombreProducto NVARCHAR(120),
    @ProveedorID INT,
    @CategoriaID INT,
    @CantidadPorUnidad NVARCHAR(60) = NULL,
    @PrecioUnidad DECIMAL(12, 2),
    @UnidadesEnExistencia SMALLINT,
    @UnidadesEnPedido SMALLINT,
    @NivelDeReorden SMALLINT,
    @Descontinuado BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Productos
        (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad,
         UnidadesEnExistencia, UnidadesEnPedido, NivelDeReorden, Descontinuado, Activo)
    VALUES
        (@NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad, @PrecioUnidad,
         @UnidadesEnExistencia, @UnidadesEnPedido, @NivelDeReorden, @Descontinuado, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Producto_Actualizar
    @ProductoID INT,
    @NombreProducto NVARCHAR(120),
    @ProveedorID INT,
    @CategoriaID INT,
    @CantidadPorUnidad NVARCHAR(60) = NULL,
    @PrecioUnidad DECIMAL(12, 2),
    @UnidadesEnExistencia SMALLINT,
    @UnidadesEnPedido SMALLINT,
    @NivelDeReorden SMALLINT,
    @Descontinuado BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos
    SET NombreProducto = @NombreProducto,
        ProveedorID = @ProveedorID,
        CategoriaID = @CategoriaID,
        CantidadPorUnidad = @CantidadPorUnidad,
        PrecioUnidad = @PrecioUnidad,
        UnidadesEnExistencia = @UnidadesEnExistencia,
        UnidadesEnPedido = @UnidadesEnPedido,
        NivelDeReorden = @NivelDeReorden,
        Descontinuado = @Descontinuado
    WHERE ProductoID = @ProductoID AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Producto_Eliminar
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos SET Activo = 0 WHERE ProductoID = @ProductoID AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Categoria_ListarActivas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria
    FROM dbo.Categorias
    WHERE Activo = 1
    ORDER BY NombreCategoria;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Proveedor_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono
    FROM dbo.Proveedores
    WHERE Activo = 1
    ORDER BY CompaniaNombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Proveedor_ExisteCompania
    @CompaniaNombre NVARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*)
    FROM dbo.Proveedores
    WHERE UPPER(LTRIM(RTRIM(CompaniaNombre))) = UPPER(LTRIM(RTRIM(@CompaniaNombre)));
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Proveedor_Insertar
    @CompaniaNombre NVARCHAR(120),
    @NombreContacto NVARCHAR(80) = NULL,
    @CargoContacto NVARCHAR(80) = NULL,
    @Direccion NVARCHAR(160) = NULL,
    @Ciudad NVARCHAR(60) = NULL,
    @CodigoPostal NVARCHAR(20) = NULL,
    @Pais NVARCHAR(60) = NULL,
    @Telefono NVARCHAR(48) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Proveedores
        (CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad,
         CodigoPostal, Pais, Telefono, Fax, Activo)
    VALUES
        (@CompaniaNombre, @NombreContacto, @CargoContacto, @Direccion, @Ciudad,
         @CodigoPostal, @Pais, @Telefono, NULL, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Lab08_Pedido_Reporte
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.FechaPedido, p.FechaRequerida,
           ISNULL(c.Empresa, N'Sin cliente') AS Cliente,
           pr.NombreProducto AS Producto, dp.Cantidad, dp.PrecioUnidad,
           CAST(dp.Cantidad * dp.PrecioUnidad * (1 - dp.Descuento) AS DECIMAL(12, 2)) AS Subtotal,
           CASE
               WHEN p.FechaEnvio IS NOT NULL THEN N'Enviado'
               WHEN p.FechaRequerida < CAST(GETDATE() AS DATE) THEN N'Pendiente vencido'
               ELSE N'Pendiente'
           END AS Estado
    FROM dbo.Pedidos AS p
    INNER JOIN dbo.DetallePedidos AS dp ON dp.PedidoID = p.PedidoID
    INNER JOIN dbo.Productos AS pr ON pr.ProductoID = dp.ProductoID
    INNER JOIN dbo.Clientes AS c ON c.ClienteID = p.ClienteID
    WHERE p.Activo = 1
      AND p.FechaPedido BETWEEN @Desde AND @Hasta
    ORDER BY p.FechaPedido DESC, p.PedidoID DESC, pr.NombreProducto;
END;
GO
