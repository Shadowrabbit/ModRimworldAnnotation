using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102A RID: 4138
	public sealed class TaleManager : IExposable
	{
		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060061AF RID: 25007 RVA: 0x00212F21 File Offset: 0x00211121
		public List<Tale> AllTalesListForReading
		{
			get
			{
				return this.tales;
			}
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x00212F2C File Offset: 0x0021112C
		public void ExposeData()
		{
			Scribe_Collections.Look<Tale>(ref this.tales, "tales", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.tales.RemoveAll((Tale x) => x == null) != 0)
				{
					Log.Error("Some tales were null after loading.");
				}
				if (this.tales.RemoveAll((Tale x) => x.def == null) != 0)
				{
					Log.Error("Some tales had null def after loading.");
				}
			}
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x00212FC3 File Offset: 0x002111C3
		public void TaleManagerTick()
		{
			this.RemoveExpiredTales();
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x00212FCB File Offset: 0x002111CB
		public void Add(Tale tale)
		{
			this.tales.Add(tale);
			this.CheckCullTales(tale);
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x00212FE0 File Offset: 0x002111E0
		private void RemoveTale(Tale tale)
		{
			if (!tale.Unused)
			{
				Log.Warning("Tried to remove used tale " + tale);
				return;
			}
			this.tales.Remove(tale);
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x00213008 File Offset: 0x00211208
		private void CheckCullTales(Tale addedTale)
		{
			this.CheckCullUnusedVolatileTales();
			this.CheckCullUnusedTalesWithMaxPerPawnLimit(addedTale);
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x00213018 File Offset: 0x00211218
		private void CheckCullUnusedVolatileTales()
		{
			int i = 0;
			for (int j = 0; j < this.tales.Count; j++)
			{
				if (this.tales[j].def.type == TaleType.Volatile && this.tales[j].Unused)
				{
					i++;
				}
			}
			while (i > 350)
			{
				Tale tale = null;
				float num = float.MaxValue;
				for (int k = 0; k < this.tales.Count; k++)
				{
					if (this.tales[k].def.type == TaleType.Volatile && this.tales[k].Unused && this.tales[k].InterestLevel < num)
					{
						tale = this.tales[k];
						num = this.tales[k].InterestLevel;
					}
				}
				this.RemoveTale(tale);
				i--;
			}
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x0021310C File Offset: 0x0021130C
		private void CheckCullUnusedTalesWithMaxPerPawnLimit(Tale addedTale)
		{
			if (addedTale.def.maxPerPawn < 0)
			{
				return;
			}
			if (addedTale.DominantPawn == null)
			{
				return;
			}
			int i = 0;
			for (int j = 0; j < this.tales.Count; j++)
			{
				if (this.tales[j].Unused && this.tales[j].def == addedTale.def && this.tales[j].DominantPawn == addedTale.DominantPawn)
				{
					i++;
				}
			}
			while (i > addedTale.def.maxPerPawn)
			{
				Tale tale = null;
				int num = -1;
				for (int k = 0; k < this.tales.Count; k++)
				{
					if (this.tales[k].Unused && this.tales[k].def == addedTale.def && this.tales[k].DominantPawn == addedTale.DominantPawn && this.tales[k].AgeTicks > num)
					{
						tale = this.tales[k];
						num = this.tales[k].AgeTicks;
					}
				}
				this.RemoveTale(tale);
				i--;
			}
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x00213258 File Offset: 0x00211458
		private void RemoveExpiredTales()
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Expired)
				{
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x002132A4 File Offset: 0x002114A4
		public TaleReference GetRandomTaleReferenceForArt(ArtGenerationContext source)
		{
			if (source == ArtGenerationContext.Outsider)
			{
				return TaleReference.Taleless;
			}
			if (this.tales.Count == 0)
			{
				return TaleReference.Taleless;
			}
			if (Rand.Value < 0.25f)
			{
				return TaleReference.Taleless;
			}
			Tale tale;
			if (!(from x in this.tales
			where x.def.usableForArt
			select x).TryRandomElementByWeight((Tale ta) => ta.InterestLevel, out tale))
			{
				return TaleReference.Taleless;
			}
			tale.Notify_NewlyUsed();
			return new TaleReference(tale);
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x00213344 File Offset: 0x00211544
		public TaleReference GetRandomTaleReferenceForArtConcerning(Thing th)
		{
			if (this.tales.Count == 0)
			{
				return TaleReference.Taleless;
			}
			Tale tale;
			if (!(from x in this.tales
			where x.def.usableForArt && x.Concerns(th)
			select x).TryRandomElementByWeight((Tale x) => x.InterestLevel, out tale))
			{
				return TaleReference.Taleless;
			}
			tale.Notify_NewlyUsed();
			return new TaleReference(tale);
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x002133C4 File Offset: 0x002115C4
		public Tale GetLatestTale(TaleDef def, Pawn pawn)
		{
			Tale tale = null;
			int num = 0;
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].def == def && this.tales[i].DominantPawn == pawn && (tale == null || this.tales[i].AgeTicks < num))
				{
					tale = this.tales[i];
					num = this.tales[i].AgeTicks;
				}
			}
			return tale;
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x0021344C File Offset: 0x0021164C
		public void Notify_PawnDestroyed(Pawn pawn)
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Unused && !this.tales[i].def.usableForArt && this.tales[i].def.type != TaleType.PermanentHistorical && this.tales[i].DominantPawn == pawn)
				{
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x002134DC File Offset: 0x002116DC
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by a tale ",
							this.tales[i],
							"."
						}));
					}
					else if (!this.tales[i].Unused)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by an active tale ",
							this.tales[i],
							"."
						}));
					}
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x060061BD RID: 25021 RVA: 0x002135C0 File Offset: 0x002117C0
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				this.tales[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x002135F8 File Offset: 0x002117F8
		public bool AnyActiveTaleConcerns(Pawn p)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (!this.tales[i].Unused && this.tales[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x00213648 File Offset: 0x00211848
		public bool AnyTaleConcerns(Pawn p)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x00213684 File Offset: 0x00211884
		public float GetMaxHistoricalTaleDay()
		{
			float num = 0f;
			for (int i = 0; i < this.tales.Count; i++)
			{
				Tale tale = this.tales[i];
				if (tale.def.type == TaleType.PermanentHistorical)
				{
					float num2 = (float)GenDate.TickAbsToGame(tale.date) / 60000f;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x002136E4 File Offset: 0x002118E4
		public void LogTales()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Tale> enumerable = from x in this.tales
			where !x.Unused
			select x;
			IEnumerable<Tale> enumerable2 = from x in this.tales
			where x.def.type == TaleType.Volatile && x.Unused
			select x;
			IEnumerable<Tale> enumerable3 = from x in this.tales
			where x.def.type == TaleType.PermanentHistorical && x.Unused
			select x;
			IEnumerable<Tale> enumerable4 = from x in this.tales
			where x.def.type == TaleType.Expirable && x.Unused
			select x;
			stringBuilder.AppendLine("All tales count: " + this.tales.Count);
			stringBuilder.AppendLine("Used count: " + enumerable.Count<Tale>());
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Unused volatile count: ",
				enumerable2.Count<Tale>(),
				" (max: ",
				350,
				")"
			}));
			stringBuilder.AppendLine("Unused permanent count: " + enumerable3.Count<Tale>());
			stringBuilder.AppendLine("Unused expirable count: " + enumerable4.Count<Tale>());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-------Used-------");
			foreach (Tale tale in enumerable)
			{
				stringBuilder.AppendLine(tale.ToString());
			}
			stringBuilder.AppendLine("-------Unused volatile-------");
			foreach (Tale tale2 in enumerable2)
			{
				stringBuilder.AppendLine(tale2.ToString());
			}
			stringBuilder.AppendLine("-------Unused permanent-------");
			foreach (Tale tale3 in enumerable3)
			{
				stringBuilder.AppendLine(tale3.ToString());
			}
			stringBuilder.AppendLine("-------Unused expirable-------");
			foreach (Tale tale4 in enumerable4)
			{
				stringBuilder.AppendLine(tale4.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x002139B8 File Offset: 0x00211BB8
		public void LogTaleInterestSummary()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (from t in this.tales
			where t.def.usableForArt
			select t).Sum((Tale t) => t.InterestLevel);
			Func<TaleDef, float> defInterest = (TaleDef def) => (from t in this.tales
			where t.def == def
			select t).Sum((Tale t) => t.InterestLevel);
			IEnumerable<TaleDef> source = from def in DefDatabase<TaleDef>.AllDefs
			where def.usableForArt
			select def;
			Func<TaleDef, float> <>9__6;
			Func<TaleDef, float> keySelector;
			if ((keySelector = <>9__6) == null)
			{
				keySelector = (<>9__6 = ((TaleDef def) => defInterest(def)));
			}
			using (IEnumerator<TaleDef> enumerator = source.OrderByDescending(keySelector).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TaleDef def = enumerator.Current;
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						def.defName,
						":   [",
						(from t in this.tales
						where t.def == def
						select t).Count<Tale>(),
						"]   ",
						(defInterest(def) / num).ToStringPercent("F2")
					}));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x040037BC RID: 14268
		private List<Tale> tales = new List<Tale>();

		// Token: 0x040037BD RID: 14269
		private const int MaxUnusedVolatileTales = 350;
	}
}
