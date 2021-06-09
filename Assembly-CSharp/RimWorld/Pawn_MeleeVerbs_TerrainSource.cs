using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001512 RID: 5394
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x0600744D RID: 29773 RVA: 0x0004E6F6 File Offset: 0x0004C8F6
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x0600744E RID: 29774 RVA: 0x0000C32E File Offset: 0x0000A52E
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x0600744F RID: 29775 RVA: 0x0004E6FE File Offset: 0x0004C8FE
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06007450 RID: 29776 RVA: 0x0004E70B File Offset: 0x0004C90B
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.parent.Pawn;
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06007451 RID: 29777 RVA: 0x0004E718 File Offset: 0x0004C918
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Terrain;
			}
		}

		// Token: 0x06007452 RID: 29778 RVA: 0x0004E71F File Offset: 0x0004C91F
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x06007453 RID: 29779 RVA: 0x0004E740 File Offset: 0x0004C940
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x06007454 RID: 29780 RVA: 0x0004E76C File Offset: 0x0004C96C
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x00236E70 File Offset: 0x00235070
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.parent.Pawn && p.Spawned && this.def == p.Position.GetTerrain(p.Map) && Find.TickManager.TicksGame >= this.parent.lastTerrainBasedVerbUseTick + 1200;
		}

		// Token: 0x04004CBC RID: 19644
		public Pawn_MeleeVerbs parent;

		// Token: 0x04004CBD RID: 19645
		public TerrainDef def;

		// Token: 0x04004CBE RID: 19646
		public VerbTracker tracker;
	}
}
