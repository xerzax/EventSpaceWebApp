using Application.Interfaces.Services;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace Infrastructure.Implementation.Services
{
	public class QRCodeGeneratorService : IQRCodeGeneratorService
	{
		public byte[] GenerateQRCode(string text)
		{
			byte[] QRCode = new byte[0];
			if (!string.IsNullOrEmpty(text))
			{
				
				QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
				QRCodeData data = qRCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
				BitmapByteQRCode bitmap = new BitmapByteQRCode(data);
				QRCode = bitmap.GetGraphic(20);
			}
			return QRCode;
		}
	}
}
