namespace Game.UserInterface;

public class UIController(TextWriter writer, TextReader reader)
{
	private TextWriter Writer { get; } = writer;
	private TextReader Reader { get; } = reader;

	public void PushText(string text)
	{
		Writer.WriteLine(text);
	}

	public string PushUserInput(string header, List<IInputRestriction> restrictions, bool repeatAsk = true)
	{
		string? input;
		bool valid = false;

		do
		{
			if (!string.IsNullOrEmpty(header))
			{
				Writer.Write($"{header}: ");
			}

			input = Reader.ReadLine();
			if (string.IsNullOrEmpty(input)) continue;

			valid = true;
			foreach (IInputRestriction r in restrictions)
			{
				if (!r.Evaluate(input))
				{
					Writer.WriteLine($"({r.ErrorMessage})");
					valid = false;
					break;
				}
			}

		} while (!valid && repeatAsk);

		return input!;
	}


	public int PushUserChoice(string header, List<string> choices, bool repeatAsk = true)
	{
		Writer.WriteLine(header);
		foreach (var choice in choices)
		{
			Writer.WriteLine($"{choices.IndexOf(choice) + 1}) {choice}");
		}

		return int.Parse(
			PushUserInput("Enter your choice", [new IntRestriction(1, choices.Count)], repeatAsk)
		) - 1;
	}
}