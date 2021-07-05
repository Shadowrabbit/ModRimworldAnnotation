using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000517 RID: 1303
	public class Noise2D : IDisposable
	{
		// Token: 0x06002758 RID: 10072 RVA: 0x000F22EA File Offset: 0x000F04EA
		protected Noise2D()
		{
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000F2304 File Offset: 0x000F0504
		public Noise2D(int size) : this(size, size, null)
		{
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000F230F File Offset: 0x000F050F
		public Noise2D(int size, ModuleBase generator) : this(size, size, generator)
		{
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000F231A File Offset: 0x000F051A
		public Noise2D(int width, int height) : this(width, height, null)
		{
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000F2328 File Offset: 0x000F0528
		public Noise2D(int width, int height, ModuleBase generator)
		{
			this.m_generator = generator;
			this.m_width = width;
			this.m_height = height;
			this.m_data = new float[width, height];
			this.m_ucWidth = width + this.m_ucBorder * 2;
			this.m_ucHeight = height + this.m_ucBorder * 2;
			this.m_ucData = new float[width + this.m_ucBorder * 2, height + this.m_ucBorder * 2];
		}

		// Token: 0x170007B7 RID: 1975
		public float this[int x, int y, bool isCropped = true]
		{
			get
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					return this.m_data[x, y];
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					return this.m_ucData[x, y];
				}
			}
			set
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Invalid y position");
					}
					this.m_data[x, y] = value;
					return;
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					this.m_ucData[x, y] = value;
					return;
				}
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x000F24CB File Offset: 0x000F06CB
		// (set) Token: 0x06002760 RID: 10080 RVA: 0x000F24D3 File Offset: 0x000F06D3
		public float Border
		{
			get
			{
				return this.m_borderValue;
			}
			set
			{
				this.m_borderValue = value;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x000F24DC File Offset: 0x000F06DC
		// (set) Token: 0x06002762 RID: 10082 RVA: 0x000F24E4 File Offset: 0x000F06E4
		public ModuleBase Generator
		{
			get
			{
				return this.m_generator;
			}
			set
			{
				this.m_generator = value;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x000F24ED File Offset: 0x000F06ED
		public int Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x000F24F5 File Offset: 0x000F06F5
		public int Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000F24FD File Offset: 0x000F06FD
		public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
		{
			return this.GetData(isCropped, xCrop, yCrop, true);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000F250C File Offset: 0x000F070C
		public float[,] GetData(bool isCropped = true, int xCrop = 0, int yCrop = 0, bool isNormalized = false)
		{
			int num;
			int num2;
			float[,] array;
			if (isCropped)
			{
				num = this.m_width;
				num2 = this.m_height;
				array = this.m_data;
			}
			else
			{
				num = this.m_ucWidth;
				num2 = this.m_ucHeight;
				array = this.m_ucData;
			}
			num -= xCrop;
			num2 -= yCrop;
			float[,] array2 = new float[num, num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num3;
					if (isNormalized)
					{
						num3 = (array[i, j] + 1f) / 2f;
					}
					else
					{
						num3 = array[i, j];
					}
					array2[i, j] = num3;
				}
			}
			return array2;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000F25B1 File Offset: 0x000F07B1
		public void Clear()
		{
			this.Clear(0f);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000F25C0 File Offset: 0x000F07C0
		public void Clear(float value)
		{
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					this.m_data[i, j] = value;
				}
			}
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000F25FD File Offset: 0x000F07FD
		private double GeneratePlanar(double x, double y)
		{
			return this.m_generator.GetValue(x, 0.0, y);
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000F2615 File Offset: 0x000F0815
		public void GeneratePlanar(double left, double right, double top, double bottom)
		{
			this.GeneratePlanar(left, right, top, bottom, true);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000F2624 File Offset: 0x000F0824
		public void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless)
		{
			if (right <= left || bottom <= top)
			{
				throw new ArgumentException("Invalid right/left or bottom/top combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = right - left;
			double num2 = bottom - top;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = left;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = top;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num7;
					if (isSeamless)
					{
						num7 = (float)this.GeneratePlanar(num5, num6);
					}
					else
					{
						double a = this.GeneratePlanar(num5, num6);
						double b = this.GeneratePlanar(num5 + num, num6);
						double a2 = this.GeneratePlanar(num5, num6 + num2);
						double b2 = this.GeneratePlanar(num5 + num, num6 + num2);
						double position = 1.0 - (num5 - left) / num;
						double position2 = 1.0 - (num6 - top) / num2;
						double a3 = Utils.InterpolateLinear(a, b, position);
						double b3 = Utils.InterpolateLinear(a2, b2, position);
						num7 = (float)Utils.InterpolateLinear(a3, b3, position2);
					}
					this.m_ucData[i, j] = num7;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = num7;
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000F27CC File Offset: 0x000F09CC
		private double GenerateCylindrical(double angle, double height)
		{
			double x = Math.Cos(angle * 0.017453292519943295);
			double z = Math.Sin(angle * 0.017453292519943295);
			return this.m_generator.GetValue(x, height, z);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000F280C File Offset: 0x000F0A0C
		public void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
		{
			if (angleMax <= angleMin || heightMax <= heightMin)
			{
				throw new ArgumentException("Invalid angle or height parameters");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = angleMax - angleMin;
			double num2 = heightMax - heightMin;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = angleMin;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = heightMin;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateCylindrical(num5, num6);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateCylindrical(num5, num6);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000F292C File Offset: 0x000F0B2C
		private double GenerateSpherical(double lat, double lon)
		{
			double num = Math.Cos(0.017453292519943295 * lat);
			return this.m_generator.GetValue(num * Math.Cos(0.017453292519943295 * lon), Math.Sin(0.017453292519943295 * lat), num * Math.Sin(0.017453292519943295 * lon));
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000F298C File Offset: 0x000F0B8C
		public void GenerateSpherical(double south, double north, double west, double east)
		{
			if (east <= west || north <= south)
			{
				throw new ArgumentException("Invalid east/west or north/south combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = east - west;
			double num2 = north - south;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = west;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = south;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateSpherical(num6, num5);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateSpherical(num6, num5);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000F2AA9 File Offset: 0x000F0CA9
		public Texture2D GetTexture()
		{
			return this.GetTexture(GradientPresets.Grayscale);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000F2AB8 File Offset: 0x000F0CB8
		public Texture2D GetTexture(Gradient gradient)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					float num;
					if (!float.IsNaN(this.m_borderValue) && (i == 0 || i == this.m_width - this.m_ucBorder || j == 0 || j == this.m_height - this.m_ucBorder))
					{
						num = this.m_borderValue;
					}
					else
					{
						num = this.m_data[i, j];
					}
					array[i + j * this.m_width] = gradient.Evaluate((num + 1f) / 2f);
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000F2BAC File Offset: 0x000F0DAC
		public Texture2D GetNormalMap(float intensity)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num = (this.m_ucData[Mathf.Max(0, i - this.m_ucBorder), j] - this.m_ucData[Mathf.Min(i + this.m_ucBorder, this.m_height + this.m_ucBorder), j]) / 2f;
					float num2 = (this.m_ucData[i, Mathf.Max(0, j - this.m_ucBorder)] - this.m_ucData[i, Mathf.Min(j + this.m_ucBorder, this.m_width + this.m_ucBorder)]) / 2f;
					Vector3 a = new Vector3(num * intensity, 0f, 1f);
					Vector3 b = new Vector3(0f, num2 * intensity, 1f);
					Vector3 vector = a + b;
					vector.Normalize();
					Vector3 zero = Vector3.zero;
					zero.x = (vector.x + 1f) / 2f;
					zero.y = (vector.y + 1f) / 2f;
					zero.z = (vector.z + 1f) / 2f;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						array[i - this.m_ucBorder + (j - this.m_ucBorder) * this.m_width] = new Color(zero.x, zero.y, zero.z);
					}
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x000F2DB5 File Offset: 0x000F0FB5
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000F2DBD File Offset: 0x000F0FBD
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000F2DD9 File Offset: 0x000F0FD9
		protected virtual bool Disposing()
		{
			if (this.m_data != null)
			{
				this.m_data = null;
			}
			this.m_width = 0;
			this.m_height = 0;
			return true;
		}

		// Token: 0x04001870 RID: 6256
		public static readonly double South = -90.0;

		// Token: 0x04001871 RID: 6257
		public static readonly double North = 90.0;

		// Token: 0x04001872 RID: 6258
		public static readonly double West = -180.0;

		// Token: 0x04001873 RID: 6259
		public static readonly double East = 180.0;

		// Token: 0x04001874 RID: 6260
		public static readonly double AngleMin = -180.0;

		// Token: 0x04001875 RID: 6261
		public static readonly double AngleMax = 180.0;

		// Token: 0x04001876 RID: 6262
		public static readonly double Left = -1.0;

		// Token: 0x04001877 RID: 6263
		public static readonly double Right = 1.0;

		// Token: 0x04001878 RID: 6264
		public static readonly double Top = -1.0;

		// Token: 0x04001879 RID: 6265
		public static readonly double Bottom = 1.0;

		// Token: 0x0400187A RID: 6266
		private int m_width;

		// Token: 0x0400187B RID: 6267
		private int m_height;

		// Token: 0x0400187C RID: 6268
		private float[,] m_data;

		// Token: 0x0400187D RID: 6269
		private int m_ucWidth;

		// Token: 0x0400187E RID: 6270
		private int m_ucHeight;

		// Token: 0x0400187F RID: 6271
		private int m_ucBorder = 1;

		// Token: 0x04001880 RID: 6272
		private float[,] m_ucData;

		// Token: 0x04001881 RID: 6273
		private float m_borderValue = float.NaN;

		// Token: 0x04001882 RID: 6274
		private ModuleBase m_generator;

		// Token: 0x04001883 RID: 6275
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
