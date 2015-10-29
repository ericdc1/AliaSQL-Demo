using StructureMap.Graph;

namespace Demo.Website.DependencyResolution
{
    using System.Reflection;

    using StructureMap.Configuration.DSL;

    public class StructureMapRegistry : Registry
	{

		public StructureMapRegistry()
		{
			this.Scan(s =>
			{
				s.WithDefaultConventions();
				s.AssembliesFromApplicationBaseDirectory(this.AssemblyFilter);
			});

		}

		private bool AssemblyFilter(Assembly assembly)
		{
			object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
			if (customAttributes.Length == 0) {
				return false;
			}
			if (((AssemblyProductAttribute)customAttributes[0]).Product.StartsWith("Demo")) {
				return true;
			}
			return false;
		}

	}

}
