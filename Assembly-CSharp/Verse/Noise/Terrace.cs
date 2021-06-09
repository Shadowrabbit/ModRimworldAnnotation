using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008EF RID: 2287
	public class Terrace : ModuleBase
	{
		// Token: 0x060038CC RID: 14540 RVA: 0x0002BE2F File Offset: 0x0002A02F
		public Terrace() : base(1)
		{
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0002BE43 File Offset: 0x0002A043
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0002BE60 File Offset: 0x0002A060
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x060038CF RID: 14543 RVA: 0x0002BE84 File Offset: 0x0002A084
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x0002BE91 File Offset: 0x0002A091
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x0002BE99 File Offset: 0x0002A099
		// (set) Token: 0x060038D2 RID: 14546 RVA: 0x0002BEA1 File Offset: 0x0002A0A1
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

		// Token: 0x060038D3 RID: 14547 RVA: 0x00163D1C File Offset: 0x00161F1C
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0002BEAA File Offset: 0x0002A0AA
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x00163D70 File Offset: 0x00161F70
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

		// Token: 0x060038D6 RID: 14550 RVA: 0x00163DCC File Offset: 0x00161FCC
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

		// Token: 0x04002720 RID: 10016
		private List<double> m_data = new List<double>();

		// Token: 0x04002721 RID: 10017
		private bool m_inverted;
	}
}
