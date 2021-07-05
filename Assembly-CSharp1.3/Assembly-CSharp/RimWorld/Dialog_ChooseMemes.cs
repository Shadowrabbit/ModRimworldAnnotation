using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E0 RID: 4832
	public class Dialog_ChooseMemes : Window
	{
		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x060073A2 RID: 29602 RVA: 0x0026E116 File Offset: 0x0026C316
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(980f, 724f);
			}
		}

		// Token: 0x060073A3 RID: 29603 RVA: 0x0026E128 File Offset: 0x0026C328
		public Dialog_ChooseMemes(Ideo ideo, MemeCategory memeCategory, bool initialSelection = false, Action done = null)
		{
			this.ideo = ideo;
			this.memeCategory = memeCategory;
			this.initialSelection = initialSelection;
			this.done = done;
			this.absorbInputAroundWindow = true;
			this.newMemes.Clear();
			this.newMemes.AddRange(ideo.memes);
		}

		// Token: 0x060073A4 RID: 29604 RVA: 0x0026E188 File Offset: 0x0026C388
		private int RandomGeneratedMemeCount()
		{
			return GenMath.RoundRandom(IdeoFoundation.MemeCountRangeAbsolute.Average);
		}

		// Token: 0x060073A5 RID: 29605 RVA: 0x0026E1A8 File Offset: 0x0026C3A8
		public override void DoWindowContents(Rect rect)
		{
			Rect outRect = rect;
			outRect.height -= Dialog_ChooseMemes.ButSize.y;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(outRect.x, outRect.y, rect.width, 30f), (this.memeCategory == MemeCategory.Normal) ? "ChooseMemes".Translate() : "ChooseStructure".Translate());
			Text.Font = GameFont.Small;
			outRect.yMin += 30f;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float b = 0f;
			if (this.memeCategory == MemeCategory.Structure)
			{
				this.DoMemeSelector(viewRect, MemeCategory.Structure, ref b);
			}
			else if (this.memeCategory == MemeCategory.Normal)
			{
				this.DoMemeSelector(viewRect, MemeCategory.Normal, ref b);
			}
			this.viewHeight = Mathf.Max(this.viewHeight, b);
			Widgets.EndScrollView();
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_ChooseMemes.ButSize.y, Dialog_ChooseMemes.ButSize.x, Dialog_ChooseMemes.ButSize.y), "Back".Translate(), true, true, true))
			{
				this.Close(true);
				if (this.memeCategory == MemeCategory.Structure)
				{
					this.NotifyConfigureIdeoPage();
				}
				else if (this.memeCategory == MemeCategory.Normal)
				{
					if (this.initialSelection)
					{
						Find.WindowStack.Add(new Dialog_ChooseMemes(this.ideo, MemeCategory.Structure, this.initialSelection, null));
					}
					else
					{
						this.NotifyConfigureIdeoPage();
					}
				}
			}
			if (Widgets.ButtonText(new Rect((float)((int)(rect.width - Dialog_ChooseMemes.ButSize.x) / 2), rect.height - Dialog_ChooseMemes.ButSize.y, Dialog_ChooseMemes.ButSize.x, Dialog_ChooseMemes.ButSize.y), "Randomize".Translate(), true, true, true))
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				FactionDef forFaction = IdeoUIUtility.FactionForRandomization(this.ideo);
				if (this.memeCategory == MemeCategory.Normal)
				{
					this.newMemes = IdeoUtility.RandomizeNormalMemes(this.RandomGeneratedMemeCount(), this.newMemes, forFaction);
				}
				else if (this.memeCategory == MemeCategory.Structure)
				{
					this.newMemes = IdeoUtility.RandomizeStructureMeme(this.newMemes, forFaction);
				}
			}
			Rect rect2 = new Rect(rect.width - Dialog_ChooseMemes.ButSize.x, rect.height - Dialog_ChooseMemes.ButSize.y, Dialog_ChooseMemes.ButSize.x, Dialog_ChooseMemes.ButSize.y);
			if (Widgets.ButtonText(rect2, "DoneButton".Translate(), true, true, true))
			{
				this.TryAccept();
			}
			string text = null;
			Pair<MemeDef, MemeDef> firstIncompatibleMemePair = this.GetFirstIncompatibleMemePair();
			if (this.GetMemeCount(MemeCategory.Structure) < 1 || (this.memeCategory == MemeCategory.Normal && (this.GetMemeCount(MemeCategory.Normal) < IdeoFoundation.MemeCountRangeAbsolute.min || this.GetMemeCount(MemeCategory.Normal) > IdeoFoundation.MemeCountRangeAbsolute.max)) || firstIncompatibleMemePair != default(Pair<MemeDef, MemeDef>))
			{
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleRight;
				GUI.color = Color.red;
				if (firstIncompatibleMemePair != default(Pair<MemeDef, MemeDef>))
				{
					text = "IncompatibleMemes".Translate(firstIncompatibleMemePair.First, firstIncompatibleMemePair.Second).CapitalizeFirst();
				}
				else if (this.memeCategory == MemeCategory.Normal)
				{
					if (this.GetMemeCount(MemeCategory.Normal) < IdeoFoundation.MemeCountRangeAbsolute.min)
					{
						text = "NotEnoughMemes".Translate(IdeoFoundation.MemeCountRangeAbsolute.min);
					}
					else
					{
						text = "TooManyMemes".Translate(IdeoFoundation.MemeCountRangeAbsolute.max);
					}
				}
				else
				{
					text = "ChooseStructureMeme".Translate();
				}
			}
			Rect rect3 = new Rect(rect.xMax - Dialog_ChooseMemes.ButSize.x - 240f - 6f, rect2.y, 240f, Dialog_ChooseMemes.ButSize.y);
			if (text != null)
			{
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				return;
			}
			if (this.memeCategory == MemeCategory.Normal)
			{
				IdeoUIUtility.DrawImpactInfo(rect3, this.newMemes);
			}
		}

		// Token: 0x060073A6 RID: 29606 RVA: 0x0026E5E0 File Offset: 0x0026C7E0
		private void NotifyConfigureIdeoPage()
		{
			Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
			if (page_ConfigureIdeo != null)
			{
				page_ConfigureIdeo.Notify_ClosedChooseMemesDialog();
			}
		}

		// Token: 0x060073A7 RID: 29607 RVA: 0x0026E604 File Offset: 0x0026C804
		private void TryAccept()
		{
			Pair<MemeDef, MemeDef> firstIncompatibleMemePair = this.GetFirstIncompatibleMemePair();
			if (firstIncompatibleMemePair != default(Pair<MemeDef, MemeDef>))
			{
				Messages.Message("MessageIncompatibleMemes".Translate(firstIncompatibleMemePair.First, firstIncompatibleMemePair.Second), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.memeCategory == MemeCategory.Normal)
			{
				if (this.GetMemeCount(MemeCategory.Normal) < IdeoFoundation.MemeCountRangeAbsolute.min)
				{
					Messages.Message("MessageNotEnoughMemes".Translate(IdeoFoundation.MemeCountRangeAbsolute.min), MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (this.GetMemeCount(MemeCategory.Normal) > IdeoFoundation.MemeCountRangeAbsolute.max)
				{
					Messages.Message("MessageTooManyMemes".Translate(IdeoFoundation.MemeCountRangeAbsolute.max), MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			else if (this.memeCategory == MemeCategory.Structure && this.GetMemeCount(MemeCategory.Structure) < 1)
			{
				Messages.Message("MessageNotEnoughStructureMemes".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (!this.newMemes.SetsEqual(this.ideo.memes))
			{
				this.ideo.memes.Clear();
				this.ideo.memes.AddRange(this.newMemes);
				this.ideo.SortMemesInDisplayOrder();
				FactionDef forFaction = IdeoUIUtility.FactionForRandomization(this.ideo);
				if (this.ideo.PreceptsListForReading.Any<Precept>())
				{
					this.ideo.foundation.RandomizePrecepts(true, new IdeoGenerationParms(forFaction, false, null, null));
					IdeoFoundation_Deity ideoFoundation_Deity;
					if ((ideoFoundation_Deity = (this.ideo.foundation as IdeoFoundation_Deity)) != null && this.memeCategory == MemeCategory.Structure)
					{
						ideoFoundation_Deity.GenerateDeities();
					}
				}
				else
				{
					this.ideo.foundation.RandomizeCulture(new IdeoGenerationParms(forFaction, false, null, null));
					this.ideo.foundation.RandomizePlace();
					IdeoFoundation_Deity ideoFoundation_Deity2;
					if ((ideoFoundation_Deity2 = (this.ideo.foundation as IdeoFoundation_Deity)) != null)
					{
						ideoFoundation_Deity2.GenerateDeities();
					}
					this.ideo.foundation.GenerateTextSymbols();
					this.ideo.foundation.RandomizePrecepts(false, new IdeoGenerationParms(forFaction, false, null, null));
					this.ideo.foundation.GenerateLeaderTitle();
					this.ideo.foundation.RandomizeIcon();
					this.ideo.foundation.InitPrecepts(new IdeoGenerationParms(forFaction, false, null, null), null);
					this.ideo.RecachePrecepts();
				}
				this.ideo.RegenerateDescription(true);
				this.ideo.foundation.RandomizeStyles();
				this.ideo.style.RecalculateAvailableStyleItems();
			}
			if (this.memeCategory == MemeCategory.Structure)
			{
				if (!this.ideo.memes.Any((MemeDef x) => x.category == MemeCategory.Normal))
				{
					Find.WindowStack.Add(new Dialog_ChooseMemes(this.ideo, MemeCategory.Normal, true, this.done));
					goto IL_2F9;
				}
			}
			if (this.done != null)
			{
				this.done();
			}
			IL_2F9:
			this.Close(true);
		}

		// Token: 0x060073A8 RID: 29608 RVA: 0x0026E911 File Offset: 0x0026CB11
		public override void OnAcceptKeyPressed()
		{
			this.TryAccept();
		}

		// Token: 0x060073A9 RID: 29609 RVA: 0x0026E91C File Offset: 0x0026CB1C
		private Pair<MemeDef, MemeDef> GetFirstIncompatibleMemePair()
		{
			for (int i = 0; i < this.newMemes.Count; i++)
			{
				for (int j = i + 1; j < this.newMemes.Count; j++)
				{
					for (int k = 0; k < this.newMemes[i].exclusionTags.Count; k++)
					{
						if (this.newMemes[j].exclusionTags.Contains(this.newMemes[i].exclusionTags[k]))
						{
							return new Pair<MemeDef, MemeDef>(this.newMemes[i], this.newMemes[j]);
						}
					}
				}
			}
			return default(Pair<MemeDef, MemeDef>);
		}

		// Token: 0x060073AA RID: 29610 RVA: 0x0026E9D4 File Offset: 0x0026CBD4
		private void DoMemeSelector(Rect viewRect, MemeCategory category, ref float curY)
		{
			curY += 17f;
			TaggedString taggedString = (category == MemeCategory.Structure) ? "ChooseStructureMemesInfo".Translate() : "ChooseNormalMemesInfo".Translate(IdeoFoundation.MemeCountRangeAbsolute.min, IdeoFoundation.MemeCountRangeAbsolute.max);
			Widgets.Label(viewRect.x, ref curY, viewRect.width, taggedString, default(TipSignal));
			curY += 27f;
			Dialog_ChooseMemes.memesInCategory.Clear();
			foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefsListForReading)
			{
				if (memeDef.category == category && this.CanUseMeme(memeDef))
				{
					Dialog_ChooseMemes.memesInCategory.Add(memeDef);
				}
			}
			if (category == MemeCategory.Structure)
			{
				this.DoStructureMemeSelector(viewRect, ref curY, Dialog_ChooseMemes.memesInCategory);
				return;
			}
			this.DoNormalMemeSelector(viewRect, ref curY, Dialog_ChooseMemes.memesInCategory);
		}

		// Token: 0x060073AB RID: 29611 RVA: 0x0026EAD8 File Offset: 0x0026CCD8
		private void DoStructureMemeSelector(Rect viewRect, ref float curY, List<MemeDef> memes)
		{
			memes.SortBy((MemeDef x) => x.groupDef != null, (MemeDef x) => x.renderOrder);
			float num = curY;
			int num2 = Mathf.FloorToInt(viewRect.width / (IdeoUIUtility.MemeBoxSize.x + 8f));
			float num3 = 8f;
			int num4 = memes.Count((MemeDef x) => x.groupDef == null);
			if (num4 < num2)
			{
				num3 = (viewRect.width - IdeoUIUtility.MemeBoxSize.x * (float)(num4 + 1)) / (float)num4;
			}
			Dialog_ChooseMemes.memeGroups.Clear();
			for (int i = 0; i < memes.Count; i++)
			{
				MemeDef memeDef = memes[i];
				if (memeDef.groupDef != null)
				{
					if (Dialog_ChooseMemes.memeGroups.ContainsKey(memeDef.groupDef))
					{
						Dialog_ChooseMemes.memeGroups[memeDef.groupDef].Add(memeDef);
					}
					else
					{
						Dialog_ChooseMemes.memeGroups.Add(memeDef.groupDef, new List<MemeDef>
						{
							memeDef
						});
					}
				}
				else
				{
					int num5 = i / num2;
					int num6 = i % num2;
					int num7 = (i >= num4 - num4 % num2) ? (num4 % num2) : num2;
					float num8 = (viewRect.width - (float)num7 * IdeoUIUtility.MemeBoxSize.x - (float)(num7 - 1) * num3) / 2f;
					Rect memeBox = new Rect(num8 + (float)num6 * IdeoUIUtility.MemeBoxSize.x + (float)num6 * num3, curY + (float)num5 * IdeoUIUtility.MemeBoxSize.y + (float)num5 * num3, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y);
					this.DrawMeme(memeDef, memeBox, true);
					num = Mathf.Max(num, memeBox.yMax);
				}
			}
			if (Dialog_ChooseMemes.memeGroups.Any<KeyValuePair<MemeGroupDef, List<MemeDef>>>())
			{
				if (num4 > 0)
				{
					num += num3;
				}
				float num9 = num;
				foreach (KeyValuePair<MemeGroupDef, List<MemeDef>> keyValuePair in Dialog_ChooseMemes.memeGroups)
				{
					num = Mathf.Max(num, this.DrawMemeGroup(keyValuePair.Value, new Vector2(viewRect.x + viewRect.width * keyValuePair.Key.drawOffset.x, viewRect.y + num9 + viewRect.height * keyValuePair.Key.drawOffset.y), keyValuePair.Key.maxRows).yMax);
				}
			}
			Dialog_ChooseMemes.memeGroups.Clear();
			GUI.color = Color.white;
			curY = num;
		}

		// Token: 0x060073AC RID: 29612 RVA: 0x0026EDAC File Offset: 0x0026CFAC
		private void DoNormalMemeSelector(Rect viewRect, ref float curY, List<MemeDef> memes)
		{
			Dialog_ChooseMemes.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.viewRect = viewRect;
			CS$<>8__locals1.<>4__this = this;
			memes.Sort(Dialog_ChooseMemes.NormalMemeSorter);
			CS$<>8__locals1.gapBetweenBoxes = 8f;
			int num = Mathf.FloorToInt(CS$<>8__locals1.viewRect.width / (IdeoUIUtility.MemeBoxSize.x + CS$<>8__locals1.gapBetweenBoxes));
			int impact;
			Func<MemeDef, bool> <>9__1;
			int impact2;
			for (impact = 3; impact > 0; impact = impact2 - 1)
			{
				int num2 = 0;
				using (List<MemeDef>.Enumerator enumerator = memes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.impact == impact)
						{
							num2++;
						}
					}
				}
				int num3 = (num2 <= num) ? num : Mathf.CeilToInt((float)num2 / 2f);
				Rect rect = new Rect(CS$<>8__locals1.viewRect.x, curY, CS$<>8__locals1.viewRect.width, 30f);
				Widgets.Label(rect, "IdeoImpact".Translate() + ": " + IdeoImpactUtility.MemeImpactLabel(impact).CapitalizeFirst());
				curY = rect.yMax;
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(rect.x, curY - 7f, rect.width);
				GUI.color = Color.white;
				Dialog_ChooseMemes.memesCurrentRow.Clear();
				Func<MemeDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MemeDef x) => x.impact == impact));
				}
				foreach (IGrouping<MemeGroupDef, MemeDef> collection in from x in memes.Where(predicate)
				group x by x.groupDef)
				{
					Dialog_ChooseMemes.memesInGroup.Clear();
					Dialog_ChooseMemes.memesInGroup.AddRange(collection);
					if (Dialog_ChooseMemes.memesCurrentRow.Count + Dialog_ChooseMemes.memesInGroup.Count > num3)
					{
						this.<DoNormalMemeSelector>g__DrawRow|26_0(ref curY, ref CS$<>8__locals1);
					}
					foreach (MemeDef item in Dialog_ChooseMemes.memesInGroup)
					{
						if (Dialog_ChooseMemes.memesCurrentRow.Count >= num3)
						{
							this.<DoNormalMemeSelector>g__DrawRow|26_0(ref curY, ref CS$<>8__locals1);
						}
						Dialog_ChooseMemes.memesCurrentRow.Add(item);
					}
				}
				this.<DoNormalMemeSelector>g__DrawRow|26_0(ref curY, ref CS$<>8__locals1);
				impact2 = impact;
			}
		}

		// Token: 0x060073AD RID: 29613 RVA: 0x0026F058 File Offset: 0x0026D258
		private Rect DrawMemeGroup(List<MemeDef> memes, Vector2 offset, int maxRows)
		{
			int num = Mathf.CeilToInt((float)memes.Count / (float)maxRows);
			int num2 = Mathf.CeilToInt((float)memes.Count / (float)num);
			Rect rect = new Rect(offset.x, offset.y, (float)num * IdeoUIUtility.MemeBoxSize.x, (float)num2 * IdeoUIUtility.MemeBoxSize.y);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawHighlight(rect);
			GUI.color = Color.white;
			bool flag = memes.Count % 2 != 0 && maxRows > 1;
			for (int i = 0; i < memes.Count; i++)
			{
				int num3 = i;
				if (flag && (float)i / (float)num > 0f)
				{
					num3++;
				}
				float num4 = offset.x + (float)(num3 % num) * IdeoUIUtility.MemeBoxSize.x;
				if (num3 / num == 0 && flag)
				{
					num4 += IdeoUIUtility.MemeBoxSize.x / 2f;
				}
				this.DrawMeme(memes[i], new Rect(num4, offset.y + (float)(num3 / num) * IdeoUIUtility.MemeBoxSize.y, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y), false);
			}
			return rect;
		}

		// Token: 0x060073AE RID: 29614 RVA: 0x0026F1A4 File Offset: 0x0026D3A4
		private void DrawMeme(MemeDef meme, Rect memeBox, bool drawHighlight = true)
		{
			if (this.newMemes.Contains(meme))
			{
				if (!this.CanRemoveMeme(meme).Accepted)
				{
					GUI.color = Color.red;
				}
				Widgets.DrawBox(memeBox, 1, null);
				GUI.color = Color.white;
			}
			IdeoUIUtility.DoMeme(memeBox, meme, null, IdeoEditMode.None, drawHighlight);
			if (Widgets.ButtonInvisible(memeBox, true))
			{
				if (this.newMemes.Contains(meme))
				{
					if (meme.category != MemeCategory.Structure)
					{
						AcceptanceReport acceptanceReport = this.CanRemoveMeme(meme);
						if (acceptanceReport.Accepted)
						{
							this.newMemes.Remove(meme);
							SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
							return;
						}
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
						return;
					}
				}
				else
				{
					if (meme.category == MemeCategory.Structure)
					{
						for (int i = this.newMemes.Count - 1; i >= 0; i--)
						{
							if (this.newMemes[i].category == MemeCategory.Structure)
							{
								this.newMemes.RemoveAt(i);
							}
						}
					}
					this.newMemes.Add(meme);
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060073AF RID: 29615 RVA: 0x0026F2B4 File Offset: 0x0026D4B4
		private bool CanUseMeme(MemeDef meme)
		{
			if (IdeoUIUtility.devEditMode)
			{
				return true;
			}
			if (Current.Game.World == null)
			{
				return IdeoUtility.IsMemeAllowedFor(meme, Find.Scenario.playerFaction.factionDef);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.def.hidden && !faction.def.isPlayer && faction.ideos != null && (faction.ideos.IsPrimary(this.ideo) || faction.ideos.IsMinor(this.ideo)) && !IdeoUtility.IsMemeAllowedFor(meme, faction.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x0026F388 File Offset: 0x0026D588
		private AcceptanceReport CanRemoveMeme(MemeDef meme)
		{
			if (IdeoUIUtility.devEditMode)
			{
				return true;
			}
			if (Current.Game.World == null)
			{
				FactionDef factionDef = Find.Scenario.playerFaction.factionDef;
				if (factionDef.requiredMemes != null && factionDef.requiredMemes.Contains(meme))
				{
					return "CannotRemoveMemeRequiredPlayer".Translate(meme.label.Named("MEME"));
				}
			}
			else
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (!faction.def.hidden && !faction.def.isPlayer && faction.ideos != null && (faction.ideos.IsPrimary(this.ideo) || faction.ideos.IsMinor(this.ideo)) && faction.def.requiredMemes != null && faction.def.requiredMemes.Contains(meme))
					{
						return "CannotRemoveMemeRequired".Translate(meme.label.Named("MEME")) + ": " + "RequiredByFaction".Translate(faction.Named("FACTION"));
					}
				}
			}
			return true;
		}

		// Token: 0x060073B1 RID: 29617 RVA: 0x0026F500 File Offset: 0x0026D700
		private int GetMemeCount(MemeCategory category)
		{
			int num = 0;
			for (int i = 0; i < this.newMemes.Count; i++)
			{
				if (this.newMemes[i].category == category)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060073B3 RID: 29619 RVA: 0x0026F5C0 File Offset: 0x0026D7C0
		[CompilerGenerated]
		private void <DoNormalMemeSelector>g__DrawRow|26_0(ref float y, ref Dialog_ChooseMemes.<>c__DisplayClass26_0 A_2)
		{
			if (Dialog_ChooseMemes.memesCurrentRow.Count == 0)
			{
				return;
			}
			float num = (A_2.viewRect.width - (float)Dialog_ChooseMemes.memesCurrentRow.Count * (IdeoUIUtility.MemeBoxSize.x + A_2.gapBetweenBoxes)) / 2f;
			MemeGroupDef memeGroupDef = null;
			for (int i = 0; i < Dialog_ChooseMemes.memesCurrentRow.Count; i++)
			{
				MemeDef memeDef = Dialog_ChooseMemes.memesCurrentRow[i];
				if (i == 0)
				{
					memeGroupDef = memeDef.groupDef;
				}
				else if (memeGroupDef != memeDef.groupDef || (memeDef.groupDef != null && memeDef.groupDef.renderWithGap))
				{
					memeGroupDef = memeDef.groupDef;
					num += A_2.gapBetweenBoxes;
				}
				Rect memeBox = new Rect(A_2.viewRect.x + num, y, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y).Rounded();
				this.DrawMeme(memeDef, memeBox, true);
				num = memeBox.xMax;
			}
			Dialog_ChooseMemes.memesCurrentRow.Clear();
			y += A_2.gapBetweenBoxes + IdeoUIUtility.MemeBoxSize.y;
		}

		// Token: 0x04003F5C RID: 16220
		private Ideo ideo;

		// Token: 0x04003F5D RID: 16221
		private Vector2 scrollPos;

		// Token: 0x04003F5E RID: 16222
		private float viewHeight;

		// Token: 0x04003F5F RID: 16223
		private List<MemeDef> newMemes = new List<MemeDef>();

		// Token: 0x04003F60 RID: 16224
		private bool initialSelection;

		// Token: 0x04003F61 RID: 16225
		private Action done;

		// Token: 0x04003F62 RID: 16226
		private MemeCategory memeCategory;

		// Token: 0x04003F63 RID: 16227
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04003F64 RID: 16228
		private const float Width = 980f;

		// Token: 0x04003F65 RID: 16229
		private const float Height = 724f;

		// Token: 0x04003F66 RID: 16230
		private static List<MemeDef> memesInCategory = new List<MemeDef>();

		// Token: 0x04003F67 RID: 16231
		private static Dictionary<MemeGroupDef, List<MemeDef>> memeGroups = new Dictionary<MemeGroupDef, List<MemeDef>>();

		// Token: 0x04003F68 RID: 16232
		private static readonly List<MemeDef> memesCurrentRow = new List<MemeDef>();

		// Token: 0x04003F69 RID: 16233
		private static readonly List<MemeDef> memesInGroup = new List<MemeDef>();

		// Token: 0x04003F6A RID: 16234
		private static readonly IComparer<MemeDef> NormalMemeSorter = GenCollection.CompareBy<MemeDef, int>(delegate(MemeDef x)
		{
			MemeGroupDef groupDef = x.groupDef;
			if (groupDef == null)
			{
				return int.MaxValue;
			}
			return groupDef.renderOrder;
		}).ThenBy(GenCollection.CompareBy<MemeDef, int>((MemeDef x) => x.renderOrder));
	}
}
