#@(#) script.ddl

DROP TABLE IF EXISTS Trumpalaike_Rezervacija;
DROP TABLE IF EXISTS Servisu_paslaugos;
DROP TABLE IF EXISTS Serviso_Irasas;
DROP TABLE IF EXISTS Rezervacija;
DROP TABLE IF EXISTS Pranesimas;
DROP TABLE IF EXISTS Perziuretas_Automobilis;
DROP TABLE IF EXISTS Draudimas;
DROP TABLE IF EXISTS Atsiliepimas;
DROP TABLE IF EXISTS Servisas;
DROP TABLE IF EXISTS Paslauga;
DROP TABLE IF EXISTS Naudotojas;
DROP TABLE IF EXISTS Automobilis;
CREATE TABLE Automobilis
(
	markė varchar (255) NOT NULL,
	modelis varchar (255) NOT NULL,
	metai int NOT NULL,
	litražas float NOT NULL,
	galia int NOT NULL,
	kaina float NOT NULL,
	statusas varchar (255) NOT NULL,
	numeris varchar (255) NOT NULL,
	vin varchar (255) NOT NULL,
	rida int NOT NULL,
	vietų skaičius int NOT NULL,
	įvertinimų vidurkis int DEFAULT 0 NOT NULL,
	kuro tipas varchar (255) NOT NULL,
	kėbulo tipas varchar (255) NOT NULL,
	id_Automobilis integer NOT NULL,
	CHECK(kuro tipas in ('Benzinas', 'Dyzelinas', 'Elektra')),
	CHECK(kėbulo tipas in ('Sedanas', 'SUV', 'Kupė', 'Hečbekas', 'Universalas', 'Visureigis', 'Kabrioletas')),
	PRIMARY KEY(id_Automobilis)
);

CREATE TABLE Naudotojas
(
	vardas varchar (255) NOT NULL,
	pavardė varchar (255) NOT NULL,
	gimimo data date NOT NULL,
	stažas int NOT NULL,
	asmens kodas int NOT NULL,
	miestas varchar (255) NOT NULL,
	slaptazodis varchar (255) NOT NULL,
	registracijos data date NOT NULL,
	paskutinis pisijungimas date NOT NULL,
	id_Naudotojas integer NOT NULL,
	PRIMARY KEY(id_Naudotojas)
);

CREATE TABLE Paslauga
(
	pavadinimas varchar (255) NOT NULL,
	kaina float NOT NULL,
	laikas int NOT NULL,
	id_Paslauga integer NOT NULL,
	PRIMARY KEY(id_Paslauga)
);

CREATE TABLE Servisas
(
	pavadinimas varchar (255) NOT NULL,
	aprašymas varchar (255) NOT NULL,
	adresas varchar (255) NOT NULL,
	id_Servisas integer NOT NULL,
	PRIMARY KEY(id_Servisas)
);

CREATE TABLE Atsiliepimas
(
	įvertinimas int NOT NULL,
	titulas varchar (255) NOT NULL,
	komentaras varchar (255) NOT NULL,
	data date NOT NULL,
	rekomenduotų kitiems boolean NOT NULL,
	id_Atsiliepimas integer NOT NULL,
	fk_Naudotojasid_Naudotojas integer NOT NULL,
	fk_Automobilisid_Automobilis integer NOT NULL,
	PRIMARY KEY(id_Atsiliepimas),
	CONSTRAINT pateikia FOREIGN KEY(fk_Naudotojasid_Naudotojas) REFERENCES Naudotojas (id_Naudotojas),
	CONSTRAINT vertinamas FOREIGN KEY(fk_Automobilisid_Automobilis) REFERENCES Automobilis (id_Automobilis)
);

CREATE TABLE Draudimas
(
	poliso numeris int NOT NULL,
	pradžios data date NOT NULL,
	pabaigos data date NOT NULL,
	draudimo tipas varchar (255) NOT NULL,
	draudimo kompanijos pavadinimas varchar (255) NOT NULL,
	id_Draudimas integer NOT NULL,
	fk_Automobilisid_Automobilis1 integer NOT NULL,
	PRIMARY KEY(id_Draudimas),
	UNIQUE(null),
	CONSTRAINT apdraustas FOREIGN KEY(fk_Automobilisid_Automobilis1) REFERENCES Automobilis (id_Automobilis)
);

