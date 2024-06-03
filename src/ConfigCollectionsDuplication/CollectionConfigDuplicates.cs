using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace ConfigCollectionsDuplication;

public record SectionWithPrimaryConstructor(string[] Values);

public record SectionWithNormalConstructor
{
    public string[] Values { get; }

    public SectionWithNormalConstructor(string[] values)
    {
        Values = values;
    }
}

public class CollectionConfigDuplicates
{
    [Fact]
    //FAILS
    public void ConifgCollectionInSection_ShouldHave2Values_WhenBindingToRecordWithPrimaryConstructor()
    {
        //Arrange
        Dictionary<string, string> values = new ()
        {
            ["Section:Values:0"] = "1",
            ["Section:Values:1"] = "2"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        var section = configuration.GetSection("Section").Get<SectionWithPrimaryConstructor>();

        //Assert
        section!.Values.Length.Should().Be(2);
        //Should be 2 but is 4
        //Values are duplicated: ["1", "2", "1", "2"]
    }

    [Fact]
    //WORKS
    public void ConifgCollectionInSection_ShouldHave2Values_WhenBindingToRecordWithNormalConstructor()
    {
        //Arrange
        Dictionary<string, string> values = new ()
        {
            ["Section:Values:0"] = "1",
            ["Section:Values:1"] = "2"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        var section = configuration.GetSection("Section").Get<SectionWithNormalConstructor>();

        //Assert
        section!.Values.Length.Should().Be(2);
    }
}