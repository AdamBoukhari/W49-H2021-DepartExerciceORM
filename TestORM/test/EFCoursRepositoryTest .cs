using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestORM.src.DAL;
using TestORMCodeFirst.Entities;
using TestORMCodeFirst.Persistence;
using Xunit;

namespace TestORMCodeFirst.DAL
{
    public class EFCoursRepositoryTest
    {

        private EFCoursRepository repoCours;
        private EFInscCoursRepository repoInscriptions;
        private void SetUp()
        {
            // Initialiser les objets nécessaires aux tests
            var builder = new DbContextOptionsBuilder<CegepContext>();
            builder.UseInMemoryDatabase(databaseName: "testCours_db");   // Database en mémoire
            var contexte = new CegepContext(builder.Options);
            repoCours = new EFCoursRepository(contexte);
            repoInscriptions = new EFInscCoursRepository(contexte);
        }

        [Fact]
        public void TestObtenirListeCours_WhenThereIsNoCours_ShouldReturnCoursList()
        {
            // Arrange
            SetUp();
           

            // Act
            var result = repoCours.ObtenirListeCours();

            // Assert
            Assert.Equal(result.Count(), 0);
        }

        [Fact]
        public void TestAjouterCours_ShouldAddCoursInDataBase()
        {
            // Arrange
            SetUp();
            Cours crs = new Cours
            {
                CodeCours = "Praxis",
                NomCours = "ZeBest",
            };

            // Act
            repoCours.AjouterCours(crs);

            // Assert
            var result = this.repoCours.ObtenirListeCours();
            Assert.Single(result);
            Assert.Same(crs, result.First());
        }

        [Fact]
        public void TestAjouterCours_ShouldAddManyCoursInDataBase()
        {
            // Arrange
            SetUp();
            Cours crs = new Cours
            {
                CodeCours = "Praxis",
                NomCours = "ZeBest",
            };

            Cours crs2 = new Cours
            {
                CodeCours = "s",
                NomCours = "ZeBest",
            };

            // Act
            repoCours.AjouterCours(crs);
            repoCours.AjouterCours(crs2);
            // Assert
            var result = this.repoCours.ObtenirListeCours();
            Assert.Equal(2, result.Count());
            Assert.Same(crs, result.First());
        }
    }
}
