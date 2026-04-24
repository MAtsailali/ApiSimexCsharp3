using System.Collections.Generic;

namespace apiSimex.Models
{
    public class DashboardResponse
    {
        public int ActiveCount { get; set; }
        public int PendingCount { get; set; }

        public List<RecentActivityDto> RecentActivities { get; set; }
    }
}