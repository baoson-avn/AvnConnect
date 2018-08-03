using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AvnConnect.Data;

namespace AvnConnect
{
   internal static class Encryption
    {
        internal static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        internal static string GetPermisionKey(UserProjectPermission permission)
        {
            string key = "";
            key = key + (permission.CanAddLinks ? "+" : "-");
            key = key + (permission.CanAddUpdate ? "+" : "-");
            key = key + (permission.CanCreateTask ? "+" : "-");
            key = key + (permission.CanUpdateAllTask ? "+" : "-");
            key = key + (permission.CanUpdateMessageAndFile ? "+" : "-");
            key = key + (permission.CanUpdateNoteBook ? "+" : "-");
            key = key + (permission.CanUpdateRisk ? "+" : "-");
            key = key + (permission.CanViewEstimatedTime ? "+" : "-");
            key = key + (permission.CanViewLinks ? "+" : "-");
            key = key + (permission.CanViewMessageAndFile ? "+" : "-");
            key = key + (permission.CanViewNoteBook ? "+" : "-");
            key = key + (permission.CanViewRisk ? "+" : "-");
            key = key + (permission.CanViewTask ? "+" : "-");
            key = key + (permission.CanViewUpdate ? "+" : "-");
            return key;
        }
    }
}
