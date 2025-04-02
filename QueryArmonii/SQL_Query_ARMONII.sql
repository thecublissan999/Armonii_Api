USE master;
GO

-- Comprobar si la base de datos Armonii ya existe y eliminarla si es necesario
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'Armonii')
BEGIN
    ALTER DATABASE Armonii SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Armonii;
END
GO

-- Crear la base de datos
CREATE DATABASE Armonii;
GO

-- Usar la base de datos
USE Armonii;
GO

-- Creación de tablas
CREATE TABLE Permisos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE Generos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    genero VARCHAR(100)
);

CREATE TABLE UsuarioAdmin (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(100) UNIQUE NOT NULL,
    contrasenya VARCHAR(255) NOT NULL,
    telefono VARCHAR(20),
    permiso INT FOREIGN KEY REFERENCES Permisos(id)
);

CREATE TABLE Usuario (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(100) UNIQUE NOT NULL,
    contrasenya VARCHAR(255) NOT NULL,
    telefono VARCHAR(20),
    latitud FLOAT,
    longitud FLOAT,
    fechaRegistro DATETIME DEFAULT GETDATE(),
    estado BIT DEFAULT 1,
    valoracion FLOAT,
    tipo VARCHAR(50) CHECK (tipo IN ('Musico', 'Local'))
);

CREATE TABLE Musico (
    id INT IDENTITY(1,1) PRIMARY KEY,
    apodo VARCHAR(100),
    apellido VARCHAR(50),
    genero VARCHAR(50),
    edad INT,
    biografia TEXT,
    imagen TEXT,
    idUsuario INT UNIQUE FOREIGN KEY REFERENCES Usuario(id)
);

CREATE TABLE GenerosMusicos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idGenero INT FOREIGN KEY REFERENCES Generos(id),
    idMusico INT FOREIGN KEY REFERENCES Musico(id)
);

CREATE TABLE Local (
    id INT IDENTITY(1,1) PRIMARY KEY,
    direccion VARCHAR(255) NOT NULL,
    tipo_local VARCHAR(100),
    descripcion TEXT,
    imagen TEXT,
    horarioApertura TIME,
    horarioCierre TIME,
    idUsuario INT UNIQUE FOREIGN KEY REFERENCES Usuario(id)
);

CREATE TABLE Evento (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    fecha DATETIME NOT NULL,
    descripcion TEXT,
    idLocal INT NOT NULL FOREIGN KEY REFERENCES Local(id),
    idMusico INT NOT NULL FOREIGN KEY REFERENCES Musico(id),
    estado BIT DEFAULT 1,
    duracion INT NOT NULL
);

CREATE TABLE Mensaje (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idUsuarioLocal INT FOREIGN KEY REFERENCES Usuario(id),
    idUsuarioMusico INT FOREIGN KEY REFERENCES Usuario(id),
    fechaEnvio DATETIME DEFAULT GETDATE(),
    mensaje VARCHAR(250),
    emisor CHAR(1)
);

CREATE TABLE Valoracion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    puntuacion INT CHECK (puntuacion BETWEEN 0 AND 5),
    titulo VARCHAR(100),
    descripcion TEXT,
    fecha DATETIME DEFAULT GETDATE(),
    idEvento INT FOREIGN KEY REFERENCES Evento(id),
    idLocal INT FOREIGN KEY REFERENCES Local(id),
    idMusico INT FOREIGN KEY REFERENCES Musico(id),
    emisor CHAR(1)
);

-- INSERT de prueba

-- Insertar permisos

INSERT INTO Permisos (nombre) VALUES 
('Superadministrador'), 
('Administrador'), 
('Organizador');


-- Insertar géneros musicales

INSERT INTO Generos (genero) VALUES 
('Pop'), 
('Rock'), 
('Jazz'), 
('R&B'), 
('Hard Rock'), 
('Electrónica'), 
('Reggaetón'), 
('Blues'), 
('Hip-Hop'), 
('Clásica');

-- Insert usuarios Admins
INSERT INTO UsuarioAdmin (nombre, correo, contrasenya, telefono, permiso)  
VALUES  
('Juan Pérez', '1', '1', '555111222', 1),  
('María López', 'maria.lopez@email.com', 'securePass456', '555333444', 2),  
('Carlos Gómez', 'carlos.gomez@email.com', 'strongPass789', '555555666', 1),  
('Ana Martínez', 'ana.martinez@email.com', 'adminSecure101', '555777888', 3),  
('Luis Fernández', 'luis.fernandez@email.com', 'SuperAdmin999', '555999000', 2);  

-- Insertar usuarios (5 músicos y 5 locales)
INSERT INTO Usuario (nombre, correo, contrasenya, telefono, latitud, longitud, valoracion, tipo)
VALUES 
('Carlos', 'carlosrivera@email.com', 'password789', '555987654', 19.432608, -99.133209, 4.3, 'Musico'),
('Laura', 'lauramrquez@email.com', 'password101', '555123789', 40.712776, -74.005974, 4.6, 'Musico'),
('Luis', 'luisgomez@email.com', 'password102', '555321654', 41.902782, 12.496366, 4.5, 'Musico'),
('Sara', 'saralopez@email.com', 'password103', '555654987', 48.856614, 2.352222, 4.4, 'Musico'),
('Pedro', 'pedrojimenez@email.com', 'password104', '555987321', 41.378887, -5.998393, 4.2, 'Musico'),
('Bar El Rincón', 'bar1@email.com', 'password201', '555456789', 40.712776, -74.005974, 4.1, 'Local'),
('Café Central', 'cafe1@email.com', 'password202', '555321987', 41.902782, 12.496366, 4.3, 'Local'),
('Restaurante La Cocina', 'restaurante1@email.com', 'password203', '555654321', 19.432608, -99.133209, 4.5, 'Local'),
('Teatro Azul', 'teatro1@email.com', 'password204', '555789654', 48.856614, 2.352222, 4.4, 'Local'),
('Sala de Conciertos', 'sala1@email.com', 'password205', '555987123', 40.712776, -74.005974, 4.2, 'Local');

