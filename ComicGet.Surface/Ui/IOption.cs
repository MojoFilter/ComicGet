namespace ComicGet.Ui;

public interface IOptionFactory
{
    IOption<T> Create<T>(T value, string label);

    IEnumerable<IOption<T>> Create<T>(IEnumerable<T> value, Func<T, string> labelSelector);
}

public interface IOption<T>
{
    string Label { get; }
    T Value { get; }
    bool IsSelected { get; set; }
}
