using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveProgrammingMEF
{
    public static class MEF
    {
        public static void Compose(object obj)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog("../../../Plugins"));
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(obj);
        }
    }
}
