using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118D RID: 4493
	public class CompSelfhealHitpoints : ThingComp
	{
		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06006C14 RID: 27668 RVA: 0x00243E9C File Offset: 0x0024209C
		public CompProperties_SelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_SelfhealHitpoints)this.props;
			}
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x00243EA9 File Offset: 0x002420A9
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}

		// Token: 0x06006C16 RID: 27670 RVA: 0x00243EBD File Offset: 0x002420BD
		public override void CompTick()
		{
			this.Tick(1);
		}

		// Token: 0x06006C17 RID: 27671 RVA: 0x00243EC6 File Offset: 0x002420C6
		public override void CompTickRare()
		{
			this.Tick(250);
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x00243ED3 File Offset: 0x002420D3
		public override void CompTickLong()
		{
			this.Tick(2000);
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x00243EE0 File Offset: 0x002420E0
		private void Tick(int ticks)
		{
			this.ticksPassedSinceLastHeal += ticks;
			if (this.ticksPassedSinceLastHeal >= this.Props.ticksPerHeal)
			{
				this.ticksPassedSinceLastHeal = 0;
				if (this.parent.HitPoints < this.parent.MaxHitPoints)
				{
					ThingWithComps parent = this.parent;
					int hitPoints = parent.HitPoints;
					parent.HitPoints = hitPoints + 1;
				}
			}
		}

		// Token: 0x04003C16 RID: 15382
		public int ticksPassedSinceLastHeal;
	}
}
