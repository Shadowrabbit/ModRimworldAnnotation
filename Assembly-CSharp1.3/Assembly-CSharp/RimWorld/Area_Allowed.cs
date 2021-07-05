using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5D RID: 3165
	public class Area_Allowed : Area
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x00186FA9 File Offset: 0x001851A9
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x00186FB1 File Offset: 0x001851B1
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x060049EB RID: 18923 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x00186FB9 File Offset: 0x001851B9
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00186FC0 File Offset: 0x001851C0
		public Area_Allowed()
		{
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00186FD4 File Offset: 0x001851D4
		public Area_Allowed(AreaManager areaManager, string label = null) : base(areaManager)
		{
			this.areaManager = areaManager;
			if (!label.NullOrEmpty())
			{
				this.labelInt = label;
			}
			else
			{
				int num = 1;
				for (;;)
				{
					this.labelInt = "AreaDefaultLabel".Translate(num);
					if (areaManager.GetLabeled(this.labelInt) == null)
					{
						break;
					}
					num++;
				}
			}
			this.colorInt = new Color(Rand.Value, Rand.Value, Rand.Value);
			this.colorInt = Color.Lerp(this.colorInt, Color.gray, 0.5f);
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x00187074 File Offset: 0x00185274
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001870B3 File Offset: 0x001852B3
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x001870BC File Offset: 0x001852BC
		public override string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Area_",
				this.ID,
				"_Named_",
				this.labelInt
			});
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x00186FA9 File Offset: 0x001851A9
		public override string ToString()
		{
			return this.labelInt;
		}

		// Token: 0x04002CF3 RID: 11507
		private string labelInt;

		// Token: 0x04002CF4 RID: 11508
		private Color colorInt = Color.red;
	}
}
