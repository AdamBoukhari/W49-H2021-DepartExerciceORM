using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestORM.src.DAL;
using TestORMCodeFirst.DAL;
using TestORMCodeFirst.Entities;
using TestORMCodeFirst.Persistence;
using Xunit;

namespace TestORMCodeFirst.DAL
{
    public class EFInsCoursRepositoryTest
    {
        private EFInscCoursRepository repoInscriptions;
        private EFEtudiantRepository repoEtudiants;
        private EFCoursRepository repoCours;


        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<CegepContext>();
            builder.UseInMemoryDatabase(databaseName: "testInscription_db");   // Database en mémoire
            var context = new CegepContext(builder.Options);
            repoInscriptions = new EFInscCoursRepository(context);
            repoEtudiants = new EFEtudiantRepository(context);
            repoCours = new EFCoursRepository(context);
        }

        [Fact]
        public void AjouterInscription()
        {
            // Arrange
            SetUp();
            Etudiant etud = new Etudiant { Nom = "Simard", Prenom = "Serge", DateNaissance = Convert.ToDateTime("1997-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud);
            string session = "H21";


            // Act
            repoInscriptions.AjouterInscription(etud.EtudiantID, "W49", session);

            // Assert
            var result = repoInscriptions.ObtenirInscriptions();
            Assert.Single(result);
            Assert.Same(etud, result.First().Etudiant);
            Assert.Same("W49", result.First().CodeCours);
            Assert.Equal(session, result.First().CodeSession);
        }

        private void DataSeed()
        {
            Etudiant etud1 = new Etudiant { Nom = "Deshaies", Prenom = "Yvan", DateNaissance = Convert.ToDateTime("1977-10-10"), NoProgramme = 420 };
            Etudiant etud2 = new Etudiant { Nom = "Simard", Prenom = "Serge", DateNaissance = Convert.ToDateTime("1980-10-10"), NoProgramme = 420 };
            Etudiant etud3 = new Etudiant { Nom = "Gingras", Prenom = "Karine", DateNaissance = Convert.ToDateTime("1977-10-10"), NoProgramme = 420 };
            Etudiant etud4 = new Etudiant { Nom = "Doré", Prenom = "Hélène", DateNaissance = Convert.ToDateTime("1992-10-10"), NoProgramme = 420 };
            Etudiant etud5 = new Etudiant { Nom = "Gingras", Prenom = "Karine", DateNaissance = Convert.ToDateTime("1977-10-10"), NoProgramme = 420 };
            Etudiant etud6 = new Etudiant { Nom = "Huot", Prenom = "Alain", DateNaissance = Convert.ToDateTime("1977-10-10"), NoProgramme = 420 };
            Etudiant etud7 = new Etudiant { Nom = "Talbot", Prenom = "Jo", DateNaissance = Convert.ToDateTime("1977-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoEtudiants.AjouterEtudiant(etud2);
            repoEtudiants.AjouterEtudiant(etud3);
            repoEtudiants.AjouterEtudiant(etud4);
            repoEtudiants.AjouterEtudiant(etud5);
            repoEtudiants.AjouterEtudiant(etud6);
            repoEtudiants.AjouterEtudiant(etud7);


            Cours cr1 = new Cours { CodeCours = "W49", NomCours = "Chronophage1" };
            Cours cr2 = new Cours { CodeCours = "W40", NomCours = "Chronophage2" };

            repoCours.AjouterCours(cr1);
            repoCours.AjouterCours(cr2);



            string sessionH20 = "H20";
            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", sessionH20);
            repoInscriptions.AjouterInscription(etud2.EtudiantID, "W49", sessionH20);
            repoInscriptions.AjouterInscription(etud3.EtudiantID, "W49", sessionH20);
            repoInscriptions.AjouterInscription(etud4.EtudiantID, "W49", sessionH20);
            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W40", sessionH20);
            repoInscriptions.AjouterInscription(etud5.EtudiantID, "W40", sessionH20);

            string sessionH21 = "H21";
            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", sessionH21);
            repoInscriptions.AjouterInscription(etud2.EtudiantID, "W49", sessionH21);
            repoInscriptions.AjouterInscription(etud6.EtudiantID, "W49", sessionH21);
        }

        [Fact]
        public void SupprimerToutesLesInscriptions()
        {
            // Arrange
            SetUp();
            DataSeed();

            // Act
            repoInscriptions.SupprimerToutesLesInscriptions();

            // Assert
            var result = repoInscriptions.ObtenirInscriptions();
            Assert.Empty(result);
        }


        [Fact]
        public void NombreEtudiantsInscritsPourUneSession_QuandAucunCours()
        {
            // Arrange
            SetUp();
            DataSeed();

            //Act
            int NbInscriptions = repoInscriptions.NombreEtudiantsInscritsAuCegep("H22");

            // Assert
            Assert.Equal(0, NbInscriptions);
        }

        [Fact]
        public void NombreEtudiantsInscritsPourUneSession_QuandUnCours()
        {
            // Arrange
            SetUp();
            DataSeed();

            //Act
            int NbInscriptions = repoInscriptions.NombreEtudiantsInscritsAuCegep("H21");

            // Assert
            Assert.Equal(3, NbInscriptions);
        }

        [Fact]
        public void NombreEtudiantsInscritsPourUneSession_QuandPlusieursCours()
        {
            // Arrange
            SetUp();
            DataSeed();

            //Act
            int NbInscriptions = repoInscriptions.NombreEtudiantsInscritsAuCegep("H20");

            // Assert
            Assert.Equal(5, NbInscriptions);
        }

        [Fact]
        public void TestMiseAJourNoteFinal_ShouldChangeNoteFinale()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H20");

            //Act
            short Expected = 59;
            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H20", Expected);

            // Assert
            Assert.Equal(Expected, repoInscriptions.ObtenirInscription(etud1.EtudiantID, "W49", "H20").NoteFinale);
        }

        [Fact]
        public void TestMiseAJourNoteFinal_ShouldChangeNoteFinaleWhenItsAlreadyChanged()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H20");

            //Act
            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H20", 2);
            short Expected = 99;
            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H20", Expected);

            // Assert
            Assert.Equal(Expected, repoInscriptions.ObtenirInscription(etud1.EtudiantID, "W49", "H20").NoteFinale);
        }


        [Fact]
        public void TestObtenirMoyenneClass_WhenThereAreManyStudents_shouldReturnAverage()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud2 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud3 = new Etudiant { Nom = "stoo", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud4 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoEtudiants.AjouterEtudiant(etud2);
            repoEtudiants.AjouterEtudiant(etud3);
            repoEtudiants.AjouterEtudiant(etud4);

            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud2.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud3.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud4.EtudiantID, "W49", "H22");

          
           
            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H22", 40);
            repoInscriptions.MettreAJourNoteFinale(etud2.EtudiantID, "W49", "H22", 80);
            repoInscriptions.MettreAJourNoteFinale(etud3.EtudiantID, "W49", "H22", 80);
            repoInscriptions.MettreAJourNoteFinale(etud4.EtudiantID, "W49", "H22", 40);
            // Assert
            Assert.Equal(60, repoInscriptions.ObtenirPourUneClasseLaMoyenne("W49", "H22"));
        }

        [Fact]
        public void TestObtenirMoyenneClass_WhenThereIsOneStudent_shouldReturnStudentsNoteFinal()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
           
            repoEtudiants.AjouterEtudiant(etud1);
           
            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H22");
           



            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H22", 40);
            
            // Assert
            Assert.Equal(40, repoInscriptions.ObtenirPourUneClasseLaMoyenne("W49", "H22"));
        }

        

