using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B4 RID: 2228
	public abstract class LordToil_HiveRelated : LordToil
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x00149215 File Offset: 0x00147415
		private LordToil_HiveRelatedData Data
		{
			get
			{
				return (LordToil_HiveRelatedData)this.data;
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x00149222 File Offset: 0x00147422
		public LordToil_HiveRelated()
		{
			this.data = new LordToil_HiveRelatedData();
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x00149235 File Offset: 0x00147435
		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x00149268 File Offset: 0x00147468
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

		// Token: 0x06003AE1 RID: 15073 RVA: 0x001492AC File Offset: 0x001474AC
		private Hive FindClosestHive(Pawn pawn)
		{
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false);
		}
	}
}
