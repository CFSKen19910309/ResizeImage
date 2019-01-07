using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace ReSizeImage
{
    class Program
    {
        public class Options
        {
            [Option('v', "Version", Required = false, HelpText = "Show the version of this App")]
            public string Version { get; set; }

            [Option('s', "Source", Required = false, HelpText = "Directory of Source")]
            public string Source { get; set; }

            [Option('d', "Destination", Required = false, HelpText = "Directory of Destination")]
            public string Destination { get; set; }

            [Option('r', "Recursive", Required = false, HelpText = "Is doing by recursive way")]
            public bool IsRecursive { get; set; }

            [Option('h', "Height", Required = true, HelpText = "Modify Image Heihgt")]
            public int Height { get; set; }

            [Option('w', "Width", Required = true, HelpText = "Modify Image Width")]
            public int Width { get; set; }

            public bool IsOverRideImage { get; set; }

           
        }
        static int Main(string[] args)
        {
            Options t_Options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                   {

                       if (String.IsNullOrEmpty(o.Source))
                       {
                           o.Source = System.IO.Directory.GetCurrentDirectory();
                       }
                       Console.WriteLine("-----info-----");
                       Console.WriteLine("Source: {0}", o.Source);
                       if (String.IsNullOrEmpty(o.Destination))
                       {
                           o.Destination = o.Source;
                       }
                       Console.WriteLine("Destination: {0}", o.Destination);
                       if (string.Compare(o.Source, o.Destination) == 0)
                       {
                           Console.Write("The Source and Destination are same path, do you want to overide all image[y/n]");
                           if (Console.Read() == 'y' || Console.Read() == 'Y')
                           {
                               o.IsOverRideImage = true;
                           }
                           else
                           {
                               Console.Write("Please Fill it completely");
                           }
                       }
                       Console.WriteLine("IsRecursive: {0}", o.IsRecursive);
                       Console.WriteLine("Modify Image Height: {0}", o.Height);
                       Console.WriteLine("Modify Image Width: {0}", o.Width);
                       Console.WriteLine("-----EndInfo-----");
                       t_Options = o;
                   });
            List<string> t_Files = new List<string>();
            t_Files.Clear();

            string[] t_SearchPatterns = new string[] { "*.jpg", "*.png", "*.bmp", "*.jpeg", "*.jpg" };
            if (t_Options.IsRecursive == true)
            {
                foreach (string t_SearchPattern in t_SearchPatterns)
                {
                    t_Files.AddRange(System.IO.Directory.GetFiles(t_Options.Source, t_SearchPattern, System.IO.SearchOption.AllDirectories));
                }
            }
            else
            {
                foreach (string t_SearchPattern in t_SearchPatterns)
                {
                    t_Files.AddRange(System.IO.Directory.GetFiles(t_Options.Source, t_SearchPattern, System.IO.SearchOption.TopDirectoryOnly));
                }
                
            }
            var t_ModifySize = new System.Drawing.Size(t_Options.Width, t_Options.Height);
            foreach (string t_File in t_Files)
            {
                try
                {
                    Emgu.CV.Mat t_Mat = new Emgu.CV.Mat(t_File);
                    Emgu.CV.CvInvoke.Resize(t_Mat, t_Mat, t_ModifySize);
                    t_Mat.Save(t_File);
                    t_Mat.Dispose();
                }
                catch(Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
            }
            t_Files.Clear();
            Console.WriteLine("-----Finish-----");
            //return CommandLine.Parser.Default.ParseArguments<ResizeImagesOptions>(args)
            //.MapResult(
            //  (ResizeImagesOptions opts) => RunResizeExitCode(opts),
            return 0;//  errs => 1);

        }

        //private static int RunResizeExitCode(ResizeImagesOptions opts)
        //{
            // throw new NotImplementedException();

          //  return 0;
        //}
    }
}
