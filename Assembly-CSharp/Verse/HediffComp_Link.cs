using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E9 RID: 1001
	public class HediffComp_Link : HediffComp
	{
		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600187C RID: 6268 RVA: 0x000173C3 File Offset: 0x000155C3
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x000DF724 File Offset: 0x000DD924
		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				if (this.other == null || !this.parent.pawn.Spawned || !this.other.Spawned)
				{
					return true;
				}
				if (this.Props.maxDistance > 0f && !this.parent.pawn.Position.InHorDistOf(this.other.Position, this.Props.maxDistance))
				{
					return true;
				}
				foreach (Hediff hediff in this.other.health.hediffSet.hediffs)
				{
					HediffWithComps hediffWithComps = hediff as HediffWithComps;
					if (hediffWithComps != null && hediffWithComps.comps.FirstOrDefault(delegate(HediffComp c)
					{
						HediffComp_Link hediffComp_Link = c as HediffComp_Link;
						return hediffComp_Link != null && hediffComp_Link.other == this.parent.pawn && hediffComp_Link.parent.def == this.parent.def;
					}) != null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x000DF820 File Offset: 0x000DDA20
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.drawConnection)
			{
				if (this.mote == null)
				{
					this.mote = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkLine, this.parent.pawn, this.other);
				}
				this.mote.Maintain();
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000173D0 File Offset: 0x000155D0
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Pawn>(ref this.other, "other", false);
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001880 RID: 6272 RVA: 0x000173E9 File Offset: 0x000155E9
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showName || this.other == null)
				{
					return null;
				}
				return this.other.LabelShort;
			}
		}

		// Token: 0x04001289 RID: 4745
		public Pawn other;

		// Token: 0x0400128A RID: 4746
		private MoteDualAttached mote;

		// Token: 0x0400128B RID: 4747
		public bool drawConnection;
	}
}
