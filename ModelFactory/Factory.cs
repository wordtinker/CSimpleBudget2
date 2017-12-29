using Models;
using Models.Interfaces;
using Unity;

namespace ModelFactory
{
    /// <summary>
    /// Container that provides model and validator classes for
    /// an app, and some internal classes.
    /// </summary>
    public class Factory
    {
        public IUnityContainer Container { get; private set; }

        public Factory()
        {
            // Bind everything within container.
            Container = new UnityContainer();
            StubFileHandler fileHandler = new StubFileHandler();
            Container.RegisterInstance<IFileHandler>(fileHandler);
            Container.RegisterInstance<IDataProvider>(new StubDataProvider(fileHandler));
        }
    }
}
