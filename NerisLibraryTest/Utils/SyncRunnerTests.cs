using FluentAssertions;
using NerisLibrary.Utils;

namespace NerisLibraryTest.Utils;

public class SyncRunnerTests
{
    [Fact]
    public void RunSync_CompletedTask_ReturnsResult()
    {
        SyncRunner.RunSync(() => Task.FromResult(42)).Should().Be(42);
    }

    [Fact]
    public void RunSync_AsyncMethodThrows_SurfacesAggregateExceptionWithOriginalInner()
    {
        // Pins current behavior: task.Wait() wraps failures in AggregateException.
        Action act = () => SyncRunner.RunSync<int>(() => throw new InvalidOperationException("boom"));

        act.Should().Throw<AggregateException>()
            .WithInnerException<InvalidOperationException>()
            .WithMessage("boom");
    }
}
