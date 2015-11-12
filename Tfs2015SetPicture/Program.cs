using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Microsoft.TeamFoundation;

namespace Tfs2015SetPicture
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            var parser = new Parser(settings =>
                                    {
                                        settings.CaseSensitive = true;
                                        settings.HelpWriter = Console.Error;
                                    });
            if (!parser.ParseArgumentsStrict(args, options))
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(1);
            }

            byte[] imageBytes = null;
            try
            {
                imageBytes = ImageLoader.LoadImage(options.ImagePath);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Image could not be found or read. Make sure the path to your image is correct.");
                Console.Error.WriteLine($"Image path: \"{options.ImagePath}\"");
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }

            try
            {
                TfsUploader.UploadImage(options.ServerName, options.Username, imageBytes);
            }
            catch (TeamFoundationServiceUnavailableException e)
            {
                Console.Error.WriteLine("Failed to connect to TeamFoundationServer. Make sure the name or ip of your server is correct.");
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (Exception)
            {
                Environment.Exit(1);
            }

            Console.WriteLine("Done!");
        }

        private class Options
        {
            [Option('s', "servername", Required = true,
                HelpText = "Name, or ip, of your TFS server name.")]
            public string ServerName { get; set; }

            [Option('u', "username", Required = true,
                HelpText = "Username to set picture for. You must be authorized to set picture for this account.")]
            public string Username { get; set; }

            [Option('i', "imagepath", Required = true,
                HelpText = "Path to the image. Desired size is 144x144 pixels, otherwise the image will be resized.")]
            public string ImagePath { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}