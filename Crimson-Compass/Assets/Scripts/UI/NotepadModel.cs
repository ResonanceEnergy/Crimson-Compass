using System.Collections.Generic;

namespace CrimsonCompass.UI
{
    public class NotepadModel
    {
        public HashSet<string> DisprovedWho = new();
        public HashSet<string> DisprovedHow = new();
        public HashSet<string> DisprovedWhere = new();

        public void MarkDisproved(string axis, string id)
        {
            switch (axis)
            {
                case "WHO": DisprovedWho.Add(id); break;
                case "HOW": DisprovedHow.Add(id); break;
                case "WHERE": DisprovedWhere.Add(id); break;
            }
        }
    }
}
