namespace ComicGet.Ui;

internal class OptionFactory : IOptionFactory
{
    public IOption<T> Create<T>(T value, string label) => new Option<T>(value, label);

    public IEnumerable<IOption<T>> Create<T>(IEnumerable<T> value, Func<T, string> labelSelector) =>
        value.Select(v => this.Create<T>(v, labelSelector(v)));

    class Option<T> : ReactiveObject, IOption<T>
    {
        public Option(T value, string label)
        {
            this.Label = label;
            this.Value = value;
        }

        public string Label { get; }

        public T Value { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        private bool _isSelected;
    }
}
