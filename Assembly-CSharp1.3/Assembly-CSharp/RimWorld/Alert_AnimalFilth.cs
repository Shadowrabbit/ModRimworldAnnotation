using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001281 RID: 4737
	public class Alert_AnimalFilth : Alert
	{
		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06007132 RID: 28978 RVA: 0x0025B8C7 File Offset: 0x00259AC7
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.CalculateTargets();
				return this.targets;
			}
		}

		// Token: 0x06007133 RID: 28979 RVA: 0x0025B8D5 File Offset: 0x00259AD5
		public Alert_AnimalFilth()
		{
			this.defaultLabel = "AlertAnimalFilth".Translate();
		}

		// Token: 0x06007134 RID: 28980 RVA: 0x0025B908 File Offset: 0x00259B08
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnEntries.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && pawn.GetStatValue(StatDefOf.FilthRate, true) >= 4f)
				{
					IntVec3 position = pawn.Position;
					Map map = pawn.Map;
					TerrainDef terrain = position.GetTerrain(map);
					if (terrain.GetStatValueAbstract(StatDefOf.FilthMultiplier, null) > 0.5f && FilthMaker.TerrainAcceptsFilth(terrain, ThingDefOf.Filth_AnimalFilth, FilthSourceFlags.Pawn) && position.GetRoof(map) != null)
					{
						Room room = position.GetRoom(map);
						if (room != null && !room.Fogged && !room.TouchesMapEdge && pawn.Map.areaManager.Home[pawn.Position])
						{
							this.targets.Add(pawn);
							this.pawnEntries.Add(pawn.NameShortColored.Resolve());
						}
					}
				}
			}
		}

		// Token: 0x06007135 RID: 28981 RVA: 0x0025BA50 File Offset: 0x00259C50
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalFilthDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x06007136 RID: 28982 RVA: 0x0025BA71 File Offset: 0x00259C71
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E42 RID: 15938
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E43 RID: 15939
		private List<string> pawnEntries = new List<string>();

		// Token: 0x04003E44 RID: 15940
		private const float MinFilthRate = 4f;

		// Token: 0x04003E45 RID: 15941
		private const float MinFilthMultiplier = 0.5f;
	}
}
