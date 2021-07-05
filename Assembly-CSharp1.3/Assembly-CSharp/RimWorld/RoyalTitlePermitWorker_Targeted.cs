using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DED RID: 3565
	public abstract class RoyalTitlePermitWorker_Targeted : RoyalTitlePermitWorker, ITargetingSource
	{
		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06005299 RID: 21145 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x0600529A RID: 21146 RVA: 0x0001276E File Offset: 0x0001096E
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x0600529B RID: 21147 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x0600529C RID: 21148 RVA: 0x0001276E File Offset: 0x0001096E
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x0600529D RID: 21149 RVA: 0x0001276E File Offset: 0x0001096E
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x0600529E RID: 21150 RVA: 0x001BDE7B File Offset: 0x001BC07B
		public Thing Caster
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x0600529F RID: 21151 RVA: 0x001BDE7B File Offset: 0x001BC07B
		public Pawn CasterPawn
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x060052A0 RID: 21152 RVA: 0x00002688 File Offset: 0x00000888
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x060052A1 RID: 21153 RVA: 0x00002688 File Offset: 0x00000888
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x060052A2 RID: 21154 RVA: 0x001BDE83 File Offset: 0x001BC083
		public TargetingParameters targetParams
		{
			get
			{
				return this.targetingParameters;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x060052A3 RID: 21155 RVA: 0x00002688 File Offset: 0x00000888
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x001BDE8C File Offset: 0x001BC08C
		public bool CanHitTarget(LocalTargetInfo target)
		{
			if (this.def.royalAid.targetingRequireLOS && !GenSight.LineOfSight(this.caller.Position, target.Cell, this.map, true, null, 0, 0))
			{
				bool flag = false;
				ShootLeanUtility.LeanShootingSourcesFromTo(this.caller.Position, target.Cell, this.map, RoyalTitlePermitWorker_Targeted.tempSourceList);
				for (int i = 0; i < RoyalTitlePermitWorker_Targeted.tempSourceList.Count; i++)
				{
					if (GenSight.LineOfSight(RoyalTitlePermitWorker_Targeted.tempSourceList[i], target.Cell, this.map, true, null, 0, 0))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x001BDF38 File Offset: 0x001BC138
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!this.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001BDF8E File Offset: 0x001BC18E
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x001BDFC8 File Offset: 0x001BC1C8
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != null && this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x040030B3 RID: 12467
		protected bool free;

		// Token: 0x040030B4 RID: 12468
		protected Pawn caller;

		// Token: 0x040030B5 RID: 12469
		protected Map map;

		// Token: 0x040030B6 RID: 12470
		protected TargetingParameters targetingParameters;

		// Token: 0x040030B7 RID: 12471
		private static List<IntVec3> tempSourceList = new List<IntVec3>();
	}
}
