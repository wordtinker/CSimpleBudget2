using Data;
using Data.Interfaces;
using Models;
using Models.Interfaces;
using Unity;
using Unity.Resolution;

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
            Container.RegisterType<IStorage, Storage>();
            FileHandler fileHandler = new FileHandler(
                fileName => Container.Resolve<IStorage>(
                    new ParameterOverride("fileName", fileName)
                    ));
            Container.RegisterType<IPredictor, Predictor>();
            Container.RegisterInstance<IFileHandler>(fileHandler);
            Container.RegisterInstance<IDataProvider>(new DataProvider(fileHandler));
        }
    }
}
