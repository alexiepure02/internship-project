using Domain;
using Infrastructure;
using Moq;

namespace UnitTests
{
    public class UserTests
    {

        [Fact]
        public void AcceptOrRemoveFriendRequestTest()
        {

            Mock<User> user1 = new();
            user1.SetupAllProperties();
            user1.Object.DisplayName = "alex";
            user1.Object.Id = 1;
            user1.Object.Friends = new List<int>();
            user1.Object.FriendRequests = new List<int> { 2, 3 };

            Mock<User> user2 = new();
            user2.SetupAllProperties();
            user2.Object.DisplayName = "andrei";
            user2.Object.Id = 2;

            Mock<User> user3 = new();
            user3.SetupAllProperties();
            user3.Object.DisplayName = "maria";
            user3.Object.Id = 3;

            InMemoryUserRepository userRepository = new();

            userRepository.AddUsers(new List<User> { user1.Object, user2.Object, user3.Object });

            userRepository.AcceptOrRemoveFriendRequest(user1.Object, 2, false);
            userRepository.AcceptOrRemoveFriendRequest(user1.Object, 3, true);

            Assert.Empty(user1.Object.FriendRequests);
            Assert.Equal(2, user1.Object.Friends[0]);
        }
    }
}