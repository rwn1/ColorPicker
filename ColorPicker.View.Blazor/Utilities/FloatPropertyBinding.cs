using System.ComponentModel;
using System.Linq.Expressions;

internal sealed class FloatPropertyBinding<T> : IDisposable
    where T : INotifyPropertyChanged
{
    private readonly T _source;
    private readonly Func<T, float> _getter;
    private readonly Action<T, float> _setter;
    private readonly PropertyChangedEventHandler _handler;

    public event Action? Changed;

    /// <summary>
    /// Initializes a new instance of the FloatPropertyBinding class.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="property"></param>
    public FloatPropertyBinding(T source, Expression<Func<T, float>> property)
    {
        _source = source;

        _getter = property.Compile();
        _setter = CreateSetter(property);

        _handler = (_, __) => Changed?.Invoke();
        _source.PropertyChanged += _handler;
    }

    /// <summary>
    /// Gets or sets the bound float value.
    /// </summary>
    public float Value
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
    private static Action<T, float> CreateSetter(Expression<Func<T, float>> expr)
    {
        if (expr.Body is not MemberExpression member)
            throw new ArgumentException("Expression must be a property access");

        var target = Expression.Parameter(typeof(T), "target");
        var value = Expression.Parameter(typeof(float), "value");

        var assign = Expression.Assign(
            Expression.Property(target, member.Member.Name),
            value
        );

        return Expression
            .Lambda<Action<T, float>>(assign, target, value)
            .Compile();
    }
}