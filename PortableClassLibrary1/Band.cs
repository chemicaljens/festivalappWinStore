using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using ProjectMVVM.Model;

namespace ProjectMVVM.Model
{
    class Band 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "De Naam is Verplicht")]
        public String Name { get; set; }
        
        public String Picture { get; set; }

        [Required(ErrorMessage = "De Datum is Verplicht")]
        public String Description { get; set; }

        [Url(ErrorMessage = "vul een juiste URL in ( me https://)")]
        public String Twitter { get; set; }

        [Url(ErrorMessage = "vul een juiste URL in ( me https://)")]
        public String Facebook { get; set; }
        
        public List<Genre> Genres { get; set; }

        internal static Band getbandwithID(int p)
        {
            string SQL = "SELECT * FROM Band where Id="+p;
            DbDataReader reader = Database.GetData(SQL);
            Band anieuw = new Band();
            while (reader.Read())
            {
                
                anieuw.Id = Int32.Parse(reader["Id"].ToString());
                anieuw.Name = reader["Name"].ToString();
                anieuw.Picture = reader["Picture"].ToString();
                anieuw.Description = reader["Despription"].ToString();
                anieuw.Twitter = reader["Twitter"].ToString();
                anieuw.Facebook = reader["Facebook"].ToString();
                anieuw.Genres = Genre.getGenres(anieuw.Id);
                

            }
            return anieuw;
        }
    }
}
