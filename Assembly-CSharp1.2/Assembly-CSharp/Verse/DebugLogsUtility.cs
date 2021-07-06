using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	// Token: 0x020007FF RID: 2047
	public class DebugLogsUtility
	{
		// Token: 0x060033B0 RID: 13232 RVA: 0x001502B8 File Offset: 0x0014E4B8
		public static string ThingListToUniqueCountString(IEnumerable<Thing> things)
		{
			if (things == null)
			{
				return "null";
			}
			Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
			foreach (Thing thing in things)
			{
				if (!dictionary.ContainsKey(thing.def))
				{
					dictionary.Add(thing.def, 0);
				}
				Dictionary<ThingDef, int> dictionary2 = dictionary;
				ThingDef def = thing.def;
				int num = dictionary2[def];
				dictionary2[def] = num + 1;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Registered things in dynamic draw list:");
			foreach (KeyValuePair<ThingDef, int> keyValuePair in from k in dictionary
			orderby k.Value descending
			select k)
			{
				stringBuilder.AppendLine(keyValuePair.Key + " - " + keyValuePair.Value);
			}
			return stringBuilder.ToString();
		}
	}
}
