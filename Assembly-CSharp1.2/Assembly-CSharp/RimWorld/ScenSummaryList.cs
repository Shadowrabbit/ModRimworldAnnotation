using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001629 RID: 5673
	public static class ScenSummaryList
	{
		// Token: 0x06007B52 RID: 31570 RVA: 0x002506C4 File Offset: 0x0024E8C4
		public static string SummaryWithList(Scenario scen, string tag, string intro)
		{
			string text = ScenSummaryList.SummaryList(scen, tag);
			if (!text.NullOrEmpty())
			{
				return "\n" + intro + ":\n" + text;
			}
			return null;
		}

		// Token: 0x06007B53 RID: 31571 RVA: 0x002506F4 File Offset: 0x0024E8F4
		private static string SummaryList(Scenario scen, string tag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (ScenPart scenPart in scen.AllParts)
			{
				if (!scenPart.summarized)
				{
					foreach (string str in scenPart.GetSummaryListEntries(tag))
					{
						if (!flag)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append("   -" + str);
						scenPart.summarized = true;
						flag = false;
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
