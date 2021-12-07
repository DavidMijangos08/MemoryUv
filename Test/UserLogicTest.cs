using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using static Logic.UserLogic;

namespace Test
{

    /// <summary>
    /// Clase que permite manejar las pruebas a la lógica de la clase UserLogic
    /// </summary>

    [TestClass]
    public class UserLogicTest
    {
       
        /// <summary>
        /// Método que realiza prueba al método para registrar un nuevo usuario
        /// </summary>

        [TestMethod]
        public void TestAddUser()
        {
            UserLogic userLogic = new UserLogic();
            Status status = userLogic.AddUser("jose2@gmail.com", "JoseJose1", "Jose");
            bool saved = false;
            if (status == Status.Sucess)
            {
                saved = true;
            }
            Assert.IsTrue(saved);
        }

        /// <summary>
        /// Método que realiza prueba al método para actualizar la contraseña de un usuario
        /// </summary>

        [TestMethod]
        public void TestUpdatePasswordUser()
        {
            UserLogic userLogic = new UserLogic();
            Status status = userLogic.UpdatePasswordUser(1, "David1234");
            bool modified = false;
            if (status == Status.Sucess)
            {
                modified = true;
            }
            Assert.IsTrue(modified);
        }

        /// <summary>
        /// Método que realiza prueba al método para actualizar el estado de un usuario
        /// </summary>

        [TestMethod]
        public void TestUpdateUserStatus()
        {
            UserLogic userLogic = new UserLogic();
            Status status = userLogic.UpdateUserStatus(1, "Inactivo");
            bool modified = false;
            if (status == Status.Sucess)
            {
                modified = true;
            }
            Assert.IsTrue(modified);
        }

        /// <summary>
        /// Método que realiza prueba al método para obtener un usuario mediante su identificador
        /// </summary>

        [TestMethod]
        public void TestGetUserGameById()
        {
            UserLogic userLogic = new UserLogic();
            UserGame userGame = userLogic.GetUserGameById(1);
            Assert.AreEqual("david", userGame.nametag);
        }

        /// <summary>
        /// Método que realiza prueba al método para obtener un usuario mediante su correo
        /// </summary>

        [TestMethod]
        public void TestGetUserGameByEmail()
        {
            UserLogic userLogic = new UserLogic();
            UserGame userGame = userLogic.GetUserGameByEmail("david.mijangos@gmail.com");
            Assert.AreEqual("david", userGame.nametag);
        }

        /// <summary>
        /// Método que realiza prueba al método para verificar la existencia de un correo en el sistema
        /// </summary>

        [TestMethod]
        public void TestVerifyMailExisting()
        {
            UserLogic userLogic = new UserLogic();
            string email = "david.mijangos.08@gmail.com";
            VerificationStatus status = userLogic.VerifyMailExistence(email);
            bool exists = false;
            if (status == VerificationStatus.Sucess)
            {
                exists = true;
            }
            Assert.IsTrue(exists);
            ;
        }

        /// <summary>
        /// Método que realiza prueba al método para obtener los usuarios que coincidan con ciertas iniciales
        /// </summary>

        [TestMethod]
        public void TestGetUsersByInitialesOfNametag()
        {
            UserLogic userLogic = new UserLogic();
            List<UserGame> users = userLogic.GetUsersByInitialesOfNametag("M");
            Assert.AreEqual(users.Count, 2);
        }

        /// <summary>
        /// Método que realiza prueba al método para verificar la existencia de un nametag que no está en el sistema
        /// </summary>

        [TestMethod]
        public void TestVerifyMailNonExistent()
        {
            UserLogic userLogic = new UserLogic();
            VerificationStatus status = userLogic.VerifyMailExistence("correoQueNoExiste@gmail.com");
            bool exists = false;
            if (status == VerificationStatus.Sucess)
            {
                exists = true;
            }
            Assert.IsFalse(exists);
        }

        /// <summary>
        /// Método que realiza prueba al método para verificar la existencia de un nametag que está en el sistema 
        /// </summary>

        [TestMethod]
        public void TestVerifyNametagExistence()
        {
            UserLogic userLogic = new UserLogic();
            VerificationStatus status = userLogic.VerifyNametagExistence("david");
            bool exists = false;
            if (status == VerificationStatus.Sucess)
            {
                exists = true;
            }
            Assert.IsTrue(exists);
        }
    }
}
