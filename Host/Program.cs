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

    [ServiceContract]
    public interface IUserClient
    {
        [OperationContract(IsOneWay = true)]
        void RecieveInvitation(string usergameApplicant);

        [OperationContract(IsOneWay = true)]
        void RecieveAnswer();
    }

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

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class MemoryServer : IChatService, IRoomService, IPreGameService, IGameService, IUserService, IFriendService, IFriendRequestService, IStatisticService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<IChatClient, string> _users = new Dictionary<IChatClient, string>();
        Dictionary<IUserClient, string> usersRoom = new Dictionary<IUserClient, string>();
        Dictionary<IPreGameClient, string> usersPreGame = new Dictionary<IPreGameClient, string>();
        Dictionary<IGameClient, string> usersGame = new Dictionary<IGameClient, string>();
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

        public UserGame GetLoggerUser(string email, string password)
        {
            try
            {
                Authentication authentication = new Authentication();
                UserGame user = authentication.Login(email, password);
                return user;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }

        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public UserGame GetUserById(int idUser)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                UserGame userGame = userLogic.GetUserGameById(idUser);
                return userGame;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public UserGame GetUserByEmail(string email)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                UserGame userGame = userLogic.GetUserGameByEmail(email);
                return userGame;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public List<UserGame> GetUsersByInitialesOfNametag(string nametag)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                List<UserGame> users = userLogic.GetUsersByInitialesOfNametag(nametag);
                return users;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public List<UserGame> GetFriendsList(int idUser)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                List<UserGame> users = friendLogic.GetFriendsList(idUser);
                return users;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public List<UserGame> GetConnectedFriends(int idUser)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                List<UserGame> usersConnected = friendLogic.GetConnectedFriends(idUser);
                return usersConnected;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public bool ExistsFriendship(int idUser, int idFriend)
        {
            try
            {
                FriendLogic friendLogic = new FriendLogic();
                bool exists = friendLogic.ExistsFriendship(idUser, idFriend);
                return exists;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public List<UserGame> GetUsersRequesting(int idUser)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                List<UserGame> usersRequesting = friendRequestLogic.GetUsersRequesting(idUser);
                return usersRequesting;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public bool ExistsPendingRequest(int idApplicant, int idReceiver)
        {
            try
            {
                FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
                bool exists = friendRequestLogic.ExistsPendingRequest(idApplicant, idReceiver);
                return exists;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public String GetBackgroundUser(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                string direccionFondo = configUserLogic.GetBackgroundUser(GetConfigUserById(idUser));
                return direccionFondo;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public ConfigUser GetConfigUserById(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                return configUserLogic.GetConfigUserById(idUser);
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public void SetBackgroundUser(int idUser, int idNewBackground)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                configUserLogic.SetBackgroundUser(idUser, idNewBackground);
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }
        public bool ExistsConfigUser(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                return configUserLogic.ExistsConfigUser(idUser);
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }
        public void NewConfigUser(int idUser)
        {
            try
            {
                ConfigUserLogic configUserLogic = new ConfigUserLogic();
                configUserLogic.NewConfigUser(idUser);
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public List<StatisticUser> GetBetterUser()
        {
            try
            {
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                List<StatisticUser> users = statisticUserLogic.GetBetterUsers();
                return users;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public bool GetStatisticUser(int idUser, int numAchievement)
        {
            try
            {
                StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
                bool value = statisticUserLogic.GetStatisticUser(idUser, numAchievement);
                return value;
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }

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
            catch (DataException ex)
            {
                throw ex;
            }
        }
    }

    class Program
    {
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
