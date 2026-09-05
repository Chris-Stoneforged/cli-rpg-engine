namespace Data.Definitions.Entities;

public class GameCore : DbEntity
{
	public virtual Character? PlayerCharacter { get; set; }

	public override string Repr()
	{
		return "";
	}
}