namespace TwitchViewerCounter.Core.Storage
{
    public interface IDataStorage
    {
        void StoreObject(object obj, string fileName);
        T RestoreObject<T>(string fileName);
    }
}
