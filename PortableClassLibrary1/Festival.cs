using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using ProjectMVVM.Model;

namespace ProjectMVVM.Model
{
    public class Festival
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Een naam is verplicht.")]
        [StringLength(20, ErrorMessage = "gelieve een naam korter dan 20 karakters te gebruiken")]
        public String Name { get; set; }

        public DateTime StarDate { get; set; }

        public DateTime EndDate { get; set; }
        public String Grondplan { get; set; }
        public String Logo { get; set; }

        public static Festival getFestival()
        {
            List<Festival> lijst = new List<Festival>();

            string SQL = "SELECT * FROM Festival";
            DbDataReader reader = Database.GetData(SQL);

            while (reader.Read())
            {
                Festival anieuw = new Festival();
                anieuw.Id = Int32.Parse(reader["Id"].ToString());
                anieuw.Name = reader["Name"].ToString();
                anieuw.StarDate = (DateTime)reader["Startdag"];
                anieuw.EndDate = (DateTime)reader["Einddag"];
                anieuw.Logo = reader["Logo"].ToString();
                anieuw.Grondplan = reader["Grondplan"].ToString();

                lijst.Add(anieuw);
            }
            return lijst.ElementAt(0);
        }
    }
}
