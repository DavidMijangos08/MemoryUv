using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestClass]
    public class StatisticUserTest
    {

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

        [TestMethod]
        public void TestGetScoreById()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            int score = statisticUserLogic.GetScoreByIdUser(5);
            Assert.AreEqual(score, 200);
        }

    }
}
