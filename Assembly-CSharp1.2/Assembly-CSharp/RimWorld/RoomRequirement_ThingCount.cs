using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB5 RID: 7605
	public class RoomRequirement_ThingCount : RoomRequirement_Thing
	{
		// Token: 0x0600A554 RID: 42324 RVA: 0x0006D8E7 File Offset: 0x0006BAE7
		public override bool Met(Room r, Pawn p = null)
		{
			return this.Count(r) >= this.count;
		}

		// Token: 0x0600A555 RID: 42325 RVA: 0x0006D8FB File Offset: 0x0006BAFB
		public int Count(Room r)
		{
			return r.ThingCount(this.thingDef);
		}

		// Token: 0x0600A556 RID: 42326 RVA: 0x003004D4 File Offset: 0x002FE6D4
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.thingDef.label;
			if (r != null)
			{
				return string.Concat(new object[]
				{
					text,
					" ",
					this.Count(r),
					"/",
					this.count
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
			return text + " x" + this.count;
		}

		// Token: 0x0600A557 RID: 42327 RVA: 0x0006D909 File Offset: 0x0006BB09
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.count <= 0)
			{
				yield return "count must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400701F RID: 28703
		public int count;
	}
}
