using System;

namespace Verse.Noise
{
	// Token: 0x020008EA RID: 2282
	public class Rotate : ModuleBase
	{
		// Token: 0x0600389F RID: 14495 RVA: 0x0002BB57 File Offset: 0x00029D57
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0002B7A3 File Offset: 0x000299A3
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x0002BB81 File Offset: 0x00029D81
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x060038A2 RID: 14498 RVA: 0x0002BB9D File Offset: 0x00029D9D
		// (set) Token: 0x060038A3 RID: 14499 RVA: 0x0002BBA5 File Offset: 0x00029DA5
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(value, this.m_y, this.m_z);
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x0002BBBA File Offset: 0x00029DBA
		// (set) Token: 0x060038A5 RID: 14501 RVA: 0x0002BBC2 File Offset: 0x00029DC2
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.SetAngles(this.m_x, value, this.m_z);
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x060038A6 RID: 14502 RVA: 0x0002BB9D File Offset: 0x00029D9D
		// (set) Token: 0x060038A7 RID: 14503 RVA: 0x0002BBD7 File Offset: 0x00029DD7
		public double Z
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(this.m_x, this.m_y, value);
			}
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x00163938 File Offset: 0x00161B38
		private void SetAngles(double x, double y, double z)
		{
			double num = Math.Cos(x * 0.017453292519943295);
			double num2 = Math.Cos(y * 0.017453292519943295);
			double num3 = Math.Cos(z * 0.017453292519943295);
			double num4 = Math.Sin(x * 0.017453292519943295);
			double num5 = Math.Sin(y * 0.017453292519943295);
			double num6 = Math.Sin(z * 0.017453292519943295);
			this.m_x1Matrix = num5 * num4 * num6 + num2 * num3;
			this.m_y1Matrix = num * num6;
			this.m_z1Matrix = num5 * num3 - num2 * num4 * num6;
			this.m_x2Matrix = num5 * num4 * num3 - num2 * num6;
			this.m_y2Matrix = num * num3;
			this.m_z2Matrix = -num2 * num4 * num3 - num5 * num6;
			this.m_x3Matrix = -num5 * num;
			this.m_y3Matrix = num4;
			this.m_z3Matrix = num2 * num;
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x00163A38 File Offset: 0x00161C38
		public override double GetValue(double x, double y, double z)
		{
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x0400270B RID: 9995
		private double m_x;

		// Token: 0x0400270C RID: 9996
		private double m_x1Matrix;

		// Token: 0x0400270D RID: 9997
		private double m_x2Matrix;

		// Token: 0x0400270E RID: 9998
		private double m_x3Matrix;

		// Token: 0x0400270F RID: 9999
		private double m_y;

		// Token: 0x04002710 RID: 10000
		private double m_y1Matrix;

		// Token: 0x04002711 RID: 10001
		private double m_y2Matrix;

		// Token: 0x04002712 RID: 10002
		private double m_y3Matrix;

		// Token: 0x04002713 RID: 10003
		private double m_z;

		// Token: 0x04002714 RID: 10004
		private double m_z1Matrix;

		// Token: 0x04002715 RID: 10005
		private double m_z2Matrix;

		// Token: 0x04002716 RID: 10006
		private double m_z3Matrix;
	}
}
