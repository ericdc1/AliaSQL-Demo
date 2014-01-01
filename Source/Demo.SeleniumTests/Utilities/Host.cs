//code from http://net.tutsplus.com/tutorials/maintainable-automated-ui-tests/
//excellent article as well regarding UI Testing

using System;
using System.IO;

namespace Demo.SeleniumTests.Utilities
{
    public class Host
    {
        internal static readonly SeleniumApplication Instance = new SeleniumApplication();
        static Host()
        {
            //location of where the project was built
            var webprojectpath = Path.Combine( ProjectLocation.GetSolutionFolderPath(), "Demo.Website");
            //Instance.Run("\\build\\Latest\\web", 4278);
            Instance.Run(webprojectpath, 4278);

        }
    }
}