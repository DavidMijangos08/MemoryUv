using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    
    /// <summary>
    /// Clase que permite manejar las pruebas de la lógica de la configuración de un usuario
    /// </summary>

    [TestClass]
    public class ConfigUserTest
    {

        /// <summary>
        /// Método que realiza test al método para obtener la configuración de un usuario por su id
        /// </summary>

        [TestMethod]
        public void TestGetConfigUserById()
        {
            ConfigUserLogic configUserLogic = new ConfigUserLogic();
            ConfigUser configUser = configUserLogic.GetConfigUserById(5);
            Assert.AreEqual(0, configUser.idBackground);
        }

        /// <summary>
        /// Método que realiza test al método para obtener la ruta de fondo predeterminado del usuario
        /// </summary>

        [TestMethod]
        public void TestGetBackgroundUser()
        {
            ConfigUserLogic configUserLogic = new ConfigUserLogic();
            ConfigUser configUser = configUserLogic.GetConfigUserById(5);
            string addressBackground = configUserLogic.GetBackgroundUser(configUser);
            Assert.AreEqual(addressBackground, "Resources/Background/backgroundDefault.png");
        }

        /// <summary>
        /// Método que realiza test al método para cambiar el fondo predeterminado de un usuario
        /// </summary>

        [TestMethod]
        public void TestSetBackgroundUser()
        {
            ConfigUserLogic configUserLogic = new ConfigUserLogic();
            configUserLogic.SetBackgroundUser(7, 2);
            ConfigUser configUser = configUserLogic.GetConfigUserById(7);
            Assert.AreEqual(1, configUser.idBackground);
        }

        /// <summary>
        /// Método que realiza test al método para verificar si un usuario ya tiene creada su configuración
        /// </summary>

        [TestMethod]
        public void TestExistsConfigUser()
        {
            ConfigUserLogic configUserLogic = new ConfigUserLogic();
            bool exists = configUserLogic.ExistsConfigUser(1);
            Assert.IsTrue(exists);
        }

        /// <summary>
        /// Método que realiza test al método que agrega una nueva configuración de usuario
        /// </summary>

        [TestMethod]
        public void TestNewConfigUser()
        {
            ConfigUserLogic configUserLogic = new ConfigUserLogic();
            configUserLogic.NewConfigUser(5);
            bool exists = configUserLogic.ExistsConfigUser(8);
            Assert.IsTrue(exists);
        }
    }
}
