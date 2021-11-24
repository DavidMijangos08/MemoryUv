using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    /// <summary>
    /// Clase que permite manejar las pruebas para la lógica de las estadisticas de un usuario
    /// </summary>

    [TestClass]
    public class StatisticUserTest
    {

        /// <summary>
        /// Método que realiza prueba al método para obtener los mejores jugadores del juego
        /// </summary>

        [TestMethod]
        public void TestGetBetterUsers()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            List<StatisticUser> statisticUsers = statisticUserLogic.GetBetterUsers();
            bool isOrdered = false;
            if((statisticUsers[0].totalWins > statisticUsers[1].totalWins) && (statisticUsers[1].totalWins > statisticUsers[2].totalWins) && (statisticUsers[2].totalWins > statisticUsers[3].totalWins))
            {
                isOrdered = true;
            }
            Assert.IsTrue(isOrdered);
        }

        /// <summary>
        /// Método que realiza prueba al método para agregar una nueva estadistica en ceros al usuario recien creado
        /// </summary>

        [TestMethod]
        public void TestAddStatisticUser()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            StatisticUserLogic.StatisticStatus status = statisticUserLogic.AddStatisticUser(3, "Jose=");
            bool added = false;
            if(status == StatisticUserLogic.StatisticStatus.Success)
            {
                added = true;
            }
            Assert.IsTrue(added);
        }

        /// <summary>
        /// Método que realiza prueba al método para incrementar una partida ganada al jugador
        /// </summary>

        [TestMethod]
        public void TestIncreaseGameWon()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            StatisticUserLogic.StatisticStatus status = statisticUserLogic.IncreaseGameWon(1, 25);
            bool added = false;
            if (status == StatisticUserLogic.StatisticStatus.Success)
            {
                added = true;
            }
            Assert.IsTrue(added);
        }

        /// <summary>
        /// Método que realiza prueba al método para incrementar una partida perdida al jugador
        /// </summary>

        [TestMethod]
        public void TestIncreaseLosingGame()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            StatisticUserLogic.StatisticStatus status = statisticUserLogic.IncreaseLosingGame(1, 25);
            bool added = false;
            if (status == StatisticUserLogic.StatisticStatus.Success)
            {
                added = true;
            }
            Assert.IsTrue(added);
        }

        /// <summary>
        /// Método que realiza prueba al método para obtener el score de un jugador mediante su id
        /// </summary>

        [TestMethod]
        public void TestGetScoreById()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            int score = statisticUserLogic.GetScoreByIdUser(5);
            Assert.AreEqual(score, 200);
        }

    }
}
