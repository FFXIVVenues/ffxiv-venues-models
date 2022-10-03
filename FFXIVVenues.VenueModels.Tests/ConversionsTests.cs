using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests
{
    public class ConversationTests
    {
        [Test]
        public void CanHandleAllFieldsNull()
        {
            var v1model = new V2021.Venue();
            var v2model = Venue.FromV1Venue(v1model);
            Assert.AreEqual(v1model.name, v2model.Name);
        }

    }
}