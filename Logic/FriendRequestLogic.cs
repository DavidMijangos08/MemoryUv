using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{

    /// <summary>
    /// Clase que controla la lógica de las solicitudes de amistad en el juego
    /// </summary>
    
    public class FriendRequestLogic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Método que registra una nueva solicitud de amistad en el juego
        /// </summary>
        /// <param name="idApplicant"> Identificador del usuario que desea enviar la solicitud </param>
        /// <param name="idReceiver"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status AddFriendRequest(int idApplicant, int idReceiver)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    FriendRequest friendRequest = new FriendRequest()
                    {
                        idApplicant = idApplicant,
                        idReceiver = idReceiver,
                        requestStatus = "Pendiente"
                    };
                    context.FriendRequests.Add(friendRequest);
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
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
        /// Método que permite aceptar una solicitud pendiente en el sistema
        /// </summary>
        /// <param name="idApplicant"> Identificador del usuario que envío la solicitud de amistad </param>
        /// <param name="idReceiver"> Identificador del usuario que aceptó la solicitud de amistad </param>
        /// <returns> Retorna el estado del procesamiento del método </returns>

        public Status AcceptFriendRequest(int idApplicant, int idReceiver)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from FriendRequest in context.FriendRequests where (FriendRequest.idApplicant == idApplicant && FriendRequest.idReceiver == idReceiver) && FriendRequest.requestStatus == "Pendiente" select FriendRequest;
                    if(coincidences.Count() > 0)
                    {
                        FriendRequest friendRequest = coincidences.First();
                        friendRequest.requestStatus = "Aceptada";
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
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
        /// Método que permite rechazar una solicitud pendeinte en el sistema
        /// </summary>
        /// <param name="idApplicant"> Identificador del usuario que envío la solicitud de amistad </param>
        /// <param name="idReceiver"> Identificador del usuario que desea rechazar la solicitud de amistad </param>
        /// <returns></returns>
        
        public Status RejectFriendRequest(int idApplicant, int idReceiver)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from FriendRequest in context.FriendRequests where FriendRequest.idApplicant == idApplicant && FriendRequest.idReceiver == idReceiver select FriendRequest;
                    if (coincidences.Count() > 0)
                    {
                        FriendRequest friendRequest = coincidences.First();
                        friendRequest.requestStatus = "Rechazada";
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
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
        /// Método que recupera las solicitudes de amistad pendientes de un usuario
        /// </summary>
        /// <param name="idUser"> Identificador del usuario que desea recuperar sus solicitudes de amistad pendientes </param>
        /// <returns> Lista con las solicitudes pendientes del usuario </returns>

        public List<UserGame> GetUsersRequesting(int idUser)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from FriendRequest in context.FriendRequests where FriendRequest.idReceiver == idUser && FriendRequest.requestStatus == "Pendiente" select FriendRequest).ToList();
                    UserLogic userLogic = new UserLogic();
                    for (int i = 0; i < coincidences.Count(); i++)
                    {
                        UserGame userGame = new UserGame();
                        userGame = userLogic.GetUserGameById(coincidences[i].idApplicant);
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
        /// Método que permite verificar si dos usuarios ya tienen una solicitud de amistad pendiente 
        /// </summary>
        /// <param name="idApplicant"> Identificador del usuario que requiere verificar la solicitud </param>
        /// <param name="idReceiver"> Identificador del usuario a verificar si existe una solicitud pendiente </param>
        /// <returns> Booleano con el resultado de la verificación, true si existe solicitud pendiente, de lo contrario, false </returns>

        public bool ExistsPendingRequest(int idApplicant, int idReceiver)
        {
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from FriendRequest in context.FriendRequests where ((FriendRequest.idApplicant == idApplicant && FriendRequest.idReceiver == idReceiver) || 
                                       (FriendRequest.idReceiver == idApplicant && FriendRequest.idApplicant == idReceiver)) && FriendRequest.requestStatus == "Pendiente" select FriendRequest;
                    if(coincidences.Count() > 0)
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
            Sucess = 0,
            Failed
        }
    }
}
