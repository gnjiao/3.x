using System;
using System.Diagnostics;
using System.IO;
using Core;

namespace Platform.Main.Util
{
    public class LinkCommand : AbstractMenuCommand
    {
        private readonly string _site;

        public LinkCommand(string site)
        {
            this._site = site;
        }

        public override void Run()
        {
            if (_site.StartsWith("home://"))
            {
                var file = Path.Combine(FileUtility.ApplicationRootPath, _site.Substring(7).Replace('/', Path.DirectorySeparatorChar));
                try
                {
                    Process.Start(file);
                }
                catch (Exception)
                {
                    MessageService.ShowError("Can't execute/view " + file + "\n Please check that the file exists and that you can open this file.");
                }
            }
        }
    }
}
