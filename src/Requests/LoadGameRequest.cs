using Requests.Definitions;

namespace Requests;

public class LoadGameRequest(string path) : IRequest
{
	public string SavePath { get; } = path;
}