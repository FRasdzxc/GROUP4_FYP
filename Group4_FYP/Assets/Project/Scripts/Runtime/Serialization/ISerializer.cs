using System.IO;
using UnityEngine;

namespace PathOfHero.Serialization
{
    internal interface ISerializer
    {
        void Serialize(object serializable, FileStream fileStream);

        bool Deserialize<T>(T serializable, FileStream fileStream) where T : Object;
    }
}
