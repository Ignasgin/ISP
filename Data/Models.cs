using System.ComponentModel.DataAnnotations;

namespace ISP.Data
{
    public class Atsiliepimas
    {
        [Key]
        public int Id_Atsiliepimas { get; set; }


        public int Ivertinimas { get; set; }
        public string Titulas { get; set; }
        public string Komentaras { get; set; }
        public DateTime Data { get; set; }
        public bool Rekomenduotu_Kitiems { get; set; }

        


        public int Fk_Naudotojas_Id_Naudotojas { get; set; }
        public int Fk_Automobilis_Id_Automobilis { get; set; }
    }

    public class Automobilis
    {
        public string Marke { get; set; }
        public string Modelis { get; set; }
        public int Metai { get; set; }
        public double Litrazas { get; set; }
        public int Galia { get; set; }
        public decimal Kaina { get; set; }
        public string Statusas { get; set; }
        public string Numeris { get; set; }
        public string Vin { get; set; }
        public int Rida { get; set; }
        public int Vietu_Sk { get; set; }
        public double Ivertinimu_Vidurkis { get; set; }
        public string Kuro_Tipas { get; set; }
        public string Kebulo_Tipas { get; set; }

        [Key]
        public int Id_Automobilis { get; set; }
    }

    public class Draudimas
    {
        public int Poliso_Numeris { get; set; }
        public DateTime Pradzios_Data { get; set; }
        public DateTime Pabaigos_Data { get; set; }
        public string Draudimo_Tipas { get; set; }
        public string Draudimo_Kompanijos_Pavadinimas { get; set; }

        [Key]
        public int Id_Draudimas { get; set; }


        public int Fk_Automobilis_Id_Automobilis { get; set; }
    }

    public class Naudotojas
    {
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public DateTime Gimimo_Data { get; set; }
        public int Stazas { get; set; }
        public string Asmens_Kodas { get; set; }
        public string Miestas { get; set; }
        public string Slaptazodis { get; set; }
        public DateTime Registracijos_Data { get; set; }
        public DateTime Paskutinis_Prisijungimas { get; set; }

        [Key]
        public int Id_Naudotojas { get; set; }
    }


}
