using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001199 RID: 4505
	public class CompSmokeCloudMaker : ThingComp
	{
		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06006C78 RID: 27768 RVA: 0x0024684E File Offset: 0x00244A4E
		private CompProperties_SmokeCloudMaker Props
		{
			get
			{
				return (CompProperties_SmokeCloudMaker)this.props;
			}
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x0024685C File Offset: 0x00244A5C
		public override void CompTick()
		{
			if (!ModLister.CheckIdeology("Smoke cloud maker"))
			{
				return;
			}
			if (!this.parent.Spawned)
			{
				return;
			}
			CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if ((compRefuelable == null || compRefuelable.HasFuel) & (compPowerTrader == null || compPowerTrader.PowerOn))
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.sourceStreamEffect.Spawn();
					this.effecter.Trigger(this.parent, this.parent);
				}
				this.effecter.EffectTick(this.parent, this.parent);
				if (Rand.MTBEventOccurs(this.Props.fleckSpawnMTB, 1f, 1f))
				{
					CompSmokeCloudMaker.tmpValidCells.Clear();
					Room room = this.parent.GetRoom(RegionType.Set_All);
					CellRect cellRect = this.parent.OccupiedRect();
					foreach (IntVec3 intVec in cellRect.ExpandedBy(Mathf.FloorToInt(this.Props.cloudRadius + 1f)))
					{
						if (cellRect.ClosestCellTo(intVec).DistanceTo(intVec) <= this.Props.cloudRadius && intVec.GetRoom(this.parent.Map) == room)
						{
							CompSmokeCloudMaker.tmpValidCells.Add(intVec);
						}
					}
					if (CompSmokeCloudMaker.tmpValidCells.Count > 0)
					{
						Vector3 loc = CompSmokeCloudMaker.tmpValidCells.RandomElement<IntVec3>().ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
						this.parent.Map.flecks.CreateFleck(FleckMaker.GetDataStatic(loc, this.parent.Map, this.Props.cloudFleck, this.Props.fleckScale));
						return;
					}
				}
			}
			else
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
			}
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x00246A74 File Offset: 0x00244C74
		public override void PostDeSpawn(Map map)
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x04003C4B RID: 15435
		private Effecter effecter;

		// Token: 0x04003C4C RID: 15436
		private static List<IntVec3> tmpValidCells = new List<IntVec3>();
	}
}
