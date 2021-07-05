using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001579 RID: 5497
	public class CompSpawnEffectersInRoom : ThingComp
	{
		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x060081F1 RID: 33265 RVA: 0x002DEBEF File Offset: 0x002DCDEF
		private CompProperties_SpawnEffectersInRoom Props
		{
			get
			{
				return (CompProperties_SpawnEffectersInRoom)this.props;
			}
		}

		// Token: 0x060081F2 RID: 33266 RVA: 0x002DEBFC File Offset: 0x002DCDFC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.effecters.Clear();
		}

		// Token: 0x060081F3 RID: 33267 RVA: 0x002DEC10 File Offset: 0x002DCE10
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			foreach (Effecter effecter in this.effecters.Values)
			{
				effecter.Cleanup();
			}
		}

		// Token: 0x060081F4 RID: 33268 RVA: 0x002DEC6C File Offset: 0x002DCE6C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			Room room = this.parent.GetRoom(RegionType.Set_All);
			if (room == null || room.TouchesMapEdge)
			{
				return;
			}
			if (this.parent.IsRitualTarget())
			{
				foreach (IntVec3 cell in room.Cells)
				{
					if (cell.InHorDistOf(this.parent.Position, this.Props.radius))
					{
						this.CheckEffecter(cell);
					}
				}
			}
		}

		// Token: 0x060081F5 RID: 33269 RVA: 0x002DED10 File Offset: 0x002DCF10
		private void CheckEffecter(IntVec3 cell)
		{
			if (this.effecters.ContainsKey(cell))
			{
				if (this.effecters[cell] == null)
				{
					this.effecters[cell] = this.Props.effecter.Spawn();
					this.effecters[cell].Trigger(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid);
				}
			}
			else
			{
				this.effecters[cell] = this.Props.effecter.Spawn();
				this.effecters[cell].Trigger(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid);
			}
			this.effecters[cell].EffectTick(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid);
		}

		// Token: 0x040050DA RID: 20698
		private Dictionary<IntVec3, Effecter> effecters = new Dictionary<IntVec3, Effecter>();
	}
}
