using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using ProjectMVVM.Model;
using System.Data.Common;

namespace ProjectMVVM.Model
{
    class Stage
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public static ObservableCollection<Stage> getStage()
        {
            ObservableCollection<Stage> lijst = new ObservableCollection<Stage>();

            string SQL = "SELECT * FROM Stage";
            DbDataReader reader = Database.GetData(SQL);

            while (reader.Read())
            {
                Stage anieuw = new Stage();
                anieuw.Id = Int32.Parse(reader["ID"].ToString());
                anieuw.Name = reader["Name"].ToString();


                lijst.Add(anieuw);
            }
            return lijst;
        }

        public static void SaveNewStage(Stage nieuwStage)
        {

            String SQL = "INSERT INTO Stage (Name)VALUES(@Name)";
            DbParameter par1 = Database.AddParameter("@Name", nieuwStage.Name);
            try
            {
                Database.ModifyData(SQL, par1);
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

        internal static void DeleteStage(Stage SelectedStages)
        {
            String SQL = "DELETE From Stage where id=@id";
            DbParameter par1 = Database.AddParameter("@Id", SelectedStages.Id);
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

        internal static void SaveStage(Stage SelectedStages)
        {
            String SQL = "Update Stage SET Name=@Name Where Id=" + SelectedStages.Id;
            DbParameter par1 = Database.AddParameter("@Name", SelectedStages.Name);
            try
            {
                Database.ModifyData(SQL, par1);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
