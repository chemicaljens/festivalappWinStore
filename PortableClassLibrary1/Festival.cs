using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMVVM.Model;

namespace ProjectMVVM.Model
{
    public class Festival
    {

        public int Id { get; set; }

        [Required(ErrorMessage="Een naam is verplicht.")]
        [StringLength(20,ErrorMessage="gelieve een naam korter dan 20 karakters te gebruiken")]
        public String Name { get; set; }
        
        public DateTime StarDate { get; set; }

        public DateTime EndDate { get; set; }
        public String Grondplan { get; set; }
        public String Logo { get; set; }

        public static Festival getFestival()
        {
            ObservableCollection<Festival> lijst = new ObservableCollection<Festival>();

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

        public static void SaveFestival(Festival updatefestival)
        {
            String SQL = "Update Festival SET Name=@Name,Startdag=@Startdag,Einddag=@Einddag,Logo=@Logo,Grondplan=@Grondplan Where Id=" + updatefestival.Id;
            DbParameter par1 = Database.AddParameter("@Name", updatefestival.Name);
            DbParameter par2 = Database.AddParameter("@Startdag", updatefestival.StarDate);
            DbParameter par3 = Database.AddParameter("@Einddag", updatefestival.EndDate);
            DbParameter par4 = Database.AddParameter("@Logo", updatefestival.Logo);
            if (par4.Value == null) par4.Value = DBNull.Value;
            DbParameter par5 = Database.AddParameter("@Grondplan", updatefestival.Grondplan);
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

    }
}
