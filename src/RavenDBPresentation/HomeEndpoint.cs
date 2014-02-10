namespace RavenDBPresentation
{
	public class HomeEndpoint
	{
		public string Index()
		{
			return @"		
* The FubuRegistry class is RavenDBFubuRegistry.  Use this class to register custom policies or
  configure FubuMVC application settings




* StructureMap configuration is in the file RavenDBRegistry
* StructureMap's default 'IFoo is Foo' conventions are configured for this project

The FubuMVC application is defined in the RavenDBApplication class.  It's unlikely that you'll
need to customize the application bootstrapping, but RavenDBApplication is where that would
happen.  You will want to refer to this class later if you use multiple forms of hosting (ASP.Net, 
Katana, or Serenity).


* The default '/' route calls the HomeEndpoint.Index() method.  
* Change that method to eliminate these ugly textual instructions with something useful;)



			";
		}
	}
}