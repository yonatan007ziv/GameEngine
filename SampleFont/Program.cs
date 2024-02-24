using GameEngine.Components;
using GameEngine.Services.Interfaces;
using GameEngine;
using GameEngine.Core.Components.Font;
using GameEngine.Core.Enums;
using GameEngine.Core.SharedServices.Implementations.FileReaders;
using System.Drawing;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using GameEngine.Core.SharedServices.Implementations;

namespace SampleFont;

internal class Program
{
	public static void Main()
	{
		TrueTypeFontFileReader trueTypeFontFileReader = new TrueTypeFontFileReader(new ConsoleLogger(), new ResourceDiscoverer());
		trueTypeFontFileReader.ReadFile("Arial.ttf", out Font font);

        Console.WriteLine($"Font Name: {font.FontName}");
        Console.WriteLine($"Font Family: {font.FontFamily}");
        Console.WriteLine($"Font SubFamily: {font.FontSubFamily}");
        Console.WriteLine($"Font Version: {font.Version}");
    }
}