using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEF RID: 3055
	public class IncidentParms : IExposable
	{
		// Token: 0x060047E7 RID: 18407 RVA: 0x0017BB10 File Offset: 0x00179D10
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
			if (Scribe.mode == LoadSaveMode.Saving && this.pawnGroups != null)
			{
				this.pawnGroups.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key == null || x.Key.Destroyed);
			}
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
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Values.Look<float>(ref this.biocodeWeaponsChance, "biocodeWeaponsChance", 0f, false);
			Scribe_Values.Look<float>(ref this.biocodeApparelChance, "biocodeApparelChance", 0f, false);
			Scribe_References.Look<Pawn>(ref this.controllerPawn, "controllerPawn", false);
			Scribe_References.Look<Ideo>(ref this.pawnIdeo, "pawnIdeo", false);
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Values.Look<int>(ref this.podOpenDelay, "podOpenDelay", 140, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScriptDef, "questScriptDef");
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<IntVec3?>(ref this.infestationLocOverride, "infestationLocOverride", null, false);
			Scribe_Values.Look<float>(ref this.totalBodySize, "totalBodySize", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.attackTargets, "attackTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.gifts, "gifts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.gifts;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.attackTargets != null)
			{
				this.attackTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0017BEA8 File Offset: 0x0017A0A8
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

		// Token: 0x04002C0E RID: 11278
		public IIncidentTarget target;

		// Token: 0x04002C0F RID: 11279
		public float points = -1f;

		// Token: 0x04002C10 RID: 11280
		public Faction faction;

		// Token: 0x04002C11 RID: 11281
		public bool forced;

		// Token: 0x04002C12 RID: 11282
		public string customLetterLabel;

		// Token: 0x04002C13 RID: 11283
		public string customLetterText;

		// Token: 0x04002C14 RID: 11284
		public LetterDef customLetterDef;

		// Token: 0x04002C15 RID: 11285
		public bool sendLetter = true;

		// Token: 0x04002C16 RID: 11286
		public List<ThingDef> letterHyperlinkThingDefs;

		// Token: 0x04002C17 RID: 11287
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04002C18 RID: 11288
		public string inSignalEnd;

		// Token: 0x04002C19 RID: 11289
		public IntVec3 spawnCenter = IntVec3.Invalid;

		// Token: 0x04002C1A RID: 11290
		public Rot4 spawnRotation = Rot4.South;

		// Token: 0x04002C1B RID: 11291
		public bool generateFightersOnly;

		// Token: 0x04002C1C RID: 11292
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x04002C1D RID: 11293
		public RaidStrategyDef raidStrategy;

		// Token: 0x04002C1E RID: 11294
		public PawnsArrivalModeDef raidArrivalMode;

		// Token: 0x04002C1F RID: 11295
		public bool raidForceOneIncap;

		// Token: 0x04002C20 RID: 11296
		public bool raidNeverFleeIndividual;

		// Token: 0x04002C21 RID: 11297
		public bool raidArrivalModeForQuickMilitaryAid;

		// Token: 0x04002C22 RID: 11298
		public float biocodeWeaponsChance;

		// Token: 0x04002C23 RID: 11299
		public float biocodeApparelChance;

		// Token: 0x04002C24 RID: 11300
		public Dictionary<Pawn, int> pawnGroups;

		// Token: 0x04002C25 RID: 11301
		public int? pawnGroupMakerSeed;

		// Token: 0x04002C26 RID: 11302
		public Ideo pawnIdeo;

		// Token: 0x04002C27 RID: 11303
		public PawnKindDef pawnKind;

		// Token: 0x04002C28 RID: 11304
		public int pawnCount;

		// Token: 0x04002C29 RID: 11305
		public TraderKindDef traderKind;

		// Token: 0x04002C2A RID: 11306
		public int podOpenDelay = 140;

		// Token: 0x04002C2B RID: 11307
		public Quest quest;

		// Token: 0x04002C2C RID: 11308
		public QuestScriptDef questScriptDef;

		// Token: 0x04002C2D RID: 11309
		public string questTag;

		// Token: 0x04002C2E RID: 11310
		public MechClusterSketch mechClusterSketch;

		// Token: 0x04002C2F RID: 11311
		public bool canTimeoutOrFlee = true;

		// Token: 0x04002C30 RID: 11312
		public Pawn controllerPawn;

		// Token: 0x04002C31 RID: 11313
		public IntVec3? infestationLocOverride;

		// Token: 0x04002C32 RID: 11314
		public List<Thing> attackTargets;

		// Token: 0x04002C33 RID: 11315
		public List<Thing> gifts;

		// Token: 0x04002C34 RID: 11316
		public float totalBodySize;

		// Token: 0x04002C35 RID: 11317
		public List<Pawn> storeGeneratedNeutralPawns;

		// Token: 0x04002C36 RID: 11318
		private List<Pawn> tmpPawns;

		// Token: 0x04002C37 RID: 11319
		private List<int> tmpGroups;
	}
}
