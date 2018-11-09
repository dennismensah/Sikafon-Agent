using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sikafon.Interfaces
{
    public interface IQrCodeScanner
    {
        Task<string> ScanAsync();
    }
}
