using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101B RID: 4123
	public static class ScenSummaryList
	{
		// Token: 0x06006132 RID: 24882 RVA: 0x00210560 File Offset: 0x0020E760
		public static string SummaryWithList(Scenario scen, string tag, string intro)
		{
			string text = ScenSummaryList.SummaryList(scen, tag);
			if (!text.NullOrEmpty())
			{
				return "\n" + intro + ":\n" + text;
			}
			return null;
		}

		// Token: 0x06006133 RID: 24883 RVA: 0x00210590 File Offset: 0x0020E790
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
