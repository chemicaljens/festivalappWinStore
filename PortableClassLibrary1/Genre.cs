using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMVVM.Model
{
    class Genre
    {

        public int Id { get; set; }
        public String Name { get; set; }
        public bool checkgenre{get; set;}

        internal static ObservableCollection<Genre> getGenre()
        {
            ObservableCollection<Genre> lijst = new ObservableCollection<Genre>();

            string SQL = "SELECT * FROM Genre";
            DbDataReader reader = Database.GetData(SQL);

            while (reader.Read())
            {
                Genre anieuw = new Genre();
                anieuw.Id = Int32.Parse(reader["Id"].ToString());
                anieuw.Name = reader["Genre"].ToString();
                anieuw.checkgenre = true;

                lijst.Add(anieuw);
            }
            return lijst;
        }

        internal static ObservableCollection<Genre> getGenres(int id)
        {
            ObservableCollection<Genre> Genrelijst = new ObservableCollection<Genre>();

            string SQLGenre = "SELECT id,Genre.Genre FROM Genre JOIN BandGenre ON Genre.Id=BandGenre.Genre AND Band=@Id";
            DbParameter par1 = Database.AddParameter("@Id", id);
            DbDataReader reader = Database.GetData(SQLGenre,par1);

            while (reader.Read())
            {
                Genre anieuw = new Genre();
                anieuw.Id = Int32.Parse(reader["Id"].ToString());
                anieuw.Name = reader["Genre"].ToString();

                Genrelijst.Add(anieuw);

            }
            return Genrelijst;
        }
    }
}
