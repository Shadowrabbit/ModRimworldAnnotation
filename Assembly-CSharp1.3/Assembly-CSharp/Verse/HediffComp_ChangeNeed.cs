using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000284 RID: 644
	public class HediffComp_ChangeNeed : HediffComp
	{
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x000697CB File Offset: 0x000679CB
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x000697D8 File Offset: 0x000679D8
		private Need Need
		{
			get
			{
				if (this.needCached == null)
				{
					this.needCached = base.Pawn.needs.TryGetNeed(this.Props.needDef);
				}
				return this.needCached;
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00069809 File Offset: 0x00067A09
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f;
			}
		}

		// Token: 0x04000DD7 RID: 3543
		private Need needCached;
	}
}
