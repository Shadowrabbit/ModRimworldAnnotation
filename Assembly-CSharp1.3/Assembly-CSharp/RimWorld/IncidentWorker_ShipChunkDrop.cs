using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C19 RID: 3097
	public class IncidentWorker_ShipChunkDrop : IncidentWorker
	{
		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x00180AB4 File Offset: 0x0017ECB4
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

		// Token: 0x060048B5 RID: 18613 RVA: 0x00180B28 File Offset: 0x0017ED28
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

		// Token: 0x060048B6 RID: 18614 RVA: 0x00180B60 File Offset: 0x0017ED60
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

		// Token: 0x060048B7 RID: 18615 RVA: 0x00180BC8 File Offset: 0x0017EDC8
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

		// Token: 0x060048B8 RID: 18616 RVA: 0x00180C00 File Offset: 0x0017EE00
		private void SpawnChunk(IntVec3 pos, Map map)
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.ShipChunkIncoming, ThingDefOf.ShipChunk, pos, map);
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00180C14 File Offset: 0x0017EE14
		private bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.ShipChunkIncoming, map, out pos, 10, nearLoc, maxDist, true, false, false, false, true, false, null);
		}

		// Token: 0x04002C70 RID: 11376
		private static readonly Pair<int, float>[] CountChance = new Pair<int, float>[]
		{
			new Pair<int, float>(1, 1f),
			new Pair<int, float>(2, 0.95f),
			new Pair<int, float>(3, 0.7f),
			new Pair<int, float>(4, 0.4f)
		};
	}
}
