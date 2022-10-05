CREATE TABLE doctor (
	id char(36) NOT NULL, /* will become GUID */
    firstName char(30),
    lastName char(50),
    UPIN char(6) UNIQUE,
    ambulanceAddress char(20),
    PRIMARY KEY (id)
);

CREATE TABLE patient (
	id char(36) NOT NULL,
    firstName char(30),
    lastName char(50),
    healthInsuranceID int UNIQUE,
    diagnosis char(100),
    doctorId char(36),
    PRIMARY KEY (id),
    FOREIGN KEY (doctorId) REFERENCES doctor (id)
);

INSERT INTO doctor VALUES 
	("hj76td", "Mike", "Simpson", "89hg53", "10th Avenue"),
	("ok876z", "Zoe", "Hark", "kk9000", "7 Road St."),
    ("vbt555", "Leo", "Ler", "78zz19", "3rd Avenue"),
	("34fgz7", "Mary", "Zick", "ab67cf", "7th Avenue"),
	("jhf7f9", "Michel", "Jouse", "gh604g", "7 Road St."),
    ("jfops8", "Thomas", "Lisan", "78zho9", "3rd Avenue");

INSERT INTO patient VALUES 
	("uhztt0", "John", "Sima", 7478458, "Contact allergy", (SELECT id FROM doctor WHERE firstName = "Mary" AND lastName = "Zick")),
    ("ijz65n", "Mark", "Merz", 4545455, "Diabetes", (SELECT id FROM doctor WHERE firstName = "Michel" AND lastName = "Jouse")),
	("lkdc34", "Jess", "Rodhos", 8859403, "Nut allergy",(SELECT id FROM doctor WHERE firstName = "Mary" AND lastName = "Zick")),
    ("98ikh2", "Ann", "Marthos", 9663839, "High blood pressure", (SELECT id FROM doctor WHERE firstName = "Thomas" AND lastName = "Lisan")),
    ("88gghh", "James", "Freti", 875340, "High blood pressure", (SELECT id FROM doctor WHERE firstName = "Zoe" AND lastName = "Hark")),
    ("jj905l", "Simon", "Orak", 1677849, "Low blood pressure", (SELECT id FROM doctor WHERE firstName = "Mike" AND lastName = "Simpson")),
    ("74okpr", "Anny", "Leti", 9763034, "Arthritis", (SELECT id FROM doctor WHERE firstName = "Zoe" AND lastName = "Hark")),
    ("g7pR41", "Julio", "Janni", 1643779, "Arrhythmia", (SELECT id FROM doctor WHERE firstName = "Thomas" AND lastName = "Lisan"));


/* create user that has access for connecting to DB from Visual Studio 

CREATE USER 'newuser'@'localhost' IDENTIFIED WITH mysql_native_password BY "trebAmo10";
GRANT ALL PRIVILEGES ON *.* TO 'newuser'@'localhost' ;
FLUSH PRIVILEGES; 

*/

/* all patients of doctor Thomas Lisan */
SELECT * FROM patient WHERE doctorId = "jfops8"; 

/* all patients with low blood pressure */
SELECT * FROM patient WHERE diagnosis IN ("Low blood pressure"); 

/* doctors on 3rd avenue */
SELECT * FROM doctor WHERE ambulanceAddress = "3rd Avenue";

/* count how many patients has some kind of disease (sum up how many has, for example, high blood pressure)  */
SELECT diagnosis, COUNT(*) AS sum 
FROM patient 
GROUP BY diagnosis;

/* order patients by last name*/
SELECT * FROM patient 
Order by lastName ASC;

/* how many doctors are on 7 Rod St. */
SELECT COUNT(id) AS Doctorson7RoadSt
FROM doctor
WHERE ambulanceAddress = "7 Road St.";

/* number of patients per doctor, show only doctorId */
SELECT doctorId, COUNT(doctorId) AS numberOfPatients 
from patient
group by doctorId;

/* patient whose healthInsuranceID is min */
SELECT id FROM patient
HAVING MIN(healthInsuranceID);

/* update patients diagnosis */
UPDATE patient
SET diagnosis = "Nut and contact allergy"
WHERE healthInsuranceID = 8859403;
/* check result */
SELECT * FROM patient WHERE healthInsuranceID = 8859403;

/* update James Freti, his doctor */
UPDATE patient
SET doctorId = (SELECT id FROM doctor WHERE firstName = "Leo" AND lastName = "Ler")
WHERE healthInsuranceID = 875340;

/* delete doctor  who's on address*/
DELETE FROM doctor 
WHERE ambulanceAddress = "10th Avenue";

/* add column number after UPIN column */
ALTER TABLE doctor
ADD number int(9)
AFTER UPIN;

 /* remove newly added column */
ALTER TABLE doctor
DROP COLUMN number;






