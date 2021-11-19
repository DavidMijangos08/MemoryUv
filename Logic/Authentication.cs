using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Clase lógica para la autenticación de usuarios en el sistema
    /// </summary>
    public partial class Authentication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor vacío de la clase Authentication
        /// </summary>
        public Authentication()
        {
        }

        /// <summary>
        /// Método para recuperar un usuario del sistema
        /// </summary>
        /// <param name="email"> Corresponde al correo del usuario a recuperar </param>
        /// <param name="password"> Corresponde a la contraseña del usuario a recuperar </param>
        /// <returns> El método retorna el usuario recuperado que corresponde a los parametros </returns>

        public UserGame Login(string email, string password)
        {
            string passwordHashed = ComputeSHA256Hash(password);
            UserGame user = new UserGame();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.email == email && UserGame.password == passwordHashed select UserGame;
                    if(coincidences.Count() > 0)
                    {
                        user = coincidences.First();
                    }
                    else
                    {
                        user = null;
                    }
                }
            } catch (DbException ex)
            {
                log.Error("Error en login", ex);
              //  throw new DbException();
            }
            return user;
        }

        /// <summary>
        /// Método que permite hashear una cadena a SHA256
        /// </summary>
        /// <param name="input"> Es la cadena a hashear </param>
        /// <returns> El método retorna la cadena hasheada </returns>

        public string ComputeSHA256Hash(string input)
        {   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static Random random = new Random();

        /// <summary>
        /// Método que permite crear una cadena con caracteres definidos aleatorios
        /// </summary>
        /// <returns> Retorna la cadena con 4 caracteres aleatorios </returns>

        public string RandomString()
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
