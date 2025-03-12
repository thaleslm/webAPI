using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class chatHub : Hub
    {

        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"{user} say {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        // Método para adicionar um usuário a um grupo específico (ex: "1001")
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", "Sistema", $"Um novo usuário entrou no grupo {groupName}.");
        }

        // Método para remover um usuário de um grupo
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", "Sistema", $"Um usuário saiu do grupo {groupName}.");
        }

        // Método para enviar mensagem apenas para o grupo específico
        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }
    }
}
