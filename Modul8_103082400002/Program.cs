using System;

namespace modul8_103082400002
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("JURNAL MODUL 8 - NIM: 103082400002");
            Console.WriteLine("APLIKASI BANK TRANSFER");
            Console.WriteLine("=========================================\n");

            // ==================================================
            // LOAD KONFIGURASI DARI FILE
            // ==================================================
            BankTransferConfig config = BankTransferConfig.LoadConfig();

            // ==================================================
            // MENU GANTI BAHASA (ENGLISH/INDONESIA)
            // ==================================================
            Console.WriteLine($"🌐 Bahasa saat ini: {(config.lang == "en" ? "ENGLISH" : "INDONESIA")}");
            Console.Write("Ganti bahasa? (y/n): ");
            string gantiBahasa = Console.ReadLine()?.ToLower();

            if (gantiBahasa == "y" || gantiBahasa == "yes")
            {
                if (config.lang == "en")
                {
                    config.lang = "id";
                    Console.WriteLine("✅ Bahasa diubah menjadi INDONESIA");
                }
                else
                {
                    config.lang = "en";
                    Console.WriteLine("✅ Bahasa diubah menjadi ENGLISH");
                }
                config.SaveConfig(); // Simpan perubahan ke file JSON
                Console.WriteLine("✅ Perubahan bahasa telah disimpan ke file konfigurasi\n");
            }
            else
            {
                Console.WriteLine($"✅ Bahasa tetap: {(config.lang == "en" ? "ENGLISH" : "INDONESIA")}\n");
            }

            // ==================================================
            // TAMPILKAN KONFIGURASI SAAT INI
            // ==================================================
            Console.WriteLine("--- KONFIGURASI SAAT INI ---");
            Console.WriteLine($"Bahasa: {config.lang}");
            Console.WriteLine($"Threshold: {config.transfer.threshold:N0}");
            Console.WriteLine($"Low Fee: {config.transfer.low_fee:N0}");
            Console.WriteLine($"High Fee: {config.transfer.high_fee:N0}");
            Console.WriteLine($"Metode Transfer: {string.Join(", ", config.methods)}");
            Console.WriteLine();

            // ==================================================
            // D-i: INPUT JUMLAH TRANSFER
            // ==================================================
            Console.WriteLine("--- FORM TRANSFER BANK ---");
            Console.Write(config.GetMessage("input_amount") + " ");

            string input = Console.ReadLine();
            int jumlahTransfer = Convert.ToInt32(input);

            Console.WriteLine();

            // ==================================================
            // D-ii: HITUNG BIAYA TRANSFER DAN TOTAL BIAYA
            // ==================================================
            int biayaTransfer = config.GetTransferFee(jumlahTransfer);
            int totalBiaya = jumlahTransfer + biayaTransfer;

            Console.WriteLine(config.GetMessage("transfer_fee") + biayaTransfer.ToString("N0"));
            Console.WriteLine(config.GetMessage("total_amount") + totalBiaya.ToString("N0"));
            Console.WriteLine();

            // ==================================================
            // D-iii & D-iv: PILIH METODE TRANSFER
            // ==================================================
            Console.WriteLine(config.GetMessage("select_method"));

            // Tampilkan metode transfer dengan numbering
            for (int i = 0; i < config.methods.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {config.methods[i]}");
            }

            Console.Write(config.GetMessage("choose_method"));
            string pilihanMetode = Console.ReadLine();
            Console.WriteLine();

            // Validasi pilihan metode
            int metodeIndex;
            if (!int.TryParse(pilihanMetode, out metodeIndex) || metodeIndex < 1 || metodeIndex > config.methods.Count)
            {
                Console.WriteLine(config.GetMessage("invalid_choice"));
                Console.WriteLine(config.GetMessage("cancel"));
                Console.WriteLine("\nTekan Enter untuk keluar...");
                Console.ReadLine();
                return;
            }

            // ==================================================
            // D-v: KONFIRMASI TRANSFER
            // ==================================================
            Console.Write(config.GetMessage("confirm") + " ");
            string konfirmasi = Console.ReadLine();

            Console.WriteLine();

            // ==================================================
            // D-vi & D-vii: HASIL KONFIRMASI
            // ==================================================
            string expectedConfirmation = (config.lang == "en") ? config.confirmation.en : config.confirmation.id;

            if (konfirmasi.ToLower() == expectedConfirmation.ToLower())
            {
                Console.WriteLine("✅ " + config.GetMessage("success"));
                Console.WriteLine($"✅ Transfer sebesar {jumlahTransfer:N0} dengan metode {config.methods[metodeIndex - 1]} berhasil!");
            }
            else
            {
                Console.WriteLine("❌ " + config.GetMessage("cancel"));
                Console.WriteLine("❌ Konfirmasi tidak sesuai, transfer dibatalkan.");
            }

            Console.WriteLine("\n=========================================");
            Console.WriteLine("Program selesai! Tekan Enter untuk keluar...");
            Console.ReadLine();
        }
    }
}