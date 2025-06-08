using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using PostCodeSerialMonitor.Assets;

namespace PostCodeSerialMonitor.Utils;

public static class LocalizationUtils
{
    public static IEnumerable<CultureInfo> GetAvailableCultures()
    {
        List<CultureInfo> result = new List<CultureInfo>()
        {
            // Prefill with the default culture info, as this won't be gathered by GetCultures
            CultureInfo.GetCultureInfo("en-US")
        };

        ResourceManager rm = new ResourceManager(typeof(Resources));

        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (CultureInfo culture in cultures)
        {
            try
            {
            if (culture.Equals(CultureInfo.InvariantCulture)) continue; //do not use "==", won't work

            ResourceSet? rs = rm.GetResourceSet(culture, true, false);
            if (rs != null)
                result.Add(culture);
            }
            catch (CultureNotFoundException)
            {
                
            }
        }
        return result;
    }
}