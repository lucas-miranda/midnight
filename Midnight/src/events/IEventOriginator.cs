
namespace Midnight;

public interface IEventOriginator<T> {
    public T Originator { get; }
}
