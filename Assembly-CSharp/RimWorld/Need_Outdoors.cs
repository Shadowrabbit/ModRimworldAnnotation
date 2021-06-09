using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EE RID: 5358
	public class Need_Outdoors : Need
	{
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06007374 RID: 29556 RVA: 0x0004DBBD File Offset: 0x0004BDBD
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

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06007375 RID: 29557 RVA: 0x00233E74 File Offset: 0x00232074
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

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06007376 RID: 29558 RVA: 0x0004DBD4 File Offset: 0x0004BDD4
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06007377 RID: 29559 RVA: 0x0004DBDF File Offset: 0x0004BDDF
		private bool Disabled
		{
			get
			{
				return this.pawn.story.traits.HasTrait(TraitDefOf.Undergrounder);
			}
		}

		// Token: 0x06007378 RID: 29560 RVA: 0x00233ED0 File Offset: 0x002320D0
		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		// Token: 0x06007379 RID: 29561 RVA: 0x0004DBFB File Offset: 0x0004BDFB
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x0600737A RID: 29562 RVA: 0x00233F40 File Offset: 0x00232140
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

		// Token: 0x04004C32 RID: 19506
		private const float Delta_IndoorsThickRoof = -0.45f;

		// Token: 0x04004C33 RID: 19507
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x04004C34 RID: 19508
		private const float Delta_IndoorsThinRoof = -0.32f;

		// Token: 0x04004C35 RID: 19509
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x04004C36 RID: 19510
		private const float Delta_OutdoorsThinRoof = 1f;

		// Token: 0x04004C37 RID: 19511
		private const float Delta_IndoorsNoRoof = 5f;

		// Token: 0x04004C38 RID: 19512
		private const float Delta_OutdoorsNoRoof = 8f;

		// Token: 0x04004C39 RID: 19513
		private const float DeltaFactor_InBed = 0.2f;

		// Token: 0x04004C3A RID: 19514
		private float lastEffectiveDelta;
	}
}
