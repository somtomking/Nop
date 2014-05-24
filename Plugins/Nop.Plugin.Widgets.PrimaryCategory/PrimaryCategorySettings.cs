using System.Collections.Generic;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.PrimaryCategory
{
    public class PrimaryCategorySettings : ISettings
    {
        public string Order1 { get; set; }
        public int Picture1Id { get; set; }
        public string Link1 { get; set; }

        public string Order2 { get; set; }
        public int Picture2Id { get; set; }
        public string Link2 { get; set; }

        public string Order3 { get; set; }
        public int Picture3Id { get; set; }
        public string Link3 { get; set; }

        public string Order4 { get; set; }
        public int Picture4Id { get; set; }
        public string Link4 { get; set; }

        public string Order5 { get; set; }
        public int Picture5Id { get; set; }
        public string Link5 { get; set; }

        public string Order6 { get; set; }
        public int Picture6Id { get; set; }
        public string Link6 { get; set; }

        public string Order7 { get; set; }
        public int Picture7Id { get; set; }
        public string Link7 { get; set; }

        public string Order8 { get; set; }
        public int Picture8Id { get; set; }
        public string Link8 { get; set; }

        public string Order9 { get; set; }
        public int Picture9Id { get; set; }
        public string Link9 { get; set; }

        public string ProductIdsToKill { get; set; }
    }
}