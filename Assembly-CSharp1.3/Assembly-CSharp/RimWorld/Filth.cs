using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A7 RID: 4263
	public class Filth : Thing
	{
		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x060065A1 RID: 26017 RVA: 0x0022562A File Offset: 0x0022382A
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x060065A2 RID: 26018 RVA: 0x00225661 File Offset: 0x00223861
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x060065A3 RID: 26019 RVA: 0x0022566C File Offset: 0x0022386C
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060065A4 RID: 26020 RVA: 0x0022567F File Offset: 0x0022387F
		public int DisappearAfterTicks
		{
			get
			{
				return this.disappearAfterTicks;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060065A5 RID: 26021 RVA: 0x00225688 File Offset: 0x00223888
		public override string Label
		{
			get
			{
				if (this.sources.NullOrEmpty<string>())
				{
					return "FilthLabel".Translate(base.Label, this.thickness.ToString());
				}
				return "FilthLabelWithSource".Translate(base.Label, this.sources.ToCommaList(true, false), this.thickness.ToString());
			}
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x0022570C File Offset: 0x0022390C
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

		// Token: 0x060065A7 RID: 26023 RVA: 0x0022577C File Offset: 0x0022397C
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
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x002257E0 File Offset: 0x002239E0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x060065A9 RID: 26025 RVA: 0x00225810 File Offset: 0x00223A10
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

		// Token: 0x060065AA RID: 26026 RVA: 0x00225884 File Offset: 0x00223A84
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

		// Token: 0x060065AB RID: 26027 RVA: 0x002258D0 File Offset: 0x00223AD0
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x0022590E File Offset: 0x00223B0E
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

		// Token: 0x060065AD RID: 26029 RVA: 0x0022593C File Offset: 0x00223B3C
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x060065AE RID: 26030 RVA: 0x0022595D File Offset: 0x00223B5D
		public bool CanDropAt(IntVec3 c, Map map, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.CanMakeFilth(c, map, this.def, additionalFlags);
		}

		// Token: 0x0400395C RID: 14684
		public int thickness = 1;

		// Token: 0x0400395D RID: 14685
		public List<string> sources;

		// Token: 0x0400395E RID: 14686
		private int growTick;

		// Token: 0x0400395F RID: 14687
		private int disappearAfterTicks;

		// Token: 0x04003960 RID: 14688
		private const int MaxThickness = 5;

		// Token: 0x04003961 RID: 14689
		private const int MinAgeToPickUp = 400;

		// Token: 0x04003962 RID: 14690
		private const int MaxNumSources = 3;
	}
}
