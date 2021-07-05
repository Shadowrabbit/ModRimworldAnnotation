using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D9 RID: 4569
	public class CompLongRangeMineralScanner : CompScanner
	{
		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06006E44 RID: 28228 RVA: 0x0024F6A7 File Offset: 0x0024D8A7
		public new CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return this.props as CompProperties_LongRangeMineralScanner;
			}
		}

		// Token: 0x06006E45 RID: 28229 RVA: 0x0024F6B4 File Offset: 0x0024D8B4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.targetMineable, "targetMineable");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetMineable == null)
			{
				this.SetDefaultTargetMineral();
			}
		}

		// Token: 0x06006E46 RID: 28230 RVA: 0x0024F6E2 File Offset: 0x0024D8E2
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetDefaultTargetMineral();
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x0024F6F1 File Offset: 0x0024D8F1
		private void SetDefaultTargetMineral()
		{
			this.targetMineable = ThingDefOf.MineableGold;
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x0024F700 File Offset: 0x0024D900
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

		// Token: 0x06006E49 RID: 28233 RVA: 0x0024F7A4 File Offset: 0x0024D9A4
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
						}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null, true, 0);
						list.Add(item);
					}
					Find.WindowStack.Add(new FloatMenu(list));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003D2E RID: 15662
		private ThingDef targetMineable;
	}
}
