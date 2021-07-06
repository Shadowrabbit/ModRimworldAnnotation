using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3E RID: 3390
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004D72 RID: 19826 RVA: 0x00036CF4 File Offset: 0x00034EF4
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06004D73 RID: 19827 RVA: 0x000236C9 File Offset: 0x000218C9
		public virtual int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06004D74 RID: 19828 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x00036CFC File Offset: 0x00034EFC
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			yield break;
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004D77 RID: 19831 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004D78 RID: 19832 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x00036D05 File Offset: 0x00034F05
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x00036D0D File Offset: 0x00034F0D
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x00036D1B File Offset: 0x00034F1B
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x00036D29 File Offset: 0x00034F29
		public virtual string PostProcessedGerund(Job job)
		{
			return this.def.gerund;
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x00036D36 File Offset: 0x00034F36
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}
	}
}
