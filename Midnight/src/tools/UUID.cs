using System.Security.Cryptography;
using System.Text;

namespace Midnight;

public class UUID {
    public static class v3 {
        public static System.Guid Convert(System.Guid namespaceUUID, string name) {
            // combine both namespace bytes with name bytes
            byte[] namespaceBytes = namespaceUUID.ToByteArray(),
                   nameBytes = Encoding.UTF8.GetBytes(name),
                   fullname = new byte[namespaceBytes.Length + nameBytes.Length];

            namespaceBytes.CopyTo(fullname, 0);
            nameBytes.CopyTo(fullname, namespaceBytes.Length);

            return Hash(fullname);
        }

        public static System.Guid Convert(string name) {
            return Hash(Encoding.UTF8.GetBytes(name));
        }

        public static System.Guid Hash(byte[] inputBytes) {
            byte[] bytes = MD5.HashData(inputBytes);

            // adjust version bit
            bytes[6] &= 0x0F;
            bytes[6] |= 0x50;

            // adjust variant to 0xa (10)
            bytes[8] &= 0x3F;
            bytes[8] |= 0x80;

            return new(bytes);
        }
    }
}
