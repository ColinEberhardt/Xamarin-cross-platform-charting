using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace ShinobiStockChart.Utilities
{
    /// <summary>
    /// Implementation of IShinobiLicenseKeyProvider which reads the keys from
    /// the Application's secrets JSON file
    /// </summary>
    public class ShinobiLicenseKeyProviderJson : IShinobiLicenseKeyProvider
    {
        public static IShinobiLicenseKeyProvider Instance = new ShinobiLicenseKeyProviderJson ();
        private string _appSecretsJsonPath = "./AppSecrets.json";
        private string _chartsKey;
        private string _gridsKey;

        public ShinobiLicenseKeyProviderJson ()
        {
            // Load the data from the JSON file
            if (File.Exists (_appSecretsJsonPath)) {
                var parsedObjects = JObject.Parse (File.ReadAllText (_appSecretsJsonPath));
                _chartsKey = (string)parsedObjects ["shinobi_license_keys"] ["charts"];
                _gridsKey = (string)parsedObjects ["shinobi_license_keys"] ["grids"];
            } else {
                Debug.WriteLine ("Unable to find the AppSecrets.json file which contains the required licence keys");
            }
        }

        #region IShinobiLicenseKeyProvider implementation

        public string ChartsLicenseKey {
            get {
                return _chartsKey;
            }
        }

        public string GridsLicenseKey {
            get {
                return _gridsKey;
            }
        }

        #endregion

    }
}

