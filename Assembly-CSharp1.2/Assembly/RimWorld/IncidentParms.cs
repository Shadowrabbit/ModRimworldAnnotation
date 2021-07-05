using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001188 RID: 4488
	public class IncidentParms : IExposable
	{
		// Token: 0x060062F0 RID: 25328 RVA: 0x001ED690 File Offset: 0x001EB890
		public void ExposeData()
		{
			Scribe_References.Look<IIncidentTarget>(ref this.target, "target", false);
			Scribe_Values.Look<float>(ref this.points, "threatPoints", 0f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.forced, "forced", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Collections.Look<ThingDef>(ref this.letterHyperlinkThingDefs, "letterHyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.letterHyperlinkHediffDefs, "letterHyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalEnd, "inSignalEnd", null, false);
			Scribe_Values.Look<IntVec3>(ref this.spawnCenter, "spawnCenter", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.spawnRotation, "spawnRotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.dontUseSingleUseRocketLaunchers, "dontUseSingleUseRocketLaunchers", false, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.raidArrivalMode, "raidArrivalMode");
			Scribe_Values.Look<bool>(ref this.raidForceOneIncap, "raidForceIncap", false, false);
			Scribe_Values.Look<bool>(ref this.raidNeverFleeIndividual, "raidNeverFleeIndividual", false, false);
			Scribe_Values.Look<bool>(ref this.raidArrivalModeForQuickMilitaryAid, "raidArrivalModeForQuickMilitaryAid", false, false);
			Scribe_Collections.Look<Pawn, int>(ref this.pawnGroups, "pawnGroups", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpGroups);
			Scribe_Values.Look<int?>(ref this.pawnGroupMakerSeed, "pawnGroupMakerSeed", null, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.pawnKind, "pawnKind");
			Scribe_Values.Look<float>(ref this.biocodeWeaponsChance, "biocodeWeaponsChance", 0f, false);
			Scribe_Values.Look<float>(ref this.biocodeApparelChance, "biocodeApparelChance", 0f, false);
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Values.Look<int>(ref this.podOpenDelay, "podOpenDelay", 140, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScriptDef, "questScriptDef");
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x001ED8D4 File Offset: 0x001EBAD4
		public override string ToString()
		{
			string text = "";
			if (this.target != null)
			{
				text = text + "target=" + this.target;
			}
			if (this.points >= 0f)
			{
				text = text + ", points=" + this.points;
			}
			if (this.generateFightersOnly)
			{
				text = text + ", generateFightersOnly=" + this.generateFightersOnly.ToString();
			}
			if (this.raidStrategy != null)
			{
				text = text + ", raidStrategy=" + this.raidStrategy.defName;
			}
			if (this.questScriptDef != null)
			{
				text = text + ", questScriptDef=" + this.questScriptDef;
			}
			return text;
		}

		// Token: 0x0400422B RID: 16939
		public IIncidentTarget target;

		// Token: 0x0400422C RID: 16940
		public float points = -1f;

		// Token: 0x0400422D RID: 16941
		public Faction faction;

		// Token: 0x0400422E RID: 16942
		public bool forced;

		// Token: 0x0400422F RID: 16943
		public string customLetterLabel;

		// Token: 0x04004230 RID: 16944
		public string customLetterText;

		// Token: 0x04004231 RID: 16945
		public LetterDef customLetterDef;

		// Token: 0x04004232 RID: 16946
		public List<ThingDef> letterHyperlinkThingDefs;

		// Token: 0x04004233 RID: 16947
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04004234 RID: 16948
		public string inSignalEnd;

		// Token: 0x04004235 RID: 16949
		public IntVec3 spawnCenter = IntVec3.Invalid;

		// Token: 0x04004236 RID: 16950
		public Rot4 spawnRotation = Rot4.South;

		// Token: 0x04004237 RID: 16951
		public bool generateFightersOnly;

		// Token: 0x04004238 RID: 16952
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x04004239 RID: 16953
		public RaidStrategyDef raidStrategy;

		// Token: 0x0400423A RID: 16954
		public PawnsArrivalModeDef raidArrivalMode;

		// Token: 0x0400423B RID: 16955
		public bool raidForceOneIncap;

		// Token: 0x0400423C RID: 16956
		public bool raidNeverFleeIndividual;

		// Token: 0x0400423D RID: 16957
		public bool raidArrivalModeForQuickMilitaryAid;

		// Token: 0x0400423E RID: 16958
		public float biocodeApparelChance;

		// Token: 0x0400423F RID: 16959
		public float biocodeWeaponsChance;

		// Token: 0x04004240 RID: 16960
		public Dictionary<Pawn, int> pawnGroups;

		// Token: 0x04004241 RID: 16961
		public int? pawnGroupMakerSeed;

		// Token: 0x04004242 RID: 16962
		public PawnKindDef pawnKind;

		// Token: 0x04004243 RID: 16963
		public int pawnCount;

		// Token: 0x04004244 RID: 16964
		public TraderKindDef traderKind;

		// Token: 0x04004245 RID: 16965
		public int podOpenDelay = 140;

		// Token: 0x04004246 RID: 16966
		public Quest quest;

		// Token: 0x04004247 RID: 16967
		public QuestScriptDef questScriptDef;

		// Token: 0x04004248 RID: 16968
		public string questTag;

		// Token: 0x04004249 RID: 16969
		public MechClusterSketch mechClusterSketch;

		// Token: 0x0400424A RID: 16970
		private List<Pawn> tmpPawns;

		// Token: 0x0400424B RID: 16971
		private List<int> tmpGroups;
	}
}
