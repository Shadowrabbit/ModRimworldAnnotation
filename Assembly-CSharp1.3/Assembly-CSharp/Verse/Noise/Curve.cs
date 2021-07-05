using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200051E RID: 1310
	public class Curve : ModuleBase
	{
		// Token: 0x06002793 RID: 10131 RVA: 0x000F318B File Offset: 0x000F138B
		public Curve() : base(1)
		{
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000F319F File Offset: 0x000F139F
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x000F31BC File Offset: 0x000F13BC
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002796 RID: 10134 RVA: 0x000F31C9 File Offset: 0x000F13C9
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000F31D4 File Offset: 0x000F13D4
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000F322E File Offset: 0x000F142E
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000F323C File Offset: 0x000F143C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num].Key)
			{
				num++;
			}
			int index = Mathf.Clamp(num - 2, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(num + 1, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num2].Value;
			}
			double key = this.m_data[num2].Key;
			double key2 = this.m_data[num3].Key;
			double position = (value - key) / (key2 - key);
			return Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num2].Value, this.m_data[num3].Value, this.m_data[index2].Value, position);
		}

		// Token: 0x0400188C RID: 6284
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
