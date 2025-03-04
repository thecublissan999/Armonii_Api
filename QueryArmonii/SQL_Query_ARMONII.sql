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
GO  -- Asegura que la base de datos se cree completamente antes de usarla

-- Usar la base de datos
USE Armonii;
GO

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
    id INT PRIMARY KEY,
    nombreArtistico VARCHAR(100),
    genero VARCHAR(50),
	edad INT,
    biografia TEXT
);

CREATE TABLE Local (
    id INT PRIMARY KEY,
    direccion VARCHAR(255) NOT NULL,
    tipo_local VARCHAR(100),
    horarioApertura TIME,
    horarioCierre TIME
);

CREATE TABLE Evento (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    fecha DATETIME NOT NULL,
    descripcion TEXT,
    idLocal INT NOT NULL,
    idMusico INT NOT NULL,
    estado BIT DEFAULT 1,
    duracion INT NOT NULL 
);

CREATE TABLE Mensaje (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idUsuarioLocal INT,
    idUsuarioMusico INT,
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
    idEvento INT,
    idLocal INT,
    idMusico INT,
    emisor CHAR(1)
);

-- Añadir claves foráneas

ALTER TABLE Musico
ADD CONSTRAINT FK_Musico_Usuario FOREIGN KEY (id) REFERENCES Usuario(id);

ALTER TABLE Local
ADD CONSTRAINT FK_Local_Usuario FOREIGN KEY (id) REFERENCES Usuario(id);

ALTER TABLE Evento
ADD CONSTRAINT FK_Evento_Local FOREIGN KEY (idLocal) REFERENCES Local(id);

ALTER TABLE Evento
ADD CONSTRAINT FK_Evento_Musico FOREIGN KEY (idMusico) REFERENCES Musico(id);

ALTER TABLE Mensaje
ADD CONSTRAINT FK_Mensaje_UsuarioLocal FOREIGN KEY (idUsuarioLocal) REFERENCES Usuario(id);

ALTER TABLE Mensaje
ADD CONSTRAINT FK_Mensaje_UsuarioMusico FOREIGN KEY (idUsuarioMusico) REFERENCES Usuario(id);

ALTER TABLE Valoracion
ADD CONSTRAINT FK_Valoracion_Evento FOREIGN KEY (idEvento) REFERENCES Evento(id);

ALTER TABLE Valoracion
ADD CONSTRAINT FK_Valoracion_UsuarioMusico FOREIGN KEY (idMusico) REFERENCES Musico(id);

ALTER TABLE Valoracion
ADD CONSTRAINT FK_Valoracion_UsuarioLocal FOREIGN KEY (idLocal) REFERENCES Local(id);

-- INSERT de prueba

-- Insertar usuarios
-- Insertar 5 usuarios como 'Musico'
INSERT INTO Usuario (nombre, correo, contrasenya, telefono, latitud, longitud, valoracion, tipo)
VALUES 
('Carlos Rivera', 'carlosrivera@email.com', 'password789', '555987654', 19.432608, -99.133209, 4.3, 'Musico'),
('Laura Márquez', 'lauramrquez@email.com', 'password101', '555123789', 40.712776, -74.005974, 4.6, 'Musico'),
('Luis Gómez', 'luisgomez@email.com', 'password102', '555321654', 41.902782, 12.496366, 4.5, 'Musico'),
('Sara López', 'saralopez@email.com', 'password103', '555654987', 48.856614, 2.352222, 4.4, 'Musico'),
('Pedro Jiménez', 'pedrojimenez@email.com', 'password104', '555987321', 41.378887, -5.998393, 4.2, 'Musico');

-- Insertar 5 usuarios como 'Local'
INSERT INTO Usuario (nombre, correo, contrasenya, telefono, latitud, longitud, valoracion, tipo)
VALUES 
('Bar El Rincón', 'bar1@email.com', 'password201', '555456789', 40.712776, -74.005974, 4.1, 'Local'),
('Café Central', 'cafe1@email.com', 'password202', '555321987', 41.902782, 12.496366, 4.3, 'Local'),
('Restaurante La Cocina', 'restaurante1@email.com', 'password203', '555654321', 19.432608, -99.133209, 4.5, 'Local'),
('Teatro Azul', 'teatro1@email.com', 'password204', '555789654', 48.856614, 2.352222, 4.4, 'Local'),
('Sala de Conciertos', 'sala1@email.com', 'password205', '555987123', 40.712776, -74.005974, 4.2, 'Local');

-- Insertar 5 músicos correspondientes
INSERT INTO Musico (id, nombreArtistico, genero, biografia, edad)
VALUES 
(1, 'Carlos Music', 'Pop', 'Cantante de música pop con 10 años de carrera', 29),
(2, 'Laura Band', 'Rock', 'Banda de rock alternativo fundada en 2015', 18),
(3, 'Luis Groove', 'Jazz', 'Músico de jazz con un estilo único de fusión', 35),
(4, 'Sara Soul', 'R&B', 'Cantante y compositora de soul y R&B', 67),
(5, 'Pedro Rock', 'Hard Rock', 'Banda de rock clásico con influencias de los 70', 52);

-- Insertar 5 locales correspondientes (ahora con IDs 6, 7, 8, 9, 10)
INSERT INTO Local (id, direccion, tipo_local, horarioApertura, horarioCierre)
VALUES 
(6, 'Calle Ficticia 101', 'Bar', '18:00', '03:00'),
(7, 'Avenida Principal 202', 'Café', '07:00', '23:00'),
(8, 'Calle de la Reforma 303', 'Restaurante', '12:00', '23:00'),
(9, 'Avenida del Teatro 404', 'Teatro', '16:00', '23:00'),
(10, 'Calle Central 505', 'Sala de conciertos', '19:00', '02:00');

-- Insertar eventos (con los usuarios como locales y músicos)
INSERT INTO Evento (nombre, fecha, descripcion, idLocal, idMusico, duracion)
VALUES 
('Concierto Carlos Music', '2025-04-01 20:00:00', 'Un gran show pop en vivo', 6, 1, 120),
('Laura Band en vivo', '2025-04-02 21:00:00', 'Un recital de rock alternativo', 7, 2, 90),
('Luis Groove Jazz Night', '2025-04-03 20:00:00', 'Jazz en vivo para relajarse', 8, 3, 120),
('Sara Soul & Friends', '2025-04-04 21:00:00', 'Noche de R&B en el escenario', 9, 4, 110),
('Pedro Rock en concierto', '2025-04-05 22:00:00', 'Rock en vivo con la banda más electrizante', 10, 5, 150);

-- Insertar Mensajes (usuarios locales y músicos)
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
(5, 'Increíble concierto', 'Carlos Music brilló en el escenario, el mejor show que he visto', 1, 6, 1, 'L'),
(4, 'Buen show', 'La banda Laura Band estuvo excelente, aunque el sonido podría mejorar', 2, 7, 2, 'M'),
(5, 'Un espectáculo memorable', 'El jazz de Luis Groove fue inolvidable, completamente único', 3, 8, 3, 'L'),
(4, 'Excelente noche de R&B', 'El show de Sara Soul fue impresionante, aunque un poco corto', 4, 9, 4, 'M'),
(5, 'Un show electrizante', 'Pedro Rock nos hizo saltar toda la noche, ¡fantástico!', 5, 10, 5, 'L');