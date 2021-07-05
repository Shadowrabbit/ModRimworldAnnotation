using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001A9C RID: 6812
	public class Page_ScenarioEditor : Page
	{
		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x06009677 RID: 38519 RVA: 0x000646F9 File Offset: 0x000628F9
		public override string PageTitle
		{
			get
			{
				return "ScenarioEditor".Translate();
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06009678 RID: 38520 RVA: 0x0006470A File Offset: 0x0006290A
		public Scenario EditingScenario
		{
			get
			{
				return this.curScen;
			}
		}

		// Token: 0x06009679 RID: 38521 RVA: 0x00064712 File Offset: 0x00062912
		public Page_ScenarioEditor(Scenario scen)
		{
			if (scen != null)
			{
				this.curScen = scen;
				this.seedIsValid = false;
				return;
			}
			this.RandomizeSeedAndScenario();
		}

		// Token: 0x0600967A RID: 38522 RVA: 0x00064744 File Offset: 0x00062944
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
		}

		// Token: 0x0600967B RID: 38523 RVA: 0x002BCBC4 File Offset: 0x002BADC4
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			GUI.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoConfigControls(rect2);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded();
			if (!this.editMode)
			{
				ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			}
			else
			{
				ScenarioUI.DrawScenarioEditInterface(rect3, this.curScen, ref this.infoScrollPosition);
			}
			GUI.EndGroup();
			base.DoBottomButtons(rect, null, null, null, true, true);
		}

		// Token: 0x0600967C RID: 38524 RVA: 0x00064757 File Offset: 0x00062957
		private void RandomizeSeedAndScenario()
		{
			this.seed = GenText.RandomSeedString();
			this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
		}

		// Token: 0x0600967D RID: 38525 RVA: 0x002BCC94 File Offset: 0x002BAE94
		private void DoConfigControls(Rect rect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(rect);
			if (listing_Standard.ButtonText("Load".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Load(delegate(Scenario loadedScen)
				{
					this.curScen = loadedScen;
					this.seedIsValid = false;
				}));
			}
			if (listing_Standard.ButtonText("Save".Translate(), null) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen));
			}
			if (listing_Standard.ButtonText("RandomizeSeed".Translate(), null))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.RandomizeSeedAndScenario();
				this.seedIsValid = true;
			}
			if (this.seedIsValid)
			{
				listing_Standard.Label("Seed".Translate().CapitalizeFirst(), -1f, null);
				string a = listing_Standard.TextEntry(this.seed, 1);
				if (a != this.seed)
				{
					this.seed = a;
					this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
				}
			}
			else
			{
				listing_Standard.Gap(Text.LineHeight + Text.LineHeight + 2f);
			}
			listing_Standard.CheckboxLabeled("EditMode".Translate().CapitalizeFirst(), ref this.editMode, null);
			if (this.editMode)
			{
				this.seedIsValid = false;
				if (listing_Standard.ButtonText("AddPart".Translate(), null))
				{
					this.OpenAddScenPartMenu();
				}
				if (SteamManager.Initialized && (this.curScen.Category == ScenarioCategory.CustomLocal || this.curScen.Category == ScenarioCategory.SteamWorkshop) && listing_Standard.ButtonText(Workshop.UploadButtonLabel(this.curScen.GetPublishedFileId()), null) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
				{
					AcceptanceReport acceptanceReport = this.curScen.TryUploadReport();
					if (!acceptanceReport.Accepted)
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								Workshop.Upload(this.curScen);
							}, true, null));
						}, true, null));
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x0600967E RID: 38526 RVA: 0x002BCED0 File Offset: 0x002BB0D0
		private static bool CheckAllPartsCompatible(Scenario scen)
		{
			foreach (ScenPart scenPart in scen.AllParts)
			{
				int num = 0;
				foreach (ScenPart scenPart2 in scen.AllParts)
				{
					if (scenPart2.def == scenPart.def)
					{
						num++;
					}
					if (num > scenPart.def.maxUses)
					{
						Messages.Message("TooMany".Translate(scenPart.def.maxUses) + ": " + scenPart.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
					if (scenPart != scenPart2 && !scenPart.CanCoexistWith(scenPart2))
					{
						Messages.Message("Incompatible".Translate() + ": " + scenPart.def.label + ", " + scenPart2.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600967F RID: 38527 RVA: 0x002BD048 File Offset: 0x002BB248
		private void OpenAddScenPartMenu()
		{
			FloatMenuUtility.MakeMenu<ScenPartDef>(from p in ScenarioMaker.AddableParts(this.curScen)
			where p.category != ScenPartCategory.Fixed
			orderby p.label
			select p, (ScenPartDef p) => p.LabelCap, (ScenPartDef p) => delegate()
			{
				this.AddScenPart(p);
			});
		}

		// Token: 0x06009680 RID: 38528 RVA: 0x002BD0D8 File Offset: 0x002BB2D8
		private void AddScenPart(ScenPartDef def)
		{
			ScenPart scenPart = ScenarioMaker.MakeScenPart(def);
			scenPart.Randomize();
			this.curScen.parts.Add(scenPart);
		}

		// Token: 0x06009681 RID: 38529 RVA: 0x00064775 File Offset: 0x00062975
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			if (!Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				return false;
			}
			Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
			return true;
		}

		// Token: 0x04006002 RID: 24578
		private Scenario curScen;

		// Token: 0x04006003 RID: 24579
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x04006004 RID: 24580
		private string seed;

		// Token: 0x04006005 RID: 24581
		private bool seedIsValid = true;

		// Token: 0x04006006 RID: 24582
		private bool editMode;
	}
}
