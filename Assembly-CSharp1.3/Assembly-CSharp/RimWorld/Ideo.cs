using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED0 RID: 3792
	public class Ideo : IExposable, ILoadReferenceable
	{
		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x0600598D RID: 22925 RVA: 0x001EA411 File Offset: 0x001E8611
		public List<Precept> PreceptsListForReading
		{
			get
			{
				return this.precepts;
			}
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x0600598E RID: 22926 RVA: 0x001EA419 File Offset: 0x001E8619
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get((this.iconDef != null) ? this.iconDef.iconPath : BaseContent.BadTexPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x0600598F RID: 22927 RVA: 0x001EA455 File Offset: 0x001E8655
		public List<Precept_Role> RolesListForReading
		{
			get
			{
				return this.cachedPossibleRoles;
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06005990 RID: 22928 RVA: 0x001EA45D File Offset: 0x001E865D
		public bool WarnPlayerOnDesignateChopTree
		{
			get
			{
				return this.warnPlayerOnDesignateChopTree;
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06005991 RID: 22929 RVA: 0x001EA465 File Offset: 0x001E8665
		public bool WarnPlayerOnDesignateMine
		{
			get
			{
				return this.warnPlayerOnDesignateMine;
			}
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06005992 RID: 22930 RVA: 0x001EA470 File Offset: 0x001E8670
		public Color Color
		{
			get
			{
				Color? color = this.primaryFactionColor;
				if (color != null)
				{
					return color.GetValueOrDefault();
				}
				if (this.colorDef == null)
				{
					return Color.white;
				}
				return this.colorDef.color;
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06005993 RID: 22931 RVA: 0x001EA4B0 File Offset: 0x001E86B0
		public Color ApparelColor
		{
			get
			{
				float h;
				float b;
				float b2;
				Color.RGBToHSV(this.Color, out h, out b, out b2);
				return Color.HSVToRGB(h, Mathf.Min(0.5f, b), Mathf.Min(0.8235294f, b2));
			}
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06005994 RID: 22932 RVA: 0x001EA4EC File Offset: 0x001E86EC
		public List<ThingDef> VeneratedAnimals
		{
			get
			{
				if (this.cachedVeneratedAnimals == null)
				{
					this.cachedVeneratedAnimals = new List<ThingDef>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Animal precept_Animal;
						if ((precept_Animal = (this.precepts[i] as Precept_Animal)) != null)
						{
							this.cachedVeneratedAnimals.Add(precept_Animal.ThingDef);
						}
					}
				}
				return this.cachedVeneratedAnimals;
			}
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06005995 RID: 22933 RVA: 0x001EA550 File Offset: 0x001E8750
		public IntRange DeityCountRange
		{
			get
			{
				int num = 0;
				int num2 = int.MaxValue;
				for (int i = 0; i < this.memes.Count; i++)
				{
					if (this.memes[i].deityCount.min >= 0)
					{
						num = Mathf.Max(num, this.memes[i].deityCount.min);
					}
					if (this.memes[i].deityCount.max >= 0)
					{
						num2 = Mathf.Min(num2, this.memes[i].deityCount.max);
					}
				}
				return new IntRange(num, num2);
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06005996 RID: 22934 RVA: 0x001EA5F0 File Offset: 0x001E87F0
		public MemeDef StructureMeme
		{
			get
			{
				for (int i = 0; i < this.memes.Count; i++)
				{
					if (this.memes[i].category == MemeCategory.Structure)
					{
						return this.memes[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06005997 RID: 22935 RVA: 0x001EA635 File Offset: 0x001E8835
		public Gender SupremeGender
		{
			get
			{
				if (this.HasMeme(MemeDefOf.MaleSupremacy))
				{
					return Gender.Male;
				}
				if (this.HasMeme(MemeDefOf.FemaleSupremacy))
				{
					return Gender.Female;
				}
				return Gender.None;
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06005998 RID: 22936 RVA: 0x001EA658 File Offset: 0x001E8858
		public int RequiredScars
		{
			get
			{
				if (this.precepts.NullOrEmpty<Precept>())
				{
					return 0;
				}
				if (this.requiredScarsCached == -1)
				{
					this.requiredScarsCached = 0;
					foreach (Precept precept in this.precepts)
					{
						if (precept.def.requiredScars > this.requiredScarsCached)
						{
							this.requiredScarsCached = precept.def.requiredScars;
						}
					}
				}
				return this.requiredScarsCached;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06005999 RID: 22937 RVA: 0x001EA6F0 File Offset: 0x001E88F0
		public float BlindPawnChance
		{
			get
			{
				if (this.precepts.NullOrEmpty<Precept>())
				{
					return 0f;
				}
				float num = 0f;
				int num2 = 0;
				foreach (Precept precept in this.precepts)
				{
					if (precept.def.blindPawnChance >= 0f)
					{
						num += precept.def.blindPawnChance;
						num2++;
					}
				}
				if (num2 <= 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x0600599A RID: 22938 RVA: 0x001EA78C File Offset: 0x001E898C
		public string KeyDeityName
		{
			get
			{
				IdeoFoundation_Deity ideoFoundation_Deity;
				if ((ideoFoundation_Deity = (this.foundation as IdeoFoundation_Deity)) != null && ideoFoundation_Deity.DeitiesListForReading.Any<IdeoFoundation_Deity.Deity>())
				{
					return ideoFoundation_Deity.DeitiesListForReading[0].name;
				}
				return null;
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x0600599B RID: 22939 RVA: 0x001EA7C8 File Offset: 0x001E89C8
		public bool ObligationsActive
		{
			get
			{
				if (this.colonistBelieverCountCached == -1)
				{
					this.RecacheColonistBelieverCount();
				}
				return this.colonistBelieverCountCached >= Ideo.MinBelieversToEnableObligations || Faction.OfPlayer.ideos.IsPrimary(this);
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x0600599C RID: 22940 RVA: 0x001EA7FC File Offset: 0x001E89FC
		public SoundDef SoundOngoingRitual
		{
			get
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.category.soundOngoingRitual != null)
					{
						return thingStyleCategoryWithPriority.category.soundOngoingRitual;
					}
				}
				return SoundDefOf.RitualSustainer_Theist;
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x0600599D RID: 22941 RVA: 0x001EA86C File Offset: 0x001E8A6C
		public RitualVisualEffectDef RitualEffect
		{
			get
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.category.ritualVisualEffectDef != null)
					{
						return thingStyleCategoryWithPriority.category.ritualVisualEffectDef;
					}
				}
				return RitualVisualEffectDefOf.Basic;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x0600599E RID: 22942 RVA: 0x001EA8DC File Offset: 0x001E8ADC
		public ThingDef RitualSeatDef
		{
			get
			{
				using (List<Precept>.Enumerator enumerator = this.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_RitualSeat precept_RitualSeat;
						if ((precept_RitualSeat = (enumerator.Current as Precept_RitualSeat)) != null)
						{
							return precept_RitualSeat.ThingDef;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x0600599F RID: 22943 RVA: 0x001EA93C File Offset: 0x001E8B3C
		// (set) Token: 0x060059A0 RID: 22944 RVA: 0x001EA978 File Offset: 0x001E8B78
		public string WorshipRoomLabel
		{
			get
			{
				if (this.overriddenWorshipRoomLabel != null)
				{
					return this.overriddenWorshipRoomLabel;
				}
				MemeDef structureMeme = this.StructureMeme;
				if (structureMeme != null)
				{
					return structureMeme.worshipRoomLabel;
				}
				return RoomRoleDefOf.WorshipRoom.label.CapitalizeFirst();
			}
			set
			{
				this.overriddenWorshipRoomLabel = value;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060059A1 RID: 22945 RVA: 0x001EA981 File Offset: 0x001E8B81
		public string MemberNamePlural
		{
			get
			{
				if (this.memberNamePluralCached.NullOrEmpty())
				{
					this.memberNamePluralCached = Find.ActiveLanguageWorker.Pluralize(this.memberName, -1);
				}
				return this.memberNamePluralCached;
			}
		}

		// Token: 0x060059A2 RID: 22946 RVA: 0x001EA9B0 File Offset: 0x001E8BB0
		public bool IsWorkTypeConsideredDangerous(WorkTypeDef workType)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.opposedWorkTypes.Count; j++)
				{
					if (this.precepts[i].def.opposedWorkTypes[j] == workType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x001EAA1B File Offset: 0x001E8C1B
		public void SortStyleCategories()
		{
			this.thingStyleCategories.SortBy((ThingStyleCategoryWithPriority x) => -x.priority);
			this.RecachePossibleBuildables();
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x001EAA50 File Offset: 0x001E8C50
		public Ideo()
		{
			this.style = new IdeoStyleTracker(this);
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x001EAAF8 File Offset: 0x001E8CF8
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.overriddenWorshipRoomLabel, "overriddenWorshipRoomLabel", null, false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<bool>(ref this.createdFromNoExpansionGame, "createdFromNoExpansionGame", false, false);
			Scribe_Deep.Look<IdeoFoundation>(ref this.foundation, "foundation", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.adjective, "adjective", null, false);
			Scribe_Values.Look<string>(ref this.memberName, "memberName", null, false);
			Scribe_Values.Look<string>(ref this.leaderTitleMale, "leaderTitleMale", null, false);
			Scribe_Values.Look<string>(ref this.leaderTitleFemale, "leaderTitleFemale", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<string>(ref this.descriptionTemplate, "descriptionTemplate", null, false);
			Scribe_Values.Look<bool>(ref this.descriptionLocked, "descriptionLocked", false, false);
			Scribe_Defs.Look<CultureDef>(ref this.culture, "culture");
			Scribe_Defs.Look<IdeoIconDef>(ref this.iconDef, "iconDef");
			Scribe_Defs.Look<ColorDef>(ref this.colorDef, "colorDef");
			Scribe_Values.Look<Color?>(ref this.primaryFactionColor, "primaryFactionColor", null, false);
			Scribe_Collections.Look<MemeDef>(ref this.memes, "memes", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<Precept>(ref this.precepts, "precepts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<ThingStyleCategoryWithPriority>(ref this.thingStyleCategories, "thingStyleCategories", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.usedSymbols, "usedSymbols", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.usedSymbolPacks, "usedSymbolPacks", LookMode.Value, Array.Empty<object>());
			Scribe_Deep.Look<IdeoStyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.relicsCollected, "relicsCollected", false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.foundation != null)
				{
					this.foundation.ideo = this;
				}
				if (this.style == null)
				{
					this.style = new IdeoStyleTracker(this);
				}
				if (this.memes.RemoveAll((MemeDef x) => x == null) != 0)
				{
					Log.Error("Some ideoligion memes were null after loading.");
				}
				if (this.precepts.RemoveAll((Precept x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some ideoligion precepts were null after loading.");
				}
				if (this.thingStyleCategories == null)
				{
					this.thingStyleCategories = new List<ThingStyleCategoryWithPriority>();
				}
				if (this.thingStyleCategories.RemoveAll((ThingStyleCategoryWithPriority x) => x == null || x.category == null) != 0)
				{
					Log.Error("Some thing style categories were null after loading.");
				}
				if (this.culture == null)
				{
					this.culture = DefDatabase<CultureDef>.AllDefsListForReading.RandomElement<CultureDef>();
					Log.Error("Ideoligion had null culture. Assigning random.");
				}
				using (List<PreceptDef>.Enumerator enumerator = DefDatabase<PreceptDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PreceptDef p = enumerator.Current;
						if ((p.takeNameFrom == null || this.precepts.Any((Precept _p) => _p.def == p.takeNameFrom)) && !(p.preceptClass != typeof(Precept_Ritual)) && !p.visible && !this.precepts.Any((Precept o) => o.def.ritualPatternBase == p.ritualPatternBase))
						{
							RitualPatternDef ritualPatternBase = p.ritualPatternBase;
							Precept precept = PreceptMaker.MakePrecept(p);
							this.AddPrecept(precept, true, null, ritualPatternBase);
							Debug.LogWarning("A hidden ritual precept was missing, adding: " + precept.def.LabelCap);
						}
					}
				}
				if (this.usedSymbols == null)
				{
					this.usedSymbols = new List<string>();
				}
				if (this.usedSymbolPacks == null)
				{
					this.usedSymbolPacks = new List<string>();
				}
				for (int i = 0; i < this.precepts.Count; i++)
				{
					if (this.precepts[i] != null)
					{
						this.precepts[i].ideo = this;
					}
				}
				if (this.description == null)
				{
					this.RegenerateDescription(false);
				}
				if (this.createdFromNoExpansionGame && ModsConfig.IdeologyActive && this.iconDef == null)
				{
					this.SetIcon(IdeoFoundation.GetRandomIconDef(this), IdeoFoundation.GetRandomColorDef(this));
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecachePrecepts();
			}
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x001EAF74 File Offset: 0x001E9174
		public bool HasMeme(MemeDef meme)
		{
			return this.memes.Contains(meme);
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x001EAF84 File Offset: 0x001E9184
		public void IdeoTick()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				this.precepts[i].Tick();
			}
			if (this.colonistBelieverCountCached == -1)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x001EAFC8 File Offset: 0x001E91C8
		public void Notify_AddBedThoughts(Pawn pawn)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_AddBedThoughts(pawn, this.precepts[i]);
				}
			}
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x001EB040 File Offset: 0x001E9240
		public void Notify_MemberTookAction(HistoryEvent ev, bool canApplySelfTookThoughts)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_MemberTookAction(ev, this.precepts[i], canApplySelfTookThoughts);
				}
			}
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x001EB0B8 File Offset: 0x001E92B8
		public void Notify_MemberKnows(HistoryEvent ev, Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_MemberWitnessedAction(ev, this.precepts[i], member);
				}
			}
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x001EB130 File Offset: 0x001E9330
		public void Notify_MemberDied(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberDied(member);
				}
				catch (Exception arg)
				{
					Log.Error("Error in Precept.Notify_MemberDied(): " + arg);
				}
			}
			if (member.Faction == Faction.OfPlayer)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x001EB1A0 File Offset: 0x001E93A0
		public void Notify_MemberLost(Pawn member, Map map)
		{
			if (member.IsColonist && map != null && member.workSettings != null && member.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
			{
				bool flag = false;
				foreach (Pawn pawn in map.mapPawns.FreeColonists)
				{
					if (pawn != member && pawn.Ideo == this && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					foreach (Pawn pawn2 in map.mapPawns.PrisonersOfColonySpawned)
					{
						pawn2.guest.Notify_WardensOfIdeoLost(this);
					}
				}
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberLost(member);
				}
				catch (Exception arg)
				{
					Log.Error("Error in Precept.Notify_MemberLost(): " + arg);
				}
			}
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x001EB2E0 File Offset: 0x001E94E0
		public void Notify_MemberCorpseDestroyed(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberCorpseDestroyed(member);
				}
				catch (Exception arg)
				{
					Log.Error("Error in Precept.Notify_MemberCorpseDestroyed(): " + arg);
				}
			}
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x001EB33C File Offset: 0x001E953C
		public void Notify_MemberSpawned(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberSpawned(member);
				}
				catch (Exception arg)
				{
					Log.Error("Error in Precept.Notify_MemberSpawned(): " + arg);
				}
			}
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x001EB398 File Offset: 0x001E9598
		public void Notify_MemberGenerated(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGenerated(member);
				}
				catch (Exception arg)
				{
					Log.Error("Error in Precept.Notify_MemberGenerated(): " + arg);
				}
			}
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x001EB3F4 File Offset: 0x001E95F4
		public void Notify_GameStarted()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_GameStarted();
				}
				catch (Exception arg)
				{
					Log.Error("Error in Precept.Notify_GameStarted(): " + arg);
				}
			}
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x001EB450 File Offset: 0x001E9650
		public void Notify_RelicSeenByPlayer(Thing relic)
		{
			if (this.AllRelicsNewlyCollected())
			{
				Ideo.tmpPlayerIdeoFollowers.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (pawn.Ideo == this && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && pawn.needs.mood.thoughts.memories != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RelicsCollected, null, null);
						if (pawn.Faction == Faction.OfPlayer)
						{
							Ideo.tmpPlayerIdeoFollowers.Add(pawn);
						}
					}
				}
				if (Ideo.tmpPlayerIdeoFollowers.Count > 0)
				{
					ChoiceLetter let = LetterMaker.MakeLetter("LetterLabelRelicsCollected".Translate() + ": " + this.name.ApplyTag(this).Resolve(), "LetterTextRelicsCollected".Translate(this) + ":\n\n" + (from p in Ideo.tmpPlayerIdeoFollowers
					select p.LabelNoCountColored.Resolve()).ToList<string>().ToLineList("- "), LetterDefOf.PositiveEvent, Ideo.tmpPlayerIdeoFollowers, null, null, null);
					Find.LetterStack.ReceiveLetter(let, null);
					Ideo.tmpPlayerIdeoFollowers.Clear();
				}
				this.relicsCollected = true;
			}
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x001EB600 File Offset: 0x001E9800
		private bool AllRelicsNewlyCollected()
		{
			if (this.relicsCollected)
			{
				return false;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Relic precept_Relic;
				if ((precept_Relic = (this.precepts[i] as Precept_Relic)) != null && !precept_Relic.RelicInPlayerPossession)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x001EB650 File Offset: 0x001E9850
		public bool MemberWillingToDo(HistoryEvent ev)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					if (!this.precepts[i].def.comps[j].MemberWillingToDo(ev))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x001EB6C0 File Offset: 0x001E98C0
		public Pair<Precept, Precept> FirstIncompatiblePreceptPair()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts.Count; j++)
				{
					if (this.precepts[i] != this.precepts[j] && !this.precepts[i].CompatibleWith(this.precepts[j]))
					{
						return new Pair<Precept, Precept>(this.precepts[i], this.precepts[j]);
					}
				}
			}
			return default(Pair<Precept, Precept>);
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x001EB75C File Offset: 0x001E995C
		public Tuple<Precept_Ritual, List<string>> FirstRitualMissingTarget()
		{
			using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Ritual precept_Ritual;
					if ((precept_Ritual = (enumerator.Current as Precept_Ritual)) != null && precept_Ritual.obligationTargetFilter != null)
					{
						List<string> list = precept_Ritual.obligationTargetFilter.MissingTargetBuilding(this);
						if (!list.NullOrEmpty<string>())
						{
							return new Tuple<Precept_Ritual, List<string>>(precept_Ritual, list);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x001EB7DC File Offset: 0x001E99DC
		public Precept_Building FirstConsumableBuildingMissingRitual()
		{
			using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Building precept_Building;
					if ((precept_Building = (enumerator.Current as Precept_Building)) != null && precept_Building.ThingDef.ritualFocus != null && precept_Building.ThingDef.ritualFocus.consumable && !this.HasRequiredRitualForBuilding(precept_Building))
					{
						return precept_Building;
					}
				}
			}
			return null;
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x001EB860 File Offset: 0x001E9A60
		private bool HasRequiredRitualForBuilding(Precept_Building building)
		{
			if (this.cachedRitualTargetFilters == null)
			{
				this.cachedRitualTargetFilters = new List<RitualObligationTargetFilter>();
				using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_Ritual precept_Ritual;
						if ((precept_Ritual = (enumerator.Current as Precept_Ritual)) != null && precept_Ritual.obligationTargetFilter != null)
						{
							this.cachedRitualTargetFilters.Add(precept_Ritual.obligationTargetFilter.def.GetInstance());
						}
					}
				}
			}
			foreach (RitualObligationTargetFilter ritualObligationTargetFilter in this.cachedRitualTargetFilters)
			{
				if (!ritualObligationTargetFilter.def.thingDefs.NullOrEmpty<ThingDef>() && ritualObligationTargetFilter.def.thingDefs.Contains(building.ThingDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x001EB958 File Offset: 0x001E9B58
		public void ClearPrecepts()
		{
			this.precepts.Clear();
			this.RecachePrecepts();
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x001EB96C File Offset: 0x001E9B6C
		public void RecachePrecepts()
		{
			this.allPreceptDefs.Clear();
			foreach (Precept precept in this.precepts)
			{
				this.allPreceptDefs.Add(precept.def);
			}
			this.requiredScarsCached = -1;
			this.cachedVeneratedAnimals = null;
			this.cachedRitualTargetFilters = null;
			this.RecachePossibleSituationalThoughts();
			this.RecachePossibleGoodwillSituations();
			this.RecachePossibleBuildings();
			this.RecachePossibleBuildables();
			this.RecachePossibleRoles();
			this.warnPlayerOnDesignateChopTree = false;
			this.warnPlayerOnDesignateMine = false;
			foreach (Precept precept2 in this.precepts)
			{
				if (precept2.def.warnPlayerOnDesignateChopTree)
				{
					this.warnPlayerOnDesignateChopTree = true;
				}
				if (precept2.def.warnPlayerOnDesignateMine)
				{
					this.warnPlayerOnDesignateMine = true;
				}
			}
			foreach (Precept precept3 in this.precepts)
			{
				precept3.Notify_RecachedPrecepts();
			}
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x001EBAB8 File Offset: 0x001E9CB8
		public void AddPrecept(Precept precept, bool init = false, FactionDef generatingFor = null, RitualPatternDef fillWith = null)
		{
			if (precept == null)
			{
				Log.Error("Tried to add a null PreceptDef.");
				return;
			}
			if (this.precepts.Contains(precept))
			{
				Log.Error("Tried to add the same PreceptDef twice.");
				return;
			}
			if ((precept.def.modContentPack == null || !precept.def.modContentPack.IsOfficialMod) && !ModLister.CheckIdeology("Precept (non-classic): " + precept.def.defName))
			{
				return;
			}
			precept.ideo = this;
			this.precepts.Add(precept);
			this.precepts.SortBy((Precept x) => -this.GetPreceptImpact(x), (Precept x) => -x.def.displayOrderInImpact, (Precept x) => x.def.defName);
			if (init)
			{
				precept.Init(this, generatingFor);
				Precept_Ritual precept_Ritual;
				if (fillWith != null && (precept_Ritual = (precept as Precept_Ritual)) != null)
				{
					fillWith.Fill(precept_Ritual);
					precept_Ritual.RegenerateName();
				}
			}
			this.RecachePrecepts();
			if (precept.def.alsoAdds != null)
			{
				Precept precept2 = this.precepts.FirstOrDefault((Precept p) => p.def == precept.def.alsoAdds);
				if (precept2 != null)
				{
					precept2.RegenerateName();
					return;
				}
				Precept precept3 = PreceptMaker.MakePrecept(precept.def.alsoAdds);
				this.AddPrecept(precept3, true, generatingFor, (fillWith != null) ? precept3.def.ritualPatternBase : null);
			}
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x001EBC63 File Offset: 0x001E9E63
		private float GetPreceptImpact(Precept precept)
		{
			if (!precept.SortByImpact)
			{
				return 0f;
			}
			return (float)precept.def.impact + 1f;
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x001EBC85 File Offset: 0x001E9E85
		public bool PreceptIsRequired(PreceptDef precept)
		{
			return this.GetMemeThatRequiresPrecept(precept) != null;
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x001EBC94 File Offset: 0x001E9E94
		public MemeDef GetMemeThatRequiresPrecept(PreceptDef precept)
		{
			Predicate<List<PreceptDef>> <>9__0;
			for (int i = 0; i < this.memes.Count; i++)
			{
				if (this.memes[i].requireOne != null)
				{
					List<List<PreceptDef>> requireOne = this.memes[i].requireOne;
					Predicate<List<PreceptDef>> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((List<PreceptDef> x) => x.Contains(precept)));
					}
					if (requireOne.Any(predicate))
					{
						return this.memes[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x001EBD1C File Offset: 0x001E9F1C
		public AcceptanceReport CanAddPreceptAllFactions(PreceptDef preceptDef)
		{
			AcceptanceReport result = this.foundation.CanAdd(preceptDef, false);
			if (!result.Accepted)
			{
				return result;
			}
			if (Find.World != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (faction.def.humanlikeFaction && faction.ideos != null && (faction.ideos.IsPrimary(this) || faction.ideos.IsMinor(this)) && !this.foundation.CanAddForFaction(preceptDef, faction.def, null, false, false, false))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x001EBDE0 File Offset: 0x001E9FE0
		public void RemovePrecept(Precept precept, bool replacing = false)
		{
			this.precepts.Remove(precept);
			for (int i = this.precepts.Count - 1; i >= 0; i--)
			{
				if (this.precepts[i].def.takeNameFrom == precept.def)
				{
					this.RemovePrecept(this.precepts[i], false);
				}
			}
			if (precept is Precept_Apparel)
			{
				this.RegenerateAllApparelRequirements(null);
			}
			if (!replacing && precept.def.defaultSelectionWeight <= 0f && precept.def.issue.HasDefaultPrecept)
			{
				PreceptDef def;
				if ((from x in DefDatabase<PreceptDef>.AllDefs
				where x.issue == precept.def.issue && this.CanAddPreceptAllFactions(x).Accepted && x.defaultSelectionWeight > 0f
				select x).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out def))
				{
					this.AddPrecept(PreceptMaker.MakePrecept(def), true, null, null);
					return;
				}
			}
			this.RecachePrecepts();
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x001EBEFA File Offset: 0x001EA0FA
		public void SetIcon(IdeoIconDef iconDef, ColorDef colorDef)
		{
			this.iconDef = iconDef;
			this.colorDef = colorDef;
			this.icon = null;
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x001EBF14 File Offset: 0x001EA114
		public Precept GetFirstPreceptAllowingSituationalThought(ThoughtDef def)
		{
			if (!this.cachedPossibleSituationalThoughts.Contains(def))
			{
				return null;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				List<PreceptComp> comps = this.precepts[i].def.comps;
				for (int j = 0; j < comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought;
					if ((preceptComp_SituationalThought = (comps[j] as PreceptComp_SituationalThought)) != null && preceptComp_SituationalThought.thought == def)
					{
						return this.precepts[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x001EBF98 File Offset: 0x001EA198
		public bool HasPreceptForBuilding(ThingDef buildingDef)
		{
			return this.cachedPossibleBuildings.Any((Precept_Building b) => b.ThingDef == buildingDef);
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x001EBFCC File Offset: 0x001EA1CC
		public bool MembersCanBuild(Thing thing)
		{
			BuildableDef buildableDef = thing.def.entityDefToBuild ?? thing.def;
			if (buildableDef.canGenerateDefaultDesignator)
			{
				return true;
			}
			Precept_ThingStyle styleSourcePrecept = thing.StyleSourcePrecept;
			if (((styleSourcePrecept != null) ? styleSourcePrecept.ideo : null) == this || this.cachedPossibleBuildables.Contains(buildableDef))
			{
				return true;
			}
			ThingDef buildingDef;
			if ((buildingDef = (buildableDef as ThingDef)) != null)
			{
				if (this.HasPreceptForBuilding(buildingDef))
				{
					return true;
				}
				for (int i = 0; i < this.precepts.Count; i++)
				{
					Precept_RitualSeat precept_RitualSeat;
					if ((precept_RitualSeat = (this.precepts[i] as Precept_RitualSeat)) != null && precept_RitualSeat.ThingDef == buildableDef)
					{
						return true;
					}
				}
			}
			for (int j = 0; j < this.precepts.Count; j++)
			{
				if (this.precepts[j].def.willingToConstructOtherIdeoBuildables)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x001EC09F File Offset: 0x001EA29F
		public bool IsVeneratedAnimal(Pawn pawn)
		{
			return this.IsVeneratedAnimal(pawn.def);
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x001EC0B0 File Offset: 0x001EA2B0
		public bool IsVeneratedAnimal(ThingDef thingDef)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			if (thingDef == null || thingDef.thingClass != typeof(Pawn) || !thingDef.race.Animal)
			{
				return false;
			}
			using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Animal precept_Animal;
					if ((precept_Animal = (enumerator.Current as Precept_Animal)) != null && thingDef == precept_Animal.ThingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x001EC148 File Offset: 0x001EA348
		public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
		{
			if (!td.IsWeapon || td.weaponClasses.NullOrEmpty<WeaponClassDef>())
			{
				return IdeoWeaponDisposition.None;
			}
			using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Weapon precept_Weapon;
					if ((precept_Weapon = (enumerator.Current as Precept_Weapon)) != null)
					{
						IdeoWeaponDisposition dispositionForWeapon = precept_Weapon.GetDispositionForWeapon(td);
						if (dispositionForWeapon != IdeoWeaponDisposition.None)
						{
							return dispositionForWeapon;
						}
					}
				}
			}
			return IdeoWeaponDisposition.None;
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x001EC1C8 File Offset: 0x001EA3C8
		public T GetFirstPreceptOfType<T>() where T : class
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				T result;
				if ((result = (this.precepts[i] as T)) != null)
				{
					return result;
				}
			}
			return default(T);
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x001EC215 File Offset: 0x001EA415
		public IEnumerable<T> GetAllPreceptsOfType<T>() where T : class
		{
			int num;
			for (int i = 0; i < this.precepts.Count; i = num + 1)
			{
				T t;
				if ((t = (this.precepts[i] as T)) != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x001EC225 File Offset: 0x001EA425
		public bool HasPrecept(PreceptDef preceptDef)
		{
			return this.allPreceptDefs.Contains(preceptDef);
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x001EC234 File Offset: 0x001EA434
		public int GetPreceptCountOfDef(PreceptDef preceptDef)
		{
			if (preceptDef == null || !this.allPreceptDefs.Contains(preceptDef))
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def == preceptDef)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x001EC288 File Offset: 0x001EA488
		public Precept GetPrecept(PreceptDef preceptDef)
		{
			if (preceptDef == null || !this.allPreceptDefs.Contains(preceptDef))
			{
				return null;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def == preceptDef)
				{
					return this.precepts[i];
				}
			}
			return null;
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x001EC2E0 File Offset: 0x001EA4E0
		public void SortMemesInDisplayOrder()
		{
			this.memes.SortBy((MemeDef x) => x.category == MemeCategory.Normal, (MemeDef x) => x.index);
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x001EC338 File Offset: 0x001EA538
		private void RecachePossibleSituationalThoughts()
		{
			this.cachedPossibleSituationalThoughts.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought;
					if ((preceptComp_SituationalThought = (this.precepts[i].def.comps[j] as PreceptComp_SituationalThought)) != null && !this.cachedPossibleSituationalThoughts.Contains(preceptComp_SituationalThought.thought))
					{
						this.cachedPossibleSituationalThoughts.Add(preceptComp_SituationalThought.thought);
					}
				}
			}
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x001EC3D8 File Offset: 0x001EA5D8
		private void RecachePossibleGoodwillSituations()
		{
			this.cachedPossibleGoodwillSituations.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_GoodwillSituation preceptComp_GoodwillSituation;
					if ((preceptComp_GoodwillSituation = (this.precepts[i].def.comps[j] as PreceptComp_GoodwillSituation)) != null && !this.cachedPossibleGoodwillSituations.Contains(preceptComp_GoodwillSituation.goodwillSituation))
					{
						this.cachedPossibleGoodwillSituations.Add(preceptComp_GoodwillSituation.goodwillSituation);
					}
				}
			}
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x001EC478 File Offset: 0x001EA678
		public void RecachePossibleBuildings()
		{
			this.cachedPossibleBuildings.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Building item;
				if ((item = (this.precepts[i] as Precept_Building)) != null)
				{
					this.cachedPossibleBuildings.Add(item);
				}
			}
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x001EC4C8 File Offset: 0x001EA6C8
		public void RecachePossibleBuildables()
		{
			this.cachedPossibleBuildables.Clear();
			foreach (MemeDef memeDef in this.memes)
			{
				this.cachedPossibleBuildables.AddRange(memeDef.AllDesignatorBuildables);
			}
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
			{
				this.cachedPossibleBuildables.AddRange(thingStyleCategoryWithPriority.category.AllDesignatorBuildables);
			}
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x001EC584 File Offset: 0x001EA784
		public void RecachePossibleRoles()
		{
			this.cachedPossibleRoles.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Role item;
				if ((item = (this.precepts[i] as Precept_Role)) != null)
				{
					this.cachedPossibleRoles.Add(item);
				}
			}
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x001EC5D4 File Offset: 0x001EA7D4
		public void RegenerateAllApparelRequirements(FactionDef generatingFor = null)
		{
			using (List<Precept>.Enumerator enumerator = this.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Role precept_Role;
					if ((precept_Role = (enumerator.Current as Precept_Role)) != null)
					{
						precept_Role.RegenerateApparelRequirements(generatingFor);
					}
				}
			}
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x001EC630 File Offset: 0x001EA830
		public void RegenerateAllPreceptNames()
		{
			foreach (Precept precept in this.precepts)
			{
				if (precept.UsesGeneratedName)
				{
					precept.RegenerateName();
				}
				precept.ClearTipCache();
			}
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x001EC690 File Offset: 0x001EA890
		public void RegenerateDescription(bool force = false)
		{
			if (this.description == null || !this.descriptionLocked)
			{
				IdeoDescriptionResult newDescription = this.GetNewDescription(force);
				this.description = newDescription.text;
				this.descriptionTemplate = newDescription.template;
			}
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x001EC6D0 File Offset: 0x001EA8D0
		public IdeoDescriptionResult GetNewDescription(bool force = false)
		{
			List<IdeoDescriptionMaker.PatternEntry> list = (from entry in this.memes.Where(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				return ((descriptionMaker != null) ? descriptionMaker.patterns : null) != null;
			}).SelectMany((MemeDef meme) => meme.descriptionMaker.patterns)
			group entry by entry.def into grp
			select grp.MaxBy((IdeoDescriptionMaker.PatternEntry entry) => entry.weight)).ToList<IdeoDescriptionMaker.PatternEntry>();
			if (!list.Any<IdeoDescriptionMaker.PatternEntry>())
			{
				if (ModsConfig.IdeologyActive && this.memes.Any<MemeDef>())
				{
					Log.Error("Memes provided no description patterns");
				}
				return new IdeoDescriptionResult
				{
					text = string.Empty
				};
			}
			IdeoStoryPatternDef def = list.RandomElementByWeight((IdeoDescriptionMaker.PatternEntry entry) => entry.weight).def;
			return IdeoDescriptionUtility.ResolveDescription(this, def, force);
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x060059D6 RID: 22998 RVA: 0x001EC7E3 File Offset: 0x001EA9E3
		public int ColonistBelieverCountCached
		{
			get
			{
				if (this.colonistBelieverCountCached == -1)
				{
					return this.RecacheColonistBelieverCount();
				}
				return this.colonistBelieverCountCached;
			}
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x001EC7FC File Offset: 0x001EA9FC
		public int RecacheColonistBelieverCount()
		{
			if (Current.ProgramState != ProgramState.Playing || Find.WindowStack.IsOpen<Dialog_ConfigureIdeo>())
			{
				return 0;
			}
			int num = this.colonistBelieverCountCached;
			int num2 = 0;
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
			{
				if (pawn.Ideo == this && !pawn.IsSlave)
				{
					num2++;
				}
			}
			this.colonistBelieverCountCached = num2;
			using (List<Precept>.Enumerator enumerator2 = this.precepts.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Precept_Role precept_Role;
					if ((precept_Role = (enumerator2.Current as Precept_Role)) != null)
					{
						precept_Role.RecacheActivity();
					}
				}
			}
			if (Faction.OfPlayer.ideos.IsMinor(this))
			{
				if (this.colonistBelieverCountCached < Ideo.MinBelieversToEnableObligations && num != -1 && num >= Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterObligationsInactive".Translate(this.name), "LetterObligationsInactiveTooFewBelievers".Translate(this.Named("IDEO"), Ideo.MinBelieversToEnableObligations), LetterDefOf.NeutralEvent, null);
				}
				if (this.colonistBelieverCountCached >= Ideo.MinBelieversToEnableObligations && num != -1 && num < Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterObligationsActive".Translate(this.name), "LetterObligationsActiveEnoughBelievers".Translate(this.Named("IDEO"), Ideo.MinBelieversToEnableObligations), LetterDefOf.NeutralEvent, null);
				}
			}
			return num2;
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x001EC9A0 File Offset: 0x001EABA0
		public Precept_Role GetRole(Pawn p)
		{
			foreach (Precept_Role precept_Role in this.RolesListForReading)
			{
				if (precept_Role.IsAssigned(p))
				{
					return precept_Role;
				}
			}
			return null;
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x001EC9FC File Offset: 0x001EABFC
		public StyleCategoryPair GetStyleAndCategoryFor(ThingDef thingDef)
		{
			Precept precept = null;
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_ThingDef precept_ThingDef;
				if ((precept_ThingDef = (this.precepts[i] as Precept_ThingDef)) != null && precept_ThingDef.ThingDef == thingDef)
				{
					precept = precept_ThingDef;
					break;
				}
			}
			return this.style.StyleForThingDef(thingDef, precept);
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x001ECA50 File Offset: 0x001EAC50
		public ThingStyleDef GetStyleFor(ThingDef thingDef)
		{
			StyleCategoryPair styleAndCategoryFor = this.GetStyleAndCategoryFor(thingDef);
			if (styleAndCategoryFor == null)
			{
				return null;
			}
			return styleAndCategoryFor.styleDef;
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x001ECA64 File Offset: 0x001EAC64
		public StyleCategoryDef GetStyleCategoryFor(ThingDef thingDef)
		{
			StyleCategoryPair styleCategoryPair = this.style.StyleForThingDef(thingDef, null);
			if (styleCategoryPair == null)
			{
				return null;
			}
			return styleCategoryPair.category;
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x001ECA80 File Offset: 0x001EAC80
		public void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			foreach (Precept precept in this.PreceptsListForReading)
			{
				precept.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			}
			if (oldFaction == Faction.OfPlayer || newFaction == Faction.OfPlayer)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x001ECAEC File Offset: 0x001EACEC
		public void Notify_NotPrimaryAnymore(Ideo newIdeo)
		{
			foreach (Precept precept in this.precepts)
			{
				precept.Notify_IdeoNotPrimaryAnymore(newIdeo);
			}
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x001ECB40 File Offset: 0x001EAD40
		public bool HasMaxPreceptsForIssue(IssueDef issue)
		{
			if (!issue.allowMultiplePrecepts)
			{
				for (int i = 0; i < this.precepts.Count; i++)
				{
					if (this.precepts[i].def.issue == issue)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x001ECB87 File Offset: 0x001EAD87
		public void MakeMemeberNamePluralDirty()
		{
			this.memberNamePluralCached = null;
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x001ECB90 File Offset: 0x001EAD90
		public string GetUniqueLoadID()
		{
			return "Ideo_" + this.id;
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x001ECBA7 File Offset: 0x001EADA7
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			return this.GetUniqueLoadID();
		}

		// Token: 0x04003480 RID: 13440
		public static int MinBelieversToEnableObligations = 3;

		// Token: 0x04003481 RID: 13441
		public int id = -1;

		// Token: 0x04003482 RID: 13442
		public IdeoFoundation foundation;

		// Token: 0x04003483 RID: 13443
		public CultureDef culture;

		// Token: 0x04003484 RID: 13444
		public List<MemeDef> memes = new List<MemeDef>();

		// Token: 0x04003485 RID: 13445
		private List<Precept> precepts = new List<Precept>();

		// Token: 0x04003486 RID: 13446
		public List<string> usedSymbols = new List<string>();

		// Token: 0x04003487 RID: 13447
		public IdeoStyleTracker style;

		// Token: 0x04003488 RID: 13448
		public bool createdFromNoExpansionGame;

		// Token: 0x04003489 RID: 13449
		public string name;

		// Token: 0x0400348A RID: 13450
		public string adjective;

		// Token: 0x0400348B RID: 13451
		public string memberName;

		// Token: 0x0400348C RID: 13452
		public IdeoIconDef iconDef;

		// Token: 0x0400348D RID: 13453
		public ColorDef colorDef;

		// Token: 0x0400348E RID: 13454
		public Color? primaryFactionColor;

		// Token: 0x0400348F RID: 13455
		public string leaderTitleMale;

		// Token: 0x04003490 RID: 13456
		public string leaderTitleFemale;

		// Token: 0x04003491 RID: 13457
		public List<string> usedSymbolPacks = new List<string>();

		// Token: 0x04003492 RID: 13458
		public string description;

		// Token: 0x04003493 RID: 13459
		public string descriptionTemplate;

		// Token: 0x04003494 RID: 13460
		public bool descriptionLocked;

		// Token: 0x04003495 RID: 13461
		private string overriddenWorshipRoomLabel;

		// Token: 0x04003496 RID: 13462
		public bool relicsCollected;

		// Token: 0x04003497 RID: 13463
		private Texture2D icon;

		// Token: 0x04003498 RID: 13464
		public List<ThoughtDef> cachedPossibleSituationalThoughts = new List<ThoughtDef>();

		// Token: 0x04003499 RID: 13465
		public List<GoodwillSituationDef> cachedPossibleGoodwillSituations = new List<GoodwillSituationDef>();

		// Token: 0x0400349A RID: 13466
		public List<Precept_Role> cachedPossibleRoles = new List<Precept_Role>();

		// Token: 0x0400349B RID: 13467
		public HashSet<Precept_Building> cachedPossibleBuildings = new HashSet<Precept_Building>();

		// Token: 0x0400349C RID: 13468
		public HashSet<BuildableDef> cachedPossibleBuildables = new HashSet<BuildableDef>();

		// Token: 0x0400349D RID: 13469
		public List<ThingStyleCategoryWithPriority> thingStyleCategories = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x0400349E RID: 13470
		private List<ThingDef> cachedVeneratedAnimals;

		// Token: 0x0400349F RID: 13471
		private int requiredScarsCached;

		// Token: 0x040034A0 RID: 13472
		private bool warnPlayerOnDesignateChopTree;

		// Token: 0x040034A1 RID: 13473
		private bool warnPlayerOnDesignateMine;

		// Token: 0x040034A2 RID: 13474
		private List<RitualObligationTargetFilter> cachedRitualTargetFilters;

		// Token: 0x040034A3 RID: 13475
		private HashSet<PreceptDef> allPreceptDefs = new HashSet<PreceptDef>();

		// Token: 0x040034A4 RID: 13476
		private string memberNamePluralCached;

		// Token: 0x040034A5 RID: 13477
		private static List<Pawn> tmpPlayerIdeoFollowers = new List<Pawn>();

		// Token: 0x040034A6 RID: 13478
		private int colonistBelieverCountCached = -1;
	}
}
