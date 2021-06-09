using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004D1 RID: 1233
	public class RectTrigger : Thing
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x0001B2A0 File Offset: 0x000194A0
		// (set) Token: 0x06001EBE RID: 7870 RVA: 0x0001B2A8 File Offset: 0x000194A8
		public CellRect Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
				if (base.Spawned)
				{
					this.rect.ClipInsideMap(base.Map);
				}
			}
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0001B2CB File Offset: 0x000194CB
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000FDD74 File Offset: 0x000FBF74
		public override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.IsHashIntervalTick(60))
			{
				Map map = base.Map;
				for (int i = this.rect.minZ; i <= this.rect.maxZ; i++)
				{
					for (int j = this.rect.minX; j <= this.rect.maxX; j++)
					{
						List<Thing> thingList = new IntVec3(j, 0, i).GetThingList(map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.category == ThingCategory.Pawn && thingList[k].def.race.intelligence == Intelligence.Humanlike && thingList[k].Faction == Faction.OfPlayer)
							{
								this.ActivatedBy((Pawn)thingList[k]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0001B2E7 File Offset: 0x000194E7
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, p.Named("SUBJECT")));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000FDE84 File Offset: 0x000FC084
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x040015D1 RID: 5585
		private CellRect rect;

		// Token: 0x040015D2 RID: 5586
		public bool destroyIfUnfogged;

		// Token: 0x040015D3 RID: 5587
		public bool activateOnExplosion;

		// Token: 0x040015D4 RID: 5588
		public string signalTag;
	}
}
