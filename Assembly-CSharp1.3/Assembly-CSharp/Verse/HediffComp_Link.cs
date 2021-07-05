using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002AC RID: 684
	public class HediffComp_Link : HediffComp
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0006B5A3 File Offset: 0x000697A3
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0006B5B0 File Offset: 0x000697B0
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
				if (this.Props.requireLinkOnOtherPawn)
				{
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
				return false;
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0006B6BC File Offset: 0x000698BC
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.drawConnection)
			{
				ThingDef moteDef = this.Props.customMote ?? ThingDefOf.Mote_PsychicLinkLine;
				if (this.mote == null)
				{
					this.mote = MoteMaker.MakeInteractionOverlay(moteDef, this.parent.pawn, this.other);
				}
				this.mote.Maintain();
			}
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0006B727 File Offset: 0x00069927
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Pawn>(ref this.other, "other", false);
			Scribe_Values.Look<bool>(ref this.drawConnection, "drawConnection", false, false);
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0006B752 File Offset: 0x00069952
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

		// Token: 0x04000E21 RID: 3617
		public Pawn other;

		// Token: 0x04000E22 RID: 3618
		private MoteDualAttached mote;

		// Token: 0x04000E23 RID: 3619
		public bool drawConnection;
	}
}
