namespace Controllers;

public abstract class AController(ControllerContext ctx)
{
	protected readonly ControllerContext _ctx = ctx;
}