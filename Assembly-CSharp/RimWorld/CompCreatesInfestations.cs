using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AE RID: 6062
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x06008607 RID: 34311 RVA: 0x00277564 File Offset: 0x00275764
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedLastTick()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x06008608 RID: 34312 RVA: 0x00059E67 File Offset: 0x00058067
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 420000;
			}
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x06008609 RID: 34313 RVA: 0x002775A0 File Offset: 0x002757A0
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

		// Token: 0x0600860A RID: 34314 RVA: 0x00059E84 File Offset: 0x00058084
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x0600860B RID: 34315 RVA: 0x00059E9C File Offset: 0x0005809C
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04005665 RID: 22117
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x04005666 RID: 22118
		private const float MinRefireDays = 7f;

		// Token: 0x04005667 RID: 22119
		private const float PreventInfestationsDist = 10f;
	}
}
