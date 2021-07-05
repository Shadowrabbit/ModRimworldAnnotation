using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200131F RID: 4895
	public class Page_ConfigureIdeo : Page
	{
		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x06007629 RID: 30249 RVA: 0x0028D67A File Offset: 0x0028B87A
		public override string PageTitle
		{
			get
			{
				return "ChooseIdeoligion".Translate();
			}
		}

		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x0600762A RID: 30250 RVA: 0x0028D68C File Offset: 0x0028B88C
		private bool PlayerHasCreatedIdeo
		{
			get
			{
				if (Faction.OfPlayer.ideos.PrimaryIdeo == null)
				{
					return false;
				}
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (!faction.IsPlayer && faction.ideos != null && faction.ideos.Has(Faction.OfPlayer.ideos.PrimaryIdeo))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600762B RID: 30251 RVA: 0x0028D71C File Offset: 0x0028B91C
		public Page_ConfigureIdeo()
		{
			this.grayOutIfOtherDialogOpen = true;
		}

		// Token: 0x0600762C RID: 30252 RVA: 0x0028D72C File Offset: 0x0028B92C
		public override void PostOpen()
		{
			base.PostOpen();
			Find.IdeoManager.RemoveUnusedStartingIdeos();
			if (IdeoUIUtility.selected != null && Find.IdeoManager.IdeosListForReading.Contains(IdeoUIUtility.selected))
			{
				this.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
			}
			else
			{
				this.ideo = null;
				IdeoUIUtility.UnselectCurrent();
			}
			if (Find.Storyteller.def.tutorialMode)
			{
				this.ideo = IdeoGenerator.GenerateTutorialIdeo();
				Find.IdeoManager.Add(this.ideo);
				Faction.OfPlayer.ideos.SetPrimary(this.ideo);
			}
			TutorSystem.Notify_Event("PageStart-ConfigureIdeo");
		}

		// Token: 0x0600762D RID: 30253 RVA: 0x0028D7D0 File Offset: 0x0028B9D0
		public void SelectOrMakeNewIdeo(Ideo newIdeo = null)
		{
			this.ideo = (newIdeo ?? IdeoUtility.MakeEmptyIdeo());
			if (!Find.IdeoManager.IdeosListForReading.Contains(this.ideo))
			{
				Find.IdeoManager.Add(this.ideo);
				Faction.OfPlayer.ideos.SetPrimary(this.ideo);
			}
			if (!this.ideo.memes.Any<MemeDef>())
			{
				Find.WindowStack.Add(new Dialog_ChooseMemes(this.ideo, MemeCategory.Structure, true, null));
			}
			IdeoUIUtility.SetSelected(this.ideo);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EditingMemes, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EditingPrecepts, OpportunityType.Critical);
		}

		// Token: 0x0600762E RID: 30254 RVA: 0x0028D878 File Offset: 0x0028BA78
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			IdeoUIUtility.DoIdeoListAndDetails(base.GetMainRect(rect, 0f, false), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, true, !this.PlayerHasCreatedIdeo, null, null);
			if (this.ideo != null)
			{
				string text = null;
				Pair<Precept, Precept> lhs = this.ideo.FirstIncompatiblePreceptPair();
				if (lhs != default(Pair<Precept, Precept>))
				{
					text = "MessageIdeoIncompatiblePrecepts".Translate(lhs.First.TipLabel.Named("PRECEPT1"), lhs.Second.TipLabel.Named("PRECEPT2")).CapitalizeFirst();
				}
				else
				{
					Tuple<Precept_Ritual, List<string>> tuple = this.ideo.FirstRitualMissingTarget();
					Precept_Building precept_Building = this.ideo.FirstConsumableBuildingMissingRitual();
					if (tuple != null)
					{
						text = "MessageRitualMissingTarget".Translate(tuple.Item1.LabelCap.Named("PRECEPT")) + ": " + tuple.Item2.ToCommaList(false, false).CapitalizeFirst() + ".";
					}
					else if (precept_Building != null)
					{
						text = "MessageBuildingMissingRitual".Translate(precept_Building.LabelCap.Named("PRECEPT"));
					}
				}
				Rect rect2 = rect;
				rect2.xMin = rect2.xMax - Page.BottomButSize.x * 2.75f;
				rect2.width = Page.BottomButSize.x * 1.7f;
				rect2.yMin = rect2.yMax - Page.BottomButSize.y;
				if (text != null)
				{
					GUI.color = Color.red;
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect2, text);
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				else
				{
					IdeoUIUtility.DrawImpactInfo(rect2, this.ideo.memes);
				}
			}
			if (this.ideo != null)
			{
				base.DoBottomButtons(rect, null, "RandomizeAll".Translate(), new Action(this.Randomize), true, true);
				return;
			}
			base.DoBottomButtons(rect, null, null, null, true, true);
		}

		// Token: 0x0600762F RID: 30255 RVA: 0x0028DAA4 File Offset: 0x0028BCA4
		public void Notify_ClosedChooseMemesDialog()
		{
			if (this.ideo != null)
			{
				if (!this.ideo.memes.Any((MemeDef x) => x.category == MemeCategory.Normal))
				{
					Faction.OfPlayer.ideos.SetPrimary(null);
					Find.IdeoManager.Remove(this.ideo);
				}
			}
		}

		// Token: 0x06007630 RID: 30256 RVA: 0x0028DB0C File Offset: 0x0028BD0C
		private void Randomize()
		{
			if (this.ideo != null && TutorSystem.AllowAction("ConfiguringIdeo"))
			{
				this.ideo.foundation.Init(new IdeoGenerationParms(IdeoUIUtility.FactionForRandomization(this.ideo), false, null, null));
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06007631 RID: 30257 RVA: 0x0028DB60 File Offset: 0x0028BD60
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.ideo == null)
			{
				Messages.Message("MessageMustChooseIdeo".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.ideo.name.NullOrEmpty())
			{
				Messages.Message("MessageIdeoNameCantBeEmpty".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pair<Precept, Precept> lhs = this.ideo.FirstIncompatiblePreceptPair();
			if (lhs != default(Pair<Precept, Precept>))
			{
				Messages.Message("MessageIdeoIncompatiblePrecepts".Translate(lhs.First.Label.Named("PRECEPT1"), lhs.Second.Label.Named("PRECEPT2")).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Tuple<Precept_Ritual, List<string>> tuple = this.ideo.FirstRitualMissingTarget();
			if (tuple != null)
			{
				Messages.Message("MessageRitualMissingTarget".Translate(tuple.Item1.LabelCap.Named("PRECEPT")) + ": " + tuple.Item2.ToCommaList(false, false).CapitalizeFirst() + ".", MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Precept_Building precept_Building = this.ideo.FirstConsumableBuildingMissingRitual();
			if (precept_Building != null)
			{
				Messages.Message("MessageBuildingMissingRitual".Translate(precept_Building.LabelCap.Named("PRECEPT")), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Faction.OfPlayer.ideos.SetPrimary(this.ideo);
			Find.Scenario.PostIdeoChosen();
			return true;
		}

		// Token: 0x04004188 RID: 16776
		public Ideo ideo;

		// Token: 0x04004189 RID: 16777
		private Vector2 scrollPosition_ideoList;

		// Token: 0x0400418A RID: 16778
		private float scrollViewHeight_ideoList;

		// Token: 0x0400418B RID: 16779
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x0400418C RID: 16780
		private float scrollViewHeight_ideoDetails;
	}
}
