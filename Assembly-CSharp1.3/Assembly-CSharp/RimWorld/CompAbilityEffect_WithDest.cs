using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7F RID: 3455
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06004FFF RID: 20479 RVA: 0x001AC1E8 File Offset: 0x001AA3E8
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06005000 RID: 20480 RVA: 0x001AC1F5 File Offset: 0x001AA3F5
		public virtual TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetLocations = true
				};
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06005001 RID: 20481 RVA: 0x0001276E File Offset: 0x0001096E
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06005002 RID: 20482 RVA: 0x001AC203 File Offset: 0x001AA403
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06005003 RID: 20483 RVA: 0x001AC203 File Offset: 0x001AA403
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06005004 RID: 20484 RVA: 0x00002688 File Offset: 0x00000888
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x0001276E File Offset: 0x0001096E
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06005007 RID: 20487 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06005008 RID: 20488 RVA: 0x001AC210 File Offset: 0x001AA410
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06005009 RID: 20489 RVA: 0x00002688 File Offset: 0x00000888
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x0600500A RID: 20490 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool HidePawnTooltips
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x001AC218 File Offset: 0x001AA418
		public LocalTargetInfo GetDestination(LocalTargetInfo target)
		{
			Map map = this.parent.pawn.Map;
			switch (this.Props.destination)
			{
			case AbilityEffectDestination.Caster:
				return new LocalTargetInfo(this.parent.pawn.InteractionCell);
			case AbilityEffectDestination.RandomInRange:
			{
				this.cells.Clear();
				int num = GenRadial.NumCellsInRadius(this.Props.randomRange.max);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = GenRadial.RadialPattern[i];
					if (intVec.DistanceTo(IntVec3.Zero) >= this.Props.randomRange.min)
					{
						IntVec3 intVec2 = target.Cell + intVec;
						if (intVec2.Standable(map) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(target.Cell, intVec2, map, false, null, 0, 0)))
						{
							this.cells.Add(intVec2);
						}
					}
				}
				if (this.cells.Any<IntVec3>())
				{
					return new LocalTargetInfo(this.cells.RandomElement<IntVec3>());
				}
				Messages.Message("NoValidDestinationFound".Translate(this.parent.def.LabelCap), MessageTypeDefOf.RejectInput, true);
				return LocalTargetInfo.Invalid;
			}
			case AbilityEffectDestination.Selected:
				return target;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x001AC36C File Offset: 0x001AA56C
		public bool CanPlaceSelectedTargetAt(LocalTargetInfo target)
		{
			Pawn pawn = this.selectedTarget.Pawn;
			if (pawn != null)
			{
				return pawn.Spawned && !target.Cell.Impassable(this.parent.pawn.Map) && target.Cell.WalkableBy(this.parent.pawn.Map, pawn);
			}
			return CompAbilityEffect_WithDest.CanTeleportThingTo(target, this.parent.pawn.Map);
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x001AC3E4 File Offset: 0x001AA5E4
		public static bool CanTeleportThingTo(LocalTargetInfo target, Map map)
		{
			Building edifice = target.Cell.GetEdifice(map);
			Building_Door building_Door;
			if (edifice != null && edifice.def.surfaceType != SurfaceType.Item && edifice.def.surfaceType != SurfaceType.Eat && ((building_Door = (edifice as Building_Door)) == null || !building_Door.Open))
			{
				return false;
			}
			List<Thing> thingList = target.Cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Item)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x001AC468 File Offset: 0x001AA668
		public virtual bool CanHitTarget(LocalTargetInfo target)
		{
			return target.IsValid && (this.Props.range <= 0f || target.Cell.DistanceTo(this.selectedTarget.Cell) <= this.Props.range) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(this.selectedTarget.Cell, target.Cell, this.parent.pawn.Map, false, null, 0, 0));
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x001AC4F5 File Offset: 0x001AA6F5
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.CanHitTarget(target);
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x001AC500 File Offset: 0x001AA700
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (this.Props.range > 0f)
			{
				if (this.Props.requiresLineOfSight)
				{
					GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.selectedTarget.Cell, c, this.parent.pawn.Map, false, null, 0, 0) && this.CanPlaceSelectedTargetAt(c));
				}
				else
				{
					GenDraw.DrawRadiusRing(this.selectedTarget.Cell, this.Props.range);
				}
			}
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x001AC584 File Offset: 0x001AA784
		public void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				icon = this.parent.def.uiIcon;
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
			string text = this.ExtraLabelMouseAttachment(target);
			if (!text.NullOrEmpty())
			{
				Widgets.MouseAttachedLabel(text, 0f, 0f);
			}
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001AC5D9 File Offset: 0x001AA7D9
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.parent.QueueCastingJob(this.selectedTarget, target);
			this.selectedTarget = LocalTargetInfo.Invalid;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x001AC5F8 File Offset: 0x001AA7F8
		public void SetTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target;
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x001AC601 File Offset: 0x001AA801
		public virtual void SelectDestination()
		{
			Find.Targeter.BeginTargeting(this, null);
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x001AC60F File Offset: 0x001AA80F
		public bool SelectedTargetInvalidated()
		{
			return this.selectedTarget.HasThing && !this.selectedTarget.Thing.Spawned;
		}

		// Token: 0x04002FDC RID: 12252
		protected LocalTargetInfo selectedTarget = LocalTargetInfo.Invalid;

		// Token: 0x04002FDD RID: 12253
		private List<IntVec3> cells = new List<IntVec3>();
	}
}
