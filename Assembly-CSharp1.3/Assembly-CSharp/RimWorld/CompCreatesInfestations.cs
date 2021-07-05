using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111C RID: 4380
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06006933 RID: 26931 RVA: 0x002377F4 File Offset: 0x002359F4
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedLastTick()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06006934 RID: 26932 RVA: 0x0023782F File Offset: 0x00235A2F
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 420000;
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06006935 RID: 26933 RVA: 0x0023784C File Offset: 0x00235A4C
		public bool CantFireBecauseSomethingElseCreatedInfestationRecently
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this.parent && list[i].Position.InHorDistOf(this.parent.Position, 10f) && list[i].TryGetComp<CompCreatesInfestations>().CantFireBecauseCreatedInfestationRecently)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x002378DC File Offset: 0x00235ADC
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x002378F4 File Offset: 0x00235AF4
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04003AE0 RID: 15072
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x04003AE1 RID: 15073
		private const float MinRefireDays = 7f;

		// Token: 0x04003AE2 RID: 15074
		private const float PreventInfestationsDist = 10f;
	}
}
