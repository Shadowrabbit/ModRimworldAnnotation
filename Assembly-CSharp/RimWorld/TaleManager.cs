using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001640 RID: 5696
	public sealed class TaleManager : IExposable
	{
		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06007BD2 RID: 31698 RVA: 0x000532B1 File Offset: 0x000514B1
		public List<Tale> AllTalesListForReading
		{
			get
			{
				return this.tales;
			}
		}

		// Token: 0x06007BD3 RID: 31699 RVA: 0x002524AC File Offset: 0x002506AC
		public void ExposeData()
		{
			Scribe_Collections.Look<Tale>(ref this.tales, "tales", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.tales.RemoveAll((Tale x) => x == null) != 0)
				{
					Log.Error("Some tales were null after loading.", false);
				}
				if (this.tales.RemoveAll((Tale x) => x.def == null) != 0)
				{
					Log.Error("Some tales had null def after loading.", false);
				}
			}
		}

		// Token: 0x06007BD4 RID: 31700 RVA: 0x000532B9 File Offset: 0x000514B9
		public void TaleManagerTick()
		{
			this.RemoveExpiredTales();
		}

		// Token: 0x06007BD5 RID: 31701 RVA: 0x000532C1 File Offset: 0x000514C1
		public void Add(Tale tale)
		{
			this.tales.Add(tale);
			this.CheckCullTales(tale);
		}

		// Token: 0x06007BD6 RID: 31702 RVA: 0x000532D6 File Offset: 0x000514D6
		private void RemoveTale(Tale tale)
		{
			if (!tale.Unused)
			{
				Log.Warning("Tried to remove used tale " + tale, false);
				return;
			}
			this.tales.Remove(tale);
		}

		// Token: 0x06007BD7 RID: 31703 RVA: 0x000532FF File Offset: 0x000514FF
		private void CheckCullTales(Tale addedTale)
		{
			this.CheckCullUnusedVolatileTales();
			this.CheckCullUnusedTalesWithMaxPerPawnLimit(addedTale);
		}

		// Token: 0x06007BD8 RID: 31704 RVA: 0x00252548 File Offset: 0x00250748
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

		// Token: 0x06007BD9 RID: 31705 RVA: 0x0025263C File Offset: 0x0025083C
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

		// Token: 0x06007BDA RID: 31706 RVA: 0x00252788 File Offset: 0x00250988
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

		// Token: 0x06007BDB RID: 31707 RVA: 0x002527D4 File Offset: 0x002509D4
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

		// Token: 0x06007BDC RID: 31708 RVA: 0x00252874 File Offset: 0x00250A74
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

		// Token: 0x06007BDD RID: 31709 RVA: 0x002528F4 File Offset: 0x00250AF4
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

		// Token: 0x06007BDE RID: 31710 RVA: 0x0025297C File Offset: 0x00250B7C
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

		// Token: 0x06007BDF RID: 31711 RVA: 0x00252A0C File Offset: 0x00250C0C
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
						}), false);
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
						}), false);
					}
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		// Token: 0x06007BE0 RID: 31712 RVA: 0x00252AF0 File Offset: 0x00250CF0
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				this.tales[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06007BE1 RID: 31713 RVA: 0x00252B28 File Offset: 0x00250D28
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

		// Token: 0x06007BE2 RID: 31714 RVA: 0x00252B78 File Offset: 0x00250D78
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

		// Token: 0x06007BE3 RID: 31715 RVA: 0x00252BB4 File Offset: 0x00250DB4
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

		// Token: 0x06007BE4 RID: 31716 RVA: 0x00252C14 File Offset: 0x00250E14
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
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06007BE5 RID: 31717 RVA: 0x00252EE8 File Offset: 0x002510E8
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
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0400512B RID: 20779
		private List<Tale> tales = new List<Tale>();

		// Token: 0x0400512C RID: 20780
		private const int MaxUnusedVolatileTales = 350;
	}
}
