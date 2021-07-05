using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E6B RID: 3691
	public class Pawn_MeleeVerbs : IExposable
	{
		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060055B2 RID: 21938 RVA: 0x001D097B File Offset: 0x001CEB7B
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x001D0983 File Offset: 0x001CEB83
		public Pawn_MeleeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x001D099D File Offset: 0x001CEB9D
		public static void PawnMeleeVerbsStaticUpdate()
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x001D09B4 File Offset: 0x001CEBB4
		public Verb TryGetMeleeVerb(Thing target)
		{
			if (this.curMeleeVerb == null || this.curMeleeVerbTarget != target || Find.TickManager.TicksGame >= this.curMeleeVerbUpdateTick + 60 || !this.curMeleeVerb.IsStillUsableBy(this.pawn) || !this.curMeleeVerb.IsUsableOn(target))
			{
				this.ChooseMeleeVerb(target);
			}
			return this.curMeleeVerb;
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x001D0A18 File Offset: 0x001CEC18
		private void ChooseMeleeVerb(Thing target)
		{
			bool flag = Rand.Chance(0.04f);
			List<VerbEntry> updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(flag);
			bool flag2 = false;
			VerbEntry verbEntry;
			if (updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry))
			{
				flag2 = true;
			}
			else if (flag)
			{
				updatedAvailableVerbsList = this.GetUpdatedAvailableVerbsList(false);
				flag2 = updatedAvailableVerbsList.TryRandomElementByWeight((VerbEntry ve) => ve.GetSelectionWeight(target), out verbEntry);
			}
			if (flag2)
			{
				this.SetCurMeleeVerb(verbEntry.verb, target);
				return;
			}
			Log.ErrorOnce(string.Concat(new string[]
			{
				this.pawn.ToStringSafe<Pawn>(),
				" has no available melee attack, spawned=",
				this.pawn.Spawned.ToString(),
				" dead=",
				this.pawn.Dead.ToString(),
				" downed=",
				this.pawn.Downed.ToString(),
				" curJob=",
				this.pawn.CurJob.ToStringSafe<Job>(),
				" verbList=",
				updatedAvailableVerbsList.ToStringSafeEnumerable(),
				" bodyVerbs=",
				this.pawn.verbTracker.AllVerbs.ToStringSafeEnumerable()
			}), this.pawn.thingIDNumber ^ 195867354);
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x001D0B80 File Offset: 0x001CED80
		public bool TryMeleeAttack(Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			if (this.pawn.stances.FullBodyBusy)
			{
				return false;
			}
			if (verbToUse != null)
			{
				if (!verbToUse.IsStillUsableBy(this.pawn))
				{
					return false;
				}
				if (!verbToUse.IsMeleeAttack)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Pawn ",
						this.pawn,
						" tried to melee attack ",
						target,
						" with non melee-attack verb ",
						verbToUse,
						"."
					}));
					return false;
				}
			}
			Verb verb;
			if (verbToUse != null)
			{
				verb = verbToUse;
			}
			else
			{
				verb = this.TryGetMeleeVerb(target);
			}
			if (verb == null)
			{
				return false;
			}
			verb.TryStartCastOn(target, surpriseAttack, true, false);
			return true;
		}

		// Token: 0x060055B8 RID: 21944 RVA: 0x001D0C24 File Offset: 0x001CEE24
		public List<VerbEntry> GetUpdatedAvailableVerbsList(bool terrainTools)
		{
			Pawn_MeleeVerbs.meleeVerbs.Clear();
			Pawn_MeleeVerbs.verbsToAdd.Clear();
			if (!terrainTools)
			{
				List<Verb> allVerbs = this.pawn.verbTracker.AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs[i]))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs[i]);
					}
				}
				if (this.pawn.equipment != null)
				{
					List<ThingWithComps> allEquipmentListForReading = this.pawn.equipment.AllEquipmentListForReading;
					for (int j = 0; j < allEquipmentListForReading.Count; j++)
					{
						CompEquippable comp = allEquipmentListForReading[j].GetComp<CompEquippable>();
						if (comp != null)
						{
							List<Verb> allVerbs2 = comp.AllVerbs;
							if (allVerbs2 != null)
							{
								for (int k = 0; k < allVerbs2.Count; k++)
								{
									if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs2[k]))
									{
										Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs2[k]);
									}
								}
							}
						}
					}
				}
				if (this.pawn.apparel != null)
				{
					List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
					for (int l = 0; l < wornApparel.Count; l++)
					{
						CompEquippable comp2 = wornApparel[l].GetComp<CompEquippable>();
						if (comp2 != null)
						{
							List<Verb> allVerbs3 = comp2.AllVerbs;
							if (allVerbs3 != null)
							{
								for (int m = 0; m < allVerbs3.Count; m++)
								{
									if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(allVerbs3[m]))
									{
										Pawn_MeleeVerbs.verbsToAdd.Add(allVerbs3[m]);
									}
								}
							}
						}
					}
				}
				using (IEnumerator<Verb> enumerator = this.pawn.health.hediffSet.GetHediffsVerbs().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Verb verb = enumerator.Current;
						if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb))
						{
							Pawn_MeleeVerbs.verbsToAdd.Add(verb);
						}
					}
					goto IL_271;
				}
			}
			if (this.pawn.Spawned)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (this.terrainVerbs == null || this.terrainVerbs.def != terrain)
				{
					this.terrainVerbs = Pawn_MeleeVerbs_TerrainSource.Create(this, terrain);
				}
				List<Verb> allVerbs4 = this.terrainVerbs.tracker.AllVerbs;
				for (int n = 0; n < allVerbs4.Count; n++)
				{
					Verb verb2 = allVerbs4[n];
					if (this.<GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(verb2))
					{
						Pawn_MeleeVerbs.verbsToAdd.Add(verb2);
					}
				}
			}
			IL_271:
			float num = 0f;
			foreach (Verb v in Pawn_MeleeVerbs.verbsToAdd)
			{
				float num2 = VerbUtility.InitialVerbWeight(v, this.pawn);
				if (num2 > num)
				{
					num = num2;
				}
			}
			foreach (Verb verb3 in Pawn_MeleeVerbs.verbsToAdd)
			{
				Pawn_MeleeVerbs.meleeVerbs.Add(new VerbEntry(verb3, this.pawn, Pawn_MeleeVerbs.verbsToAdd, num));
			}
			return Pawn_MeleeVerbs.meleeVerbs;
		}

		// Token: 0x060055B9 RID: 21945 RVA: 0x001D0F68 File Offset: 0x001CF168
		public void Notify_PawnKilled()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x060055BA RID: 21946 RVA: 0x001D0F68 File Offset: 0x001CF168
		public void Notify_PawnDespawned()
		{
			this.SetCurMeleeVerb(null, null);
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x001D0F72 File Offset: 0x001CF172
		public void Notify_UsedTerrainBasedVerb()
		{
			this.lastTerrainBasedVerbUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x001D0F84 File Offset: 0x001CF184
		private void SetCurMeleeVerb(Verb v, Thing target)
		{
			this.curMeleeVerb = v;
			this.curMeleeVerbTarget = target;
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.curMeleeVerbUpdateTick = 0;
				return;
			}
			this.curMeleeVerbUpdateTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060055BD RID: 21949 RVA: 0x001D0FB4 File Offset: 0x001CF1B4
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.curMeleeVerb != null && !this.curMeleeVerb.IsStillUsableBy(this.pawn))
			{
				this.curMeleeVerb = null;
			}
			Scribe_References.Look<Verb>(ref this.curMeleeVerb, "curMeleeVerb", false);
			Scribe_Values.Look<int>(ref this.curMeleeVerbUpdateTick, "curMeleeVerbUpdateTick", 0, false);
			Scribe_Deep.Look<Pawn_MeleeVerbs_TerrainSource>(ref this.terrainVerbs, "terrainVerbs", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastTerrainBasedVerbUseTick, "lastTerrainBasedVerbUseTick", -99999, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.terrainVerbs != null)
			{
				this.terrainVerbs.parent = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curMeleeVerb != null && this.curMeleeVerb.BuggedAfterLoading)
			{
				this.curMeleeVerb = null;
				Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had a bugged melee verb after loading.");
			}
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x001D10A9 File Offset: 0x001CF2A9
		[CompilerGenerated]
		private bool <GetUpdatedAvailableVerbsList>g__IsUsableMeleeVerb|18_0(Verb v)
		{
			return v.IsStillUsableBy(this.pawn) && v.IsMeleeAttack;
		}

		// Token: 0x040032AA RID: 12970
		private Pawn pawn;

		// Token: 0x040032AB RID: 12971
		private Verb curMeleeVerb;

		// Token: 0x040032AC RID: 12972
		private Thing curMeleeVerbTarget;

		// Token: 0x040032AD RID: 12973
		private int curMeleeVerbUpdateTick;

		// Token: 0x040032AE RID: 12974
		private Pawn_MeleeVerbs_TerrainSource terrainVerbs;

		// Token: 0x040032AF RID: 12975
		public int lastTerrainBasedVerbUseTick = -99999;

		// Token: 0x040032B0 RID: 12976
		private static List<VerbEntry> meleeVerbs = new List<VerbEntry>();

		// Token: 0x040032B1 RID: 12977
		private static List<Verb> verbsToAdd = new List<Verb>();

		// Token: 0x040032B2 RID: 12978
		private const int BestMeleeVerbUpdateInterval = 60;

		// Token: 0x040032B3 RID: 12979
		public const int TerrainBasedVerbUseDelay = 1200;

		// Token: 0x040032B4 RID: 12980
		private const float TerrainBasedVerbChooseChance = 0.04f;
	}
}
