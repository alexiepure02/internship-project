using Domain;
using Domain.Exceptions;
using Infrastructure;

namespace xUnitTests
{
    public class UserTests
    {
        [Fact]
        public void GetUserByUsernameAndPasswordTest()
        {
            var repo = new InMemoryUserRepository();
         
            // user found scenario

            var user = new User {Username = "alexiepure", Password = "1234" };
            repo.AddUser(user);

            var searchedUser = repo.GetUserByUsernameAndPassword("alexiepure", "1234");
            
            Assert.Equal(user, searchedUser);

            // user not found scenario

            try
            {
                searchedUser = repo.GetUserByUsernameAndPassword("dummyUsername", "1234");
            }
            catch (UserNotFoundException ex)
            {
                Assert.Equal("Error: User dummyUsername not found.", ex.Message);
            }
        }

        [Fact]
        public void GetUserById()
        {
            var repo = new InMemoryUserRepository();

            var user = new User { Id = 1 };
            repo.AddUser(user);

            // user found scenario

            var searchedUser = repo.GetUserById(1);

            Assert.Equal(user, searchedUser);

            // user not found scenario

            try
            {
                searchedUser = repo.GetUserById(2);
            }
            catch (UserNotFoundException ex)
            {
                Assert.Equal("Error: User with the id 2 not found.", ex.Message);
            }
        }
        
        [Fact]
        public void ValidateIdFriendTest()
        {
            var repo = new InMemoryUserRepository();

            var user = new User { DisplayName = "Alex",Id = 1, Friends = new List<int> { 2 } };
            var friend = new User { DisplayName = "Andrei", Id = 2 };
            var nonFriend = new User { DisplayName = "Maria", Id = 3 };

            repo.AddUsers(new List<User> { user, friend, nonFriend});

            // id valid scenario

            repo.ValidateIdFriend(user, nonFriend.Id);

            // same id as user scenario

            try
            {
                repo.ValidateIdFriend(user, user.Id);
            }
            catch (SameIdException ex)
            {
                Assert.Equal($"Error: {user.Id} is your ID.", ex.Message);
            }

            // friend not found scenario

            try
            {
                repo.ValidateIdFriend(user, 4);
            }
            catch (UserNotFoundException ex)
            {
                Assert.Equal("Error: User with the id 4 not found.", ex.Message);
            }

            // user already in friends list scenario

            try
            {
                repo.ValidateIdFriend(user, friend.Id);
            }
            catch (UserInFriendsException ex)
            {
                Assert.Equal($"Error: User with the id {friend.Id} is already a friend.", ex.Message);
            }
        }

        [Fact]
        public void SendFriendRequestTest()
        {
            var repo = new InMemoryUserRepository();

            var user = new User { DisplayName = "Alex", Id = 1 };
            var futureFriend = new User { DisplayName = "Andrei", Id = 2, FriendRequests = new List<int>() };

            repo.AddUsers(new List<User> { user, futureFriend });

            // friend request sent scenario

            repo.SendFriendRequest(user, 2);

            Assert.Contains(user.Id, repo.GetUserById(futureFriend.Id).FriendRequests);

            // friend request already sent scenario

            repo.SendFriendRequest(user, 2);
            repo.SendFriendRequest(user, 2);
            repo.SendFriendRequest(user, 2);

            Assert.Single(repo.GetUserById(futureFriend.Id).FriendRequests);
        }

        [Fact]
        public void AcceptOrRemoveFriendRequestTest()
        {
            var repo = new InMemoryUserRepository();

            var user = new User { DisplayName = "Alex", Id = 1, Friends = new List<int>(), FriendRequests = new List<int> { 2, 3 } };
            var futureFriend = new User { DisplayName = "Andrei", Id = 2, Friends = new List<int>() };
            var futureNonFriend = new User { DisplayName = "Maria", Id = 3, Friends = new List<int>() };

            repo.AddUsers(new List<User> { user, futureFriend, futureNonFriend });

            // accept friend request scenario

            repo.AcceptOrRemoveFriendRequest(user, futureFriend.Id, false); // remove = false

            Assert.Contains(futureFriend.Id, user.Friends);
            Assert.DoesNotContain(futureFriend.Id, user.FriendRequests);

            // remove friend request scenario

            repo.AcceptOrRemoveFriendRequest(user, futureNonFriend.Id, true); // remove = true

            Assert.DoesNotContain(futureNonFriend.Id, futureFriend.Friends);
            Assert.DoesNotContain(futureNonFriend.Id, user.FriendRequests);
        }
    }
}