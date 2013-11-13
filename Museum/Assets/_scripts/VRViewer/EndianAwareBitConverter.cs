using System;

namespace CustomClasses.IO
{
	public class EndianAwareBitConverter
	{
		private bool swap;
		
		public EndianAwareBitConverter (bool littleEndian)
		{
			swap = littleEndian != BitConverter.IsLittleEndian;
		}
		
		public bool TargetIsLittleEndian
		{
			get { return BitConverter.IsLittleEndian == !swap; }
			set { swap = value != BitConverter.IsLittleEndian; }
		}

		public bool ToBoolean (byte[] value, int startIndex)
		{
			CheckArray<byte> (value, startIndex, 1);
			return BitConverter.ToBoolean (value, startIndex);
		}

		public char ToChar (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 2);
			return BitConverter.ToChar (buffer, startIndex);
		}
		
		public char[] ToCharArray (byte[] value, int startIndex, int count /*chars*/)
		{
			CheckArray<byte> (value, startIndex, count * 2);
			
			char[] buffer = new char[count];
			
			int end = startIndex + (count * 2);			
			for (int i=startIndex, j=0; i<end; i+=2, j++) {
				byte[] tmp = CopyAndSwapBytesIfNeeded (value, i, 2);
				buffer[j] = BitConverter.ToChar (tmp, 0);
			}
			
			return buffer;
		}

		public short ToInt16 (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 2);
			return BitConverter.ToInt16 (buffer, startIndex);
		}

		public int ToInt32 (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 4);
			return BitConverter.ToInt32 (buffer, startIndex);
		}

		public long ToInt64 (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 8);
			return BitConverter.ToInt64 (buffer, startIndex);
		}

		public ushort ToUInt16 (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 2);
			return BitConverter.ToUInt16 (buffer, startIndex);
		}

		public uint ToUInt32 (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 4);
			return BitConverter.ToUInt32 (buffer, startIndex);
		}

		public ulong ToUInt64 (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 8);
			return BitConverter.ToUInt64 (buffer, startIndex);
		}
		
		public float ToSingle (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 4);
			return BitConverter.ToSingle (buffer, startIndex);
		}
		
		public double ToDouble (byte[] value, int startIndex)
		{
			byte[] buffer = CopyAndSwapBytesIfNeeded (value, startIndex, 8);
			return BitConverter.ToDouble (buffer, startIndex);
		}
		
		public byte[] GetBytes (bool value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}

		public byte[] GetBytes (char value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}
		
		public byte[] GetBytes (char[] value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			
			return GetBytes (value, 0, value.Length);
		}
		
		public byte[] GetBytes (char[] value, int startIndex, int count)
		{
			CheckArray<char> (value, startIndex, count);

			int end = startIndex + count;
			byte[] buffer = new byte[count * 2];
			
			for (int i=startIndex, j=0; i < end; i++, j+=2) {
				byte[] tmp = BitConverter.GetBytes (value[i]);
				if (swap)
					Array.Reverse (tmp);
				
				buffer[j] = tmp[0];
				buffer[j+1] = tmp[1];
			}
			
			return buffer;
		}

		public byte[] GetBytes (short value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}

		public byte[] GetBytes (int value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}

		public byte[] GetBytes (long value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}
		
		public byte[] GetBytes (ushort value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}

		public byte[] GetBytes (uint value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}

		public byte[] GetBytes (ulong value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}

		public byte[] GetBytes (float value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}
		
		public byte[] GetBytes(double value)
		{
			byte[] tmp = BitConverter.GetBytes (value);
			if (swap)
				Array.Reverse (tmp);
			return tmp;
		}
		
		[System.Diagnostics.DebuggerHidden]
		protected void CheckArray<T> (T[] value, int startIndex, int bytesRequired)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			if (startIndex < 0 || startIndex > (value.Length - bytesRequired))
				throw new ArgumentOutOfRangeException ("startIndex");
		}
		
		[System.Diagnostics.DebuggerHidden]
		protected byte[] CopyAndSwapBytesIfNeeded (byte[] value, int startIndex, int length)
		{
			CheckArray<byte> (value, startIndex, 1);
			
			byte[] buffer = new byte[length];
			Array.Copy (value, startIndex, buffer, 0, length);
			
			if (swap)
				Array.Reverse (buffer, startIndex, length);
			
			return buffer;
		}
	}
	
	public class LittleEndianBitConverter : EndianAwareBitConverter
	{
		public LittleEndianBitConverter ()
			: base (true)
		{
		}
	}
	
	public class BigEndianBitConverter : EndianAwareBitConverter
	{
		public BigEndianBitConverter ()
			: base (false)
		{
		}
	}
}
