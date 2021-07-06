using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FC RID: 5372
	public class Outfit : IExposable, ILoadReferenceable
	{
		// Token: 0x060073AD RID: 29613 RVA: 0x0004DEB8 File Offset: 0x0004C0B8
		public Outfit()
		{
		}

		// Token: 0x060073AE RID: 29614 RVA: 0x0004DECB File Offset: 0x0004C0CB
		public Outfit(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
		}

		// Token: 0x060073AF RID: 29615 RVA: 0x0004DEEC File Offset: 0x0004C0EC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x0004DF27 File Offset: 0x0004C127
		public string GetUniqueLoadID()
		{
			return "Outfit_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04004C6F RID: 19567
		public int uniqueId;

		// Token: 0x04004C70 RID: 19568
		public string label;

		// Token: 0x04004C71 RID: 19569
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04004C72 RID: 19570
		public static readonly Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}
