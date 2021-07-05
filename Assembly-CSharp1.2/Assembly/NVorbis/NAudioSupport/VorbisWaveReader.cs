using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	// Token: 0x02000004 RID: 4
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000068BC File Offset: 0x00004ABC
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000068F1 File Offset: 0x00004AF1
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00006927 File Offset: 0x00004B27
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._reader != null)
			{
				this._reader.Dispose();
				this._reader = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000694D File Offset: 0x00004B4D
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00079ED4 File Offset: 0x000780D4
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00079F1C File Offset: 0x0007811C
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00079F64 File Offset: 0x00078164
		public override long Position
		{
			get
			{
				return (long)(this._reader.DecodedTime.TotalMilliseconds * (double)this._reader.SampleRate * (double)this._reader.Channels * 4.0);
			}
			set
			{
				if (value < 0L || value > this.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._reader.DecodedTime = TimeSpan.FromSeconds((double)value / (double)this._reader.SampleRate / (double)this._reader.Channels / 4.0);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00079FC0 File Offset: 0x000781C0
		public override int Read(byte[] buffer, int offset, int count)
		{
			count /= 4;
			count -= count % this._reader.Channels;
			float[] array;
			if ((array = VorbisWaveReader._conversionBuffer) == null)
			{
				array = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			float[] array2 = array;
			if (array2.Length < count)
			{
				array2 = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			int num = this.Read(array2, 0, count) * 4;
			Buffer.BlockCopy(array2, 0, buffer, offset, num);
			return num;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00006955 File Offset: 0x00004B55
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00006965 File Offset: 0x00004B65
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00006972 File Offset: 0x00004B72
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000697F File Offset: 0x00004B7F
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000698C File Offset: 0x00004B8C
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00006994 File Offset: 0x00004B94
		public int? NextStreamIndex { get; set; }

		// Token: 0x0600001C RID: 28 RVA: 0x0007A024 File Offset: 0x00078224
		public bool GetNextStreamIndex()
		{
			if (this.NextStreamIndex == null)
			{
				int streamCount = this._reader.StreamCount;
				if (this._reader.FindNextStream())
				{
					this.NextStreamIndex = new int?(streamCount);
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000699D File Offset: 0x00004B9D
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0007A06C File Offset: 0x0007826C
		public int CurrentStream
		{
			get
			{
				return this._reader.StreamIndex;
			}
			set
			{
				if (!this._reader.SwitchStreams(value))
				{
					throw new InvalidDataException("The selected stream is not a valid Vorbis stream!");
				}
				if (this.NextStreamIndex != null && value == this.NextStreamIndex.Value)
				{
					this.NextStreamIndex = null;
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000069AA File Offset: 0x00004BAA
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000069B7 File Offset: 0x00004BB7
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000069C4 File Offset: 0x00004BC4
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000069D1 File Offset: 0x00004BD1
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000069DE File Offset: 0x00004BDE
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000069EB File Offset: 0x00004BEB
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000069F8 File Offset: 0x00004BF8
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		// Token: 0x04000006 RID: 6
		private VorbisReader _reader;

		// Token: 0x04000007 RID: 7
		private WaveFormat _waveFormat;

		// Token: 0x04000008 RID: 8
		[ThreadStatic]
		private static float[] _conversionBuffer;
	}
}
