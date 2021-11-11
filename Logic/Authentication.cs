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
            } catch (DbException)
            {

            }
            return user;
        }

        public string ComputeSHA256Hash(string input)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }       

        private static Random random = new Random();
        public string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
