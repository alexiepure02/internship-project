using Domain;
using Domain.Exceptions;
using Infrastructure;

namespace xUnitTests
{
    public class MessageTests
    {
        [Fact]
        public void CheckIfMessageValidTest()
        {
            var repo = new InMemoryMessageRepository();

            string nullMessage = null;
            string whiteSpaceMessage = " ";
            string longMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit." +
                "Morbi ut suscipit nisi. Pellentesque et mauris ut massa ullamcorper efficitur." +
                "In nisl justo, placerat vitae sodales quis, posuere id nisi." +
                "Pellentesque odio metus, convallis ut dolor eu, lobortis cursus"; // 260 characters
            string profaneMessage = "alligator";

            // null or white space scenario

            try
            {
                repo.CheckIfMessageValid(nullMessage);
            }
            catch (InvalidMessageException ex)
            {
                Assert.Equal("Please type a message to continue.", ex.Message);
            }

            try
            {
                repo.CheckIfMessageValid(whiteSpaceMessage);
            }
            catch (InvalidMessageException ex)
            {
                Assert.Equal("Please type a message to continue.", ex.Message);
            }

            // message longer than 256 characters scenario

            try
            {
                repo.CheckIfMessageValid(longMessage);
            }
            catch (InvalidMessageException ex)
            {
                Assert.Equal("The message should have a maximum of 256 characters.", ex.Message);
            }

            // message contains profanity scenario

            try
            {
                repo.CheckIfMessageValid(profaneMessage);
            }
            catch (InvalidMessageException ex)
            {
                Assert.Equal("The message contains profanity.", ex.Message);
            }
        }

        [Fact]
        public void GetMessagesBetweenUsers()
        {
            var repo = new InMemoryMessageRepository();

            var messagesBetween1And2 = new List<Message>
            {
                new Message { IdSender = 1, IdReceiver = 2, Text = "hello"},
                new Message { IdSender = 2, IdReceiver = 1, Text = "hi"},
                new Message { IdSender = 1, IdReceiver = 2, Text = "how are you"},
                new Message { IdSender = 2, IdReceiver = 1, Text = "i'm fine"},
                new Message { IdSender = 1, IdReceiver = 2, Text = "ok"},
                new Message { IdSender = 1, IdReceiver = 2, Text = "bye"},
            };

            var dummyMessages = new List<Message>
            {
                new Message { IdSender = 3, IdReceiver = 1, Text = "dummy"},
                new Message { IdSender = 1, IdReceiver = 3, Text = "dummy"},
                new Message { IdSender = 3, IdReceiver = 2, Text = "dummy"},
            };

            repo.AddMessages(messagesBetween1And2);
            repo.AddMessages(dummyMessages);

            // when we call this operation, the ids are valid so there's only the
            // working scenario

            Assert.Equal(messagesBetween1And2, repo.GetMessagesBetweenTwoUsers(1, 2));
        }
    }
}