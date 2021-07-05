using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000803 RID: 2051
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x00136B6A File Offset: 0x00134D6A
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000B955A File Offset: 0x000B775A
		public virtual int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060036BF RID: 14015 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x00136B72 File Offset: 0x00134D72
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			yield break;
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060036C2 RID: 14018 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060036C3 RID: 14019 RVA: 0x0009007E File Offset: 0x0008E27E
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x00136B7B File Offset: 0x00134D7B
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x00136B83 File Offset: 0x00134D83
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x00136B91 File Offset: 0x00134D91
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x00136B9F File Offset: 0x00134D9F
		public virtual string PostProcessedGerund(Job job)
		{
			return this.def.gerund;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x00136BAC File Offset: 0x00134DAC
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}
	}
}
