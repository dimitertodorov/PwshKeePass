using System;
using System.Security;
using PwshKeePass.Common;
using PwshKeePass.Profile;

namespace PwshKeePass.Service
{
    public class KeePassService
    {
        public KeePassProfile KeePassProfile;
        protected readonly KeePassCmdlet Cmdlet;

        
        public KeePassService(KeePassProfile keePassProfile, KeePassCmdlet keePassCmdlet)
        {
            KeePassProfile = keePassProfile;
            Cmdlet = keePassCmdlet;
        }
        
        public static SecureString GetPassword(string prompt = "Enter Password:")
        {
            Console.Write(prompt);
            var pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000' ) // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            }
            return pwd;
        }
    }
}