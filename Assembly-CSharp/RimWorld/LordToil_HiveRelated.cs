using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DFB RID: 3579
	public abstract class LordToil_HiveRelated : LordToil
	{
		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x0003904C File Offset: 0x0003724C
		private LordToil_HiveRelatedData Data
		{
			get
			{
				return (LordToil_HiveRelatedData)this.data;
			}
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x00039059 File Offset: 0x00037259
		public LordToil_HiveRelated()
		{
			this.data = new LordToil_HiveRelatedData();
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0003906C File Offset: 0x0003726C
		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x001BBA34 File Offset: 0x001B9C34
		protected Hive GetHiveFor(Pawn pawn)
		{
			Hive hive;
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				return hive;
			}
			hive = this.FindClosestHive(pawn);
			if (hive != null)
			{
				this.Data.assignedHives.Add(pawn, hive);
			}
			return hive;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x001BBA78 File Offset: 0x001B9C78
		private Hive FindClosestHive(Pawn pawn)
		{
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false);
		}
	}
}
