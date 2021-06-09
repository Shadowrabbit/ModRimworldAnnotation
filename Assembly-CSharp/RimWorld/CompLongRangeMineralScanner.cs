using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AB RID: 6315
	public class CompLongRangeMineralScanner : CompScanner
	{
		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06008C25 RID: 35877 RVA: 0x0005DFA8 File Offset: 0x0005C1A8
		public new CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return this.props as CompProperties_LongRangeMineralScanner;
			}
		}

		// Token: 0x06008C26 RID: 35878 RVA: 0x0005DFB5 File Offset: 0x0005C1B5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.targetMineable, "targetMineable");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetMineable == null)
			{
				this.SetDefaultTargetMineral();
			}
		}

		// Token: 0x06008C27 RID: 35879 RVA: 0x0005DFE3 File Offset: 0x0005C1E3
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetDefaultTargetMineral();
		}

		// Token: 0x06008C28 RID: 35880 RVA: 0x0005DFF2 File Offset: 0x0005C1F2
		private void SetDefaultTargetMineral()
		{
			this.targetMineable = ThingDefOf.MineableGold;
		}

		// Token: 0x06008C29 RID: 35881 RVA: 0x0028C070 File Offset: 0x0028A270
		protected override void DoFind(Pawn worker)
		{
			Slate slate = new Slate();
			slate.Set<Map>("map", this.parent.Map, false);
			slate.Set<ThingDef>("targetMineable", this.targetMineable, false);
			slate.Set<ThingDef>("targetMineableThing", this.targetMineable.building.mineableThing, false);
			slate.Set<Pawn>("worker", worker, false);
			if (!QuestScriptDefOf.LongRangeMineralScannerLump.CanRun(slate))
			{
				return;
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.LongRangeMineralScannerLump, slate);
			Find.LetterStack.ReceiveLetter(quest.name, quest.description, LetterDefOf.PositiveEvent, null, null, quest, null, null);
		}

		// Token: 0x06008C2A RID: 35882 RVA: 0x0005DFFF File Offset: 0x0005C1FF
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction == Faction.OfPlayer)
			{
				ThingDef mineableThing = this.targetMineable.building.mineableThing;
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandSelectMineralToScanFor".Translate() + ": " + mineableThing.LabelCap;
				command_Action.icon = mineableThing.uiIcon;
				command_Action.iconAngle = mineableThing.uiIconAngle;
				command_Action.iconOffset = mineableThing.uiIconOffset;
				command_Action.action = delegate()
				{
					List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (ThingDef localD2 in mineables)
					{
						ThingDef localD = localD2;
						FloatMenuOption item = new FloatMenuOption(localD.building.mineableThing.LabelCap, delegate()
						{
							foreach (object obj in Find.Selector.SelectedObjects)
							{
								Thing thing = obj as Thing;
								if (thing != null)
								{
									CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
									if (compLongRangeMineralScanner != null)
									{
										compLongRangeMineralScanner.targetMineable = localD;
									}
								}
							}
						}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null);
						list.Add(item);
					}
					Find.WindowStack.Add(new FloatMenu(list));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x040059C0 RID: 22976
		private ThingDef targetMineable;
	}
}
