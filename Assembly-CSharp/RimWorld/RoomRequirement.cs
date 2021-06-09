using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAD RID: 7597
	public abstract class RoomRequirement
	{
		// Token: 0x0600A51B RID: 42267
		public abstract bool Met(Room r, Pawn p = null);

		// Token: 0x0600A51C RID: 42268 RVA: 0x0006D71D File Offset: 0x0006B91D
		public virtual string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x0600A51D RID: 42269 RVA: 0x0006D72F File Offset: 0x0006B92F
		public string LabelCap(Room r = null)
		{
			return this.Label(r).CapitalizeFirst();
		}

		// Token: 0x0600A51E RID: 42270 RVA: 0x0006D73D File Offset: 0x0006B93D
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x0600A51F RID: 42271 RVA: 0x0006D746 File Offset: 0x0006B946
		public virtual bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.GetType() == other.GetType();
		}

		// Token: 0x0600A520 RID: 42272 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool PlayerHasResearched()
		{
			return true;
		}

		// Token: 0x0400700A RID: 28682
		[NoTranslate]
		public string labelKey;
	}
}
