//code from http://net.tutsplus.com/tutorials/maintainable-automated-ui-tests/
//excellent article as well regarding UI Testing

namespace Demo.SeleniumTests.Utilities
{
    public class Host
    {
        internal static readonly SeleniumApplication Instance = new SeleniumApplication();
        static Host()
        {
            //location of where the project was built
            Instance.Run("C:\\inetpub\\websites\\AliaSQL-Demo-master\\build\\Latest\\web", 4278);

        }
    }
}