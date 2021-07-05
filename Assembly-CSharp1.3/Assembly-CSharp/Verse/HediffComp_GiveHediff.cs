using System;

namespace Verse
{
	// Token: 0x0200029B RID: 667
	public class HediffComp_GiveHediff : HediffComp
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x0006A6FA File Offset: 0x000688FA
		private HediffCompProperties_GiveHediff Props
		{
			get
			{
				return (HediffCompProperties_GiveHediff)this.props;
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0006A708 File Offset: 0x00068908
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.Props.skipIfAlreadyExists && this.parent.pawn.health.hediffSet.HasHediff(this.Props.hediffDef, false))
			{
				return;
			}
			this.parent.pawn.health.AddHediff(this.Props.hediffDef, null, null, null);
		}
	}
}
