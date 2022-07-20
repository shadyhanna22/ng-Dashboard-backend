namespace Advantage.API
{
    public class Helpers
    {
        private static Random rand = new();
        private static readonly List<string> pre = new()
        {
            "Sales", "Marketing", "Computer", "Network", "Meta", "Accounting", "XYZ", "Family", "Happy", "Cars"
        };

        private static readonly List<string> suf = new()
        {
            "y", "INC.", "Enterprise", "Net", "Group", "Corporation", "CO.", "End", "Logs"
        };

        private static readonly List<string> emailStart = new()
        {
            "info", "controller", "main", "admin", "accounting"
        };

        private static readonly List<string> prov = new()
        {
            "NL","PE","NS","NB","QC","ON","MB","SK"
        };

        internal static string MakeUniqueCustomerName(IList<string> existingNames)
        {
            if (pre.Count() * suf.Count() <= existingNames.Count())
            {
                throw new System.InvalidOperationException("Maximum number of unique names reached");
            }
            var prefix = getRand(pre);
            var suffix = getRand(suf);
            var finalName = prefix+suffix;
            if (existingNames.Contains(finalName))
            {
                MakeUniqueCustomerName(existingNames);
            }
            return finalName;
        }

         internal static string MakeCustomerEmail(string name)
        {
            var Semail = getRand(emailStart);
            return $"{Semail}@{name.ToLower()}.com";
        }
        
        internal static string MakeCustomerProvince()
        {
            return getRand(prov);
        }

        internal static DateTime GetRandOrderPlaced()
        {
            var end = DateTime.UtcNow;
            var start = end.AddDays(-60);

            TimeSpan maxTimeSpan = end - start;
            TimeSpan newTimeSpan = new TimeSpan(0, rand.Next(0, (int)maxTimeSpan.TotalMinutes), 0);

            return start + newTimeSpan;
        }
        internal static DateTime? GetRandOrdercompleted(DateTime orderPlaced)
        {
            var leadTime = TimeSpan.FromDays(7);
            var timeSinceOrder = DateTime.UtcNow - orderPlaced;

            if (timeSinceOrder < leadTime)
            {
                return null;
            }
            return orderPlaced.AddDays(rand.Next(7,14));
        }
        private static string getRand(IList<string> strList)
        {
            return strList[rand.Next(strList.Count)];
        }

        
    }
}