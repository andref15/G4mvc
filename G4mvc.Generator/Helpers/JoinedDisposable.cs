namespace G4mvc.Generator.Helpers;

internal class JoinedDisposable : IDisposable
{
    private readonly IEnumerable<IDisposable> _disposables;

    private JoinedDisposable(params IEnumerable<IDisposable> disposables)
    {
        _disposables = disposables;
    }

    public static IDisposable Create(params IEnumerable<IDisposable> disposables)
    {
        return new JoinedDisposable(disposables);
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
