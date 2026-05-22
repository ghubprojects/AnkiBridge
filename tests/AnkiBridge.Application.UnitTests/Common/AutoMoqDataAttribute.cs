using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit3;

namespace AnkiBridge.Application.UnitTests.Common;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true
            });
            return fixture;
        })
    {
    }
}