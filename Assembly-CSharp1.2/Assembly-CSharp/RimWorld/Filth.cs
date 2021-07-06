using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016FE RID: 5886
	public class Filth : Thing
	{
		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x06008161 RID: 33121 RVA: 0x00056D9B File Offset: 0x00054F9B
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06008162 RID: 33122 RVA: 0x00056DD2 File Offset: 0x00054FD2
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06008163 RID: 33123 RVA: 0x00056DDD File Offset: 0x00054FDD
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x06008164 RID: 33124 RVA: 0x00056DF0 File Offset: 0x00054FF0
		public int DisappearAfterTicks
		{
			get
			{
				return this.disappearAfterTicks;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x06008165 RID: 33125 RVA: 0x002665C4 File Offset: 0x002647C4
		public override string Label
		{
			get
			{
				if (this.sources.NullOrEmpty<string>())
				{
					return "FilthLabel".Translate(base.Label, this.thickness.ToString());
				}
				return "FilthLabelWithSource".Translate(base.Label, this.sources.ToCommaList(true), this.thickness.ToString());
			}
		}

		// Token: 0x06008166 RID: 33126 RVA: 0x00266644 File Offset: 0x00264844
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.thickness, "thickness", 1, false);
			Scribe_Values.Look<int>(ref this.growTick, "growTick", 0, false);
			Scribe_Values.Look<int>(ref this.disappearAfterTicks, "disappearAfterTicks", 0, false);
			if (Scribe.mode != LoadSaveMode.Saving || this.sources != null)
			{
				Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, Array.Empty<object>());
			}
		}

		// Token: 0x06008167 RID: 33127 RVA: 0x002666B4 File Offset: 0x002648B4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing)
			{
				base.Map.listerFilthInHomeArea.Notify_FilthSpawned(this);
			}
			if (!respawningAfterLoad)
			{
				this.growTick = Find.TickManager.TicksGame;
				this.disappearAfterTicks = (int)(this.def.filth.disappearsInDays.RandomInRange * 60000f);
			}
			if (!FilthMaker.TerrainAcceptsFilth(base.Map.terrainGrid.TerrainAt(base.Position), this.def, FilthSourceFlags.None))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06008168 RID: 33128 RVA: 0x00266744 File Offset: 0x00264944
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x06008169 RID: 33129 RVA: 0x00266774 File Offset: 0x00264974
		public void AddSource(string newSource)
		{
			if (this.sources == null)
			{
				this.sources = new List<string>();
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i] == newSource)
				{
					return;
				}
			}
			while (this.sources.Count > 3)
			{
				this.sources.RemoveAt(0);
			}
			this.sources.Add(newSource);
		}

		// Token: 0x0600816A RID: 33130 RVA: 0x002667E8 File Offset: 0x002649E8
		public void AddSources(IEnumerable<string> sources)
		{
			if (sources == null)
			{
				return;
			}
			foreach (string newSource in sources)
			{
				this.AddSource(newSource);
			}
		}

		// Token: 0x0600816B RID: 33131 RVA: 0x00056DF8 File Offset: 0x00054FF8
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x00056E36 File Offset: 0x00055036
		public void ThinFilth()
		{
			this.thickness--;
			if (base.Spawned)
			{
				if (this.thickness == 0)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				this.UpdateMesh();
			}
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x00056E64 File Offset: 0x00055064
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x0600816E RID: 33134 RVA: 0x00056E85 File Offset: 0x00055085
		public bool CanDropAt(IntVec3 c, Map map, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.CanMakeFilth(c, map, this.def, additionalFlags);
		}

		// Token: 0x040053F4 RID: 21492
		public int thickness = 1;

		// Token: 0x040053F5 RID: 21493
		public List<string> sources;

		// Token: 0x040053F6 RID: 21494
		private int growTick;

		// Token: 0x040053F7 RID: 21495
		private int disappearAfterTicks;

		// Token: 0x040053F8 RID: 21496
		private const int MaxThickness = 5;

		// Token: 0x040053F9 RID: 21497
		private const int MinAgeToPickUp = 400;

		// Token: 0x040053FA RID: 21498
		private const int MaxNumSources = 3;
	}
}
