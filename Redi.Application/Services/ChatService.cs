using Microsoft.EntityFrameworkCore;

using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities;
using Redi.Domain.Models.Chats;
using Redi.Domain.Services;

namespace Redi.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly RediDbContext _context;
        public ChatService(RediDbContext context)
        {
            _context = context;
        }

        public async Task AddNewMessage(Guid chatId, string userId, string message)
        {
            var chat = _context.Chats.SingleOrDefault(c => c.Id == chatId);

            var user = _context.Users.SingleOrDefault(c => c.Id == userId);

            chat.Messages.Add(new ChatMessage()
            {
                Sender = user,
                Message = message,
                SentDate = DateTime.Now
            });

            _context.Chats.Update(chat);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckJoin(string userId, Guid chatId)
        {
            return await _context.Chats.AnyAsync(x => x.Client.Id == userId && x.Id == chatId);
        }

        public async Task<Domain.Models.Chats.Chat> GetChatAsync(Guid chatId)
        {
            var chat = await _context.Chats.Include(c => c.Messages)
                .Select(c => new Domain.Models.Chats.Chat
                {
                    Id = c.Id,
                    Messages = c.Messages.Select(m => new Message
                    {
                        Id = m.Id,
                        Sender = m.Sender.UserName,
                        Text = m.Message,
                    }).ToArray(),
                }).SingleOrDefaultAsync(c => c.Id == chatId);

            return chat;
        }

        public async Task<IReadOnlyCollection<ChatPreview>> GetChatsPreviews(string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            var chats = await _context.Chats.Where(x => x.Client.Id == userId)
                .Select(c => new ChatPreview
                {
                    Id = c.Id,
                    InterlocutorPhoto = c.Deliverer.Picture,
                    LastMessage = c.Messages.OrderBy(x => x.Id)
                    .LastOrDefault().Message
                }).ToArrayAsync();

            return chats;
        }
    }
}