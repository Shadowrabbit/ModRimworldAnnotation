using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5F RID: 3679
	public class Outfit : IExposable, ILoadReferenceable
	{
		// Token: 0x06005523 RID: 21795 RVA: 0x001CD690 File Offset: 0x001CB890
		public Outfit()
		{
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x001CD6A3 File Offset: 0x001CB8A3
		public Outfit(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x001CD6C4 File Offset: 0x001CB8C4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x001CD6FF File Offset: 0x001CB8FF
		public string GetUniqueLoadID()
		{
			return "Outfit_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04003276 RID: 12918
		public int uniqueId;

		// Token: 0x04003277 RID: 12919
		public string label;

		// Token: 0x04003278 RID: 12920
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04003279 RID: 12921
		public static readonly Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}
