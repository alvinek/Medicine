using System.Collections.Generic;

namespace Medicine
{
    public class DbBrowser
    {
        enum Navigation
        {
            Next,
            Previous,
            ByID,
            Exit,
            Incorrect
        }
        public static void Browse(ref MedicineDb mainDb)
        {
            int currentId = 1;
            while(true)
            {
                var medicine = mainDb.GetMedicineById(currentId);
                if (medicine == null)
                {
                    ConsoleGUI.Render("Brak leku o ID " + currentId + " w bazie danych");
                }
                else
                {
                    RenderView(medicine);
                }
                Navigation choice = MenuChoice();
                if (choice != Navigation.Exit)
                {
                    if(choice == Navigation.Next)
                    {
                        currentId++;
                        continue;
                    }

                    if (choice == Navigation.Previous)
                    {
                        if(currentId != 1) currentId--;
                        continue;
                    }

                    if(choice == Navigation.ByID)
                    {
                        var z = ConsoleGUI.PromptRender("ID: ");
                        if (int.TryParse(z, out int s))
                            currentId = s;
                        else
                            ConsoleGUI.ErrorRender("ID: " + z + " nie jest poprawne", true);
                    }

                    if(choice == Navigation.Incorrect)
                    {
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private static void RenderView(Medicine medicine)
        {
            List<string> toRender = new List<string>();

            toRender.Add("Id w bazie: " + medicine.Id);
            toRender.Add("Nazwa leku: \"" + medicine.MedicineName + "\"");
            toRender.Add("Producent: " + medicine.MedicineProducer);
            toRender.Add("Ilość leku: " + medicine.MedicineQuantity + " " + medicine.MedicineQuantityType);
            toRender.Add("Cena normalna: " + medicine.MedicinePrice);

            if(medicine.MedicineRefundPossible)
            {
                toRender.Add("Refundacja tego leku jest możliwa i wynosi " + medicine.MedicineRefundPercentage + "%");
                toRender.Add("Cena po refundacji: " + medicine.MedicinePriceWithRefund);
            }
            else
            {
                toRender.Add("Refundacja tego leku nie jest możliwa");
            }

            ConsoleGUI.Render(toRender);
        }

        private static Navigation MenuChoice()
        {
            var choice = ConsoleGUI.PromptRender("n - Następny, p - Poprzedni, s - Skocz do ID, w - Wyjście").Trim();

            switch (choice)
            {
                case "n":
                    return Navigation.Next;
                case "p":
                    return Navigation.Previous;
                case "s":
                    return Navigation.ByID;
                case "w":
                    return Navigation.Exit;

                default:
                    return Navigation.Incorrect;
            }
        }
    }
}
