using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200052F RID: 1327
	public class Terrace : ModuleBase
	{
		// Token: 0x060027F1 RID: 10225 RVA: 0x000F3DE4 File Offset: 0x000F1FE4
		public Terrace() : base(1)
		{
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000F3DF8 File Offset: 0x000F1FF8
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000F3E15 File Offset: 0x000F2015
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000F3E39 File Offset: 0x000F2039
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000F3E46 File Offset: 0x000F2046
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000F3E4E File Offset: 0x000F204E
		// (set) Token: 0x060027F7 RID: 10231 RVA: 0x000F3E56 File Offset: 0x000F2056
		public bool IsInverted
		{
			get
			{
				return this.m_inverted;
			}
			set
			{
				this.m_inverted = value;
			}
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000F3E60 File Offset: 0x000F2060
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000F3EB1 File Offset: 0x000F20B1
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000F3EC0 File Offset: 0x000F20C0
		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int i = 0; i < steps; i++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000F3F1C File Offset: 0x000F211C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num])
			{
				num++;
			}
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num3];
			}
			double num4 = this.m_data[num2];
			double num5 = this.m_data[num3];
			double num6 = (value - num4) / (num5 - num4);
			if (this.m_inverted)
			{
				num6 = 1.0 - num6;
				double num7 = num4;
				num4 = num5;
				num5 = num7;
			}
			num6 *= num6;
			return Utils.InterpolateLinear(num4, num5, num6);
		}

		// Token: 0x040018A7 RID: 6311
		private List<double> m_data = new List<double>();

		// Token: 0x040018A8 RID: 6312
		private bool m_inverted;
	}
}
