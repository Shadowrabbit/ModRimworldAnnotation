using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C2 RID: 6082
	public class CompForbiddable : ThingComp
	{
		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x0600867E RID: 34430 RVA: 0x0005A3CA File Offset: 0x000585CA
		// (set) Token: 0x0600867F RID: 34431 RVA: 0x0027900C File Offset: 0x0027720C
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
			}
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x0005A3D2 File Offset: 0x000585D2
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x002790D4 File Offset: 0x002772D4
		public override void PostDraw()
		{
			if (this.forbiddenInt)
			{
				if (this.parent is Blueprint || this.parent is Frame)
				{
					if (this.parent.def.size.x > 1 || this.parent.def.size.z > 1)
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenBig);
						return;
					}
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.Forbidden);
					return;
				}
				else
				{
					if (this.parent.def.category == ThingCategory.Building)
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenBig);
						return;
					}
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.Forbidden);
				}
			}
		}

		// Token: 0x06008682 RID: 34434 RVA: 0x0005A3E6 File Offset: 0x000585E6
		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		// Token: 0x06008683 RID: 34435 RVA: 0x0005A3F5 File Offset: 0x000585F5
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent is Building && this.parent.Faction != Faction.OfPlayer)
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

		// Token: 0x04005694 RID: 22164
		private bool forbiddenInt;
	}
}
