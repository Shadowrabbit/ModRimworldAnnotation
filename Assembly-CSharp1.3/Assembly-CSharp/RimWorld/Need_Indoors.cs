using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4B RID: 3659
	public class Need_Indoors : Need
	{
		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x060054C5 RID: 21701 RVA: 0x001CB654 File Offset: 0x001C9854
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x060054C6 RID: 21702 RVA: 0x001CB65F File Offset: 0x001C985F
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x060054C7 RID: 21703 RVA: 0x001CB678 File Offset: 0x001C9878
		public IndoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > Need_Indoors.Thresholds[0])
				{
					return IndoorsCategory.ComfortablyIndoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[1])
				{
					return IndoorsCategory.JustOutdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[2])
				{
					return IndoorsCategory.Outdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[3])
				{
					return IndoorsCategory.LongOutdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[4])
				{
					return IndoorsCategory.VeryLongOutdoors;
				}
				return IndoorsCategory.BrutalOutdoors;
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x060054C8 RID: 21704 RVA: 0x001CB6DC File Offset: 0x001C98DC
		private bool Disabled
		{
			get
			{
				return !this.pawn.story.traits.HasTrait(TraitDefOf.Undergrounder) && (this.pawn.ideo == null || !this.pawn.Ideo.IdeoDisablesCrampedRoomThoughts());
			}
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x001CB729 File Offset: 0x001C9929
		public Need_Indoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>(Need_Indoors.Thresholds);
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x001CB742 File Offset: 0x001C9942
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x001CB750 File Offset: 0x001C9950
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float curLevel = this.CurLevel;
			float num;
			if ((roofDef == null || !roofDef.isThickRoof) && curLevel >= 0.5f)
			{
				num = -0.5f;
			}
			else if (!flag)
			{
				if (roofDef == null)
				{
					num = 0f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = 1f;
				}
				else
				{
					num = 2f;
				}
			}
			else if (roofDef == null)
			{
				num = -0.25f;
			}
			else if (roofDef.isThickRoof)
			{
				num = 0f;
			}
			else
			{
				num = -0.25f;
			}
			num *= 0.0025f;
			if (num < 0f)
			{
				this.CurLevel = Mathf.Min(this.CurLevel, this.CurLevel + num);
			}
			else
			{
				this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
			}
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x001CB894 File Offset: 0x001C9A94
		public override string GetTipString()
		{
			string text = base.GetTipString();
			if (this.pawn.Ideo != null && this.pawn.Ideo.IdeoDisablesCrampedRoomThoughts())
			{
				text += "\n\n" + "ComesFromIdeo".Translate() + ": " + this.pawn.Ideo.name;
			}
			return text;
		}

		// Token: 0x0400320F RID: 12815
		private static readonly float[] Thresholds = new float[]
		{
			0.8f,
			0.6f,
			0.4f,
			0.2f,
			0.05f
		};

		// Token: 0x04003210 RID: 12816
		private const float Max_NotUnderThickRoof = 0.5f;

		// Token: 0x04003211 RID: 12817
		private const float Delta_Indoors_ThickRoof = 2f;

		// Token: 0x04003212 RID: 12818
		private const float Delta_Indoors_ThinRoof = 1f;

		// Token: 0x04003213 RID: 12819
		private const float Delta_Indoors_NoRoof = 0f;

		// Token: 0x04003214 RID: 12820
		private const float Delta_Outdoors_ThickRoof = 0f;

		// Token: 0x04003215 RID: 12821
		private const float Delta_Outdoors_ThinRoof = -0.25f;

		// Token: 0x04003216 RID: 12822
		private const float Delta_Outdoors_NoRoof = -0.25f;

		// Token: 0x04003217 RID: 12823
		private const float Delta_NotUnderThickRoofOverThreshold = -0.5f;

		// Token: 0x04003218 RID: 12824
		private float lastEffectiveDelta;
	}
}
