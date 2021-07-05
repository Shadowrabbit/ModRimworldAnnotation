using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000B88 RID: 2952
	public class QuestPart_NewColony : QuestPart
	{
		// Token: 0x06004509 RID: 17673 RVA: 0x0016DF40 File Offset: 0x0016C140
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				Find.WindowStack.Add(new Dialog_ChooseColonistsForNewColony(new Action<List<Thing>>(this.PostThingsSelected), 5, 5, this.maxRelics));
			}
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x0016DF98 File Offset: 0x0016C198
		private void PostThingsSelected(List<Thing> allThings)
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(allThings.First<Thing>()));
			MoveColonyUtility.PickNewColonyTile(delegate(int choseTile)
			{
				this.TileChosen(choseTile, allThings);
			}, delegate
			{
				if (!this.outSignalCancelled.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalCancelled));
				}
			});
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x0016DFF0 File Offset: 0x0016C1F0
		private void TileChosen(int chosenTile, List<Thing> allThings)
		{
			if (ModsConfig.IdeologyActive)
			{
				this.tmpPawns.Clear();
				for (int i = 0; i < allThings.Count; i++)
				{
					Pawn item;
					if ((item = (allThings[i] as Pawn)) != null)
					{
						this.tmpPawns.Add(item);
					}
				}
				Find.WindowStack.Add(new Dialog_ConfigureIdeo(Faction.OfPlayer.ideos.PrimaryIdeo, this.tmpPawns, delegate()
				{
					this.InitMoveColony(allThings, chosenTile);
				}));
				this.tmpPawns.Clear();
				return;
			}
			this.InitMoveColony(allThings, chosenTile);
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x0016E0AF File Offset: 0x0016C2AF
		private void InitMoveColony(List<Thing> things, int choseTile)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				List<Thing> list = new List<Thing>();
				list.AddRange(MoveColonyUtility.GetStartingThingsForNewColony());
				list.AddRange(things);
				Settlement settlement = MoveColonyUtility.MoveColonyAndReset(choseTile, list, this.otherFaction, this.worldObjectDef);
				CameraJumper.TryJump(MapGenerator.PlayerStartSpot, settlement.Map);
				if (this.colonyStartSound != null)
				{
					this.colonyStartSound.PlayOneShotOnCamera(null);
				}
				if (!this.outSignalCompleted.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalCompleted));
				}
			}, "GeneratingMap", false, null, true);
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x0016E0E3 File Offset: 0x0016C2E3
		public override void Notify_FactionRemoved(Faction f)
		{
			base.Notify_FactionRemoved(f);
			if (this.otherFaction == f)
			{
				this.otherFaction = null;
			}
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x0016E0FC File Offset: 0x0016C2FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.otherFaction, "otherFaction", false);
			Scribe_Values.Look<string>(ref this.outSignalCompleted, "outSignalCompleted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalCancelled, "outSignalCancelled", null, false);
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjecctDef");
			Scribe_Defs.Look<SoundDef>(ref this.colonyStartSound, "colonyStartSound");
			Scribe_Values.Look<int>(ref this.maxRelics, "maxRelics", 0, false);
		}

		// Token: 0x040029EE RID: 10734
		public string inSignal;

		// Token: 0x040029EF RID: 10735
		public Faction otherFaction;

		// Token: 0x040029F0 RID: 10736
		public string outSignalCompleted;

		// Token: 0x040029F1 RID: 10737
		public string outSignalCancelled;

		// Token: 0x040029F2 RID: 10738
		public WorldObjectDef worldObjectDef;

		// Token: 0x040029F3 RID: 10739
		public SoundDef colonyStartSound;

		// Token: 0x040029F4 RID: 10740
		public int maxRelics = 1;

		// Token: 0x040029F5 RID: 10741
		private List<Pawn> tmpPawns = new List<Pawn>();
	}
}
