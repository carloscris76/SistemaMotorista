USE [master]
GO
CREATE DATABASE [Motoristadb]
GO
USE [Motoristadb]
GO
CREATE TABLE dbo.Rol(
 Id int primary key identity(1,1) not null,
 Nombre varchar (60) not null
);
GO 
CREATE TABLE dbo.Usuario(
  Id int primary key identity (1,1) not null,
  IdRol int not null,
  Nombre varchar (60) not null,
  Apellido varchar (60) not null,
  Login varchar (25) not null,
  Estatus tinyint not null,
  Password char(32) not null,
  FecharRegistro datetime not null,
  constraint fk1_Rol_Usuario foreign key (IdRol) references Rol (id)
);
GO
CREATE TABLE dbo.Taxista (
  Id int primary key identity (1,1) not null,
  Nombre varchar (60) not null,
  Apellido varchar (60) not null,
  Placa varchar (60) not null,
  Color tinyint not null,
  Marca tinyint not null,
  Estatus tinyint not null,
  Licencia int not null,
  Nacionalidad tinyint not null,
  Estado tinyint not null
);
GO 
CREATE TABLE dbo.Cliente(
  Id int primary key identity (1,1) not null,
  IdTaxista int not null,
  Nombre varchar (60) not null,
  Apellido varchar (60) not null,s
  TipoCliente tinyint not null
  constraint fk1_Taxista_Cliente foreign key (IdTaxista) references Taxista (Id)
);