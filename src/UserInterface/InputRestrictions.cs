using UserInterface.Definitions;

namespace UserInterface;

public class StringLengthRestriction(
	int? minLength = null,
	int? maxLength = null
) : IInputRestriction
{
	private int? MinLength { get; } = minLength;
	private int? MaxLength { get; } = maxLength;

	public string ErrorMessage
	{
		get
		{
			if (MinLength != null && MaxLength != null)
			{
				return $"Must contain between {MinLength} and {MaxLength} characters";
			}
			else if (MinLength != null)
			{
				return $"Must contain at least {MinLength} characters";
			}
			else if (MaxLength != null)
			{
				return $"Must contain no more than {MaxLength} characters";
			}

			return "";
		}
	}

	public bool Evaluate(string value)
	{
		return
			(MinLength == null || value.Length >= MinLength) &&
			(MaxLength == null || value.Length <= MaxLength);
	}
}


public class IntRestriction(int? minValue = null, int? maxValue = null) : IInputRestriction
{
	private int? MinValue { get; } = minValue;
	private int? MaxValue { get; } = maxValue;

	public string ErrorMessage
	{
		get
		{
			if (MinValue != null && MaxValue != null)
			{
				return $"Must be a number between {MinValue} and {MaxValue}";
			}
			else
			{
				var message = "Must be a number";
				if (MinValue != null)
				{
					message += $" greater than or equal to {MinValue}";
				}
				else if (MaxValue != null)
				{
					message += $" less than or equal to {MaxValue}";
				}
				return message;
			}
		}
	}

	public bool Evaluate(string value)
	{
		return int.TryParse(value, out var intValue) &&
			(MinValue == null || intValue >= MinValue) &&
			(MaxValue == null || intValue <= MaxValue);
	}
}

public class ValidPathRestriction : IInputRestriction
{
	public string ErrorMessage => "Must be a valid path";

	public bool Evaluate(string value)
	{
		return Path.Exists(value);
	}
}