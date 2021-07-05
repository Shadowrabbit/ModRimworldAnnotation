using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001138 RID: 4408
	public class CompForbiddable : ThingComp
	{
		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x060069E0 RID: 27104 RVA: 0x0023A929 File Offset: 0x00238B29
		public CompProperties_Forbiddable Props
		{
			get
			{
				return (CompProperties_Forbiddable)this.props;
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x060069E1 RID: 27105 RVA: 0x0023A936 File Offset: 0x00238B36
		// (set) Token: 0x060069E2 RID: 27106 RVA: 0x0023A940 File Offset: 0x00238B40
		public bool Forbidden
		{
			get
			{
				return this.forbiddenInt;
			}
			set
			{
				if (value == this.forbiddenInt)
				{
					return;
				}
				this.forbiddenInt = value;
				if (this.parent.Spawned)
				{
					if (this.forbiddenInt)
					{
						this.parent.Map.listerHaulables.Notify_Forbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Forbidden(this.parent);
					}
					else
					{
						this.parent.Map.listerHaulables.Notify_Unforbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Unforbidden(this.parent);
					}
					if (this.parent is Building_Door)
					{
						this.parent.Map.reachability.ClearCache();
					}
				}
				this.UpdateOverlayHandle();
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x060069E3 RID: 27107 RVA: 0x0023AA0C File Offset: 0x00238C0C
		private OverlayTypes MyOverlayType
		{
			get
			{
				if (this.parent is Blueprint || this.parent is Frame)
				{
					if (this.parent.def.size.x > 1 || this.parent.def.size.z > 1)
					{
						return OverlayTypes.ForbiddenBig;
					}
					return OverlayTypes.Forbidden;
				}
				else
				{
					if (this.parent.def.category == ThingCategory.Building)
					{
						return OverlayTypes.ForbiddenBig;
					}
					return OverlayTypes.Forbidden;
				}
			}
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x0023AA80 File Offset: 0x00238C80
		private void UpdateOverlayHandle()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayHandle);
			if (this.parent.Spawned && this.forbiddenInt)
			{
				this.overlayHandle = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, this.MyOverlayType));
			}
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x0023AAFD File Offset: 0x00238CFD
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.UpdateOverlayHandle();
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x0023AB05 File Offset: 0x00238D05
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x0023AB19 File Offset: 0x00238D19
		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x0023AB28 File Offset: 0x00238D28
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent is Building && !this.Props.allowNonPlayer && this.parent.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
			command_Toggle.icon = TexCommand.ForbidOff;
			command_Toggle.isActive = (() => !this.Forbidden);
			command_Toggle.defaultLabel = "CommandAllow".TranslateWithBackup("DesignatorUnforbid");
			command_Toggle.activateIfAmbiguous = false;
			if (this.forbiddenInt)
			{
				command_Toggle.defaultDesc = "CommandForbiddenDesc".TranslateWithBackup("DesignatorUnforbidDesc");
			}
			else
			{
				command_Toggle.defaultDesc = "CommandNotForbiddenDesc".TranslateWithBackup("DesignatorForbidDesc");
			}
			if (this.parent.def.IsDoor)
			{
				command_Toggle.tutorTag = "ToggleForbidden-Door";
				command_Toggle.toggleAction = delegate()
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ForbiddingDoors, KnowledgeAmount.SpecificInteraction);
				};
			}
			else
			{
				command_Toggle.tutorTag = "ToggleForbidden";
				command_Toggle.toggleAction = delegate()
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
				};
			}
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x04003B20 RID: 15136
		private bool forbiddenInt;

		// Token: 0x04003B21 RID: 15137
		private OverlayHandle? overlayHandle;
	}
}