-- Insertar músicos correspondientes
INSERT INTO Musico (apodo, apellido, genero, edad, biografia, imagen, idUsuario)
VALUES 
('Carlos Music', 'Pérez', 'Hombre', 29, 'Cantante de música pop con 10 años de carrera', 'imagen_carlos.jpg', 1),
('Laura Band', 'Márquez', 'Mujer', 18, 'Banda de rock alternativo fundada en 2015', 'imagen_laura.jpg', 2),
('Luis Groove', 'Gómez', 'Otros', 35, 'Músico de jazz con un estilo único de fusión', 'imagen_luis.jpg', 3),
('Sara Soul', 'López', 'Mujer', 67, 'Cantante y compositora de soul y R&B', 'imagen_sara.jpg', 4),
('Pedro Rock', 'Jiménez', 'Hombre', 52, 'Banda de rock clásico con influencias de los 70', 'imagen_pedro.jpg', 5);

-- Asociar músicos con géneros
INSERT INTO GenerosMusicos (idGenero, idMusico) 
VALUES 
(1, 1), -- Carlos Music -> Pop
(1, 2), -- Carlos Music -> Rock
(2, 2), -- Laura Band -> Rock
(3, 3), -- Luis Groove -> Jazz
(4, 4), -- Sara Soul -> R&B
(5, 5); -- Pedro Rock -> Hard Rock

-- Insertar locales correspondientes
INSERT INTO Local (direccion, tipo_local, descripcion, imagen, horarioApertura, horarioCierre, idUsuario)
VALUES 
('Calle Ficticia 101', 'Bar', 'Un acogedor bar de barrio con música en vivo', 'imagen_bar.jpg', '18:00', '03:00', 6),
('Avenida Principal 202', 'Café', 'Café con ambiente relajado para disfrutar de una buena conversación', 'imagen_cafe.jpg', '07:00', '23:00', 7),
('Calle de la Reforma 303', 'Restaurante', 'Restaurante elegante con una oferta culinaria internacional', 'imagen_restaurante.jpg', '12:00', '23:00', 8),
('Avenida del Teatro 404', 'Teatro', 'Teatro histórico con obras y conciertos en vivo', 'imagen_teatro.jpg', '16:00', '23:00', 9),
('Calle Central 505', 'Sala de conciertos', 'Sala de conciertos de música en vivo, ideal para bandas emergentes', 'imagen_sala.jpg', '19:00', '02:00', 10);

-- Insertar eventos
INSERT INTO Evento (nombre, fecha, descripcion, idLocal, idMusico, duracion)
VALUES 
('Concierto Carlos Music', '2025-01-04', 'Un gran show pop en vivo', 1, 1, 120),
('Laura Band en vivo', '2025-02-04', 'Un recital de rock alternativo', 2, 2, 90),
('Luis Groove Jazz Night', '2025-03-04', 'Jazz en vivo para relajarse', 3, 3, 120),
('Sara Soul & Friends', '2025-04-04', 'Noche de R&B en el escenario', 4, 4, 110),
('Pedro Rock en concierto', '2025-05-04', 'Rock en vivo con la banda más electrizante', 5, 5, 150);

-- Insertar mensajes entre usuarios
INSERT INTO Mensaje (idUsuarioLocal, idUsuarioMusico, mensaje, emisor)
VALUES 
(6, 2, 'Hola Laura, ¿cuándo llegas al bar?', 'L'),
(7, 1, 'Todo listo para el concierto, nos vemos pronto!', 'M'),
(8, 4, 'Sara, tu show de R&B suena increíble, ¿nos vemos en el teatro?', 'L'),
(9, 3, 'Luis, ¿te gustaría hacer un jam session este fin de semana?', 'L'),
(10, 2, 'Laura, tu concierto fue increíble, ¡quién pudiera tener un show como el tuyo!', 'M');

-- Insertar valoraciones de eventos
INSERT INTO Valoracion (puntuacion, titulo, descripcion, idEvento, idLocal, idMusico, emisor)
VALUES 
(5, 'Increíble concierto', 'Carlos Music brilló en el escenario, el mejor show que he visto', 1, 1, 1, 'L'),
(4, 'Buen show', 'La banda Laura Band estuvo excelente, aunque el sonido podría mejorar', 2, 2, 2, 'M'),
(5, 'Un espectáculo memorable', 'El jazz de Luis Groove fue inolvidable, completamente único', 3, 3, 3, 'L'),
(4, 'Excelente noche de R&B', 'El show de Sara Soul fue impresionante, aunque un poco corto', 4, 4, 4, 'M'),
(5, 'Un show electrizante', 'Pedro Rock nos hizo saltar toda la noche, ¡fantástico!', 5, 5, 5, 'L');
