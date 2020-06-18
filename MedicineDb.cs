using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Medicine
{
    public class Medicine
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public int MedicineQuantity { get; set; }
        public string MedicineQuantityType { get; set; }
        public string MedicineProducer { get; set; }
        public bool MedicineRefundPossible { get; set; }
        public int MedicineRefundPercentage { get; set; }
        public decimal MedicinePrice { get; set; }
        public decimal MedicinePriceWithRefund { get; set; }

        public Medicine(int id, string medName, int quantity, string quantityType, string producer, decimal price, bool refundPossible = false, int refundPercentage = 0)
        {
            Id = id;
            MedicineName = medName;
            MedicineQuantity = quantity;
            MedicineQuantityType = quantityType;
            MedicineProducer = producer;
            MedicineRefundPossible = refundPossible;
            MedicineRefundPercentage = refundPercentage;
            MedicinePrice = price;

            if (refundPossible)
            {
                if (MedicineRefundPercentage != 0)
                {
                    MedicineRefundPercentage = refundPercentage;
                    MedicinePriceWithRefund = Math.Round(MedicinePrice - (MedicineRefundPercentage / (decimal)100 * MedicinePrice), 2);
                }
            }
            else
            {
                MedicineRefundPercentage = 0;
                MedicinePriceWithRefund = -1;
            }
        }

    }
    public class MedicineDb
    {
        List<Medicine> medicineDb = new List<Medicine>();
        public MedicineDb(List<string> file)
        {
            foreach (var line in file)
            {
                if (line.StartsWith("#")) continue;

                var data = line.Split(',');
                if (data.Count() != 8) continue;

                int id = int.Parse(data[0]);
                string name = data[1];
                int quantity = int.Parse(data[2]);
                string quantityType = data[3];
                string producer = data[4];

                bool isRefund;

                if (data[5].ToLower() == "false")
                {
                    isRefund = false;
                }
                else
                {
                    isRefund = true;
                }

                int refundPercentage = int.Parse(data[6]);

                decimal price = decimal.Parse(data[7]);

                Medicine medicine = new Medicine(id, name, quantity, quantityType, producer, price, isRefund, refundPercentage);

                medicineDb.Add(medicine);
            }
        }

        public Medicine GetMedicineById(int id)
        {
            if (id < 0) return null;

            var medicineToBeReturned = medicineDb.Where(x => x.Id == id);
            if (medicineToBeReturned != null && medicineToBeReturned.Any())
                return medicineToBeReturned.First();

            return null;
        }

        private bool DeleteMedicine(int id)
        {
            var medicineRemoved = GetMedicineById(id);

            bool success = false;

            if (medicineRemoved != null)
            {
                success = medicineDb.Remove(medicineRemoved);
                Flush();
            }

            return success;
        }

        private void Flush()
        {
            DbFiles dbFiles = new DbFiles();

            File.Delete(dbFiles.MainDbFile);

            List<string> newFile = new List<string>();

            foreach (var data in medicineDb)
            {
                newFile.Add($"{data.Id},"
                    + $"{data.MedicineName},"
                    + $"{data.MedicineQuantity},"
                    + $"{data.MedicineQuantityType},"
                    + $"{data.MedicineProducer},"
                    + $"{data.MedicineRefundPossible.ToString().ToLower()},"
                    + $"{data.MedicineRefundPercentage},"
                    + $"{data.MedicinePrice}");
            }

            File.WriteAllLines(dbFiles.MainDbFile, newFile);
        }

        public void RemoveMedicine()
        {
            var z = ConsoleGUI.PromptRender("Podaj ID leku do usunięcia z bazy danych");
            if (int.TryParse(z, out int i))
            {
                if (!DeleteMedicine(i))
                {
                    ConsoleGUI.ErrorRender("Nie udało się usunąć leku o ID " + i + " z bazy", true);
                }
            }
            else
            {
                ConsoleGUI.ErrorRender("Podano niepoprawne ID " + z, true);
            }
        }

        public void SearchMedicine()
        {
            ConsoleGUI.Render("Wyszukiwanie leku");
            var searchString = ConsoleGUI.PromptRender("Podaj frazę do wyszukania: ").Trim();
            var foundMedicines = medicineDb.Where(x => x.MedicineName.ToLower().Contains(searchString.ToLower()));

            if (foundMedicines.Any())
            {
                List<string> render = new List<string>();
                render.Add("Znaleziono: ");
                foreach (var found in foundMedicines)
                {
                    render.Add($"ID: {found.Id}");
                    render.Add($"Nazwa: {found.MedicineProducer} {found.MedicineName}");
                    render.Add($"Cena: {found.MedicinePrice}");
                    render.Add($"Ilość: {found.MedicineQuantity}{found.MedicineQuantityType}");
                }
                ConsoleGUI.Render(render);
                ConsoleGUI.PromptRender("Dowolny przycisk powróci do MENU");
            }
            else
            {
                ConsoleGUI.ErrorRender("Nie znaleziono produktów o nazwie " + searchString, true);
                ConsoleGUI.PromptRender("Dowolny przycisk powróci do MENU");
            }
        }

        public int GetNextId() => medicineDb.Max(x => x.Id) + 1;

        public void AddMedicine()
        {
            string medicineName = ConsoleGUI.PromptRender("Nazwa leku: ");
            string medicineProducer = ConsoleGUI.PromptRender("Producent: ");
            int medicineQuantity = ConsoleGUI.PromptRenderInt("Ilość: ");
            string quantityType = ConsoleGUI.PromptRender("Określenie jednostki: ");
            decimal price = ConsoleGUI.PromptRenderDecimal("Cena: ");

            bool refundPossible = ConsoleGUI.PromptRender("Czy jest dostępna refundacja? (t/n)").Trim().ToLower().First() == 't';
            Medicine medicine;

            if (refundPossible)
            {
                int refundPercentage = ConsoleGUI.PromptRenderInt("% refundacji: ");
                medicine = new Medicine(GetNextId(), medicineName, medicineQuantity, quantityType, medicineProducer, price, refundPossible, refundPercentage);
            }
            else
            {
                medicine = new Medicine(GetNextId(), medicineName, medicineQuantity, quantityType, medicineProducer, price);
            }

            medicineDb.Add(medicine);
            Flush();
        }
    }
}
