
namespace BaseFramework.Core
{
	public interface IFacade
	{
		T RetrieveObject<T>();
		void RegisterObject<T>( T instance );
	}
}