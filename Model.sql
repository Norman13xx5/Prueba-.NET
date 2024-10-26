Database: Test

CREATE TABLE roles (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);


CREATE TABLE usuarios (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100) NOT NULL,
    idrol INT NOT NULL,
    FOREIGN KEY (idrol) REFERENCES roles(id),
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);


CREATE TABLE empresas (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100) NOT NULL,
    direccion NVARCHAR(255) NOT NULL,
    telefono NVARCHAR(15) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);


CREATE TABLE ordenes (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100) NOT NULL,
    idusuario INT NOT NULL,
    FOREIGN KEY (idusuario) REFERENCES usuarios(id),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);


CREATE TABLE categorias (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);


CREATE TABLE productos (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100) NOT NULL,
    idorden INT NOT NULL,
    FOREIGN KEY (idorden) REFERENCES ordenes(id),
    idcategoria INT NOT NULL,
    FOREIGN KEY (idcategoria) REFERENCES categorias(id),
    idempresa INT NOT NULL,
    FOREIGN KEY (idempresa) REFERENCES empresas(id),
    valor DECIMAL(10, 2) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);






select * from roles;

insert into roles (nombre) values ('administrador');
insert into roles (nombre) values ('cliente');


select * from usuarios

insert into usuarios (nombre, idrol, email, password) values ('admin', 1, 'admin@local.com', 'password1');
insert into usuarios (nombre, idrol, email, password) values ('cliente', 2, 'cliente@local.com', 'password2');


select * from empresas

insert into empresas (nombre, direccion, telefono) values ('BraSoluciones', 'calle 45', '+573182834018');
insert into empresas (nombre, direccion, telefono) values ('BraProblemas', 'calle 46', '+573182834019');


select * from categorias;

insert into categorias (nombre) values ('Pintruas'), ('Perifericos');

select * from ordenes;

insert into ordenes (nombre, idusuario) values ('Compras Hogar', 1);
insert into ordenes (nombre, idusuario) values ('Compras Divertida', 4);

select * from productos;

insert into productos (nombre, idorden, idcategoria, idempresa, valor) values ('Pintuco',1, 1, 1, 100000)
insert into productos (nombre, idorden, idcategoria, idempresa, valor) values ('Mouse',4, 2, 2, 10000)

select o.id, o.nombre, u.nombre  from ordenes o 
join productos p on p.idorden = o.id
join usuarios u on u.id = o.idusuario

select p.id, p.nombre, o.nombre, c.nombre, e.nombre, p.valor from productos p 
join ordenes o on o.id = p.idorden 
join categorias c on c.id = p.idcategoria 
join empresas e on e.id = p.idempresa 
