using StructureMap.Configuration.DSL;

namespace RavenDBPresentation
{
	public class RavenDBRegistry : Registry
	{
		public RavenDBRegistry()
		{
			// make any StructureMap configuration here
			
            // Sets up the default "IFoo is Foo" naming convention
            // for auto-registration within this assembly
            Scan(x => {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });
		}
	}
}