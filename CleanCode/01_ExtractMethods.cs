using System;
using System.IO;

namespace CleanCode
{
	public static class RefactorMethod
	{
		private static void SaveData(string path, byte[] data)
		{
		    var dataToWrite = new Data {Length = data.Length, Offset = 0, RawData = data};
            WriteWithBackup(path, dataToWrite);

			WriteLastChangeTime(path);
		}

	    private static void WriteLastChangeTime(string path)
	    {
	        string timeFilePath = path + ".time";
	        var currentTime = BitConverter.GetBytes(DateTime.Now.Ticks);

	        var dataToWrite = new Data {Length = currentTime.Length, Offset = 0, RawData = currentTime};

            WriteData(dataToWrite, timeFilePath, FileMode.Create);
	    }

	    private static void WriteWithBackup(string path, Data dataToWrite)
	    {
	        WriteData(dataToWrite, path, FileMode.OpenOrCreate);
	        WriteData(dataToWrite, Path.ChangeExtension(path, "bkp"), FileMode.OpenOrCreate);
	    }

	    private static void WriteData(Data data, string path, FileMode mode)
	    {
	        using (var fileStream = new FileStream(path, mode))
	        {
	            fileStream.Write(data.RawData, data.Offset, data.Length);
	        }
	    }
	}

    internal class Data
    {
        public byte[] RawData { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}