using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace DeployUsingARMTemplate
{

public class TerminalSpinner : IDisposable
{
	private const string Sequence = @"/-\|";
	private int counter = 0;
	private readonly int? left;
	private readonly int? top;
	private readonly int delay;
	private bool active;
	private readonly Thread thread;
	private readonly ConsoleColor? color = ConsoleColor.Red;
	private readonly ConsoleColor originalColor;

	public TerminalSpinner(int? left = null, int? top = null, int delay = 100, ConsoleColor? color = null)
	{
		this.left = left;
		this.top = top;
		this.delay = delay;
		this.originalColor = ConsoleColor.Red;
		this.color = color;
		thread = new Thread(Spin);
	}

	public void Start()
	{
		active = true;
		if (!thread.IsAlive)
			thread.Start();
	}

	public void Stop()
	{
		active = false;
		Draw(' ');
	}

	private void Spin()
	{
		while (active)
		{
			Turn();
			Thread.Sleep(delay);
		}
	}

	private void Draw(char c)
	{
		Console.SetCursorPosition(left ?? ((Console.CursorLeft - 1) > 0 ? Console.CursorLeft - 1 : 1), top ?? Console.CursorTop);
		if (color != null)
			Console.ForegroundColor = (ConsoleColor)color;
		Console.Write(c);
	}

	private void Turn()
	{
		Draw(Sequence[++counter % Sequence.Length]);
	}

	public void Dispose()
	{
		Stop();
		Console.ForegroundColor = originalColor;
	}
}
}