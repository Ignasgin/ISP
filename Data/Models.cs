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
        
            [Required(ErrorMessage = "Markė yra privaloma.")]
            public string Marke { get; set; }

            [Required(ErrorMessage = "Modelis yra privalomas.")]
            public string Modelis { get; set; }

            [Required(ErrorMessage = "Išleidimo metai yra privalomi.")]
            [Range(1900, 2100, ErrorMessage = "Išleidimo metai turi būti tarp 1900 ir 2100.")]
            public int Metai { get; set; }

            [Required(ErrorMessage = "Litražas yra privalomas.")]
            [Range(0, 10000, ErrorMessage = "Litražas turi būti teisingas.")]
            public double? Litrazas { get; set; }

            [Required(ErrorMessage = "Galia yra privaloma.")]
            public int Galia { get; set; }

            [Required(ErrorMessage = "Numeris yra privalomas.")]
            public string Numeris { get; set; }

            [Required(ErrorMessage = "Vin kodas yra privalomas.")]
            public string Vin { get; set; }

            [Required(ErrorMessage = "Vietų skaičius yra privalomas.")]
            [Range(1, 50, ErrorMessage = "Vietų skaičius turi būti tarp 1 ir 50.")]
            public int Vietu_Sk { get; set; }

            [Required(ErrorMessage = "Kuro tipas yra privalomas.")]
            public string Kuro_Tipas { get; set; }

            [Required(ErrorMessage = "Kėbulo tipas yra privalomas.")]
            public string Kebulo_Tipas { get; set; }

            [Required(ErrorMessage = "Rida yra privaloma.")]
            public int Rida { get; set; }

            public double? Ivertinimu_Vidurkis { get; set; }

            [Required(ErrorMessage = "Statusas yra privalomas.")]
            public string Statusas { get; set; }

            [Required(ErrorMessage = "Kaina yra privaloma.")]
            [Range(0, 1000000, ErrorMessage = "Kaina turi būti teisinga.")]
            public decimal Kaina { get; set; }
        

        [Key]
        public int Id_Automobilis { get; set; }

        public ICollection<Draudimas>? Draudimai { get; set; }
        public ICollection<Perziuretas_Automobilis>? Perziuretas_Automobilis { get; set; }
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

        public Automobilis Automobilis { get; set; }
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

    public class Perziuretas_Automobilis
    {

        public DateTime Data { get; set; }

        [Key]
        public int Id_Perziuretas_Automobilis { get; set; }


        public int Fk_Automobilis_Id_Automobilis { get; set; }

        public int Fk_Naudotojas_Id_Naudotojas { get; set; }
        public Automobilis Automobilis { get; set; }

    }
    public class Rezervacija
    {
        [Key]
        public int Id_Rezervacija { get; set; }


        public DateTime Pradzia { get; set; }

        public DateTime Pabaiga { get; set; }

        public DateTime Pateikimo_Data { get; set; }

        public string Paemimo_Vieta { get; set; }

        public string Atidavimo_Vieta { get; set; }

        // Foreign Key relationships
        public int Fk_Automobilis_Id_Automobilis { get; set; }
        public int Fk_Naudotojas_Id_Naudotojas { get; set; }
        


    }
    public class Trumpalaike_Rezervacija
    {
        [Key]
        public int Id_Trumpalaike_Rezervacija { get; set; }


        public DateTime Pateikimo_Data { get; set; }

        public int Laikas { get; set; }

        // Foreign Key relationships
        public int Fk_Automobilis_Id_Automobilis { get; set; }
        public int Fk_Naudotojas_Id_Naudotojas { get; set; }
        

    }


}
