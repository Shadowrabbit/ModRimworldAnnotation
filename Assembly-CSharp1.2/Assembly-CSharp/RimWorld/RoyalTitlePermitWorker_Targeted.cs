using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001478 RID: 5240
	public abstract class RoyalTitlePermitWorker_Targeted : RoyalTitlePermitWorker, ITargetingSource
	{
		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06007114 RID: 28948 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06007115 RID: 28949 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06007116 RID: 28950 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06007117 RID: 28951 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06007118 RID: 28952 RVA: 0x0004C290 File Offset: 0x0004A490
		public Thing Caster
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x06007119 RID: 28953 RVA: 0x0004C290 File Offset: 0x0004A490
		public Pawn CasterPawn
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x0600711A RID: 28954 RVA: 0x0000C32E File Offset: 0x0000A52E
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x0600711B RID: 28955 RVA: 0x0000C32E File Offset: 0x0000A52E
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600711C RID: 28956 RVA: 0x0004C298 File Offset: 0x0004A498
		public TargetingParameters targetParams
		{
			get
			{
				return this.targetingParameters;
			}
		}

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x0600711D RID: 28957 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x002291C4 File Offset: 0x002273C4
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

		// Token: 0x0600711F RID: 28959 RVA: 0x00228CFC File Offset: 0x00226EFC
		public virtual bool ValidateTarget(LocalTargetInfo target)
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

		// Token: 0x06007120 RID: 28960 RVA: 0x0004C2A0 File Offset: 0x0004A4A0
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
		}

		// Token: 0x06007122 RID: 28962 RVA: 0x00229270 File Offset: 0x00227470
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

		// Token: 0x04004AA9 RID: 19113
		protected bool free;

		// Token: 0x04004AAA RID: 19114
		protected Pawn caller;

		// Token: 0x04004AAB RID: 19115
		protected Map map;

		// Token: 0x04004AAC RID: 19116
		protected TargetingParameters targetingParameters;

		// Token: 0x04004AAD RID: 19117
		private static List<IntVec3> tempSourceList = new List<IntVec3>();
	}
}
