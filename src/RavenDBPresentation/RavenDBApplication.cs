using FubuMVC.Core;
using FubuMVC.StructureMap;

namespace RavenDBPresentation
{
	public class RavenDBApplication : IApplicationSource
	{
	    public FubuApplication BuildApplication()
	    {
            return FubuApplication.For<RavenDBFubuRegistry>()
				.StructureMap<RavenDBRegistry>();
	    }
	}
}