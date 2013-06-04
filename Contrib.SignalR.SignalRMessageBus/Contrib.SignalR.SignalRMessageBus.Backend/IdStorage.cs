using System.IO;
using System.IO.IsolatedStorage;

namespace Contrib.SignalR.SignalRMessageBus.Backend
{
	public class IdStorage
	{
		private const string FileName = "LastKnownId.key";

		public void OnStart()
		{
			var store = IsolatedStorageFile.GetUserStoreForAssembly();
			if (store.FileExists(FileName))
			{
				using (var stream = store.OpenFile(FileName, FileMode.OpenOrCreate, FileAccess.Read))
				using (var streamReader = new StreamReader(stream))
				{
					var result = streamReader.ReadToEnd();

					ulong value;
					if (ulong.TryParse(result, out value))
					{
						LastId = value;
						return;
					}
				}
			}
			LastId = ulong.MinValue;
		}

		public void OnStop()
		{
			var store = IsolatedStorageFile.GetUserStoreForAssembly();

			using (var stream = store.OpenFile(FileName, FileMode.OpenOrCreate, FileAccess.Write))
			using (var streamWriter = new StreamWriter(stream))
			{
				streamWriter.Write(LastId);
			}
		}

		public static ulong LastId { get; set; }
	}
}
