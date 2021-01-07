using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GolayCode
{
    public class FileService
    {
        /// <summary>
        /// Baitai nuskaitomi iš failo
        /// </summary>
        /// <param name="path">Kelias į failą</param>
        /// <returns>Nuskaityti baitai</returns>
        public byte[] ReadAsBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Baitai išvedami į failą
        /// </summary>
        /// <param name="path">Kelias į failą</param>
        /// <param name="bytes">Įrašytini baitai</param>
        public void WriteBytes(string path, IEnumerable<byte> bytes)
        {
            File.WriteAllBytes(path, bytes.ToArray());
        }

        /// <summary>
        /// OS pagalba atidaromas failas su susieta programa
        /// </summary>
        /// <param name="path">Failo</param>
        public void Open(string path)
        {
            new Process
            {
                StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}