using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001832 RID: 6194
	public class CompSelfhealHitpoints : ThingComp
	{
		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06008950 RID: 35152 RVA: 0x0005C33E File Offset: 0x0005A53E
		public CompProperties_SelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_SelfhealHitpoints)this.props;
			}
		}

		// Token: 0x06008951 RID: 35153 RVA: 0x0005C34B File Offset: 0x0005A54B
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}

		// Token: 0x06008952 RID: 35154 RVA: 0x0005C35F File Offset: 0x0005A55F
		public override void CompTick()
		{
			this.Tick(1);
		}

		// Token: 0x06008953 RID: 35155 RVA: 0x0005C368 File Offset: 0x0005A568
		public override void CompTickRare()
		{
			this.Tick(250);
		}

		// Token: 0x06008954 RID: 35156 RVA: 0x0005C375 File Offset: 0x0005A575
		public override void CompTickLong()
		{
			this.Tick(2000);
		}

		// Token: 0x06008955 RID: 35157 RVA: 0x00281FC8 File Offset: 0x002801C8
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

		// Token: 0x04005810 RID: 22544
		public int ticksPassedSinceLastHeal;
	}
}
