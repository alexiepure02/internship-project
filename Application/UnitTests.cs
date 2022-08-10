using Application.Users.AcceptOrRemoveFriendRequest;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application
{
    public class UnitTests
    {
        static ServiceCollection services = new ServiceCollection()
                .AddScoped<IUserRepository, InMemoryUserRepository>()
                .AddScoped<IMessageRepository, InMemoryMessageRepository>()
                .AddMediatR(typeof(IUserRepository))
                .BuildServiceProvider();

        // not sure how to implement tests using mediatr

        IMediator mediator = services.GetRequiredService<IMediator>();



        [Fact]
        public void AcceptOrR()
        {
            var acceptOrRemoveFriendRequest = new AcceptOrRemoveFriendRequest();


        }
    }
}
