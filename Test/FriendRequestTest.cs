using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logic.FriendRequestLogic;

namespace Test
{

    /// <summary>
    /// Clase que permite manejar las pruebas a la lógica de la clase FriendRequest
    /// </summary>

    [TestClass]
    public class FriendRequestTest
    {

        /// <summary>
        /// Método que realiza prueba del método para agregar una nueva solicitud de amistad
        /// </summary>

        [TestMethod]
        public void TestAddFriendRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            Status status = friendRequestLogic.AddFriendRequest(8, 7);
            bool saved = false;
            if(status == Status.Sucess)
            {
                saved = true;
            }
            Assert.IsTrue(saved);
        }

        /// <summary>
        /// Método que realiza prueba al método para aceptar una solicitud de amistad
        /// </summary>

        [TestMethod]
        public void TestAcceptFriendRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            Status status = friendRequestLogic.AcceptFriendRequest(2, 7);
            bool accepted = false;
            if (status == Status.Sucess)
            {
                accepted = true;
            }
            Assert.IsTrue(accepted);
        }

        /// <summary>
        /// Método que realiza prueba al método para rechazar una solicitud de amistad
        /// </summary>

        [TestMethod]
        public void TestRejectFriendRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            Status status = friendRequestLogic.RejectFriendRequest(1, 7);
            bool rejected = false;
            if (status == Status.Sucess)
            {
                rejected = true;
            }
            Assert.IsTrue(rejected);
        }

        /// <summary>
        /// Método que realiza prueba al método para obtener los usuarios que solicitan una amistad
        /// </summary>

        [TestMethod]
        public void TestGetUsersRequesting()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            List<UserGame> usersRequesting = friendRequestLogic.GetUsersRequesting(6);
            Assert.AreEqual(usersRequesting.Count(), 2);
        }

        /// <summary>
        /// Método que realiza prueba al método para verificar la existencia de una solicitud pendiente entre dos usuarios
        /// </summary>

        [TestMethod]
        public void TestExistsPendingRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            bool exists = friendRequestLogic.ExistsPendingRequest(1, 6);
            Assert.IsTrue(exists);
        }
    }
}
