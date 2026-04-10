using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel
{
    public class ProgressSnapshot
    {
        public double DurationRemainingS { get; set; }
        public double DurationS { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public double Percentage { get; set; }
        public long Max { get; set; }
        public long Current { get; set; }
        public string Visualization { get; set; }

        public ProgressSnapshot(ProgressInfoItem pii)
        {
            Current = pii.Current;
            Max = pii.Max;
            Percentage = Current / (double) Max * 100d;
            StartTime = pii.StartTime;
            EndTime = pii.EndTime;

            if (StartTime.HasValue)
                DurationS = (EndTime ?? DateTime.Now).Subtract(StartTime.Value).TotalSeconds;

            DurationRemainingS = EndTime.HasValue ? 0d : DurationS / Current * Max;
            Visualization = pii.Format(80);
        }
    }
}