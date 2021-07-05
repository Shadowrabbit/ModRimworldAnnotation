using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6D RID: 3693
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x060055C5 RID: 21957 RVA: 0x001D1141 File Offset: 0x001CF341
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x060055C6 RID: 21958 RVA: 0x00002688 File Offset: 0x00000888
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x060055C7 RID: 21959 RVA: 0x001D1149 File Offset: 0x001CF349
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x060055C8 RID: 21960 RVA: 0x001D1156 File Offset: 0x001CF356
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.parent.Pawn;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x060055C9 RID: 21961 RVA: 0x001D1163 File Offset: 0x001CF363
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Terrain;
			}
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x001D116A File Offset: 0x001CF36A
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x001D118B File Offset: 0x001CF38B
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x001D11B7 File Offset: 0x001CF3B7
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x060055CD RID: 21965 RVA: 0x001D11D4 File Offset: 0x001CF3D4
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.parent.Pawn && p.Spawned && this.def == p.Position.GetTerrain(p.Map) && Find.TickManager.TicksGame >= this.parent.lastTerrainBasedVerbUseTick + 1200;
		}

		// Token: 0x040032B7 RID: 12983
		public Pawn_MeleeVerbs parent;

		// Token: 0x040032B8 RID: 12984
		public TerrainDef def;

		// Token: 0x040032B9 RID: 12985
		public VerbTracker tracker;
	}
}
