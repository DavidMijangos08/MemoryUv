using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{

    /// <summary>
    /// Clase que permite manejar la lógica de amigos para el sistema
    /// </summary>

    public class FriendLogic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Método que permite agregar una nueva amistad al juego
        /// </summary>
        /// <param name="idUser"> Se refiere al identificador del usuario que tendrá un nuevo amigo </param>
        /// <param name="idFriend"> Se refiere al identificador del usuario que será un nuevo amigo de otro usuario </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status AddFriend(int idUser, int idFriend)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    Friend friend = new Friend()
                    {
                        idUser = idUser,
                        idFriend = idFriend
                    };
                    context.Friends.Add(friend);
                    int numberChanges = context.SaveChanges();
                    if(numberChanges > 0)
                    {
                        status = Status.Success;
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return status;
        }

        /// <summary>
        /// Método que permite eliminar una amistad del juego
        /// </summary>
        /// <param name="idUser"> Identificador del usuario que quiere eliminar un amigo </param>
        /// <param name="idFriend"> Identificador del usuario a eliminar </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status DeleteFriend(int idUser, int idFriend)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from Friend in context.Friends where (Friend.idUser == idUser && Friend.idFriend == idFriend) || (Friend.idUser == idFriend && Friend.idFriend == idUser) select Friend;
                    if(coincidences.Count() > 0)
                    {
                        Friend friend = coincidences.First();
                        context.Friends.Remove(friend);
                        int numberChanges = context.SaveChanges();
                        if(numberChanges > 0)
                        {
                            status = Status.Success;
                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return status;
        }

        /// <summary>
        /// Método que permite recuperar la lista de amigos de un jugador
        /// </summary>
        /// <param name="idUser"> Identificado del usuario que desea recuperar su lista de amigos </param>
        /// <returns> Lista con los usuarios que son amigos del jugador </returns>

        public List<UserGame> GetFriendsList(int idUser)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from Friend in context.Friends where Friend.idUser == idUser || Friend.idFriend == idUser select Friend).ToList();
                    UserLogic userLogic = new UserLogic();
                    for(int i = 0; i < coincidences.Count(); i++)
                    {
                        UserGame userGame = new UserGame();
                        if(coincidences[i].idUser == idUser)
                        {
                            userGame = userLogic.GetUserGameById(coincidences[i].idFriend);
                        }
                        else
                        {
                            userGame = userLogic.GetUserGameById(coincidences[i].idUser);
                        }
                        users.Add(userGame);
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return users;
        }

        /// <summary>
        /// Método que recupera los amigos que estan activos en el juego de cierto jugador
        /// </summary>
        /// <param name="idUser"> Identificador del usuario que desea recuperar la lista de amigos conectados en el juego </param>
        /// <returns> Lista con los usuarios conectados en el juego que son amigos del jugador </returns>
 
        public List<UserGame> GetConnectedFriends(int idUser)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from Friend in context.Friends where Friend.idUser == idUser || Friend.idFriend == idUser select Friend).ToList();
                    UserLogic userLogic = new UserLogic();
                    for (int i = 0; i < coincidences.Count(); i++)
                    {
                        UserGame userGame = new UserGame();
                        if (coincidences[i].idUser == idUser)
                        {
                            userGame = userLogic.GetUserGameById(coincidences[i].idFriend);
                        }
                        else
                        {
                            userGame = userLogic.GetUserGameById(coincidences[i].idUser);
                        }

                        if (userGame.status.Equals("Activo"))
                        {
                            users.Add(userGame);
                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return users;
        }

        /// <summary>
        /// Método que verifica si existe una relación de amistad entre dos jugadores
        /// </summary>
        /// <param name="idUser"> Identificador del jugador que desea verificar la amistad </param>
        /// <param name="idFriend"> Identificador del jugador con el que se verificará la amistad </param>
        /// <returns> Booleano con el resultado de la verificación </returns>

        public bool ExistsFriendship(int idUser, int idFriend)
        {
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    var concidences = (from Friend in context.Friends where (Friend.idUser == idUser || Friend.idFriend == idUser) && (Friend.idFriend == idFriend || Friend.idUser == idFriend)
                                       select Friend).Count();
                    if(concidences > 0)
                    {
                        exists = true;
                    }
                }
            }
            catch (SystemException ex)
            {
                log.Error(ex.Message, ex);
                throw new SystemException();
            }
            return exists;
        }

        /// <summary>
        /// Enumerador que contiene dos estados (ÉXITO, FALLADO) para el retorno de diversos métodos
        /// </summary>

        public enum Status
        {
            Success = 0,
            Failed
        }
    }
}
