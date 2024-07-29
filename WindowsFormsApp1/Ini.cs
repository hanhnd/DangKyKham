using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DieuKhienTiepDon.Moduls
{
    public class IniFile
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            string lpReturnString,
            int nSize,
            string lpFilename);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFilename);

        

        /// <summary>
        /// Main program
        /// </summary>
        //static void Main()
        //{
        

        //    /*
        //     * Try to read the content of a simle INI file
        //     */
        //    List<string> categories = GetCategories(iniFile);
        //    foreach (string category in categories)
        //    {
        //        Console.WriteLine(category);

        //        /*
        //         * Get the keys
        //         */
        //        List<string> keys = GetKeys(iniFile, category);
        //        foreach (string key in keys)
        //        {
        //            /*
        //             * Now output the content
        //             */
        //            string content = GetIniFileString(iniFile, category, key, defaultValue);
        //            Console.WriteLine(string.Concat("  ", key, "\t", content));
        //        }
        //    }

        //    /*
        //     * Last but not least, write your own INI File
        //     * If you do not specify a directory, the INI File will be created within the Windows Directory.
        //     * C:\Windows\MyIniFile.ini
        //     */
        //    WritePrivateProfileString("GLOBAL", "Stuff", "The content of that stuff", "MyIniFile.ini");

        //    Console.ReadKey();
        //}

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="iniFile">The ini file.</param>
        /// <param name="category">The category.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        
        private string iniFile = @"c:\windows\CSDLDV.ini";
        public IniFile(string INIPath)
        {
            iniFile = INIPath;
        }
        public void WriteIniFileString(string category, string key, string Value)
        {
            WritePrivateProfileString(category, key, Value, iniFile);
        }
        public string GetIniFileString(string category, string key, string defaultValue)
        {
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, iniFile);
            return returnString.Split('\0')[0];
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <param name="iniFile">The ini file.</param>
        /// <param name="category">The category.</param>
        private static List<string> GetKeys(string iniFile, string category)
        {
            string returnString = new string(' ', 32768);
            GetPrivateProfileString(category, null, null, returnString, 32768, iniFile);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <param name="iniFile">The ini file.</param>
        /// <returns></returns>
        private static List<string> GetCategories(string iniFile)
        {
            string returnString = new string(' ', 65536);
            GetPrivateProfileString(null, null, null, returnString, 65536, iniFile);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }
    }
}
