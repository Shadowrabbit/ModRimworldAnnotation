using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A4 RID: 5028
	public abstract class CompAbilityEffect_WithDest : CompAbilityEffect, ITargetingSource
	{
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x06006D02 RID: 27906 RVA: 0x0004A286 File Offset: 0x00048486
		public new CompProperties_EffectWithDest Props
		{
			get
			{
				return (CompProperties_EffectWithDest)this.props;
			}
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x06006D03 RID: 27907 RVA: 0x0004A293 File Offset: 0x00048493
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

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06006D04 RID: 27908 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06006D05 RID: 27909 RVA: 0x0004A2A1 File Offset: 0x000484A1
		public Thing Caster
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x06006D06 RID: 27910 RVA: 0x0004A2A1 File Offset: 0x000484A1
		public Pawn CasterPawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x06006D07 RID: 27911 RVA: 0x0000C32E File Offset: 0x0000A52E
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06006D08 RID: 27912 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06006D09 RID: 27913 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x06006D0A RID: 27914 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06006D0B RID: 27915 RVA: 0x0004A2AE File Offset: 0x000484AE
		public Texture2D UIIcon
		{
			get
			{
				return BaseContent.BadTex;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06006D0C RID: 27916 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06006D0D RID: 27917 RVA: 0x00217018 File Offset: 0x00215218
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

		// Token: 0x06006D0E RID: 27918 RVA: 0x0021716C File Offset: 0x0021536C
		public bool CanPlaceSelectedTargetAt(LocalTargetInfo target)
		{
			if (this.selectedTarget.Pawn != null)
			{
				return !target.Cell.Impassable(this.parent.pawn.Map) && target.Cell.Walkable(this.parent.pawn.Map);
			}
			return CompAbilityEffect_WithDest.CanTeleportThingTo(target, this.parent.pawn.Map);
		}

		// Token: 0x06006D0F RID: 27919 RVA: 0x002171DC File Offset: 0x002153DC
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

		// Token: 0x06006D10 RID: 27920 RVA: 0x00217260 File Offset: 0x00215460
		public virtual bool CanHitTarget(LocalTargetInfo target)
		{
			return target.IsValid && (this.Props.range <= 0f || target.Cell.DistanceTo(this.selectedTarget.Cell) <= this.Props.range) && (!this.Props.requiresLineOfSight || GenSight.LineOfSight(this.selectedTarget.Cell, target.Cell, this.parent.pawn.Map, false, null, 0, 0));
		}

		// Token: 0x06006D11 RID: 27921 RVA: 0x0004A2B5 File Offset: 0x000484B5
		public virtual bool ValidateTarget(LocalTargetInfo target)
		{
			return this.CanHitTarget(target);
		}

		// Token: 0x06006D12 RID: 27922 RVA: 0x002172F0 File Offset: 0x002154F0
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

		// Token: 0x06006D13 RID: 27923 RVA: 0x00217374 File Offset: 0x00215574
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
			string text = this.ExtraLabel(target);
			if (!text.NullOrEmpty())
			{
				Widgets.MouseAttachedLabel(text);
			}
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x0004A2BE File Offset: 0x000484BE
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.parent.QueueCastingJob(this.selectedTarget, target);
			this.selectedTarget = LocalTargetInfo.Invalid;
		}

		// Token: 0x06006D15 RID: 27925 RVA: 0x0004A2DD File Offset: 0x000484DD
		public void SetTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target;
		}

		// Token: 0x06006D16 RID: 27926 RVA: 0x0004A2E6 File Offset: 0x000484E6
		public virtual void SelectDestination()
		{
			Find.Targeter.BeginTargeting(this, null);
		}

		// Token: 0x04004837 RID: 18487
		protected LocalTargetInfo selectedTarget = LocalTargetInfo.Invalid;

		// Token: 0x04004838 RID: 18488
		private List<IntVec3> cells = new List<IntVec3>();
	}
}
