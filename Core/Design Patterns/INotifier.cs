
namespace BaseFramework.Core
{
	public interface INotifier<T>
	{
		bool RegisterSubscription( IObserver<T> subscriber );
		bool UnregisterSubscription( IObserver<T> subscriber );
		void NotifySubscribers( );
	}
}