        [Fact]
        public void TestObtenirNombreEchec_WhenThereAreNoFailure_shouldReturnZero()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud2 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud3 = new Etudiant { Nom = "stoo", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud4 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoEtudiants.AjouterEtudiant(etud2);
            repoEtudiants.AjouterEtudiant(etud3);
            repoEtudiants.AjouterEtudiant(etud4);

            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud2.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud3.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud4.EtudiantID, "W49", "H22");



            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H22", 60);
            repoInscriptions.MettreAJourNoteFinale(etud2.EtudiantID, "W49", "H22", 80);
            repoInscriptions.MettreAJourNoteFinale(etud3.EtudiantID, "W49", "H22", 80);
            repoInscriptions.MettreAJourNoteFinale(etud4.EtudiantID, "W49", "H22", 60);
            // Assert
            Assert.Equal(0, repoInscriptions.ObtenirPourUneClasseNombreEchecs("W49", "H22"));
        }

        public void TestObtenirNombreEchec_WhenThereAreOneFailure_shouldReturnOne()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud2 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud3 = new Etudiant { Nom = "stoo", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud4 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoEtudiants.AjouterEtudiant(etud2);
            repoEtudiants.AjouterEtudiant(etud3);
            repoEtudiants.AjouterEtudiant(etud4);

            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud2.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud3.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud4.EtudiantID, "W49", "H22");



            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H22", 59);
            repoInscriptions.MettreAJourNoteFinale(etud2.EtudiantID, "W49", "H22", 80);
            repoInscriptions.MettreAJourNoteFinale(etud3.EtudiantID, "W49", "H22", 80);
            repoInscriptions.MettreAJourNoteFinale(etud4.EtudiantID, "W49", "H22", 60);
            // Assert
            Assert.Equal(1, repoInscriptions.ObtenirPourUneClasseNombreEchecs("W49", "H22"));
        }

        public void TestObtenirNombreEchec_WhenThereAreManyFailures_shouldReturnMoreThanOne()
        {
            // Arrange
            SetUp();
            DataSeed();
            Etudiant etud1 = new Etudiant { Nom = "stoo", Prenom = "peed", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud2 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud3 = new Etudiant { Nom = "stoo", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            Etudiant etud4 = new Etudiant { Nom = "stu", Prenom = "pid", DateNaissance = Convert.ToDateTime("1978-10-10"), NoProgramme = 420 };
            repoEtudiants.AjouterEtudiant(etud1);
            repoEtudiants.AjouterEtudiant(etud2);
            repoEtudiants.AjouterEtudiant(etud3);
            repoEtudiants.AjouterEtudiant(etud4);

            repoInscriptions.AjouterInscription(etud1.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud2.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud3.EtudiantID, "W49", "H22");
            repoInscriptions.AjouterInscription(etud4.EtudiantID, "W49", "H22");



            repoInscriptions.MettreAJourNoteFinale(etud1.EtudiantID, "W49", "H22", 59);
            repoInscriptions.MettreAJourNoteFinale(etud2.EtudiantID, "W49", "H22", 50);
            repoInscriptions.MettreAJourNoteFinale(etud3.EtudiantID, "W49", "H22", 40);
            repoInscriptions.MettreAJourNoteFinale(etud4.EtudiantID, "W49", "H22", 60);
            // Assert
            Assert.Equal(3, repoInscriptions.ObtenirPourUneClasseNombreEchecs("W49", "H22"));
        }
    }
}
