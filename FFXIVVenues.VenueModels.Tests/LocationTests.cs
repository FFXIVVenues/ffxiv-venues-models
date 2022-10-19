using NUnit.Framework;

namespace FFXIVVenues.VenueModels.Tests;

public class LocationTests
{
    private Location _location;

    [SetUp]
    public void Setup()
    {
        this._location = new Location()
        {
            Apartment = 4,
            District = "Lavender Beds",
            Ward = 4,
            DataCenter = "Aether",
            World = "Jenova"
        };
    }
    
    [Test]
    public void WhenOverrideIsSetItIsReturned()
    {
        this._location.Override = "This is an override";
        Assert.AreEqual("This is an override", this._location.ToString());
    }
    
    [Test]
    [TestCase("")]
    [TestCase("  ")]
    [TestCase(" \n  ")]
    [TestCase(null)]
    public void WhenOverrideIsNotSetItIsNotReturned(string str)
    {
        this._location.Override = str;
        Assert.AreEqual("Aether, Jenova, Lavender Beds, Ward 4, Apt 4", this._location.ToString());
    }
    
}