using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DE RID: 4830
	public class Dialog_ChooseColonistsForNewColony : Window
	{
		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06007390 RID: 29584 RVA: 0x0026CE50 File Offset: 0x0026B050
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight - 17f);
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06007391 RID: 29585 RVA: 0x0026D0D8 File Offset: 0x0026B2D8
		public int ColonistCount
		{
			get
			{
				int num = 0;
				foreach (TransferableOneWay transferableOneWay in this.transferables)
				{
					Pawn pawn;
					if ((pawn = (transferableOneWay.AnyThing as Pawn)) != null && pawn.RaceProps.Humanlike && transferableOneWay.CountToTransfer > 0)
					{
						num += transferableOneWay.CountToTransfer;
					}
				}
				return num;
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06007392 RID: 29586 RVA: 0x0026D158 File Offset: 0x0026B358
		public int AnimalCount
		{
			get
			{
				int num = 0;
				foreach (TransferableOneWay transferableOneWay in this.transferables)
				{
					Pawn pawn;
					if ((pawn = (transferableOneWay.AnyThing as Pawn)) != null && pawn.RaceProps.Animal && transferableOneWay.CountToTransfer > 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06007393 RID: 29587 RVA: 0x0026D1D0 File Offset: 0x0026B3D0
		public int RelicCount
		{
			get
			{
				int num = 0;
				foreach (TransferableOneWay transferableOneWay in this.transferables)
				{
					if (transferableOneWay.AnyThing.StyleSourcePrecept is Precept_Relic)
					{
						num += transferableOneWay.CountToTransfer;
					}
				}
				return num;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06007394 RID: 29588 RVA: 0x0026D23C File Offset: 0x0026B43C
		private AcceptanceReport AcceptanceReport
		{
			get
			{
				int colonistCount = this.ColonistCount;
				if (colonistCount > this.maxColonists)
				{
					return "MessageNewColonyMax".Translate(this.maxColonists, Faction.OfPlayer.def.pawnsPlural);
				}
				if (colonistCount == 0)
				{
					return "MessageNewColoyRequiresOneColonist".Translate();
				}
				if (this.AnimalCount > this.maxAnimals)
				{
					return "MessageNewColonyMax".Translate(this.maxAnimals, "AnimalsLower".Translate());
				}
				if (this.RelicCount > this.maxRelics)
				{
					return "MessageNewColonyMax".Translate(this.maxRelics, (this.maxRelics == 1) ? "RelicLower".Translate() : "RelicsLower".Translate());
				}
				return AcceptanceReport.WasAccepted;
			}
		}

		// Token: 0x06007395 RID: 29589 RVA: 0x0026D328 File Offset: 0x0026B528
		public Dialog_ChooseColonistsForNewColony(Action<List<Thing>> postAccepted, int maxColonists = 5, int maxAnimals = 5, int maxRelics = 1)
		{
			if (!ModLister.CheckIdeology("Choose new colony"))
			{
				return;
			}
			this.postAccepted = postAccepted;
			this.transferableWidget = new TransferableOneWayWidget(null, null, null, null, false, IgnorePawnsInventoryMode.DontIgnore, false, null, 0f, false, -1, false, false, false, false, false, false, false, false);
			this.maxAnimals = maxAnimals;
			this.maxColonists = maxColonists;
			this.maxRelics = maxRelics;
			this.forcePause = true;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.openMenuOnCancel = true;
			this.preventSave = true;
		}

		// Token: 0x06007396 RID: 29590 RVA: 0x0026D3D4 File Offset: 0x0026B5D4
		public override void PostOpen()
		{
			foreach (Pawn thing in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
			{
				this.AddTransferable(thing);
			}
			using (List<Precept>.Enumerator enumerator2 = Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Precept_Relic precept_Relic;
					if ((precept_Relic = (enumerator2.Current as Precept_Relic)) != null && precept_Relic.RelicInPlayerPossession)
					{
						this.AddTransferable(precept_Relic.GeneratedRelic);
					}
				}
			}
			this.transferableWidget.AddSection("ColonistsSection".Translate(), this.transferables.Where(delegate(TransferableOneWay t)
			{
				Pawn pawn;
				return (pawn = (t.AnyThing as Pawn)) != null && pawn.IsColonist && !pawn.IsQuestLodger();
			}));
			this.transferableWidget.AddSection("AnimalsSection".Translate(), this.transferables.Where(delegate(TransferableOneWay t)
			{
				Pawn pawn;
				return (pawn = (t.AnyThing as Pawn)) != null && pawn.RaceProps.Animal;
			}));
			this.transferableWidget.AddSection("RelicsSection".Translate(), from t in this.transferables
			where t.AnyThing.StyleSourcePrecept is Precept_Relic
			select t);
		}

		// Token: 0x06007397 RID: 29591 RVA: 0x0026D558 File Offset: 0x0026B758
		private void AddTransferable(Thing thing)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(thing, this.transferables, TransferAsOneMode.Normal);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(thing);
		}

		// Token: 0x06007398 RID: 29592 RVA: 0x0026D594 File Offset: 0x0026B794
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 50f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "ChooseColonistsForNewColony".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			inRect.yMin += 82f;
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			Rect inRect2 = rect2;
			inRect2.yMax -= 21f;
			bool flag = false;
			this.transferableWidget.OnGUI(inRect2, out flag);
			AcceptanceReport acceptanceReport = this.AcceptanceReport;
			Rect rect3 = new Rect(rect2.width / 2f - this.BottomButtonSize.x / 2f, rect2.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (!acceptanceReport.Accepted)
			{
				Rect rect4 = rect3;
				rect4.x -= rect3.width + 2f;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleRight;
				GUI.color = Color.red;
				Widgets.Label(rect4, acceptanceReport.Reason);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			if (Widgets.ButtonText(rect3, "AcceptButton".Translate(), true, true, true))
			{
				if (acceptanceReport.Accepted)
				{
					this.Close(true);
					List<Thing> things = new List<Thing>();
					Action<Thing, int> <>9__0;
					foreach (TransferableOneWay transferableOneWay in this.transferables)
					{
						List<Thing> things2 = transferableOneWay.things;
						int countToTransfer = transferableOneWay.CountToTransfer;
						Action<Thing, int> transfer;
						if ((transfer = <>9__0) == null)
						{
							transfer = (<>9__0 = delegate(Thing thing, int count)
							{
								things.Add(thing);
							});
						}
						TransferableUtility.TransferNoSplit(things2, countToTransfer, transfer, true, true);
					}
					this.postAccepted(things);
				}
				else
				{
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x04003F40 RID: 16192
		private TransferableOneWayWidget transferableWidget;

		// Token: 0x04003F41 RID: 16193
		private List<TransferableOneWay> transferables = new List<TransferableOneWay>();

		// Token: 0x04003F42 RID: 16194
		private int maxColonists;

		// Token: 0x04003F43 RID: 16195
		private int maxAnimals;

		// Token: 0x04003F44 RID: 16196
		private int maxRelics;

		// Token: 0x04003F45 RID: 16197
		private Action<List<Thing>> postAccepted;

		// Token: 0x04003F46 RID: 16198
		private const float TitleRectHeight = 50f;

		// Token: 0x04003F47 RID: 16199
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04003F48 RID: 16200
		private const float BottomAreaHeight = 55f;
	}
}
