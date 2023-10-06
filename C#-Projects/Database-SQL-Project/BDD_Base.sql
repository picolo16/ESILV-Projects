drop database if exists velomax;
create database if not exists velomax;
use velomax;
drop table if exists Modele;
create table if not exists Modele(
	NumP_M int primary key,
    Nom varchar(20),
    Grandeur varchar(20),
    LigneProduit varchar(20),
    Prix_unitaire float,
    Date_Intro datetime,
    Date_Fin datetime
    );
drop table if exists Piece_detachee;
create table if not exists Piece_detachee(
	NumP_P int primary key,
    Description varchar(50),
    PrixUnitaire float,
    QuantiteStock int,
    DateIntro datetime,
    DateFin datetime
    );
drop table if exists Commande;
create table if not exists Commande(
	NumCommande varchar(20) primary key,
    Date_Commande datetime,
    Date_Livraison datetime
    );
drop table if exists Adresse;
create table if not exists Adresse(
	IdAdresse int primary key,
    Rue varchar(20),
    Ville varchar(20),
    CodePostal varchar(10),
    Province varchar(20)
    );
drop table if exists Fournisseur;
create table if not exists Fournisseur(
	Siret varchar(14) primary key,
    Nom_Fournisseur varchar(50),
    Contact varchar(20),
    Libelle int,
    IdAdresse int,
    foreign key (IdAdresse)
		references Adresse (IdAdresse)
        on delete cascade
        on update no action
    );
    
drop table if exists Fidelite;
create table if not exists Fidelite(
	NumProgramme int primary key,
    NomProgramme varchar(20),
    Cout int,
    Duree int,
    Remise float
    );

drop table if exists Client;
create table if not exists Client(
	IdClient int primary key,
    TelephoneClient varchar(20),
    CourrielClient varchar(30),
    IdAdresse int,
    foreign key (IdAdresse)
		references Adresse (IdAdresse)
        on delete cascade
        on update no action
    );

drop table if exists IndividuClient;
create table if not exists IndividuClient(
	IdClient int primary key,
    NomIndClient varchar(20),
    PrenomIndClient varchar(20),
	NumProgramme int,
	foreign key (NumProgramme)
        references Fidelite (NumProgramme)
        on delete cascade
        on update no action,
    foreign key (IdClient)
		references Client (IdClient)
        on delete cascade
        on update no action
);

drop table if exists BoutiqueClient;
create table if not exists BoutiqueClient(
	IdClient int primary key,
    NomBoutiqueClient varchar(20),
    PrenomBoutiqueClient varchar(20),
	PourcentageRemise float,
    foreign key (IdClient)
		references Client (IdClient)
        on delete cascade
        on update no action
);

drop table if exists QuantiteModeleC;
create table if not exists QuantiteModeleC(
	NumP_M int,
    NumCommande varchar(20),
    Quantite_modele int,
	primary key(NumP_M,NumCommande),
    foreign key(NumP_M)
		references Modele (NumP_M)
        on delete cascade
        on update no action,
	foreign key(NumCommande)
		references Commande (NumCommande)
        on delete cascade
        on update no action
);

drop table if exists QuantitePieceC;
create table if not exists QuantitePieceC(
	NumP_P int,
    NumCommande varchar(20),
    Quantite_piece int,
	primary key(NumP_P,NumCommande),
    foreign key(NumP_P)
		references Piece_detachee (NumP_P)
        on delete cascade
        on update no action,
	foreign key(NumCommande)
		references Commande (NumCommande)
        on delete cascade
        on update no action
);

drop table if exists ListeProduitsFournis;
create table if not exists ListeProduitsFournis(
	NumP_Fournisseur varchar(20),
    NumP_P int,
    Siret varchar(14),
    PrixFournisseur int,
    Delai_appro int,
    primary key(NumP_Fournisseur, NumP_P, Siret),
    foreign key(Siret)
		references Fournisseur(Siret)
        on delete cascade
        on update no action,
	foreign key(NumP_P)
		references Piece_detachee (NumP_P)
        on delete cascade
        on update no action
);

