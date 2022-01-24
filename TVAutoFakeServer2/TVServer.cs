using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TVAutoFakeServer2
{
	class TVServer
	{
		// do code này mình dịch ngược từ bản exe năm 2016 do mình không giữ
		// source nên code đã được Visual Studio optimize, nhiều đoạn sẽ hơi khác với cách con người code
		// ví dụ như cái biến writing này
		bool writing = false;
		TcpListener listener;

		public TVServer()
		{
			this.listener = new TcpListener(IPAddress.Parse("103.92.26.100"), 63001);
			this.listener.Start();
		}

		public void Start()
        {
			// bind len IP cua server TVAuto

			while (true)
			{
				TcpClient client = this.listener.AcceptTcpClient();
				new Thread(ServerServe).Start(client);
			}
		}

		private void ServerServe(object obj)
		{
			Thread.CurrentThread.IsBackground = true;
			TcpClient server = (TcpClient)obj;
			StreamWriter writer = new StreamWriter(server.GetStream());
			StreamReader reader = new StreamReader(server.GetStream());

			writer.AutoFlush = true;

			try
			{
				while (true)
				{
					string message = ReadUntilEnd(server);

					message = FromHex(message);

					string ret = "";
					if (message.StartsWith("10008")) // get time
					{
						ret = GetTime();
					}
					else if (message.StartsWith("10002")) // login
					{
						ret += "31303032300D0A31" + "\r\n"; // 31 ở cuối tương ứng số 1 tức là chức năng enabled
						ret += "31303031360D0A31" + "\r\n";
						ret += "31303031320D0A31" + "\r\n";
						ret += "31303031340D0A31" + "\r\n";
						ret += "31303031350D0A31" + "\r\n";
						ret += "31303031330D0A31" + "\r\n";
						ret += "31303031390D0A31" + "\r\n";
						ret += "31303031370D0A31" + "\r\n";
						ret += "31303032320D0A31" + "\r\n";
						ret += "31303032330D0A31" + "\r\n";
						ret += "31303033390D0A31" + "\r\n";
						ret += "31303033380D0A31" + "\r\n";
						ret += "3130303130";
					}
					else if (message.StartsWith("10011")) // query chuc nang
					{
						ret += "31303032300D0A31" + "\r\n";
						ret += "31303031360D0A31" + "\r\n";
						ret += "31303031320D0A31" + "\r\n";
						ret += "31303031340D0A31" + "\r\n";
						ret += "31303031350D0A31" + "\r\n";
						ret += "31303031330D0A31" + "\r\n";
						ret += "31303031390D0A31" + "\r\n";
						ret += "31303031370D0A31" + "\r\n";
						ret += "31303032320D0A31" + "\r\n";
						ret += "31303032330D0A31" + "\r\n";
						ret += "31303033390D0A31" + "\r\n";
						ret += "31303033380D0A31";
					}

					writer.Write(ret + "\r\n");
				}
			}
			catch (Exception ex)
            {
				
			}
		}

		private string ReadUntilEnd(TcpClient client)
		{
			Socket socket = client.Client;

			byte[] buffer = new byte[150];
			int pointer = 0;
			while (true)
			{
				socket.Receive(buffer, pointer, 1, SocketFlags.None);

				// neu nhan duoc 0d 0a thi break;
				if (buffer.Length > 1 && (buffer[pointer] == 0x0a && buffer[pointer - 1] == 0x0d))
					break;

				pointer++;
			}

			return Encoding.ASCII.GetString(buffer, 0, pointer - 1); // bo di 2 ki tu cuoi cung
		}

		// update thời gian của server xuống cho client. Dưới footer của tvauto có cái chỗ chạy thời gian là nó
		private string GetTime()
		{
			int thu_trong_tuan = (int)DateTime.Today.DayOfWeek + 1;
			thu_trong_tuan = (thu_trong_tuan == 1 ? 8 : thu_trong_tuan);
			string time = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");

			string ret = "";
			ret += ToHex("10008") + ToHex("Th") + "F820" + ToHex((thu_trong_tuan).ToString()) + "20" + ToHex(time);

			return ret;
		}

		private string ToHex(string str)
		{
			var sb = new StringBuilder();

			var bytes = Encoding.ASCII.GetBytes(str);
			foreach (var t in bytes)
			{
				sb.Append(t.ToString("X2"));
			}

			return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
		}

		private string FromHex(string hexString)
		{
			var bytes = new byte[hexString.Length / 2];
			for (var i = 0; i < bytes.Length; i++)
			{
				bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}

			return Encoding.ASCII.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
		}
	}
}
