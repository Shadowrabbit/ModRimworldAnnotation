using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DE RID: 4574
	public class IncidentWorker_ShipChunkDrop : IncidentWorker
	{
		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06006438 RID: 25656 RVA: 0x001F2130 File Offset: 0x001F0330
		private int RandomCountToDrop
		{
			get
			{
				float x2 = (float)Find.TickManager.TicksGame / 3600000f;
				float timePassedFactor = Mathf.Clamp(GenMath.LerpDouble(0f, 1.2f, 1f, 0.1f, x2), 0.1f, 1f);
				return IncidentWorker_ShipChunkDrop.CountChance.RandomElementByWeight(delegate(Pair<int, float> x)
				{
					if (x.First == 1)
					{
						return x.Second;
					}
					return x.Second * timePassedFactor;
				}).First;
			}
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x001F21A4 File Offset: 0x001F03A4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec);
		}

		// Token: 0x0600643A RID: 25658 RVA: 0x001F21DC File Offset: 0x001F03DC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec))
			{
				return false;
			}
			this.SpawnShipChunks(intVec, map, this.RandomCountToDrop);
			Messages.Message("MessageShipChunkDrop".Translate(), new TargetInfo(intVec, map, false), MessageTypeDefOf.NeutralEvent, true);
			return true;
		}

		// Token: 0x0600643B RID: 25659 RVA: 0x001F2244 File Offset: 0x001F0444
		private void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
		{
			this.SpawnChunk(firstChunkPos, map);
			for (int i = 0; i < count - 1; i++)
			{
				IntVec3 pos;
				if (this.TryFindShipChunkDropCell(firstChunkPos, map, 5, out pos))
				{
					this.SpawnChunk(pos, map);
				}
			}
		}

		// Token: 0x0600643C RID: 25660 RVA: 0x00044C00 File Offset: 0x00042E00
		private void SpawnChunk(IntVec3 pos, Map map)
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.ShipChunkIncoming, ThingDefOf.ShipChunk, pos, map);
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x001F227C File Offset: 0x001F047C
		private bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.ShipChunkIncoming, map, out pos, 10, nearLoc, maxDist, true, false, false, false, true, false, null);
		}

		// Token: 0x040042ED RID: 17133
		private static readonly Pair<int, float>[] CountChance = new Pair<int, float>[]
		{
			new Pair<int, float>(1, 1f),
			new Pair<int, float>(2, 0.95f),
			new Pair<int, float>(3, 0.7f),
			new Pair<int, float>(4, 0.4f)
		};
	}
}
