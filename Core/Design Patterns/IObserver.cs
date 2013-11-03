
namespace BaseFramework.Core
{
	public interface IObserver<T>
	{
		//TODO: How to pass data to a notifier?
		void OnNotify( T data );
	}
}