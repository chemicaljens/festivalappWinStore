using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMVVM.Model;

namespace ProjectMVVM.Model
{
    class Band : IDataErrorInfo
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
        
        public ObservableCollection<Genre> Genres { get; set; }

        internal static ObservableCollection<Band> getBand()
        {
            ObservableCollection<Band> lijst = new ObservableCollection<Band>();

            string SQL = "SELECT * FROM Band";
            DbDataReader reader = Database.GetData(SQL);

            while (reader.Read())
            {
                Band anieuw = new Band();
                anieuw.Id = Int32.Parse(reader["Id"].ToString());
                anieuw.Name = reader["Name"].ToString();
                anieuw.Picture = reader["Picture"].ToString();
                anieuw.Description = reader["Despription"].ToString();
                anieuw.Twitter = reader["Twitter"].ToString();
                anieuw.Facebook = reader["Facebook"].ToString();
                anieuw.Genres = Genre.getGenres(anieuw.Id);
                lijst.Add(anieuw);

            }
            return lijst;
        }

        public static void SaveNewBand(Band nieuwBand)
        {

            String SQL = "INSERT INTO Band (Name,Picture,Despription,Twitter,Facebook)VALUES(@Name,@Picture,@Despription,@Twitter,@Facebook)";
            DbParameter par1 = Database.AddParameter("@Name", nieuwBand.Name);
            DbParameter par2 = Database.AddParameter("@Picture", nieuwBand.Picture);
            if (par2.Value == null) par2.Value = DBNull.Value;
            DbParameter par3 = Database.AddParameter("@Despription", nieuwBand.Description);
            if (par3.Value == null) par3.Value = DBNull.Value;
            DbParameter par4 = Database.AddParameter("@Twitter", nieuwBand.Twitter);
            if (par4.Value == null) par4.Value = DBNull.Value;
            DbParameter par5 = Database.AddParameter("@Facebook", nieuwBand.Facebook);
            if (par5.Value == null) par5.Value = DBNull.Value;
            try
            {
                Database.ModifyData(SQL, par1, par2, par3, par4, par5);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Error
        {
            get { return "er is een fout"; }
        }

        public string this[string columnName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columnName
                    });
                }
                catch (ValidationException ex)
                {
                    return ex.Message;
                }
                return String.Empty;
            }
        }

        internal static void SaveBand(Band SelectedBand)
        {
            String SQL = "Update Band SET Name=@Name,Picture=@Picture,Despription=@Despription,Twitter=@Twitter,Facebook=@Facebook Where Id=" + SelectedBand.Id;
            DbParameter par1 = Database.AddParameter("@Name", SelectedBand.Name);
            DbParameter par2 = Database.AddParameter("@Picture", SelectedBand.Picture);
            if (par2.Value == null) par2.Value = DBNull.Value;
            DbParameter par3 = Database.AddParameter("@Despription", SelectedBand.Description);
            if (par3.Value == null) par3.Value = DBNull.Value;
            DbParameter par4 = Database.AddParameter("@Twitter", SelectedBand.Twitter);
            if (par4.Value == null) par4.Value = DBNull.Value;
            DbParameter par5 = Database.AddParameter("@Facebook", SelectedBand.Facebook);
            if (par5.Value == null) par5.Value = DBNull.Value;
            try
            {
                Database.ModifyData(SQL, par1, par2, par3, par4, par5);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static void DeleteBand(Band SelectedBand)
        {
            String SQL = "DELETE From Band where id=@id";
            DbParameter par1 = Database.AddParameter("@Id", SelectedBand.Id);
            if (par1.Value == null) par1.Value = DBNull.Value;
            try
            {
                Database.ModifyData(SQL, par1);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

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