drop table if exists AchatFournisseur;
create table if not exists AchatFournisseur(
	NumP_Fournisseur varchar(20),
    NumP_P int,
    Siret varchar(14),
    QuantiteAchat int,
    primary key(NumP_Fournisseur, NumP_P, Siret),
    foreign key(Siret)
		references Fournisseur(Siret),
	foreign key(NumP_P)
		references Piece_detachee (NumP_P)
        on delete cascade
        on update no action,
	foreign key(NumP_Fournisseur)
		references ListeProduitsFournis(NumP_Fournisseur)
        on delete cascade
        on update no action
);

drop table if exists Assemblage;
create table if not exists Assemblage(
	NumP_M int primary key,
    Cadre varchar(10),
    Guidon varchar(10),
    Freins varchar(10),
    Selle varchar(10),
    DerailleurAvant varchar(10),
    DerailleurArriere varchar(10),
    RoueAvant varchar(10),
    RoueArriere varchar(10),
    Reflecteurs varchar(10),
    Pedalier varchar(10),
    Ordinateur varchar(10),
    Panier varchar(10),
    foreign key(NumP_M)
		references Modele (NumP_M)
        on delete cascade
        on update no action
	);

desc Modele;
desc Piece_detachee;
desc Commande;
desc Fournisseur;
desc Client;
desc Adresse;
desc Assemblage;

insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (101,"Kilimandjaro","Adultes","VTT",569,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (102,"NorthPole","Adultes","VTT",329,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (103,"MontBlanc","Jeunes","VTT",399,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (104,"Hooligan","Jeunes","VTT",199,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (105,"Orléans","Hommes","VTT",229,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (106,"Orléans","Dames","VTT",229,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (107,"BlueJay","Hommes","VTT",349,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (108,"BlueJay","Dames","VTT",349,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (109,"Trail Explorer","Filles","VTT",129,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (110,"Trail Explorer","Garçons","VTT",129,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (111,"Night Hawk","Jeunes","VTT",189,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (112,"Tierra Verde","Hommes","VTT",199,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (113,"Tierra Verde","Dames","VTT",199,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (114,"Mud Zinger I","Jeunes","VTT",279,"0000-00-00","0000-00-00");
insert into velomax.Modele(NumP_M,Nom,Grandeur,LigneProduit,Prix_unitaire,Date_Intro,Date_Fin) values (115,"Mud Zinger II","Adultes","VTT",359,"0000-00-00","0000-00-00");
select * from Modele;
select max(NumP_M) from Modele; 

insert into velomax.Fidelite(NumProgramme, NomProgramme, Cout, Duree, Remise) values (1,"Fidélio",15,1,0.05);
insert into velomax.Fidelite(NumProgramme, NomProgramme, Cout, Duree, Remise) values (2,"Fidélio Or",25,2,0.08);
insert into velomax.Fidelite(NumProgramme, NomProgramme, Cout, Duree, Remise) values (3,"Fidélio Platine",60,2,0.1);
insert into velomax.Fidelite(NumProgramme, NomProgramme, Cout, Duree, Remise) values (4,"Fidélio Max",100,3,0.12);
select * from Fidelite;

insert into velomax.Adresse(IdAdresse,Rue,Ville,CodePostal,Province) values (1,"Pasteur","Bois-Colombes","92270","Ile-De-France");
insert into velomax.Client(IdClient,TelephoneClient,CourrielClient,IdAdresse) values (1,"0652759435","arthur.varet@edu.devinci.fr",1);
insert into velomax.IndividuClient(IdClient,NomIndClient,PrenomIndClient,NumProgramme) values (1,"Varet","Arthur",4);

select * from client;
select * from Adresse;
delete from IndividuClient where IdClient = 1;
select * from client;