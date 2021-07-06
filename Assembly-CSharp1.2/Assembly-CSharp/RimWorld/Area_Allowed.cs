using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001259 RID: 4697
	public class Area_Allowed : Area
	{
		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x0600666C RID: 26220 RVA: 0x00045F67 File Offset: 0x00044167
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x0600666D RID: 26221 RVA: 0x00045F6F File Offset: 0x0004416F
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x0600666E RID: 26222 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x0600666F RID: 26223 RVA: 0x00045F77 File Offset: 0x00044177
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x00045F7E File Offset: 0x0004417E
		public Area_Allowed()
		{
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x001F985C File Offset: 0x001F7A5C
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

		// Token: 0x06006672 RID: 26226 RVA: 0x001F98FC File Offset: 0x001F7AFC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x00045F91 File Offset: 0x00044191
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x00045F9A File Offset: 0x0004419A
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

		// Token: 0x06006676 RID: 26230 RVA: 0x00045F67 File Offset: 0x00044167
		public override string ToString()
		{
			return this.labelInt;
		}

		// Token: 0x04004442 RID: 17474
		private string labelInt;

		// Token: 0x04004443 RID: 17475
		private Color colorInt = Color.red;
	}
}
