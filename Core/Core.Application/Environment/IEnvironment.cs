namespace Core.Application.Environment
{
    public interface IEnvironment
    {
        /// <summary>
        /// Unique device ID.
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Vendor-specific, readable device model.
        /// </summary>
        string DeviceModel { get; }

        /// <summary>
        /// Defined by user device name.
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// A path to the folder where the application embedded data is.
        /// </summary>
        string EmbeddedDataPath { get; }

        /// <summary>
        /// Meaningful operating system name.
        /// </summary>
        string OperatingSystem { get; }

        /// <summary>
        /// A path to the folder the application data may be persisted between runs (non-secure).
        /// </summary>
        string PersistentDataPath { get; }
        
        /// <summary>
        /// A path to the folder the application downloaded data may be persisted between runs (non-secure).
        /// </summary>
        string SharedFolderPath { get; }
    }
}