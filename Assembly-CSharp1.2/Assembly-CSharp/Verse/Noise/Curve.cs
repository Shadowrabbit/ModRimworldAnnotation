using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008DD RID: 2269
	public class Curve : ModuleBase
	{
		// Token: 0x0600386B RID: 14443 RVA: 0x0002B96C File Offset: 0x00029B6C
		public Curve() : base(1)
		{
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x0002B980 File Offset: 0x00029B80
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600386D RID: 14445 RVA: 0x0002B99D File Offset: 0x00029B9D
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600386E RID: 14446 RVA: 0x0002B9AA File Offset: 0x00029BAA
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x00163554 File Offset: 0x00161754
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0002B9B2 File Offset: 0x00029BB2
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x001635B0 File Offset: 0x001617B0
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

		// Token: 0x04002703 RID: 9987
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
