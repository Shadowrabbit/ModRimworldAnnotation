using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008D6 RID: 2262
	public class Noise2D : IDisposable
	{
		// Token: 0x06003830 RID: 14384 RVA: 0x0002B6A6 File Offset: 0x000298A6
		protected Noise2D()
		{
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x0002B6C0 File Offset: 0x000298C0
		public Noise2D(int size) : this(size, size, null)
		{
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0002B6CB File Offset: 0x000298CB
		public Noise2D(int size, ModuleBase generator) : this(size, size, generator)
		{
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x0002B6D6 File Offset: 0x000298D6
		public Noise2D(int width, int height) : this(width, height, null)
		{
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x00162978 File Offset: 0x00160B78
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

		// Token: 0x170008DF RID: 2271
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

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06003837 RID: 14391 RVA: 0x0002B6E1 File Offset: 0x000298E1
		// (set) Token: 0x06003838 RID: 14392 RVA: 0x0002B6E9 File Offset: 0x000298E9
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

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003839 RID: 14393 RVA: 0x0002B6F2 File Offset: 0x000298F2
		// (set) Token: 0x0600383A RID: 14394 RVA: 0x0002B6FA File Offset: 0x000298FA
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

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x0600383B RID: 14395 RVA: 0x0002B703 File Offset: 0x00029903
		public int Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x0002B70B File Offset: 0x0002990B
		public int Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x0002B713 File Offset: 0x00029913
		public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
		{
			return this.GetData(isCropped, xCrop, yCrop, true);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00162B1C File Offset: 0x00160D1C
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

		// Token: 0x0600383F RID: 14399 RVA: 0x0002B71F File Offset: 0x0002991F
		public void Clear()
		{
			this.Clear(0f);
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00162BC4 File Offset: 0x00160DC4
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

		// Token: 0x06003841 RID: 14401 RVA: 0x0002B72C File Offset: 0x0002992C
		private double GeneratePlanar(double x, double y)
		{
			return this.m_generator.GetValue(x, 0.0, y);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x0002B744 File Offset: 0x00029944
		public void GeneratePlanar(double left, double right, double top, double bottom)
		{
			this.GeneratePlanar(left, right, top, bottom, true);
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x00162C04 File Offset: 0x00160E04
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

		// Token: 0x06003844 RID: 14404 RVA: 0x00162DAC File Offset: 0x00160FAC
		private double GenerateCylindrical(double angle, double height)
		{
			double x = Math.Cos(angle * 0.017453292519943295);
			double z = Math.Sin(angle * 0.017453292519943295);
			return this.m_generator.GetValue(x, height, z);
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x00162DEC File Offset: 0x00160FEC
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

		// Token: 0x06003846 RID: 14406 RVA: 0x00162F0C File Offset: 0x0016110C
		private double GenerateSpherical(double lat, double lon)
		{
			double num = Math.Cos(0.017453292519943295 * lat);
			return this.m_generator.GetValue(num * Math.Cos(0.017453292519943295 * lon), Math.Sin(0.017453292519943295 * lat), num * Math.Sin(0.017453292519943295 * lon));
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00162F6C File Offset: 0x0016116C
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

		// Token: 0x06003848 RID: 14408 RVA: 0x0002B752 File Offset: 0x00029952
		public Texture2D GetTexture()
		{
			return this.GetTexture(GradientPresets.Grayscale);
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x0016308C File Offset: 0x0016128C
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

		// Token: 0x0600384A RID: 14410 RVA: 0x00163180 File Offset: 0x00161380
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

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600384B RID: 14411 RVA: 0x0002B75F File Offset: 0x0002995F
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x0002B767 File Offset: 0x00029967
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0002B783 File Offset: 0x00029983
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

		// Token: 0x040026E7 RID: 9959
		public static readonly double South = -90.0;

		// Token: 0x040026E8 RID: 9960
		public static readonly double North = 90.0;

		// Token: 0x040026E9 RID: 9961
		public static readonly double West = -180.0;

		// Token: 0x040026EA RID: 9962
		public static readonly double East = 180.0;

		// Token: 0x040026EB RID: 9963
		public static readonly double AngleMin = -180.0;

		// Token: 0x040026EC RID: 9964
		public static readonly double AngleMax = 180.0;

		// Token: 0x040026ED RID: 9965
		public static readonly double Left = -1.0;

		// Token: 0x040026EE RID: 9966
		public static readonly double Right = 1.0;

		// Token: 0x040026EF RID: 9967
		public static readonly double Top = -1.0;

		// Token: 0x040026F0 RID: 9968
		public static readonly double Bottom = 1.0;

		// Token: 0x040026F1 RID: 9969
		private int m_width;

		// Token: 0x040026F2 RID: 9970
		private int m_height;

		// Token: 0x040026F3 RID: 9971
		private float[,] m_data;

		// Token: 0x040026F4 RID: 9972
		private int m_ucWidth;

		// Token: 0x040026F5 RID: 9973
		private int m_ucHeight;

		// Token: 0x040026F6 RID: 9974
		private int m_ucBorder = 1;

		// Token: 0x040026F7 RID: 9975
		private float[,] m_ucData;

		// Token: 0x040026F8 RID: 9976
		private float m_borderValue = float.NaN;

		// Token: 0x040026F9 RID: 9977
		private ModuleBase m_generator;

		// Token: 0x040026FA RID: 9978
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
