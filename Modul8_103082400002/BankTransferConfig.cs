using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace modul8_103082400002
{
    public class TransferConfig
    {
        public int threshold { get; set; }
        public int low_fee { get; set; }
        public int high_fee { get; set; }
    }

    public class ConfirmationConfig
    {
        public string en { get; set; }
        public string id { get; set; }
    }

    public class BankTransferConfig
    {
        // Properties sesuai format JSON
        public string lang { get; set; }
        public TransferConfig transfer { get; set; }
        public List<string> methods { get; set; }
        public ConfirmationConfig confirmation { get; set; }

        // Nama file konfigurasi
        private const string fileName = "bank_transfer_config.json";

        // Constructor dengan nilai default
        public BankTransferConfig()
        {
            lang = "en";
            transfer = new TransferConfig
            {
                threshold = 25000000,
                low_fee = 6500,
                high_fee = 15000
            };
            methods = new List<string> { "RTO (real-time)", "SKN", "RTGS", "BI FAST" };
            confirmation = new ConfirmationConfig
            {
                en = "yes",
                id = "ya"
            };
        }

        // Method untuk membaca konfigurasi dari file JSON
        public static BankTransferConfig LoadConfig()
        {
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<BankTransferConfig>(json);
            }
            else
            {
                BankTransferConfig defaultConfig = new BankTransferConfig();
                defaultConfig.SaveConfig();
                return defaultConfig;
            }
        }

        // Method untuk menyimpan konfigurasi ke file JSON
        public void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        // Method untuk mendapatkan pesan berdasarkan bahasa
        // Method untuk mendapatkan pesan berdasarkan bahasa
        public string GetMessage(string messageKey)
        {
            if (lang == "en")
            {
                switch (messageKey)
                {
                    case "input_amount": return "Please insert the amount of money to transfer:";
                    case "transfer_fee": return "Transfer fee = ";
                    case "total_amount": return "Total amount = ";
                    case "select_method": return "Select transfer method:";
                    case "choose_method": return "Choose method (1-4): ";
                    case "confirm": return $"Please type \"{confirmation.en}\" to confirm the transaction:";
                    case "success": return "The transfer is completed";
                    case "cancel": return "Transfer is cancelled";
                    case "invalid_choice": return "Invalid choice!";
                    default: return "";
                }
            }
            else // bahasa Indonesia
            {
                switch (messageKey)
                {
                    case "input_amount": return "Masukkan jumlah uang yang akan di-transfer:";
                    case "transfer_fee": return "Biaya transfer = ";
                    case "total_amount": return "Total biaya = ";
                    case "select_method": return "Pilih metode transfer:";
                    case "choose_method": return "Pilih metode (1-4): ";
                    case "confirm": return $"Ketik \"{confirmation.id}\" untuk mengkonfirmasi transaksi:";
                    case "success": return "Proses transfer berhasil";
                    case "cancel": return "Transfer dibatalkan";
                    case "invalid_choice": return "Pilihan tidak valid!";
                    default: return "";
                }
            }
        }

        // Method untuk mendapatkan biaya transfer berdasarkan jumlah
        public int GetTransferFee(int amount)
        {
            if (amount <= transfer.threshold)
            {
                return transfer.low_fee;
            }
            else
            {
                return transfer.high_fee;
            }
        }
    }
}