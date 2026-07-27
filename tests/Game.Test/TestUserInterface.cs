using Game.UserInterface;

namespace Game.Test;

public class TestUserInterface
{
	[Fact]
	public void TestLogOutput()
	{
		var sw = new StringWriter();
		var sr = new StringReader("");
		var controller = new UIController(sw, sr);
		controller.PushText("Hello, World!");
		Assert.Contains("Hello, World!", sw.ToString());
	}

	[Fact]
	public void TestGetUserInput()
	{
		var sw = new StringWriter();
		var sr = new StringReader("test output");
		var controller = new UIController(sw, sr);
		var result = controller.PushUserInput("test header", []);
		Assert.Equal("test output", result);
	}

	[Fact]
	public void TestGetUserInputStringLengthMinViolated()
	{
		var sw = new StringWriter();
		var sr = new StringReader("short");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new StringLengthRestriction(minLength: 8)], false);
		Assert.Contains("(Must contain at least 8 characters)", sw.ToString());
	}

	[Fact]
	public void TestGetUserInputStringLengthMaxViolated()
	{
		var sw = new StringWriter();
		var sr = new StringReader("too long");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new StringLengthRestriction(maxLength: 5)], false);
		Assert.Contains("(Must contain no more than 5 characters)", sw.ToString());
	}

	[Fact]
	public void TestGetUserInputStringLengthViolated()
	{
		var sw = new StringWriter();
		var sr = new StringReader("too long");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new StringLengthRestriction(3, 5)], false);
		Assert.Contains("(Must contain between 3 and 5 characters)", sw.ToString());
	}

	[Fact]
	public void TestGetUserInputInteger()
	{
		var sw = new StringWriter();
		var sr = new StringReader("1");
		var controller = new UIController(sw, sr);

		var result = controller.PushUserInput("test header", [], false);
		Assert.Equal("1", result);
	}

	[Fact]
	public void TestGetUserInputIntegerNotANumber()
	{
		var sw = new StringWriter();
		var sr = new StringReader("not an integer");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new IntRestriction()], false);
		Assert.Contains("(Must be a number)", sw.ToString());
	}

	[Fact]
	public void TestGetUserInputIntegerConstraintsViolated()
	{
		var sw = new StringWriter();
		var sr = new StringReader("22");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new IntRestriction(1, 10)], false);
		Assert.Contains("(Must be a number between 1 and 10)", sw.ToString());
	}

	[Fact]
	public void TestGetUserInputIntegerMinViolated()
	{
		var sw = new StringWriter();
		var sr = new StringReader("-1");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new IntRestriction(minValue: 0)], false);
		Assert.Contains("(Must be a number greater than or equal to 0)", sw.ToString());
	}

	[Fact]
	public void TestGetUserInputIntegerMaxViolated()
	{
		var sw = new StringWriter();
		var sr = new StringReader("22");
		var controller = new UIController(sw, sr);

		controller.PushUserInput("test header", [new IntRestriction(maxValue: 10)], false);
		Assert.Contains("(Must be a number less than or equal to 10)", sw.ToString());
	}

	[Fact]
	public void TestUserChoice()
	{
		var sw = new StringWriter();
		var sr = new StringReader("3");
		var controller = new UIController(sw, sr);

		var result = controller.PushUserChoice("test header", ["one", "two", "three"], false);
		Assert.Contains("test header", sw.ToString());
		Assert.Contains("1) one", sw.ToString());
		Assert.Contains("2) two", sw.ToString());
		Assert.Contains("3) three", sw.ToString());
		Assert.Contains("Enter your choice: ", sw.ToString());
		Assert.Equal(2, result);
	}

	[Fact]
	public void TestUserChoiceInvalid()
	{
		var sw = new StringWriter();
		var sr = new StringReader("8");
		var controller = new UIController(sw, sr);

		controller.PushUserChoice("test header", ["one", "two", "three"], false);
		Assert.Contains("(Must be a number between 1 and 3)", sw.ToString());
	}
}