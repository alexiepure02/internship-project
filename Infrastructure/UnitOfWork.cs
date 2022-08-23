using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext, IUserRepository userRepository,
            IMessageRepository messageRepository)
        {
            _appDbContext = appDbContext;
            UserRepository = userRepository;
            MessageRepository = messageRepository;
        }

        public IUserRepository UserRepository { get; private set; }

        public IMessageRepository MessageRepository { get; private set; }
        
        public async Task Save()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }

    }
}
