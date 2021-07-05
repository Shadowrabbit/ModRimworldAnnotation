using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E51 RID: 3665
	public class Need_Outdoors : Need
	{
		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x060054DF RID: 21727 RVA: 0x001CBFFA File Offset: 0x001CA1FA
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

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x060054E0 RID: 21728 RVA: 0x001CC014 File Offset: 0x001CA214
		public OutdoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.8f)
				{
					return OutdoorsCategory.Free;
				}
				if (this.CurLevel > 0.6f)
				{
					return OutdoorsCategory.NeedFreshAir;
				}
				if (this.CurLevel > 0.4f)
				{
					return OutdoorsCategory.CabinFeverLight;
				}
				if (this.CurLevel > 0.2f)
				{
					return OutdoorsCategory.CabinFeverSevere;
				}
				if (this.CurLevel > 0.05f)
				{
					return OutdoorsCategory.Trapped;
				}
				return OutdoorsCategory.Entombed;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060054E1 RID: 21729 RVA: 0x001CC06D File Offset: 0x001CA26D
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060054E2 RID: 21730 RVA: 0x001CC078 File Offset: 0x001CA278
		private bool Disabled
		{
			get
			{
				return this.pawn.story.traits.HasTrait(TraitDefOf.Undergrounder) || (this.pawn.Ideo != null && this.pawn.Ideo.IdeoDisablesCrampedRoomThoughts());
			}
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x001CC0B8 File Offset: 0x001CA2B8
		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x001CB742 File Offset: 0x001C9942
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x001CC128 File Offset: 0x001CA328
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
			float b = 0.2f;
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float num;
			if (!flag)
			{
				if (roofDef == null)
				{
					num = 5f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = -0.32f;
				}
				else
				{
					num = -0.45f;
					b = 0f;
				}
			}
			else if (roofDef == null)
			{
				num = 8f;
			}
			else if (roofDef.isThickRoof)
			{
				num = -0.4f;
			}
			else
			{
				num = 1f;
			}
			if (this.pawn.InBed() && num < 0f)
			{
				num *= 0.2f;
			}
			num *= 0.0025f;
			float curLevel = this.CurLevel;
			if (num < 0f)
			{
				this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num, b));
			}
			else
			{
				this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
			}
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x04003237 RID: 12855
		private const float Delta_IndoorsThickRoof = -0.45f;

		// Token: 0x04003238 RID: 12856
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x04003239 RID: 12857
		private const float Delta_IndoorsThinRoof = -0.32f;

		// Token: 0x0400323A RID: 12858
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x0400323B RID: 12859
		private const float Delta_OutdoorsThinRoof = 1f;

		// Token: 0x0400323C RID: 12860
		private const float Delta_IndoorsNoRoof = 5f;

		// Token: 0x0400323D RID: 12861
		private const float Delta_OutdoorsNoRoof = 8f;

		// Token: 0x0400323E RID: 12862
		private const float DeltaFactor_InBed = 0.2f;

		// Token: 0x0400323F RID: 12863
		private float lastEffectiveDelta;
	}
}
