using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000341 RID: 833
	public class RectTrigger : PawnTrigger
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0008DD5E File Offset: 0x0008BF5E
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x0008DD66 File Offset: 0x0008BF66
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

		// Token: 0x060017BF RID: 6079 RVA: 0x0008DD89 File Offset: 0x0008BF89
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0008DDA8 File Offset: 0x0008BFA8
		public override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
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
							if (base.TriggeredBy(thingList[k]))
							{
								base.ActivatedBy((Pawn)thingList[k]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0008DE78 File Offset: 0x0008C078
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
		}

		// Token: 0x04001057 RID: 4183
		private CellRect rect;

		// Token: 0x04001058 RID: 4184
		public bool destroyIfUnfogged;

		// Token: 0x04001059 RID: 4185
		public bool activateOnExplosion;
	}
}
