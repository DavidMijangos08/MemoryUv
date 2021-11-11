using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{

    /// <summary>
    /// Clase que permite controlar la lógica del usuario en el sistema
    /// </summary>
    public class UserLogic
    {

        /// <summary>
        /// Método que permite crear un nuevo usuario en el sistema
        /// </summary>
        /// <param name="email"> Corresponde al correo electrónico verificado del nuevo jugador </param>
        /// <param name="password"> Corresponde a la contraseña del nuevo jugador </param>
        /// <param name="nametag"> Corresponde al nametag del nuevo jugador </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status AddUser(string email, string password, string nametag)
        {
            Authentication authentication = new Authentication();
            string passwordHashed = authentication.ComputeSHA256Hash(password);
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    UserGame userGame = new UserGame()
                    {
                        email = email,
                        password = passwordHashed,
                        nametag = nametag
                    };
                    context.UsersGame.Add(userGame);
                    int numberChanges = context.SaveChanges();
                    if(numberChanges > 0)
                    {
                        status = Status.Sucess;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        /// <summary>
        /// Método que permite modificar la contraseña de un usuario
        /// </summary>
        /// <param name="idUser"> Identificador del usuario al cual se le modificará la contraseña </param>
        /// <param name="newPassword"> Corresponde a la nueva contraseña del usuario </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status UpdatePasswordUser(int idUser, string newPassword)
        {
            Status status = Status.Failed;
            Authentication authentication = new Authentication();
            string newPasswordHashed = authentication.ComputeSHA256Hash(newPassword);
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.id == idUser select UserGame;
                    if(coincidences.Count() > 0)
                    {
                        UserGame userGame = coincidences.First();
                        userGame.password = newPasswordHashed;
                    }
                    int numberChanges = context.SaveChanges();
                    if(numberChanges > 0)
                    {
                        status = Status.Sucess;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        /// <summary>
        /// Método que permite modificar el estado de un jugador 
        /// </summary>
        /// <param name="idUser"> Identificador del usuario al que se le modificará el estado </param>
        /// <param name="userStatus"> Corresponde el estado al que pasará el jugador </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status updateUserStatus(int idUser, string userStatus) 
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.id == idUser select UserGame;
                    if (coincidences.Count() > 0)
                    {
                        UserGame userGame = coincidences.First();
                        userGame.status = userStatus;
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        /// <summary>
        /// Método que recupera un usuario de acuerdo a su identificador personal
        /// </summary>
        /// <param name="idUser"> Identificador del usuario que se desea recuperar </param>
        /// <returns> Usuario que se recuperó de acuerdo al identificador </returns>

        public UserGame GetUserGameById(int idUser)
        {
            UserGame userGame = new UserGame();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.id == idUser select UserGame;
                    if(coincidences.Count() > 0)
                    {
                        userGame = coincidences.First();
                    }
                    else
                    {
                        userGame = null;
                    }
                }
            }
            catch (DbException)
            {

            }
            return userGame;
        }

        /// <summary>
        /// Método que recupera un usuario de acuerdo a su correo electrónico
        /// </summary>
        /// <param name="email"> Correo electrónico del usuario a recuperar </param>
        /// <returns> Usuario que coincide con el correo electrónico </returns>

        public UserGame GetUserGameByEmail(string email)
        {
            UserGame userGame = null;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.email == email select UserGame;
                    if (coincidences.Count() > 0)
                    {
                        userGame = coincidences.First();
                    }
                }
            }
            catch (DbException)
            {

            }
            return userGame;
        }

        /// <summary>
        /// Método que recupera una lista con los usuarios que coincidan su nametag con ciertas iniciales 
        /// </summary>
        /// <param name="nametag"> Corresponde a las iniciales del nametag o nametag completo de el o los usuarios a recuperar </param>
        /// <returns> Lista con los usuarios recuperados de acuerdo a las iniciales del nametag </returns>

        public List<UserGame> GetUsersByInitialesOfNametag(string nametag)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from UserGame in context.UsersGame where UserGame.nametag.StartsWith(nametag) select UserGame).ToList();
                    users = coincidences;
                } 
            }
            catch (DbException)
            {

            }
            return users;
        }

        /// <summary>
        /// Método que verifica si un correo ya está registrado en el sistema
        /// </summary>
        /// <param name="email"> Correo electrónico a verificar </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public VerificationStatus VerifyMailExistence(string email)
        {
            VerificationStatus status = VerificationStatus.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from UserGame in context.UsersGame
                                        where UserGame.email == email
                                        select UserGame).Count();
                    if (coincidences > 0)
                    {
                        status = VerificationStatus.Sucess;
                    }
                }
            }
            catch (DbException )
            {

            }
            
            return status;
        }

        /// <summary>
        /// Método que verifica si un nametag ya está registrado en el sistema
        /// </summary>
        /// <param name="nametag"> Corresponde al nametag a verificar </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public VerificationStatus VerifyNametagExistence(string nametag)
        {
            VerificationStatus status = VerificationStatus.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from UserGame in context.UsersGame
                                        where UserGame.nametag == nametag
                                        select UserGame).Count();
                    if (coincidences > 0)
                    {
                        status = VerificationStatus.Sucess;
                    }
                }
            } catch (DbException)
            {

            }
            
            return status;
        }

        /// <summary>
        /// Enumerador que contiene dos estados (ÉXITO, FALLADO) para el retorno de diversos métodos
        /// </summary>

        public enum Status
        {
            Sucess = 0, 
            Failed
        }

        /// <summary>
        /// Enumerador que contiene dos estados (ÉXITO, FALLADO) para el retorno de diversos métodos de verificación
        /// </summary>

        public enum VerificationStatus
        {
            Sucess = 0,
            Failed
        }
    }
}
