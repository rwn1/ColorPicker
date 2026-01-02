using System.ComponentModel;
using System.Linq.Expressions;

internal sealed class BytePropertyBinding<T> : IDisposable
    where T : INotifyPropertyChanged
{
    private readonly T _source;
    private readonly Func<T, byte> _getter;
    private readonly Action<T, byte> _setter;
    private readonly PropertyChangedEventHandler _handler;

    public event Action? Changed;

    /// <summary>
    /// Initializes a new instance of the BytePropertyBinding class.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="property"></param>
    public BytePropertyBinding(T source, Expression<Func<T, byte>> property)
    {
        _source = source;

        _getter = property.Compile();
        _setter = CreateSetter(property);

        _handler = (_, __) => Changed?.Invoke();
        _source.PropertyChanged += _handler;
    }

    /// <summary>
    /// Gets or sets the bound byte value.
    /// </summary>
    public byte Value
    {
        get => _getter(_source);
        set => _setter(_source, value);
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _source.PropertyChanged -= _handler;
    }

    /// <summary>
    /// Builds a setter delegate from a property expression.
    /// </summary>
    /// <param name="expr"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static Action<T, byte> CreateSetter(Expression<Func<T, byte>> expr)
    {
        if (expr.Body is not MemberExpression member)
            throw new ArgumentException("Expression must be a property access");

        var target = Expression.Parameter(typeof(T), "target");
        var value = Expression.Parameter(typeof(byte), "value");

        var assign = Expression.Assign(
            Expression.Property(target, member.Member.Name),
            value
        );

        return Expression
            .Lambda<Action<T, byte>>(assign, target, value)
            .Compile();
    }
}