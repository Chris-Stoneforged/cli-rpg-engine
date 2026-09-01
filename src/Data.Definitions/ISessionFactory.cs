namespace Data.Definitions;

public interface ISessionFactory
{
	IGameDatabase GetReadonlySession();
}