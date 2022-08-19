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

            var user = new User { ID = 1 };
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

            var user = new User { DisplayName = "Alex",ID = 1, Friends = new List<int> { 2 } };
            var friend = new User { DisplayName = "Andrei", ID = 2 };
            var nonFriend = new User { DisplayName = "Maria", ID = 3 };

            repo.AddUsers(new List<User> { user, friend, nonFriend});

            // id valid scenario

            repo.ValidateIdFriend(user, nonFriend.ID);

            // same id as user scenario

            try
            {
                repo.ValidateIdFriend(user, user.ID);
            }
            catch (SameIdException ex)
            {
                Assert.Equal($"Error: {user.ID} is your ID.", ex.Message);
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
                repo.ValidateIdFriend(user, friend.ID);
            }
            catch (UserInFriendsException ex)
            {
                Assert.Equal($"Error: User with the id {friend.ID} is already a friend.", ex.Message);
            }
        }

        [Fact]
        public void SendFriendRequestTest()
        {
            var repo = new InMemoryUserRepository();

            var user = new User { DisplayName = "Alex", ID = 1 };
            var futureFriend = new User { DisplayName = "Andrei", ID = 2, FriendRequests = new List<int>() };

            repo.AddUsers(new List<User> { user, futureFriend });

            // friend request sent scenario

            repo.SendFriendRequest(user, 2);

            Assert.Contains(user.ID, repo.GetUserById(futureFriend.ID).FriendRequests);

            // friend request already sent scenario

            repo.SendFriendRequest(user, 2);
            repo.SendFriendRequest(user, 2);
            repo.SendFriendRequest(user, 2);

            Assert.Single(repo.GetUserById(futureFriend.ID).FriendRequests);
        }

        [Fact]
        public void AcceptOrRemoveFriendRequestTest()
        {
            var repo = new InMemoryUserRepository();

            var user = new User { DisplayName = "Alex", ID = 1, Friends = new List<int>(), FriendRequests = new List<int> { 2, 3 } };
            var futureFriend = new User { DisplayName = "Andrei", ID = 2, Friends = new List<int>() };
            var futureNonFriend = new User { DisplayName = "Maria", ID = 3, Friends = new List<int>() };

            repo.AddUsers(new List<User> { user, futureFriend, futureNonFriend });

            // accept friend request scenario

            repo.AcceptOrRemoveFriendRequest(user, futureFriend.ID, false); // remove = false

            Assert.Contains(futureFriend.ID, user.Friends);
            Assert.DoesNotContain(futureFriend.ID, user.FriendRequests);

            // remove friend request scenario

            repo.AcceptOrRemoveFriendRequest(user, futureNonFriend.ID, true); // remove = true

            Assert.DoesNotContain(futureNonFriend.ID, futureFriend.Friends);
            Assert.DoesNotContain(futureNonFriend.ID, user.FriendRequests);
        }
    }
}