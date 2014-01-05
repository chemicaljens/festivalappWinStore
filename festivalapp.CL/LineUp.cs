using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMVVM.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVVM.Model
{
    class LineUp : IDataErrorInfo
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "De Datum is Verplicht")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "De Start tijdstip is Verplicht")]
        public String From { get; set; }
        [Required(ErrorMessage = "De eind tijd is Verplicht")]
        public String Until { get; set; }
        [Required(ErrorMessage = "De Datum is Verplicht")]
        public int stage { get; set; }
        [Required(ErrorMessage = "een band is Verplicht")]
        public Band band { get; set; }

        public static ObservableCollection<LineUp> getLineUp(Stage selstage)
        {
            ObservableCollection<LineUp> lijst = new ObservableCollection<LineUp>();

            string SQL = "SELECT * FROM LineUp where Stage="+selstage.Id;
            DbDataReader reader = Database.GetData(SQL);

            while (reader.Read())
            {
                LineUp anieuw = new LineUp();
                anieuw.Id = Int32.Parse(reader["Id"].ToString());
                anieuw.Date = (DateTime)reader["Date"];
                anieuw.From = reader["StartTime"].ToString();
                anieuw.Until = reader["EndTime"].ToString();
                anieuw.stage = Int32.Parse(reader["Stage"].ToString());

                anieuw.band = Band.getbandwithID(Int32.Parse(reader["Band"].ToString()));

                lijst.Add(anieuw);
            }
            return lijst;
        }

        public static void SaveNewLineUp(LineUp nieuwLineUP)
        {

            String SQL = "INSERT INTO LineUp (Date,StartTime,EndTime,Stage,Band)VALUES(@Date,@StartTime,@EndTime,@Stage,@Band)";
            DbParameter par1 = Database.AddParameter("@Date", nieuwLineUP.Date);
            DbParameter par2 = Database.AddParameter("@StartTime", nieuwLineUP.From);
            DbParameter par3 = Database.AddParameter("@EndTime", nieuwLineUP.Until);
            DbParameter par4 = Database.AddParameter("@Stage", nieuwLineUP.stage);
            DbParameter par5 = Database.AddParameter("@Band", nieuwLineUP.band);
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

        internal static void DeleteLineUp(LineUp SelectedLineUp)
        {
            String SQL = "DELETE From LineUp where id=@id";
            DbParameter par1 = Database.AddParameter("@Id", SelectedLineUp.Id);
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

        internal static void SaveLineUp(LineUp SelectedLineUp)
        {
            String SQL = "Update LineUp SET Date=@Date,StartTime=@StartTime,EndTime=@EndTime,Stage=@Stage,Band=@Band Where Id=" + SelectedLineUp.Id;
            DbParameter par1 = Database.AddParameter("@Date", SelectedLineUp.Date);
            DbParameter par2 = Database.AddParameter("@StartTime", SelectedLineUp.From);
            DbParameter par3 = Database.AddParameter("@EndTime", SelectedLineUp.Until);
            DbParameter par4 = Database.AddParameter("@Stage", SelectedLineUp.stage);
            DbParameter par5 = Database.AddParameter("@Band", SelectedLineUp.band);
            try
            {
                Database.ModifyData(SQL, par1, par2, par3, par4, par5);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
