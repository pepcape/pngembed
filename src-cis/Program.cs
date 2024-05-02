using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PngEmbed;

public class Options
{
  [Option('i', "input", Required = true, Default = "test.zip",
    HelpText = "Input file-name (PNG for extract, everything else for embed).")]
  public string InputFileName { get; set; } = "test.zip";

  [Option('o', "output", Required = false, Default = "",
    HelpText = "Output file-name (PNG for embed, everything else for renaming extracted file).")]
  public string FileName { get; set; } = "";

  [Option('w', "width", Required = false, Default = 1024,
    HelpText = "Image width in pixels for embedding.")]
  public int Width { get; set; }

  [Option('m', "message", Required = false, Default = "Use https://github.com/pepcape/pngembed to extract the file",
    HelpText = "Readable message for the PNG.")]
  public string Message { get; set; } = "";
}

internal class Program
{
  // Constants for tile colors
  private static Rgba32 FirstColor  = new Rgba32(0x20, 0x20, 0xFF); // default is Blue (#2020ff)
  private static Rgba32 SecondColor = new Rgba32(0xFF, 0x20, 0x20); // default is Red  (#ff2020)

  static void Main (string[] args)
  {
    Parser.Default.ParseArguments<Options>(args)
       .WithParsed<Options>(o =>
       {
         // Create a new image with the specified dimensions
         using (var image = new Image<Rgba32>(o.Width, o.Width))
         {
           for (int y = 0; y < o.Width; y++)
           {
             for (int x = 0; x < o.Width; x++)
             {
               // Determine the tile color based on position
               Rgba32 tileColor = ((x / 100) + (y / 100)) % 2 == 0
                ? FirstColor   // even tiles
                : SecondColor; // odd tiles

               // Set the pixel color
               image[x, y] = tileColor;
             }
           }

           // Save the image to a file with the specified filename
           image.Save(o.FileName);

           Console.WriteLine($"Image '{o.FileName}' created successfully.");
         }
       });
  }
}
