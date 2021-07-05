using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152B RID: 5419
	public class RoomRequirement_ThingCount : RoomRequirement_Thing
	{
		// Token: 0x060080F2 RID: 33010 RVA: 0x002DA1DE File Offset: 0x002D83DE
		public override bool Met(Room r, Pawn p = null)
		{
			return this.Count(r) >= this.count;
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x002DA1F2 File Offset: 0x002D83F2
		public int Count(Room r)
		{
			return r.ThingCount(this.thingDef);
		}

		// Token: 0x060080F4 RID: 33012 RVA: 0x002DA200 File Offset: 0x002D8400
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

		// Token: 0x060080F5 RID: 33013 RVA: 0x002DA2A9 File Offset: 0x002D84A9
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

		// Token: 0x060080F6 RID: 33014 RVA: 0x002DA2B9 File Offset: 0x002D84B9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x04005056 RID: 20566
		public int count;
	}
}
