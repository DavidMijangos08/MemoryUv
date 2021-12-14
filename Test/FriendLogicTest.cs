using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows;
using static Logic.FriendLogic;

namespace Test
{

    /// <summary>
    /// Clase que permite manejar las pruebas de la lógica de la clase de FriendLogic
    /// </summary>

    [TestClass]
    public class FriendLogicTest
    {

        /// <summary>
        /// Método que realiza prueba del método para agregar un amigo
        /// </summary>

        [TestMethod]
        public void TestAddFriend()
        {
            FriendLogic friendLogic = new FriendLogic();
            Status status = friendLogic.AddFriend(2, 7);
            bool saved = false;
            if (status == Status.Success)
            {
                saved = true;
            }
            Assert.IsTrue(saved);
        }

        /// <summary>
        /// Método que realiza prueba del método para eliminar un amigo
        /// </summary>

        [TestMethod]
        public void TestDeleteFriend()
        {
            FriendLogic friendLogic = new FriendLogic();
            Status status = friendLogic.DeleteFriend(2, 7);
            bool deleted = false;
            if (status == Status.Success)
            {
                deleted = true;
            }
            Assert.IsTrue(deleted);
        }

        /// <summary>
        /// Método que realiza prueba del método para obtener la lista de amigos de un jugador
        /// </summary>

        [TestMethod]
        public void TestGetFriendsList()
        {
            FriendLogic friendLogic = new FriendLogic();
            List<UserGame> users = friendLogic.GetFriendsList(1);
            Assert.AreEqual(1, users.Count);
        }

        /// <summary>
        /// Método que realiza prueba del método para obtener la lista de amigos conectados de un jugador
        /// </summary>

        [TestMethod]
        public void TestGetConnectedFriends()
        {
            FriendLogic friendLogic = new FriendLogic();
            List<UserGame> usersConnected = friendLogic.GetConnectedFriends(1);
            Assert.AreEqual(0, usersConnected.Count);
        }

        /// <summary>
        /// Método que realiza prueba del método para verificar la existencia de una amistad
        /// </summary>

        [TestMethod]
        public void TestExistsFriendship()
        {
            FriendLogic friendLogic = new FriendLogic();
            bool exists = friendLogic.ExistsFriendship(2, 1);
            Assert.IsTrue(exists);
        }

        /// <summary>
        /// Segundo método que realiza prueba del método para verificar la existencia de una amistad pero con parametros invertidos
        /// </summary>

        [TestMethod]
        public void SecondTestExistsFriendship()
        {
            FriendLogic friendLogic = new FriendLogic();
            bool exists = friendLogic.ExistsFriendship(1, 2);
            Assert.IsTrue(exists);
        }

    }
}