CREATE TABLE Perziuretas_Automobilis
(
	data date NOT NULL,
	id_Peržiūrėtas Automobilis integer NOT NULL,
	fk_Automobilisid_Automobilis integer NOT NULL,
	fk_Naudotojasid_Naudotojas integer NOT NULL,
	PRIMARY KEY(id_Peržiūrėtas Automobilis),
	CONSTRAINT įtrauktas į FOREIGN KEY(fk_Automobilisid_Automobilis) REFERENCES Automobilis (id_Automobilis),
	CONSTRAINT peržiūri FOREIGN KEY(fk_Naudotojasid_Naudotojas) REFERENCES Naudotojas (id_Naudotojas)
);

CREATE TABLE Pranesimas
(
	data date NOT NULL,
	tekstas varchar (255) NOT NULL,
	tipas varchar (255) NOT NULL,
	id_Pranešimas integer NOT NULL,
	fk_Naudotojasid_Naudotojas integer NOT NULL,
	CHECK(tipas in ('Informacija', 'Pagalba', 'Mokėjimas', 'Rezervacija')),
	PRIMARY KEY(id_Pranešimas),
	CONSTRAINT gauna FOREIGN KEY(fk_Naudotojasid_Naudotojas) REFERENCES Naudotojas (id_Naudotojas)
);

CREATE TABLE Rezervacija
(
	pradžia date NOT NULL,
	pabaiga date NOT NULL,
	pateikimo data date NOT NULL,
	paėmimo vieta varchar (255) NOT NULL,
	atidavimo vieta varchar (255) NOT NULL,
	id_Rezervacija integer NOT NULL,
	fk_Automobilisid_Automobilis integer NOT NULL,
	fk_Naudotojasid_Naudotojas integer NOT NULL,
	PRIMARY KEY(id_Rezervacija),
	UNIQUE(fk_Automobilisid_Automobilis),
	CONSTRAINT įtrauktas į FOREIGN KEY(fk_Automobilisid_Automobilis) REFERENCES Automobilis (id_Automobilis),
	CONSTRAINT pateikia FOREIGN KEY(fk_Naudotojasid_Naudotojas) REFERENCES Naudotojas (id_Naudotojas)
);

CREATE TABLE Serviso_Irasas
(
	data date NOT NULL,
	būsena varchar (255) NOT NULL,
	id_Serviso Įrašas integer NOT NULL,
	fk_Automobilisid_Automobilis integer NOT NULL,
	fk_Naudotojasid_Naudotojas integer NOT NULL,
	fk_Servisasid_Servisas integer NOT NULL,
	CHECK(būsena in ('Atliktas', 'Nesuplanuotas', 'Suplanuotas', 'Atšauktas')),
	PRIMARY KEY(id_Serviso Įrašas),
	CONSTRAINT priklauso FOREIGN KEY(fk_Automobilisid_Automobilis) REFERENCES Automobilis (id_Automobilis),
	CONSTRAINT yra susijęs su FOREIGN KEY(fk_Servisasid_Servisas) REFERENCES Servisas (id_Servisas)
);

CREATE TABLE Servisu_paslaugos
(
	fk_Servisasid_Servisas integer NOT NULL,
	fk_Paslaugaid_Paslauga integer NOT NULL,
	PRIMARY KEY(fk_Servisasid_Servisas, fk_Paslaugaid_Paslauga),
	CONSTRAINT teikia FOREIGN KEY(fk_Servisasid_Servisas) REFERENCES Servisas (id_Servisas)
);

CREATE TABLE Trumpalaike_Rezervacija
(
	peteikimo data date NOT NULL,
	laikas int NOT NULL,
	id_Trumpalaikė Rezervacija integer NOT NULL,
	fk_Automobilisid_Automobilis integer NOT NULL,
	fk_Naudotojasid_Naudotojas integer NOT NULL,
	PRIMARY KEY(id_Trumpalaikė Rezervacija),
	UNIQUE(fk_Automobilisid_Automobilis),
	CONSTRAINT įtrauktas į FOREIGN KEY(fk_Automobilisid_Automobilis) REFERENCES Automobilis (id_Automobilis),
	CONSTRAINT pateikia FOREIGN KEY(fk_Naudotojasid_Naudotojas) REFERENCES Naudotojas (id_Naudotojas)
);
