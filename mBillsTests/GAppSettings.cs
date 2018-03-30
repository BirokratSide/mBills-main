using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace mBillsTests
{
    public static class GAppSettings
    {
        #region // locals //
        private static Dictionary<string, string> dictionaryStrings = new Dictionary<string, string>();
        #endregion
        #region // public //
        public static string Get(string key, string def = "")
        {
            if (dictionaryStrings.Count < 1)
                loadAppSettings();
            if (dictionaryStrings.ContainsKey(key))
                return dictionaryStrings[key];
            else
                return def;
        }
        #endregion
        #region // private //
        private static void loadAppSettings()
        {
            Dictionary<string, string> dictionaryGlobal = new Dictionary<string, string>();
            Dictionary<string, string> dictionaryMachine = new Dictionary<string, string>();
            Dictionary<string, string> dictionaryUser = new Dictionary<string, string>();

            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                string[] keyParts = key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                switch (keyParts.Length)
                {
                    case 1:
                        if (dictionaryGlobal.ContainsKey(keyParts[0]))
                            dictionaryGlobal[keyParts[0]] = ConfigurationManager.AppSettings[key];
                        else
                            dictionaryGlobal.Add(keyParts[0], ConfigurationManager.AppSettings[key]);
                        break;
                    case 2:
                        if (keyParts[0].ToLower().Equals(Environment.MachineName.ToLower()))
                        {
                            if (dictionaryGlobal.ContainsKey(keyParts[1]))
                                dictionaryGlobal[keyParts[1]] = ConfigurationManager.AppSettings[key];
                            else
                                dictionaryGlobal.Add(keyParts[1], ConfigurationManager.AppSettings[key]);
                        }
                        break;
                    case 3:
                        if (keyParts[0].ToLower().Equals(Environment.MachineName.ToLower()))
                        {
                            if (keyParts[1].ToLower().Equals(Environment.UserName.ToLower()))
                            {
                                if (dictionaryGlobal.ContainsKey(keyParts[1]))
                                    dictionaryGlobal[keyParts[2]] = ConfigurationManager.AppSettings[key];
                                else
                                    dictionaryGlobal.Add(keyParts[2], ConfigurationManager.AppSettings[key]);
                            }
                        }
                        break;
                }
            }

            foreach (KeyValuePair<string, string> itm in dictionaryGlobal)
            {
                set(itm.Key, itm.Value);
            }
            foreach (KeyValuePair<string, string> itm in dictionaryMachine)
            {
                set(itm.Key, itm.Value);
            }
            foreach (KeyValuePair<string, string> itm in dictionaryUser)
            {
                set(itm.Key, itm.Value);
            }
        }
        private static void set(string key, string val)
        {
            if (dictionaryStrings.ContainsKey(key))
                dictionaryStrings[key] = val;
            else
                dictionaryStrings.Add(key, val);
        }
        #endregion
    }
}