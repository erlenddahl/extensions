using net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel.Items;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel
{
    public class ConsoleInformationPanelSnapshot
    {
        public Dictionary<string, string> Info { get; set; }
        public Dictionary<string, ProgressSnapshot> Progress { get; set; }

        public ConsoleInformationPanelSnapshot(ConsoleInformationPanel cip)
        {
            Progress = cip.Items
                .Where(p => p.Value is ProgressInfoItem)
                .OrderBy(p => p.Value.Sequence).ToDictionary(k => k.Key, p => new ProgressSnapshot(p.Value as ProgressInfoItem));
            
            Info = cip.Items
                .Where(p => !(p.Value is ProgressInfoItem)).OrderBy(p => p.Value.Sequence)
                .ToDictionary(k => k.Key, v => v.Value.Format(80));
        }
    }
}