using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBB RID: 3771
	public class FactionIdeosTracker : IExposable
	{
		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060058D1 RID: 22737 RVA: 0x001E4E4E File Offset: 0x001E304E
		public List<Ideo> IdeosMinorListForReading
		{
			get
			{
				return this.ideosMinor;
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060058D2 RID: 22738 RVA: 0x001E4E56 File Offset: 0x001E3056
		public Ideo PrimaryIdeo
		{
			get
			{
				return this.primaryIdeo;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060058D3 RID: 22739 RVA: 0x001E4E5E File Offset: 0x001E305E
		public int LastAnimalSlaughterTick
		{
			get
			{
				return this.lastAnimalSlaughterTick;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060058D4 RID: 22740 RVA: 0x001E4E66 File Offset: 0x001E3066
		public IEnumerable<Ideo> AllIdeos
		{
			get
			{
				if (this.primaryIdeo != null)
				{
					yield return this.primaryIdeo;
				}
				int num;
				for (int i = 0; i < this.ideosMinor.Count; i = num + 1)
				{
					yield return this.ideosMinor[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060058D5 RID: 22741 RVA: 0x001E4E76 File Offset: 0x001E3076
		public CultureDef PrimaryCulture
		{
			get
			{
				if (this.PrimaryIdeo == null)
				{
					return null;
				}
				return this.PrimaryIdeo.culture;
			}
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x001E4E8D File Offset: 0x001E308D
		public FactionIdeosTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x001E4EAE File Offset: 0x001E30AE
		public FactionIdeosTracker()
		{
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x001E4EC8 File Offset: 0x001E30C8
		public bool Has(Ideo ideo)
		{
			return ideo == this.primaryIdeo || this.ideosMinor.Contains(ideo);
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x001E4EE4 File Offset: 0x001E30E4
		public bool HasAnyIdeoWithMeme(MemeDef meme)
		{
			if (this.primaryIdeo.memes.Contains(meme))
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = this.ideosMinor.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.memes.Contains(meme))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x001E4F58 File Offset: 0x001E3158
		public bool IsPrimary(Ideo ideo)
		{
			return ideo == this.primaryIdeo;
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x001E4F63 File Offset: 0x001E3163
		public bool IsMinor(Ideo ideo)
		{
			return this.ideosMinor.Contains(ideo);
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x001E4F71 File Offset: 0x001E3171
		public void Notify_MemberGainedOrLost()
		{
			if (this.faction.IsPlayer)
			{
				this.RecalculateIdeosBasedOnPlayerPawns();
			}
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x001E4F86 File Offset: 0x001E3186
		public void Notify_ColonistChangedIdeo()
		{
			this.RecalculateIdeosBasedOnPlayerPawns();
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x001E4F90 File Offset: 0x001E3190
		private void RecalculateIdeosBasedOnPlayerPawns()
		{
			if (Current.ProgramState != ProgramState.Playing || Find.WindowStack.IsOpen<Dialog_ConfigureIdeo>())
			{
				return;
			}
			this.ideosMinor.Clear();
			List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
			FactionIdeosTracker.tmpPlayerIdeos.Clear();
			for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count; i++)
			{
				if (allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].HomeFaction == Faction.OfPlayer)
				{
					Ideo ideo = allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].Ideo;
					if (ideo != null)
					{
						FactionIdeosTracker.tmpPlayerIdeos.Increment(ideo);
					}
				}
			}
			int num = 0;
			Ideo ideo2 = null;
			foreach (KeyValuePair<Ideo, int> keyValuePair in FactionIdeosTracker.tmpPlayerIdeos)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
					ideo2 = keyValuePair.Key;
				}
			}
			if (ideo2 != null && num > FactionIdeosTracker.tmpPlayerIdeos.TryGetValue(this.primaryIdeo, 0))
			{
				if (this.primaryIdeo != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelNewPrimaryIdeo".Translate(this.primaryIdeo.Named("OLDIDEO"), ideo2.Named("NEWIDEO")), "LetterNewPrimaryIdeo".Translate(this.primaryIdeo.Named("OLDIDEO"), ideo2.Named("NEWIDEO"), Faction.OfPlayer.Named("FACTION")), LetterDefOf.NeutralEvent, null);
					this.primaryIdeo.Notify_NotPrimaryAnymore(ideo2);
				}
				if (this.primaryIdeo != null && ideo2.ColonistBelieverCountCached < Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterObligationsActive".Translate(ideo2), "LetterObligationsActiveIdeoBecameMajor".Translate(ideo2.Named("IDEO")), LetterDefOf.NeutralEvent, null);
				}
				this.primaryIdeo = ideo2;
			}
			foreach (KeyValuePair<Ideo, int> keyValuePair2 in FactionIdeosTracker.tmpPlayerIdeos)
			{
				if (keyValuePair2.Key != this.primaryIdeo && !this.ideosMinor.Contains(keyValuePair2.Key))
				{
					this.ideosMinor.Add(keyValuePair2.Key);
				}
			}
			FactionIdeosTracker.tmpPlayerIdeos.Clear();
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x001E51D4 File Offset: 0x001E33D4
		public void SetPrimary(Ideo ideo)
		{
			this.primaryIdeo = ideo;
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x001E51E0 File Offset: 0x001E33E0
		public void ChooseOrGenerateIdeo(IdeoGenerationParms parms)
		{
			this.primaryIdeo = null;
			this.ideosMinor.Clear();
			if (!ModsConfig.IdeologyActive || parms.forceNoExpansionIdeo)
			{
				Ideo ideo;
				if ((from x in Find.IdeoManager.IdeosListForReading
				where IdeoUtility.CanUseIdeo(this.faction.def, x, parms.disallowedPrecepts) && (this.faction.def.allowedCultures == null || this.faction.def.allowedCultures.Contains(x.culture))
				select x).TryRandomElement(out ideo))
				{
					this.primaryIdeo = ideo;
					return;
				}
				Ideo ideo2 = IdeoGenerator.GenerateNoExpansionIdeo((!this.faction.def.allowedCultures.NullOrEmpty<CultureDef>()) ? this.faction.def.allowedCultures[0] : DefDatabase<CultureDef>.AllDefs.RandomElement<CultureDef>(), parms);
				this.primaryIdeo = ideo2;
				Find.IdeoManager.Add(ideo2);
				return;
			}
			else
			{
				Ideo ideo3;
				if ((Rand.Chance(0.2f) || Find.IdeoManager.IdeosListForReading.Count >= 10 || this.faction.def.hidden) && (from x in Find.IdeoManager.IdeosListForReading
				where IdeoUtility.CanUseIdeo(this.faction.def, x, parms.disallowedPrecepts)
				select x).TryRandomElement(out ideo3))
				{
					this.primaryIdeo = ideo3;
					return;
				}
				Ideo ideo4 = IdeoGenerator.GenerateIdeo(parms);
				ideo4.primaryFactionColor = new Color?(this.faction.Color);
				this.primaryIdeo = ideo4;
				Find.IdeoManager.Add(ideo4);
				return;
			}
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x001E5345 File Offset: 0x001E3545
		public Ideo GetRandomIdeoForNewPawn()
		{
			return this.AllIdeos.RandomElementByWeightWithFallback(delegate(Ideo x)
			{
				if (!this.IsPrimary(x))
				{
					return 1f;
				}
				return 4f;
			}, null);
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x001E535F File Offset: 0x001E355F
		public void ResetLastAnimalSlaughteredTick()
		{
			this.lastAnimalSlaughterTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x001E5374 File Offset: 0x001E3574
		public Precept GetPrecept(PreceptDef precept)
		{
			foreach (Ideo ideo in this.AllIdeos)
			{
				foreach (Precept precept2 in ideo.PreceptsListForReading)
				{
					if (precept2.def == precept)
					{
						return precept2;
					}
				}
			}
			return null;
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x001E5404 File Offset: 0x001E3604
		public bool AnyPreceptWithRequiredScars()
		{
			using (IEnumerator<Ideo> enumerator = this.AllIdeos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.RequiredScars > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x001E5458 File Offset: 0x001E3658
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.primaryIdeo, "primaryIdeo", false);
			Scribe_Collections.Look<Ideo>(ref this.ideosMinor, "ideosMinor", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastAnimalSlaughterTick, "lastAnimalSlaughterTick", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ideosMinor.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Some ideoligion references were null after loading.");
				}
				if (this.lastAnimalSlaughterTick < 0)
				{
					this.lastAnimalSlaughterTick = Find.TickManager.TicksGame;
				}
				if (this.primaryIdeo == null)
				{
					Log.Error("Faction had no ideoligions after loading. Adding random one.");
					Ideo ideo;
					if (Find.IdeoManager.IdeosListForReading.TryRandomElement(out ideo))
					{
						this.primaryIdeo = ideo;
					}
				}
			}
		}

		// Token: 0x04003444 RID: 13380
		private Faction faction;

		// Token: 0x04003445 RID: 13381
		private Ideo primaryIdeo;

		// Token: 0x04003446 RID: 13382
		private List<Ideo> ideosMinor = new List<Ideo>();

		// Token: 0x04003447 RID: 13383
		private int lastAnimalSlaughterTick = -1;

		// Token: 0x04003448 RID: 13384
		private const float MajorIdeoSelectionWeight = 4f;

		// Token: 0x04003449 RID: 13385
		private const float MinorIdeoSelectionWeight = 1f;

		// Token: 0x0400344A RID: 13386
		private const float ChanceToReuseExistingIdeo = 0.2f;

		// Token: 0x0400344B RID: 13387
		private const int MaxIdeos = 10;

		// Token: 0x0400344C RID: 13388
		private static Dictionary<Ideo, int> tmpPlayerIdeos = new Dictionary<Ideo, int>();
	}
}
