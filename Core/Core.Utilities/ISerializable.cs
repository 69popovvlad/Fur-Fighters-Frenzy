using System.IO;

namespace Core.Utilities
{
    public interface ISerializable
    {
        void Deserialize(BinaryReader reader);

        void Serialize(BinaryWriter writer);
    }
}