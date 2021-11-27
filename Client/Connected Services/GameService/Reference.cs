﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.GameService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameService.IChatService", CallbackContract=typeof(Client.GameService.IChatServiceCallback))]
    public interface IChatService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Join")]
        void Join(string username);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Join")]
        System.Threading.Tasks.Task JoinAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/SendMessage")]
        void SendMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/SendMessage")]
        System.Threading.Tasks.Task SendMessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Leave")]
        void Leave(string username);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Leave")]
        System.Threading.Tasks.Task LeaveAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/PrivateSendMessage")]
        void PrivateSendMessage(string message, string username);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/PrivateSendMessage")]
        System.Threading.Tasks.Task PrivateSendMessageAsync(string message, string username);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/RecieveMessage")]
        void RecieveMessage(string user, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/UsersUpdate")]
        void UsersUpdate(System.Collections.Generic.Dictionary<object, string>.ValueCollection users);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceChannel : Client.GameService.IChatService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatServiceClient : System.ServiceModel.DuplexClientBase<Client.GameService.IChatService>, Client.GameService.IChatService {
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Join(string username) {
            base.Channel.Join(username);
        }
        
        public System.Threading.Tasks.Task JoinAsync(string username) {
            return base.Channel.JoinAsync(username);
        }
        
        public void SendMessage(string message) {
            base.Channel.SendMessage(message);
        }
        
        public System.Threading.Tasks.Task SendMessageAsync(string message) {
            return base.Channel.SendMessageAsync(message);
        }
        
        public void Leave(string username) {
            base.Channel.Leave(username);
        }
        
        public System.Threading.Tasks.Task LeaveAsync(string username) {
            return base.Channel.LeaveAsync(username);
        }
        
        public void PrivateSendMessage(string message, string username) {
            base.Channel.PrivateSendMessage(message, username);
        }
        
        public System.Threading.Tasks.Task PrivateSendMessageAsync(string message, string username) {
            return base.Channel.PrivateSendMessageAsync(message, username);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameService.IRoomService", CallbackContract=typeof(Client.GameService.IRoomServiceCallback))]
    public interface IRoomService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/ConnectWaitingRoom")]
        void ConnectWaitingRoom(string userGameConnect);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/ConnectWaitingRoom")]
        System.Threading.Tasks.Task ConnectWaitingRoomAsync(string userGameConnect);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/DisconnectRoom")]
        void DisconnectRoom(string usergame);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/DisconnectRoom")]
        System.Threading.Tasks.Task DisconnectRoomAsync(string usergame);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/SendInvitation")]
        void SendInvitation(string usergameApplicant, string usergameReceiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/SendInvitation")]
        System.Threading.Tasks.Task SendInvitationAsync(string usergameApplicant, string usergameReceiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/SendAcceptance")]
        void SendAcceptance(string usergameApplicant, string usergameReceiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/SendAcceptance")]
        System.Threading.Tasks.Task SendAcceptanceAsync(string usergameApplicant, string usergameReceiver);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRoomServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/RecieveInvitation")]
        void RecieveInvitation(string usergameApplicant);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRoomService/RecieveAnswer")]
        void RecieveAnswer();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRoomServiceChannel : Client.GameService.IRoomService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RoomServiceClient : System.ServiceModel.DuplexClientBase<Client.GameService.IRoomService>, Client.GameService.IRoomService {
        
        public RoomServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public RoomServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public RoomServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public RoomServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public RoomServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void ConnectWaitingRoom(string userGameConnect) {
            base.Channel.ConnectWaitingRoom(userGameConnect);
        }
        
        public System.Threading.Tasks.Task ConnectWaitingRoomAsync(string userGameConnect) {
            return base.Channel.ConnectWaitingRoomAsync(userGameConnect);
        }
        
        public void DisconnectRoom(string usergame) {
            base.Channel.DisconnectRoom(usergame);
        }
        
        public System.Threading.Tasks.Task DisconnectRoomAsync(string usergame) {
            return base.Channel.DisconnectRoomAsync(usergame);
        }
        
        public void SendInvitation(string usergameApplicant, string usergameReceiver) {
            base.Channel.SendInvitation(usergameApplicant, usergameReceiver);
        }
        
        public System.Threading.Tasks.Task SendInvitationAsync(string usergameApplicant, string usergameReceiver) {
            return base.Channel.SendInvitationAsync(usergameApplicant, usergameReceiver);
        }
        
        public void SendAcceptance(string usergameApplicant, string usergameReceiver) {
            base.Channel.SendAcceptance(usergameApplicant, usergameReceiver);
        }
        
        public System.Threading.Tasks.Task SendAcceptanceAsync(string usergameApplicant, string usergameReceiver) {
            return base.Channel.SendAcceptanceAsync(usergameApplicant, usergameReceiver);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameService.IPreGameService", CallbackContract=typeof(Client.GameService.IPreGameServiceCallback))]
    public interface IPreGameService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/ConnectPlayer")]
        void ConnectPlayer(string usergameConnected, string usergameAdmin);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/ConnectPlayer")]
        System.Threading.Tasks.Task ConnectPlayerAsync(string usergameConnected, string usergameAdmin);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/DisconnectPlayer")]
        void DisconnectPlayer(string status, string usergameToDisconnect, string usergameToNotify);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/DisconnectPlayer")]
        System.Threading.Tasks.Task DisconnectPlayerAsync(string status, string usergameToDisconnect, string usergameToNotify);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/SendAccessGame")]
        void SendAccessGame(string usergam1, string usergame2);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/SendAccessGame")]
        System.Threading.Tasks.Task SendAccessGameAsync(string usergam1, string usergame2);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/SendExitNotification")]
        void SendExitNotification(string usergameToDisconnect, string usergameToNotify);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/SendExitNotification")]
        System.Threading.Tasks.Task SendExitNotificationAsync(string usergameToDisconnect, string usergameToNotify);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/SendConfigurationGame")]
        void SendConfigurationGame(string userToSend, string section, string difficulty);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/SendConfigurationGame")]
        System.Threading.Tasks.Task SendConfigurationGameAsync(string userToSend, string section, string difficulty);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPreGameServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/UpdateUsersRoom")]
        void UpdateUsersRoom(System.Collections.Generic.Dictionary<object, string>.ValueCollection usersPreGame);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/RecieveAccessGame")]
        void RecieveAccessGame();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/RecieveExitNotification")]
        void RecieveExitNotification(string userDisconnected);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreGameService/ReceiveConfigurationGame")]
        void ReceiveConfigurationGame(string section, string difficulty);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPreGameServiceChannel : Client.GameService.IPreGameService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PreGameServiceClient : System.ServiceModel.DuplexClientBase<Client.GameService.IPreGameService>, Client.GameService.IPreGameService {
        
        public PreGameServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PreGameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PreGameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PreGameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PreGameServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void ConnectPlayer(string usergameConnected, string usergameAdmin) {
            base.Channel.ConnectPlayer(usergameConnected, usergameAdmin);
        }
        
        public System.Threading.Tasks.Task ConnectPlayerAsync(string usergameConnected, string usergameAdmin) {
            return base.Channel.ConnectPlayerAsync(usergameConnected, usergameAdmin);
        }
        
        public void DisconnectPlayer(string status, string usergameToDisconnect, string usergameToNotify) {
            base.Channel.DisconnectPlayer(status, usergameToDisconnect, usergameToNotify);
        }
        
        public System.Threading.Tasks.Task DisconnectPlayerAsync(string status, string usergameToDisconnect, string usergameToNotify) {
            return base.Channel.DisconnectPlayerAsync(status, usergameToDisconnect, usergameToNotify);
        }
        
        public void SendAccessGame(string usergam1, string usergame2) {
            base.Channel.SendAccessGame(usergam1, usergame2);
        }
        
        public System.Threading.Tasks.Task SendAccessGameAsync(string usergam1, string usergame2) {
            return base.Channel.SendAccessGameAsync(usergam1, usergame2);
        }
        
        public void SendExitNotification(string usergameToDisconnect, string usergameToNotify) {
            base.Channel.SendExitNotification(usergameToDisconnect, usergameToNotify);
        }
        
        public System.Threading.Tasks.Task SendExitNotificationAsync(string usergameToDisconnect, string usergameToNotify) {
            return base.Channel.SendExitNotificationAsync(usergameToDisconnect, usergameToNotify);
        }
        
        public void SendConfigurationGame(string userToSend, string section, string difficulty) {
            base.Channel.SendConfigurationGame(userToSend, section, difficulty);
        }
        
        public System.Threading.Tasks.Task SendConfigurationGameAsync(string userToSend, string section, string difficulty) {
            return base.Channel.SendConfigurationGameAsync(userToSend, section, difficulty);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameService.IGameService", CallbackContract=typeof(Client.GameService.IGameServiceCallback))]
    public interface IGameService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ConnectToGame")]
        void ConnectToGame(string userConnected, string userOpponent);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ConnectToGame")]
        System.Threading.Tasks.Task ConnectToGameAsync(string userConnected, string userOpponent);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/DisconnectTheGame")]
        void DisconnectTheGame(string status, string userDisconnect, string userOpponent);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/DisconnectTheGame")]
        System.Threading.Tasks.Task DisconnectTheGameAsync(string status, string userDisconnect, string userOpponent);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendExitGameNotification")]
        void SendExitGameNotification(string userDisconnect, string userToNotify);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendExitGameNotification")]
        System.Threading.Tasks.Task SendExitGameNotificationAsync(string userDisconnect, string userToNotify);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendGameTurn")]
        void SendGameTurn(string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendGameTurn")]
        System.Threading.Tasks.Task SendGameTurnAsync(string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendGameBoard")]
        void SendGameBoard(int[] messyCards, string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendGameBoard")]
        System.Threading.Tasks.Task SendGameBoardAsync(int[] messyCards, string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendMove")]
        void SendMove(string userSend, string btn, string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendMove")]
        System.Threading.Tasks.Task SendMoveAsync(string userSend, string btn, string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendCleanBoard")]
        void SendCleanBoard(string userReceiving);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/SendCleanBoard")]
        System.Threading.Tasks.Task SendCleanBoardAsync(string userReceiving);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ReceiveExitNotification")]
        void ReceiveExitNotification(string userDisconnected);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ReceiveGameTurn")]
        void ReceiveGameTurn();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ReceiveGameBoard")]
        void ReceiveGameBoard(int[] messyCards);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ReceiveMove")]
        void ReceiveMove(string user, string btn);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/ReceiveCleanBoard")]
        void ReceiveCleanBoard();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceChannel : Client.GameService.IGameService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GameServiceClient : System.ServiceModel.DuplexClientBase<Client.GameService.IGameService>, Client.GameService.IGameService {
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void ConnectToGame(string userConnected, string userOpponent) {
            base.Channel.ConnectToGame(userConnected, userOpponent);
        }
        
        public System.Threading.Tasks.Task ConnectToGameAsync(string userConnected, string userOpponent) {
            return base.Channel.ConnectToGameAsync(userConnected, userOpponent);
        }
        
        public void DisconnectTheGame(string status, string userDisconnect, string userOpponent) {
            base.Channel.DisconnectTheGame(status, userDisconnect, userOpponent);
        }
        
        public System.Threading.Tasks.Task DisconnectTheGameAsync(string status, string userDisconnect, string userOpponent) {
            return base.Channel.DisconnectTheGameAsync(status, userDisconnect, userOpponent);
        }
        
        public void SendExitGameNotification(string userDisconnect, string userToNotify) {
            base.Channel.SendExitGameNotification(userDisconnect, userToNotify);
        }
        
        public System.Threading.Tasks.Task SendExitGameNotificationAsync(string userDisconnect, string userToNotify) {
            return base.Channel.SendExitGameNotificationAsync(userDisconnect, userToNotify);
        }
        
        public void SendGameTurn(string userReceiving) {
            base.Channel.SendGameTurn(userReceiving);
        }
        
        public System.Threading.Tasks.Task SendGameTurnAsync(string userReceiving) {
            return base.Channel.SendGameTurnAsync(userReceiving);
        }
        
        public void SendGameBoard(int[] messyCards, string userReceiving) {
            base.Channel.SendGameBoard(messyCards, userReceiving);
        }
        
        public System.Threading.Tasks.Task SendGameBoardAsync(int[] messyCards, string userReceiving) {
            return base.Channel.SendGameBoardAsync(messyCards, userReceiving);
        }
        
        public void SendMove(string userSend, string btn, string userReceiving) {
            base.Channel.SendMove(userSend, btn, userReceiving);
        }
        
        public System.Threading.Tasks.Task SendMoveAsync(string userSend, string btn, string userReceiving) {
            return base.Channel.SendMoveAsync(userSend, btn, userReceiving);
        }
        
        public void SendCleanBoard(string userReceiving) {
            base.Channel.SendCleanBoard(userReceiving);
        }
        
        public System.Threading.Tasks.Task SendCleanBoardAsync(string userReceiving) {
            return base.Channel.SendCleanBoardAsync(userReceiving);
        }
    }
}