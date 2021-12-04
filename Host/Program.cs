using Data;
using Logic;
using System;
using Host;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using static Logic.UserLogic;
using static Logic.FriendLogic;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Host
{
    /// <summary>
    /// Interfaz correspondiente al cliente del chat global
    /// </summary>

    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(string user, string message);

        [OperationContract(IsOneWay = true)]
        void UsersUpdate(Dictionary<IChatClient, string>.ValueCollection users);
    }

    /// <summary>
    /// Interfaz correspondiente al cliente del usuario
    /// </summary>

    [ServiceContract]
    public interface IUserClient
    {
        [OperationContract(IsOneWay = true)]
        void RecieveInvitation(string usergameApplicant);

        [OperationContract(IsOneWay = true)]
        void RecieveAnswer();
    }

    /// <summary>
    /// Interfaz correspondiente al cliente del prejuego
    /// </summary>

    [ServiceContract]
    public interface IPreGameClient
    {
        [OperationContract(IsOneWay = true)]
        void UpdateUsersRoom(Dictionary<IPreGameClient, string>.ValueCollection usersPreGame);

        [OperationContract(IsOneWay = true)]
        void RecieveAccessGame();

        [OperationContract(IsOneWay = true)]
        void RecieveExitNotification(string userDisconnected);

        [OperationContract(IsOneWay = true)]
        void ReceiveConfigurationGame(string section, string difficulty);
    }

    /// <summary>
    /// Interfaz correspondiente al cliente del juego
    /// </summary>

    [ServiceContract]
    public interface IGameClient
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveExitNotification(string userDisconnected);

        [OperationContract(IsOneWay = true)]
        void ReceiveGameTurn();

        [OperationContract(IsOneWay = true)]
        void ReceiveGameBoard(int[] messyCards);

        [OperationContract(IsOneWay = true)]
        void ReceiveMove(string user, string btn);

        [OperationContract(IsOneWay = true)]
        void ReceiveCleanBoard();

        [OperationContract(IsOneWay = true)]
        void RecieveMessageInGame(string user, string message);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio del chat global
    /// </summary>

    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Join(string username);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string message);

        [OperationContract(IsOneWay = true)]
        void Leave(string username);

        [OperationContract(IsOneWay = true)]
        void PrivateSendMessage(string message, string username);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio de la sala
    /// </summary>

    [ServiceContract(CallbackContract = typeof(IUserClient))]
    public interface IRoomService
    {
        [OperationContract(IsOneWay = true)]
        void ConnectWaitingRoom(string userGameConnect);

        [OperationContract(IsOneWay = true)]
        void DisconnectRoom(string usergame);

        [OperationContract(IsOneWay = true)]
        void SendInvitation(string usergameApplicant, string usergameReceiver);

        [OperationContract(IsOneWay = true)]
        void SendAcceptance(string usergameApplicant, string usergameReceiver);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio del prejuego
    /// </summary>

    [ServiceContract(CallbackContract = typeof(IPreGameClient))]
    public interface IPreGameService
    {
        [OperationContract(IsOneWay = true)]
        void ConnectPlayer(string usergameConnected, string usergameAdmin);

        [OperationContract(IsOneWay = true)]
        void DisconnectPlayer(string status, string usergameToDisconnect, string usergameToNotify);

        [OperationContract(IsOneWay = true)]
        void SendAccessGame(string usergam1, string usergame2);

        [OperationContract(IsOneWay = true)]
        void SendExitNotification(string usergameToDisconnect, string usergameToNotify);

        [OperationContract(IsOneWay = true)]
        void SendConfigurationGame(string userToSend, string section, string difficulty);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio del juego
    /// </summary>

    [ServiceContract(CallbackContract = typeof(IGameClient))]
    public interface IGameService
    {
        [OperationContract(IsOneWay = true)]
        void ConnectToGame(string userConnected, string userOpponent);

        [OperationContract(IsOneWay = true)]
        void DisconnectTheGame(string status, string userDisconnect, string userOpponent);

        [OperationContract(IsOneWay = true)]
        void SendExitGameNotification(string userDisconnect, string userToNotify);

        [OperationContract(IsOneWay = true)]
        void SendGameTurn(string userReceiving);

        [OperationContract(IsOneWay = true)]
        void SendGameBoard(int[] messyCards, string userReceiving);

        [OperationContract(IsOneWay = true)]
        void SendMove(string userSend, string btn, string userReceiving);

        [OperationContract(IsOneWay = true)]
        void SendCleanBoard(string userReceiving);

        [OperationContract(IsOneWay = true)]
        void SendMessageInGame(string userSend, string userReceiving, string message);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio del usuario
    /// </summary>

    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserGame GetLoggerUser(string email, string password);

        [OperationContract]
        Boolean RegisterUser(string email, string password, string nametag);

        [OperationContract]
        Boolean ExistsEmail(string email);

        [OperationContract]
        Boolean ExistsNametag(string nametag);

        [OperationContract]
        String SendEmail(string email);

        [OperationContract]
        bool UpdatePassword(int idUser, string newPassword);

        [OperationContract]
        UserGame GetUserById(int idUser);

        [OperationContract]
        UserGame GetUserByEmail(string email);

        [OperationContract]
        List<UserGame> GetUsersByInitialesOfNametag(string nametag);

        [OperationContract]
        bool UpdateUserStatus(int idUser, string userStatus);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio de amigo
    /// </summary>

    [ServiceContract]
    public interface IFriendService
    {
        [OperationContract]
        bool AddFriend(int idUser, int idFriend);

        [OperationContract]
        bool DeleteFriend(int idUser, int idFriend);

        [OperationContract]
        List<UserGame> GetFriendsList(int idUser);

        [OperationContract]
        List<UserGame> GetConnectedFriends(int idUser);

        [OperationContract]
        bool ExistsFriendship(int idUser, int idFriend);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio de solicitud de amistad
    /// </summary>

    [ServiceContract]
    public interface IFriendRequestService
    {
        [OperationContract]
        bool AddFriendRequest(int idApplicant, int idReceiver);

        [OperationContract]
        bool AcceptFriendRequest(int idApplicant, int idReceiver);

        [OperationContract]
        bool RejectFriendRequest(int idApplicant, int idReceiver);

        [OperationContract]
        List<UserGame> GetUsersRequesting(int idUser);

        [OperationContract]
        bool ExistsPendingRequest(int idApplicant, int idReceiver);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio de la estadística de usuario
    /// </summary>

    [ServiceContract]
    public interface IStatisticService
    {
        [OperationContract]
        List<StatisticUser> GetBetterUser();

        [OperationContract]
        bool GetStatisticUser(int idUser, int numAchievement);

        [OperationContract]
        bool AddOneWinGame(int idUser);

        [OperationContract]
        bool AddOneLoseGame(int idUser);

        [OperationContract]
        bool AddedStatisticUser(int idUser, string nametag);
    }

    /// <summary>
    /// Interfaz correspondiente al servicio de la configuración
    /// </summary>

    [ServiceContract]
    public interface IConfigService
    {
        [OperationContract]
        string GetConfigUser();

        [OperationContract]
        ConfigUser GetConfigUserById(int idUser);

        [OperationContract]
        void SetBackgroundUser(int idUser, int idNewBackground);

        [OperationContract]
        string GetBackgroundUser(ConfigUser user);

        [OperationContract]
        void NewConfigUser(int idUser);

        [OperationContract]
        bool ExistsConfigUser(int idUser);
    }

    /// <summary>
    /// Clase correspondiente al servidor del juego MemoryUv, la cual implementa los diferentes servicios
    /// </summary>

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class MemoryServer : IChatService, IRoomService, IPreGameService, IGameService, IUserService, IFriendService, IFriendRequestService, IStatisticService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<IChatClient, string> _users = new Dictionary<IChatClient, string>();
        Dictionary<IUserClient, string> usersRoom = new Dictionary<IUserClient, string>();
        Dictionary<IPreGameClient, string> usersPreGame = new Dictionary<IPreGameClient, string>();
        Dictionary<IGameClient, string> usersGame = new Dictionary<IGameClient, string>();

        /// <summary>
        /// Método del servicio del chat que permite conectar un nuevo usuario al chat global
        /// </summary>
        /// <param name="username"> Corresponde al nombre del usuario que se conectará al chat </param>

        public void Join(string username)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
                _users[connection] = username;
                Console.WriteLine("El usuario " + username + " se conecto...");
                foreach (var userConnection in _users.Keys)
                {
                    userConnection.UsersUpdate(_users.Values);
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método del servicio del chat que permite enviar un mensaje al chat global
        /// </summary>
        /// <param name="message"> Corresponde al mensaje que se desea enviar </param>

        public void SendMessage(string message)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
                string user;
                if (!_users.TryGetValue(connection, out user))
                {
                    return;
                }
                foreach (var other in _users.Keys)
                {
                    if (other == connection)
                    {
                        other.RecieveMessage(user, message);
                    }
                    other.RecieveMessage(user, message);
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método del servicio del chat que permite desconectar a un jugador del chat global
        /// </summary>
        /// <param name="username"> Corresponde al nombre del usuario que se desconectará del chat </param>

        public void Leave(string username)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
                Console.WriteLine("El usuario " + _users[connection] + " se desconecto!");
                _users.Remove(connection);
                foreach (var userConnection in _users.Keys)
                {
                    userConnection.UsersUpdate(_users.Values);
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método del servicio del chat que permite enviar un mensaje privado en el chat global
        /// </summary>
        /// <param name="message"> Corresponde al mensaje que se desea enviar de manera privada </param>
        /// <param name="username"> Corresponde al nombre del usuario que recibirá el mensaje privado </param>

        public void PrivateSendMessage(string message, string username)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
                string user;
                string userPrivate;
                if (!_users.TryGetValue(connection, out user))
                {
                    return;
                }

                foreach (var uPrivate in _users.Keys)
                {
                    if (_users.TryGetValue(uPrivate, out userPrivate))
                    {
                        if (userPrivate == username)
                        {
                            uPrivate.RecieveMessage(user, message);
                        }

                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método del servicio de room que permite conectar a un usuario a la espera de una invitación para el juego
        /// </summary>
        /// <param name="userGameConnect"> Corresponde al usuario que está en la espera de alguna invitación </param>

        public void ConnectWaitingRoom(string userGameConnect)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IUserClient>();
                usersRoom[connection] = userGameConnect;
                Console.WriteLine("El usuario " + userGameConnect + "esta en espera de alguna invitacion");
            }catch(CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que desconecta a un usuario de la espera de alguna invitación para jugar
        /// </summary>
        /// <param name="usergame"> Corresponde al nombre del usuario que se desconectará de la espera </param>

        public void DisconnectRoom(string usergame)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IUserClient>();
                usersRoom.Remove(connection);
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía una invitación a un usuario para unirse a una sala de juego
        /// </summary>
        /// <param name="usergameApplicant"> Nombre del usuario que envía la invitación de juego </param>
        /// <param name="usergameReceiver">  Nombre del usuario que recibe la invitación de juego </param>

        public void SendInvitation(string usergameApplicant, string usergameReceiver)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IUserClient>();
                string usergame;
                string usergameReceiverAux;
                if (!usersRoom.TryGetValue(connection, out usergame))
                {
                    return;
                }

                foreach (var userReceiver in usersRoom.Keys)
                {
                    if (usersRoom.TryGetValue(userReceiver, out usergameReceiverAux))
                    {
                        if (usergameReceiverAux == usergameReceiver)
                        {
                            userReceiver.RecieveInvitation(usergameApplicant);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía la aceptación de una invitación a un juego
        /// </summary>
        /// <param name="usergameApplicant"> Nombre del usuario que envió la invitación </param>
        /// <param name="usergameReceiver"> Nombre del usuario que recibió y aceptó la invitación de juego </param>

        public void SendAcceptance(string usergameApplicant, string usergameReceiver)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IUserClient>();
                string usergame;
                string usergameApplicantAux;
                if (!usersRoom.TryGetValue(connection, out usergame))
                {
                    return;
                }

                foreach (var userApplicant in usersRoom.Keys)
                {
                    if (usersRoom.TryGetValue(userApplicant, out usergameApplicantAux))
                    {
                        if (usergameApplicantAux == usergameApplicant)
                        {
                            Console.WriteLine("El usuario " + usergameReceiver + " acepto la invitacion de " + usergameApplicant);
                            userApplicant.RecieveAnswer();
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite conectar un jugador al prejuego de una partida
        /// </summary>
        /// <param name="usergameConnected"> Corresponde al nombre del usuario que se conectará al prejuego </param>
        /// <param name="usergameAdmin">  Corresponde al nombre del usuario administrador de la partida </param>

        public void ConnectPlayer(string usergameConnected, string usergameAdmin)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IPreGameClient>();
                usersPreGame[connection] = usergameConnected;
                Console.WriteLine("El jugador " + usergameConnected + " se conecto a la sala de " + usergameAdmin);
                foreach (var userConnection in usersPreGame.Keys)
                {
                    userConnection.UpdateUsersRoom(usersPreGame.Values);
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite desconectar a un jugador de un prejuego
        /// </summary>
        /// <param name="status"> Corresponde al estado de la salida del prejuego. Permitiendo verificar si abandonó el prejuego </param>
        /// <param name="usergameToDisconnect">  Corresponde al nombre del usuario a desconectarse </param>
        /// <param name="usergameToNotify">  Corresponde al nombre del usuario que recibirá una notificación de que su oponente se retiró </param>

        public void DisconnectPlayer(string status, string usergameToDisconnect, string usergameToNotify)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IPreGameClient>();
                Console.WriteLine("El jugador " + usergameToDisconnect + " se desconectó de la sala de juego entre " + usergameToDisconnect + " y " + usergameToNotify);
                if (status.Equals("Abandonado"))
                {
                    SendExitNotification(usergameToDisconnect, usergameToNotify);
                }
                usersPreGame.Remove(connection);
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía una notificación de salida de un prejuego
        /// </summary>
        /// <param name="usergameToDisconnect"> Corresponde al nombre del usuario que se desconectó </param>
        /// <param name="usergameToNotify">  Corresponde al nombre del usuario a notificar </param>

        public void SendExitNotification(string usergameToDisconnect, string usergameToNotify)
        {
            try
            {
                string userNotify;
                var connection = OperationContext.Current.GetCallbackChannel<IPreGameClient>();

                foreach (var userToNotify in usersPreGame.Keys)
                {
                    if (usersPreGame.TryGetValue(userToNotify, out userNotify))
                    {
                        if (userNotify == usergameToNotify)
                        {
                            userToNotify.RecieveExitNotification(usergameToDisconnect);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite que el administrador envíe el acceso al juego al usuario invitado
        /// </summary>
        /// <param name="usergam1"> Corresponde al nombre del adminsitrador del juego </param>
        /// <param name="usergame2"> Corresponde al nombre del usuario invitado </param>

        public void SendAccessGame(string usergam1, string usergame2)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IPreGameClient>();
                string usergame;
                string userToAccess;
                if (!usersPreGame.TryGetValue(connection, out usergame))
                {
                    return;
                }

                foreach (var userAccess in usersPreGame.Keys)
                {
                    if (usersPreGame.TryGetValue(userAccess, out userToAccess))
                    {
                        if (userToAccess == usergame2)
                        {
                            userAccess.RecieveAccessGame();
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía la configuración de la partida al jugador invitado
        /// </summary>
        /// <param name="userToSend"> Corresponde al nombre del usuario invitado que se envía la configuración </param>
        /// <param name="section"> Corresponde a la sección del juego </param>
        /// <param name="difficulty"> Corresponde a la dificultad del juego </param>

        public void SendConfigurationGame(string userToSend, string section, string difficulty)
        {
            try
            {
                string userNotify;
                var connection = OperationContext.Current.GetCallbackChannel<IPreGameClient>();

                foreach (var userToNotify in usersPreGame.Keys)
                {
                    if (usersPreGame.TryGetValue(userToNotify, out userNotify))
                    {
                        if (userNotify == userToSend)
                        {
                            userToNotify.ReceiveConfigurationGame(section, difficulty);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite conectar un jugador a una partida 
        /// </summary>
        /// <param name="userConnected"> Corresponde al nombre del usuario que se conectará </param>
        /// <param name="userOpponent">  Corresponde al nombre del usuario oponente del usuario que se conectará </param>

        public void ConnectToGame(string userConnected, string userOpponent)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                Console.WriteLine("El jugador " + userConnected + " se conectó a la partida vs " + userOpponent);
                usersGame[connection] = userConnected;
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite desconectar a un jugador de una partida
        /// </summary>
        /// <param name="status"> Corresponde al estatus de la salida, verificando si se abandonó la partida </param>
        /// <param name="userDisconnect"> Correspodne al nombre del usuario que se desconectará de la partida </param>
        /// <param name="userOpponent"> Corresponde al nombre del usuario oponente del que se desconectará de la partida </param>

        public void DisconnectTheGame(string status, string userDisconnect, string userOpponent)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                Console.WriteLine("El jugador " + userDisconnect + " se desconectó del juego vs " + userOpponent);
                if (status.Equals("Abandonado"))
                {
                    SendExitGameNotification(userDisconnect, userOpponent);
                }
                usersGame.Remove(connection);
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía notificación de salida de un jugador, al jugador oponente
        /// </summary>
        /// <param name="userDisconnect"> Corresponde al nombre del usuario que se desconectó </param>
        /// <param name="userToNotify">  Corresponde al nombre del usuario que recibirá la notificación </param>

        public void SendExitGameNotification(string userDisconnect, string userToNotify) {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                string userNotify;
                foreach (var usergameToNotify in usersGame.Keys)
                {
                    if (usersGame.TryGetValue(usergameToNotify, out userNotify))
                    {
                        if (userNotify == userToNotify)
                        {
                            usergameToNotify.ReceiveExitNotification(userDisconnect);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía el turno del juego a otro jugador
        /// </summary>
        /// <param name="userReceiving">  Corresponde al nombre del usuario que recibirá el turno en la partida </param>

        public void SendGameTurn(string userReceiving)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                string userNotify;
                foreach (var usergameReceiving in usersGame.Keys)
                {
                    if (usersGame.TryGetValue(usergameReceiving, out userNotify))
                    {
                        if (userNotify == userReceiving)
                        {
                            usergameReceiving.ReceiveGameTurn();
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que envía el tablero de la partida al jugador invitado por parte del administrador
        /// </summary>
        /// <param name="messyCards"> Corresponde a las cartas revueltas para la partida </param>
        /// <param name="userReceiving">  Corresponde al nombre del usuario que recibirá el tablero de la partida </param>

        public void SendGameBoard(int[] messyCards, string userReceiving)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                string userNotify;
                foreach (var usergameReceiving in usersGame.Keys)
                {
                    if (usersGame.TryGetValue(usergameReceiving, out userNotify))
                    {
                        if (userNotify == userReceiving)
                        {
                            usergameReceiving.ReceiveGameBoard(messyCards);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite enviar el movimiento de un jugador al jugador oponente
        /// </summary>
        /// <param name="user"> Corresponde al usuario que hizo el movimiento </param>
        /// <param name="btn"> Corresponde al botón en el que el usuario hizo clic /param>
        /// <param name="userReceiving"> Corresponde al usuario que recibirá el movimiento </param>

        public void SendMove(string user, string btn, string userReceiving)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                string userNotify;
                foreach (var usergameReceiving in usersGame.Keys)
                {
                    if (usersGame.TryGetValue(usergameReceiving, out userNotify))
                    {
                        if (userNotify == userReceiving)
                        {
                            usergameReceiving.ReceiveMove(user, btn);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite enviar una limpieza de los movimientos que hizo el jugador en el tablero 
        /// </summary>
        /// <param name="userReceiving"> Corresponde al nombre del usuario que recibe la limpieza de su tablero </param>

        public void SendCleanBoard(string userReceiving)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                string userNotify;
                foreach (var usergameReceiving in usersGame.Keys)
                {
                    if (usersGame.TryGetValue(usergameReceiving, out userNotify))
                    {
                        if (userNotify == userReceiving)
                        {
                            usergameReceiving.ReceiveCleanBoard();
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que permite el envío de mensajes en chat dentro del juego
        /// </summary>
        /// <param name="userSend"> Corresponde al usuario que envía el mensaje en el juego </param>
        /// <param name="userReceiving"> Corresponde al usuario que recibe el mensaje </param>
        /// <param name="message"> Corresponde al mensaje que se envía </param>

        public void SendMessageInGame(string userSend, string userReceiving, string message)
        {
            try
            {
                var connection = OperationContext.Current.GetCallbackChannel<IGameClient>();
                string userNotify;
                foreach (var usergameReceiving in usersGame.Keys)
                {
                    if (usersGame.TryGetValue(usergameReceiving, out userNotify))
                    {
                        if (userNotify == userReceiving)
                        {
                            usergameReceiving.RecieveMessageInGame(userSend, message);
                        }
                    }
                }
            }
            catch (CommunicationException ex)
            {
                log.Error(ex.Message, ex);
                throw new CommunicationException();
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener un usuario
        /// </summary>
        /// <param name="email"> Corresponde al correo del usuario que se desea obtener </param>
        /// <param name="password"> Corresponde a la contraseña del usuario que se desea obtener </param>
        /// <returns> Retorna un UserGame correspondiente a el email y password de entrada </returns>

        public UserGame GetLoggerUser(string email, string password)
        {
            try
            {
                Authentication authentication = new Authentication();
                UserGame user = authentication.Login(email, password);
                return user;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para registrar un nuevo usuario al juego
        /// </summary>
        /// <param name="email"> Corresponde al correo del jugador que se desea registrar </param>
        /// <param name="password"> Corresponde a la contraseña del jugador que se desea registrar </param>
        /// <param name="nametag"> Corresponde al nametag del usuario que se desea registrar </param>
        /// <returns> Retorna un booleano con la respuesta del registro, true si fue existoso, de lo contrario, false </returns>

        public Boolean RegisterUser(string email, string password, string nametag)
        {
            try
            {
                bool saved = false;
                UserLogic userLogic = new UserLogic();
                UserLogic.Status status = userLogic.AddUser(email, password, nametag);
                if (status == UserLogic.Status.Sucess)
                {
                    saved = true;
                }
                return saved;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para verificar la existencia de un correo electrónico
        /// </summary>
        /// <param name="email"> Corresponde al correo que se verificará </param>
        /// <returns> Booleano con el resultado de la operación, true si existe, de lo contrario, false </returns>

        public bool ExistsEmail(string email)
        {
            try
            {
                bool exists = false;
                UserLogic userLogic = new UserLogic();
                VerificationStatus status = userLogic.VerifyMailExistence(email);
                if (status == VerificationStatus.Sucess)
                {
                    exists = true;
                }
                return exists;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para verificar la existencia de un nametag 
        /// </summary>
        /// <param name="nametag"> Corresponde al nametag que se verificará </param>
        /// <returns></returns>

        public bool ExistsNametag(string nametag)
        {
            try
            {
                bool exists = false;
                UserLogic userLogic = new UserLogic();
                VerificationStatus status = userLogic.VerifyNametagExistence(nametag);
                if (status == VerificationStatus.Sucess)
                {
                    exists = true;
                }
                return exists;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que envía un correo electronico con un código de verificación
        /// </summary>
        /// <param name="email"> Corresponde al correo electrónico al que se enviará el correo </param>
        /// <returns> Cadena con el código que se envió al correo </returns>

        public String SendEmail(string email)
        {
            try
            {
                Authentication authentication = new Authentication();
                String code = authentication.RandomString();
                System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
                mmsg.To.Add(email);
                mmsg.Subject = "CODE UV";
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mmsg.Body = code;
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = true;

                mmsg.From = new System.Net.Mail.MailAddress("memoryuv@gmail.com");
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("memoryuv@gmail.com", "0qwerty0");

                client.Port = 587;
                client.EnableSsl = true;

                client.Host = "smtp.gmail.com";
                client.Send(mmsg);
                return code;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para modificar la contraseña de un usuario
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario que desea modificar su contraseña </param>
        /// <param name="newPassword"> Corresponde a la nueva contraseña del usuario </param>
        /// <returns> Booleano con el resultado de la operación, true si se modificó, de lo contrario, false </returns>

        public bool UpdatePassword(int idUser, string newPassword)
        {
            try
            {
                bool updated = false;
                UserLogic userLogic = new UserLogic();
                UserLogic.Status status = userLogic.UpdatePasswordUser(idUser, newPassword);
                if (status == UserLogic.Status.Sucess)
                {
                    updated = true;
                }
                return updated;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para modificar el estado de un usuario
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del usuario que desea modificar su estado </param>
        /// <param name="userStatus"> Corresponde al estado al que pasará el usuario </param>
        /// <returns> Booleano con el resultado de la operación, true si se modificó, de lo contrario, false </returns>

        public bool UpdateUserStatus(int idUser, string userStatus)
        {
            try
            {
                bool updated = false;
                UserLogic userLogic = new UserLogic();
                UserLogic.Status status = userLogic.updateUserStatus(idUser, userStatus);
                if (status == UserLogic.Status.Sucess)
                {
                    updated = true;
                }
                return updated;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener un usuario mediante su id
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del usuario que se desea obtener </param>
        /// <returns> Variable de tipo UserGame con el usuario que coincide con la id </returns>

        public UserGame GetUserById(int idUser)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                UserGame userGame = userLogic.GetUserGameById(idUser);
                return userGame;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener un usuario de acuerdo a su correo
        /// </summary>
        /// <param name="email"> Corresponde al correo del usuario que se desea obtener </param>
        /// <returns> Variable de tipo UserGame con el usuario que coincide con el correo</returns>

        public UserGame GetUserByEmail(string email)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                UserGame userGame = userLogic.GetUserGameByEmail(email);
                return userGame;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener el o los usuarios que coincidan con ciertas iniciales de su nametag
        /// </summary>
        /// <param name="nametag"> Corresponde a las iniciales del nametag que se desea buscar </param>
        /// <returns> Lista con el o los usuarios que coincidan con las iniciales </returns>

        public List<UserGame> GetUsersByInitialesOfNametag(string nametag)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                List<UserGame> users = userLogic.GetUsersByInitialesOfNametag(nametag);
                return users;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para agregar un nuevo amigo a un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del usuario que agregará un nuevo amigo </param>
        /// <param name="idFriend"> Corresponde a la id del usuario que se agregará como amigo </param>
        /// <returns> Booleano con el resultado de la operación, true si se agregó, de lo contrario, false </returns>

        public bool AddFriend(int idUser, int idFriend)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                bool saved = false;
                FriendLogic.Status status = friendLogic.AddFriend(idUser, idFriend);
                if (status == FriendLogic.Status.Success)
                {
                    saved = true;
                }
                return saved;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para eliminar un amigo de un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario que desea eliminar un amigo </param>
        /// <param name="idFriend"> Corresponde al id del usuario que se eliminará </param>
        /// <returns> Booleano con el resultado de la operación, true si es verdadero, de lo contrario, false </returns>

        public bool DeleteFriend(int idUser, int idFriend)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                bool deleted = false;
                FriendLogic.Status status = friendLogic.DeleteFriend(idUser, idFriend);
                if (status == FriendLogic.Status.Success)
                {
                    deleted = true;
                }
                return deleted;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener la lista de amigos de un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario que desea obtener su lista de amigos </param>
        /// <returns> Lista con los usuarios amigos del jugador </returns>

        public List<UserGame> GetFriendsList(int idUser)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                List<UserGame> users = friendLogic.GetFriendsList(idUser);
                return users;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener la lista de amigos conectados de un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario que desea obtener su lista de amigos conectados </param>
        /// <returns> Lista con los usuarios conectados que son amigos del jugador </returns>

        public List<UserGame> GetConnectedFriends(int idUser)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                List<UserGame> usersConnected = friendLogic.GetConnectedFriends(idUser);
                return usersConnected;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para verificar la existencia de una amistad entre dos jugadores
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del primer jugador a verificar </param>
        /// <param name="idFriend"> Corresponde a la id del segundo jugador a verificar </param>
        /// <returns> Booleano con el resultado de la operación, true si existe, de lo contrario, false </returns>

        public bool ExistsFriendship(int idUser, int idFriend)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                bool exists = friendLogic.ExistsFriendship(idUser, idFriend);
                return exists;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para agregar una nueva solicitud de amistad
        /// </summary>
        /// <param name="idApplicant"> Corresponde a la id del jugador que desea enviar una solicitud de amistad </param>
        /// <param name="idReceiver"> Corresponde a la id del jugador que recibirá la solicitud de amistad </param>
        /// <returns> Booleano con el resultado de la operación, true si se agregó, de lo contrario, false </returns>

        public bool AddFriendRequest(int idApplicant, int idReceiver)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                bool saved = false;
                FriendRequestLogic.Status status = friendRequestLogic.AddFriendRequest(idApplicant, idReceiver);
                if (status == FriendRequestLogic.Status.Sucess)
                {
                    saved = true;
                }
                return saved;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para aceptar una solicitud de amistad
        /// </summary>
        /// <param name="idApplicant"> Corresponde a la id del jugador que envía la solicitud </param>
        /// <param name="idReceiver"> Corresponde al id del jugador que recibió la solicitud y que la aceptará </param>
        /// <returns> Booleano con el resultado de la operación, true si aceptó correctamente, de lo contrario, false </returns>

        public bool AcceptFriendRequest(int idApplicant, int idReceiver)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                bool accepted = false;
                FriendRequestLogic.Status status = friendRequestLogic.AcceptFriendRequest(idApplicant, idReceiver);
                if (status == FriendRequestLogic.Status.Sucess)
                {
                    accepted = true;
                }
                return accepted;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para rechazar una solicitud de amistad
        /// </summary>
        /// <param name="idApplicant"> Corresponde al id del usuario que envío la solicitud de amistad </param>
        /// <param name="idReceiver"> Corresponde al id del usuario que recibió la solicitud y la rechazará </param>
        /// <returns> Booleano con el resultado de la operación, true si rechazó correctamente, de lo contrario, false</returns>

        public bool RejectFriendRequest(int idApplicant, int idReceiver)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                bool rejected = false;
                FriendRequestLogic.Status status = friendRequestLogic.RejectFriendRequest(idApplicant, idReceiver);
                if (status == FriendRequestLogic.Status.Sucess)
                {
                    rejected = true;
                }
                return rejected;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener los usuarios que enviaron una solicitud de amistad a un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario que desea obtener los usuarios que le enviaron solicitud </param>
        /// <returns> Lista con los usuarios que le enviaron una solicitud de amistad al jugador </returns>

        public List<UserGame> GetUsersRequesting(int idUser)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                List<UserGame> usersRequesting = friendRequestLogic.GetUsersRequesting(idUser);
                return usersRequesting;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para verificar si existe una solicitud pendiente entre dos jugadores
        /// </summary>
        /// <param name="idApplicant"> Corresponde al id del usuario que desea enviar una solicitud de amistad </param>
        /// <param name="idReceiver"> Corresponde al id del usuario que recibirá la solicitud de amistad </param>
        /// <returns> Booleano con el resultado de la operación, true si ya existe una solicitud, de lo contrario, false </returns>

        public bool ExistsPendingRequest(int idApplicant, int idReceiver)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                bool exists = friendRequestLogic.ExistsPendingRequest(idApplicant, idReceiver);
                return exists;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener el fondo del juego del usuario
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario que desea obtener su fondo predeterminado </param>
        /// <returns> Cadena con la dirección del fondo predeterminado </returns>

        public String GetBackgroundUser(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                string addressBackground = configUserLogic.GetBackgroundUser(GetConfigUserById(idUser));
                return addressBackground;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener la configuración de un jugador 
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del jugador que desea obtener su configuración </param>
        /// <returns> Variable de tipo configuser con la configuración del usuario </returns>

        public ConfigUser GetConfigUserById(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                return configUserLogic.GetConfigUserById(idUser);
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para modificar el fondo del juego de un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del usuario que desea modificar su fondo predeterminado </param>
        /// <param name="idNewBackground"> Corresponde a la id del nuevo fondo del juego </param>

        public void SetBackgroundUser(int idUser, int idNewBackground)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                configUserLogic.SetBackgroundUser(idUser, idNewBackground);
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para verificar si existe una configuración dle usuario
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del jugador que desea verificar </param>
        /// <returns> Booleano con el resultado de la operación, true si existe, de lo contrario, false </returns>

        public bool ExistsConfigUser(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                return configUserLogic.ExistsConfigUser(idUser);
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para crear una configuración predeterminada a un usuario
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del usuario que creará su configuración predeterminada </param>

        public void NewConfigUser(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                configUserLogic.NewConfigUser(idUser);
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener los jugadores en orden de partidas ganadas
        /// </summary>
        /// <returns> Lista con los usuarios ordenados de mayor a menor partidas ganadas </returns>

        public List<StatisticUser> GetBetterUser()
        {
            try
            {
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                List<StatisticUser> users = statisticUserLogic.GetBetterUsers();
                return users;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para obtener la estadística de un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del jugador que desea obtener su estadística </param>
        /// <param name="numAchievement"> Corresponde al número de logros </param>
        /// <returns> booleano con el resultado de la operación, true si existe, de lo contrario, false </returns>

        public bool GetStatisticUser(int idUser, int numAchievement)
        {
            try
            {
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                bool value = statisticUserLogic.GetStatisticUser(idUser, numAchievement);
                return value;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para agregar un nuevo juego ganado a un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del jugador al que se le agregará el juego ganado </param>
        /// <returns> Booleano con el resultado de la operación, true si agregó, de lo contrario, false </returns>

        public bool AddOneWinGame(int idUser)
        {
            try
            {
                bool added = false;
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                StatisticUserLogic.StatisticStatus status = statisticUserLogic.IncreaseGameWon(idUser);
                if (status == StatisticUserLogic.StatisticStatus.Success)
                {
                    added = true;
                }
                return added;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para agregar un nuevo juego perdido a un jugador
        /// </summary>
        /// <param name="idUser"> Corresponde a la id del jugador al que se le agregará un nuevo juego perdido </param>
        /// <returns> Booleano con el resultado de la operación, true si agregó, de lo contrario, false </returns>

        public bool AddOneLoseGame(int idUser)
        {
            try
            {
                bool added = false;
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                StatisticUserLogic.StatisticStatus status = statisticUserLogic.IncreaseLosingGame(idUser);
                if (status == StatisticUserLogic.StatisticStatus.Success)
                {
                    added = true;
                }
                return added;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que se comunica con la lógica del juego para agregar la estadística vacía de un nuevo jugador
        /// </summary>
        /// <param name="idUser"> Corresponde al id del usuario al que se le agregará su estadística </param>
        /// <param name="nametag">Corresponde al nametag usuario al que se le agregará su estadística </param>
        /// <returns> Booleano con el resultado de la operación, true si agregó, de lo contrario, false </returns>

        public bool AddedStatisticUser(int idUser, string nametag)
        {
            try
            {
                bool added = false;
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                StatisticUserLogic.StatisticStatus status = statisticUserLogic.AddStatisticUser(idUser, nametag);
                if (status == StatisticUserLogic.StatisticStatus.Success)
                {
                    added = true;
                }
                return added;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// Corresponde a la clase principal del Host del MemoryUv
    /// </summary>
    /// 
    class Program
    {

        /// <summary>
        /// Método principal de la clase principal del HOST
        /// </summary>
        /// <param name="args"> Corresponde al parámetro predeterminado </param>
       
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Host.MemoryServer)))
            {
                host.Open();
                Console.WriteLine("Servidor listo...");
                Console.WriteLine("Presiona cualquier letra para finalizar...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
