namespace BullseyeTests
{
    using Bullseye.Internal;
    using BullseyeTests.Infra;
    using Xbehave;
    using Xunit;

    public class LoggerTests
    {
        [Scenario]
        [Example(0.001D, "<1 ms")]
        [Example(1D, "1 ms")]
        [Example(1_000D, "1 s")]
        [Example(119_000D, "1 min 59 s")]
        [Example(1_000_000D, "16 min 40 s")]
        [Example(1_000_000_000D, "16,667 min")]
        public void Timings(double elapsed, string expectedSubstring, Logger log, TestConsole console)
        {
            "Given a logger"
                .x(() => log = new Logger(console = new TestConsole(), false, false, false, new Palette(false, Host.Unknown, OperatingSystem.Unknown), false, Host.Unknown));

            $"When logging a message with an elapsed time in milliseconds of {elapsed}"
                .x(() => log.Succeeded("foo", elapsed));

            $"Then the message contains \"{expectedSubstring}\""
                .x(() => Assert.Contains(expectedSubstring, console.Out.ToString()));
        }

        [Scenario]
        public void TeamCityServiceMessagesStarting(Logger log, TestConsole console)
        {
            "Given a logger"
                .x(() => log = new Logger(console = new TestConsole(), false, false, false, new Palette(false, Host.TeamCity, OperatingSystem.Unknown), false, Host.TeamCity));

            "When starting"
                .x(() => log.Starting("foo"));

            "Then the message contains teamcity block opened service message"
                .x(() => Assert.Contains("##teamcity[blockOpened name='foo']", console.Out.ToString()));

            "And the message contains teamcity block closed service message"
                .x(() => Assert.Contains("##teamcity[blockClosed name='foo']", console.Out.ToString()));
        }

        [Scenario]
        public void TeamCityServiceMessagesStartingWithInput(Logger log, TestConsole console)
        {
            "Given a logger"
                .x(() => log = new Logger(console = new TestConsole(), false, false, false, new Palette(false, Host.TeamCity, OperatingSystem.Unknown), false, Host.TeamCity));

            "When starting"
                .x(() => log.Starting("foo", "bar"));

            "Then the message contains teamcity block opened service message"
                .x(() => Assert.Contains("##teamcity[blockOpened name='foo']", console.Out.ToString()));

            "And the message contains teamcity block closed service message"
                .x(() => Assert.Contains("##teamcity[blockClosed name='foo']", console.Out.ToString()));
        }
    }
}
