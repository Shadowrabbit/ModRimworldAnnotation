using System;

namespace Verse.Noise
{
	// Token: 0x0200052A RID: 1322
	public class Rotate : ModuleBase
	{
		// Token: 0x060027C4 RID: 10180 RVA: 0x000F3729 File Offset: 0x000F1929
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000F2E95 File Offset: 0x000F1095
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000F3753 File Offset: 0x000F1953
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000F376F File Offset: 0x000F196F
		// (set) Token: 0x060027C8 RID: 10184 RVA: 0x000F3777 File Offset: 0x000F1977
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

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060027C9 RID: 10185 RVA: 0x000F378C File Offset: 0x000F198C
		// (set) Token: 0x060027CA RID: 10186 RVA: 0x000F3794 File Offset: 0x000F1994
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

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x000F376F File Offset: 0x000F196F
		// (set) Token: 0x060027CC RID: 10188 RVA: 0x000F37A9 File Offset: 0x000F19A9
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

		// Token: 0x060027CD RID: 10189 RVA: 0x000F37C0 File Offset: 0x000F19C0
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

		// Token: 0x060027CE RID: 10190 RVA: 0x000F38C0 File Offset: 0x000F1AC0
		public override double GetValue(double x, double y, double z)
		{
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04001892 RID: 6290
		private double m_x;

		// Token: 0x04001893 RID: 6291
		private double m_x1Matrix;

		// Token: 0x04001894 RID: 6292
		private double m_x2Matrix;

		// Token: 0x04001895 RID: 6293
		private double m_x3Matrix;

		// Token: 0x04001896 RID: 6294
		private double m_y;

		// Token: 0x04001897 RID: 6295
		private double m_y1Matrix;

		// Token: 0x04001898 RID: 6296
		private double m_y2Matrix;

		// Token: 0x04001899 RID: 6297
		private double m_y3Matrix;

		// Token: 0x0400189A RID: 6298
		private double m_z;

		// Token: 0x0400189B RID: 6299
		private double m_z1Matrix;

		// Token: 0x0400189C RID: 6300
		private double m_z2Matrix;

		// Token: 0x0400189D RID: 6301
		private double m_z3Matrix;
	}
}
