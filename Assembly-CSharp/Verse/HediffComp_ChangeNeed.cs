using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C5 RID: 965
	public class HediffComp_ChangeNeed : HediffComp
	{
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x00016CF2 File Offset: 0x00014EF2
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00016CFF File Offset: 0x00014EFF
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

		// Token: 0x060017FA RID: 6138 RVA: 0x00016D30 File Offset: 0x00014F30
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f;
			}
		}

		// Token: 0x0400123B RID: 4667
		private Need needCached;
	}
}
