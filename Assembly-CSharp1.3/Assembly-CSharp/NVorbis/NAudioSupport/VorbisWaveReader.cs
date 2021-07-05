using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	// Token: 0x0200000D RID: 13
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		// Token: 0x0600002C RID: 44 RVA: 0x000039B2 File Offset: 0x00001BB2
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000039E7 File Offset: 0x00001BE7
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003A1D File Offset: 0x00001C1D
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
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00003A43 File Offset: 0x00001C43
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003A4C File Offset: 0x00001C4C
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003A94 File Offset: 0x00001C94
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00003ADC File Offset: 0x00001CDC
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

		// Token: 0x06000033 RID: 51 RVA: 0x00003B38 File Offset: 0x00001D38
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

		// Token: 0x06000034 RID: 52 RVA: 0x00003B9B File Offset: 0x00001D9B
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00003BAB File Offset: 0x00001DAB
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003BC5 File Offset: 0x00001DC5
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003BD2 File Offset: 0x00001DD2
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00003BDA File Offset: 0x00001DDA
		public int? NextStreamIndex { get; set; }

		// Token: 0x0600003A RID: 58 RVA: 0x00003BE4 File Offset: 0x00001DE4
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
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003C29 File Offset: 0x00001E29
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003C38 File Offset: 0x00001E38
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
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003C8E File Offset: 0x00001E8E
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003C9B File Offset: 0x00001E9B
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003CB5 File Offset: 0x00001EB5
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003CC2 File Offset: 0x00001EC2
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003CCF File Offset: 0x00001ECF
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003CDC File Offset: 0x00001EDC
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		// Token: 0x0400001A RID: 26
		private VorbisReader _reader;

		// Token: 0x0400001B RID: 27
		private WaveFormat _waveFormat;

		// Token: 0x0400001C RID: 28
		[ThreadStatic]
		private static float[] _conversionBuffer;
	}
}
